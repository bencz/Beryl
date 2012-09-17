using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Beryl.AST;

namespace Beryl
{
    public class Parser
    {
        private SymbolTable _symbols;
        private Scanner _scanner;
        private Token _lookahead;

        public Parser(SymbolTable symbols, Scanner scanner)
        {
            _symbols = symbols;
            _scanner = scanner;

            _lookahead = _scanner.ReadToken();
        }

        private Token ReadToken()
        {
            Token result = _lookahead;
            _lookahead = _scanner.ReadToken();
            return result;
        }

        private Token Match(TokenKind kind)
        {
            if (_lookahead.Kind == kind)
                return ReadToken();

            throw new ParserError(_lookahead.Position, "Expected '" + kind.ToString() + "', found '" + _lookahead.ToString() + "'");
        }

        private bool IsOperator(TokenKind kind)
        {
            switch (kind)
            {
                case TokenKind.Asterisk:
                case TokenKind.Backslash:
                case TokenKind.Equal:
                case TokenKind.GreaterThan:
                case TokenKind.LessThan:
                case TokenKind.Minus:
                case TokenKind.Plus:
                    return true;

                default:
                    return false;
            }
        }


        private Expression[] ParseArguments()
        {
            Match(TokenKind.LeftParenthesis);
            List<Expression> arguments = new List<Expression>();
            while (_lookahead.Kind != TokenKind.RightParenthesis)
            {
                Expression argument = ParseExpression();
                arguments.Add(argument);

                if (_lookahead.Kind != TokenKind.Comma)
                    break;

                Match(TokenKind.Comma);
            }
            Match(TokenKind.RightParenthesis);

            return arguments.ToArray();
        }

        /*
         * Parses an assignment or a procedure call command.
         */
        private Command ParseAssignOrCallCommand()
        {
            Token name = Match(TokenKind.Identifier);

            switch (_lookahead.Kind)
            {
                case TokenKind.Assignment:      // an assignment statement
                    Match(TokenKind.Assignment);
                    Expression expression = ParseExpression();
                    return new AssignCommand(name.Position, name.Text, expression);

                case TokenKind.LeftParenthesis: // a procedure call
                    Expression[] arguments = ParseArguments();
                    return new CallCommand(name.Position, name.Text, arguments);

                default:
                    throw new ParserError(_lookahead.Position, "Unexpected token: " + _lookahead.ToString());
            }
        }

        private Command ParseBeginCommand()
        {
            Token start = Match(TokenKind.Keyword_Begin);
            Commands commands = ParseCommands();
            Match(TokenKind.Keyword_End);
            return new BeginCommand(start.Position, commands);
        }

        /*
         * Parses a single command.
         */
        private Command ParseCommand()
        {
            switch (_lookahead.Kind)
            {
                case TokenKind.Identifier:
                    return ParseAssignOrCallCommand();

                case TokenKind.Keyword_Begin:
                    return ParseBeginCommand();

                case TokenKind.Keyword_If:
                    return ParseIfCommand();

                case TokenKind.Keyword_Let:
                    return ParseLetCommand();

                case TokenKind.Keyword_While:
                    return ParseWhileCommand();

                default:
                    throw new ParserError(_lookahead.Position, "Unexpected token: " + _lookahead.ToString());
            }

        }

        private Commands ParseCommands()
        {
            Token start = _lookahead;

            List<Command> commands = new List<Command>();
            for (;;)
            {
                Command command = ParseCommand();
                commands.Add(command);

                if (_lookahead.Kind != TokenKind.Semicolon)
                    break;
                Match(TokenKind.Semicolon);
            }

            return new Commands(start.Position, commands.ToArray());
        }

        private Declaration ParseConstantDeclaration()
        {
            Token start = Match(TokenKind.Keyword_Const);
            Token name = Match(TokenKind.Identifier);
            Match(TokenKind.Tilde);
            Expression expression = ParseExpression();
            return new ConstantDeclaration(start.Position, name.Text, new IntegerType(start.Position), expression);
        }

        private Declaration[] ParseDeclaration()
        {
            List<Declaration> declarations = new List<Declaration>();
            for (;;)
            {
                Declaration declaration;
                switch (_lookahead.Kind)
                {
                    case TokenKind.Keyword_Const:
                        declaration = ParseConstantDeclaration();
                        declarations.Add(declaration);
                        break;

                    case TokenKind.Keyword_Func:
                        declaration = ParseFuncDeclaration();
                        declarations.Add(declaration);
                        break;

                    case TokenKind.Keyword_Var:
                        declaration = ParseVariableDeclaration();
                        declarations.Add(declaration);
                        break;

                    default:
                        throw new ParserError(_lookahead.Position, "Expected 'const', 'func', or 'var' declaration");
                }

                if (_lookahead.Kind != TokenKind.Semicolon)
                    break;
                Match(TokenKind.Semicolon);
            }

            return declarations.ToArray();
        }

