﻿/*
 * Issues:
 * 
 *    1. The syntax specifies that a command can be a sequence of commands, the parser does not yet handle this.
 *    
 * Declarations = Declaration | Declaration ";" Declaration .
 * Declaration  = "var" ... | "const" ... .
 */

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
 
        /*
         * Parses a single command.
         */
        private Command ParseCommand()
        {
            switch (_lookahead.Kind)
            {
                case TokenKind.Identifier:
                    return ParseAssignOrCallCommand();

                case TokenKind.Keyword_If:
                    return ParseIfCommand();

                case TokenKind.Keyword_While:
                    return ParseWhileCommand();

                case TokenKind.Keyword_Let:
                    return ParseLetCommand();

                case TokenKind.Keyword_Begin:
                    return ParseBeginCommand();

                default:
                    throw new ParserError(_lookahead.Position, "Unexpected token: " + _lookahead.ToString());
            }

        }

        /*
         * Parses an assignment or a procedure call command.
         */
        private Command ParseAssignOrCallCommand()
        {
            Token name = Match(TokenKind.Identifier);

            Expression expression;
            switch (_lookahead.Kind)
            {
                case TokenKind.Assignment:      // an assignment statement
                    Match(TokenKind.Assignment);
                    expression = ParseExpression();
                    return new AssignCommand(name.Position, name.Text, expression);
                    
                case TokenKind.LeftParenthesis: // a procedure call
                    Match(TokenKind.LeftParenthesis);
                    expression = ParseExpression();
                    Match(TokenKind.RightParenthesis);
                    return new CallCommand(name.Position, name.Text, expression);

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

        private Declaration ParseConstDeclaration()
        {
            Token start = Match(TokenKind.Keyword_Const);
            Token name = Match(TokenKind.Identifier);
			Match(TokenKind.Tilde);
            Expression expression = ParseExpression();
            return new ConstDeclaration(start.Position, name.Text, expression);
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
                        declaration = ParseConstDeclaration();
                        declarations.Add(declaration);
                        break;

                    case TokenKind.Keyword_Var:
                        declaration = ParseVarDeclaration();
                        declarations.Add(declaration);
                        break;

                    default:
                        throw new ParserError(_lookahead.Position, "Expected 'const' or 'var' declaration");
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
                    return new IntegerLiteral(token.Position, int.Parse(token.Text));

                case TokenKind.Identifier:
                    token = Match(TokenKind.Identifier);
                    return new Variable(token.Position, token.Text);

                case TokenKind.Plus:
                case TokenKind.Minus:
                    token = ReadToken();
                    Operator @operator = TokenKindToOperator(token.Position, token.Kind);
                    return new UnaryExpression(token.Position, @operator, ParseExpression());

                default:
                    throw new ParserError(_lookahead.Position, "Expected integer literal, identifier or unary operator");
            }
        }

        private Expression ParseExpression()
        {
            Expression first = ParseExpressionAtom();
            while (IsOperator(_lookahead.Kind))
            {
                Token token = ReadToken();
                Operator @operator = TokenKindToOperator(token.Position, token.Kind);
                Expression other = ParseExpressionAtom();

                first = new BinaryExpression(first.Position, first, @operator, other);
            }

            return first;
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

        private Declaration ParseVarDeclaration()
        {
            Token start = Match(TokenKind.Keyword_Var);
            Token name = Match(TokenKind.Identifier);
            Match(TokenKind.Colon);
            Token type = Match(TokenKind.Identifier);
            return new VarDeclaration(start.Position, name.Text, type.Text);
        }

        private Command ParseWhileCommand()
        {
            Token start = Match(TokenKind.Keyword_While);
            Expression expression = ParseExpression();
            Match(TokenKind.Keyword_Do);
            Command command = ParseCommand();
            return new WhileCommand(start.Position, expression, command);
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

        private Operator TokenKindToOperator(Position position, TokenKind kind)
        {
            switch (kind)
            {
                case TokenKind.Asterisk: return Operator.Multiplication;
                case TokenKind.Backslash: return Operator.Difference;
                case TokenKind.Equal: return Operator.Equality;
                case TokenKind.GreaterThan: return Operator.GreaterThan;
                case TokenKind.LessThan: return Operator.LessThan;
                case TokenKind.Minus: return Operator.Subtraction;
                case TokenKind.Plus: return Operator.Addition;
                default: throw new ParserError(position, "Invalid operator encountered: " + kind.ToString());
            }
        }
    }
}