        private Expression ParseExpressionAtom()
        {
            Token token;
            switch (_lookahead.Kind)
            {
                case TokenKind.Literal_Integer:
                    token = Match(TokenKind.Literal_Integer);
                    return new IntegerExpression(token.Position, int.Parse(token.Text));

                case TokenKind.Literal_String:
                    token = Match(TokenKind.Literal_String);
                    return new StringExpression(token.Position, token.Text);

                case TokenKind.Identifier:
                    token = Match(TokenKind.Identifier);
                    if (_lookahead.Kind != TokenKind.LeftParenthesis)
                        return new VariableExpression(token.Position, token.Text);
                    // parse function invokation
                    Match(TokenKind.LeftParenthesis);
                    List<Expression> arguments = new List<Expression>();
                    while (_lookahead.Kind != TokenKind.RightParenthesis)
                    {
                        arguments.Add(ParseExpression());

                        if (_lookahead.Kind != TokenKind.Comma)
                            break;
                        Match(TokenKind.Comma);
                    }
                    Match(TokenKind.RightParenthesis);
                    return new FunctionExpression(token.Position, token.Text, arguments.ToArray());

                case TokenKind.Plus:
                case TokenKind.Minus:
                    token = ReadToken();
                    string name = TokenKindToOperatorName(token.Position, token.Kind);
                    return new FunctionExpression(token.Position, name, new Expression[]{ ParseExpression() });

                default:
                    throw new ParserError(_lookahead.Position, "Expected integer literal, string literal, identifier, or unary operator");
            }
        }

        private Expression ParseExpression()
        {
            Expression first = ParseExpressionAtom();
            while (IsOperator(_lookahead.Kind))
            {
                Token token = ReadToken();
                string name = TokenKindToOperatorName(token.Position, token.Kind);
                Expression other = ParseExpressionAtom();

                first = new FunctionExpression(token.Position, name, new Expression[] { first, other });
            }

            return first;
        }

        private Declaration ParseFuncDeclaration()
        {
            Token start = Match(TokenKind.Keyword_Func);
            string identifier = Match(TokenKind.Identifier).Text;

            // parse parameter list
            Match(TokenKind.LeftParenthesis);
            List<ParameterDeclaration> parameters = new List<ParameterDeclaration>();
            while (_lookahead.Kind != TokenKind.RightParenthesis)
            {
                Token name = Match(TokenKind.Identifier);
                Match(TokenKind.Colon);
                AST.Type type = ParseType();
                parameters.Add(new ParameterDeclaration(name.Position, name.Text, type));

                if (_lookahead.Kind != TokenKind.Comma)
                    break;

                Match(TokenKind.Comma);
            }
            Match(TokenKind.RightParenthesis);

            // parse return type specification
            Match(TokenKind.Colon);
            AST.Type returnType = ParseType();

            Match(TokenKind.Tilde);
            Expression body = ParseExpression();

            return new FunctionDeclaration(start.Position, identifier, returnType, parameters.ToArray(), body);
        }

        private Command ParseIfCommand()
        {
            Token start = Match(TokenKind.Keyword_If);
            Expression expression = ParseExpression();
            Match(TokenKind.Keyword_Then);
            Command @if = ParseCommand();
            Match(TokenKind.Keyword_Else);
            Command @else = ParseCommand();
            return new IfCommand(start.Position, expression, @if, @else);
        }

        private Command ParseLetCommand()
        {
            Token start = Match(TokenKind.Keyword_Let);
            Declaration[] declarations = ParseDeclaration();
            Match(TokenKind.Keyword_In);
            Command command = ParseCommand();
            return new LetCommand(start.Position, declarations, command);
        }

        /*
         * Parses a sequence of commands.
         */
        public AST.Program ParseProgram()
        {
            Token start = _lookahead;
            Commands commands = ParseCommands();
            return new AST.Program(start.Position, commands);
        }

        private AST.Type ParseType()
        {
            Token type = Match(TokenKind.Identifier);
            switch (type.Text)
            {
                case "Boolean":
                    return new BooleanType(type.Position);

                case "Integer":
                    return new IntegerType(type.Position);

                case "String":
                    return new StringType(type.Position);

                default:
                    throw new ParserError(type.Position, "Unknown type '" + type.Text + "' encountered");
            }
        }

        private Declaration ParseVariableDeclaration()
        {
            Token start = Match(TokenKind.Keyword_Var);
            Token name = Match(TokenKind.Identifier);
            Match(TokenKind.Colon);
            AST.Type type = ParseType();
            return new VariableDeclaration(start.Position, name.Text, type);
        }

        private Command ParseWhileCommand()
        {
            Token start = Match(TokenKind.Keyword_While);
            Expression expression = ParseExpression();
            Match(TokenKind.Keyword_Do);
            Command command = ParseCommand();
            return new WhileCommand(start.Position, expression, command);
        }

#if false
        private OperatorKind TokenKindToOperator(Position position, TokenKind kind)
        {
            switch (kind)
            {
                case TokenKind.Asterisk: return OperatorKind.Multiplication;
                case TokenKind.Backslash: return OperatorKind.Difference;
                case TokenKind.Equal: return OperatorKind.Equality;
                case TokenKind.GreaterThan: return OperatorKind.GreaterThan;
                case TokenKind.LessThan: return OperatorKind.LessThan;
                case TokenKind.Minus: return OperatorKind.Subtraction;
                case TokenKind.Plus: return OperatorKind.Addition;
                default: throw new ParserError(position, "Invalid operator encountered: " + kind.ToString());
            }
        }
#endif

        private string TokenKindToOperatorName(Position position, TokenKind kind)
        {
            switch (kind)
            {
                case TokenKind.Asterisk   : return "*";
                case TokenKind.Backslash  : return "\\";
                case TokenKind.Equal      : return "=";
                case TokenKind.GreaterThan: return ">";
                case TokenKind.LessThan   : return "<";
                case TokenKind.Minus      : return "-";
                case TokenKind.Plus       : return "+";
                default: throw new ParserError(position, "Invalid operator encountered: " + kind.ToString());
            }
        }
    }
}
