using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Beryl.AST;

namespace Beryl
{
    public class Checker: Visitor
    {
        private SymbolTable _symbols;
        private AST.Program _program;

        public Checker(SymbolTable symbols, AST.Program program)
        {
            _symbols = symbols;
            _program = program;

            // signal that the following symbols are part of the standard library
            Position position = new Position("(library)", 0, 0);

            // enter the predefined functions (getint and putint) into the symbol table
            Declaration declaration = new FunctionDeclaration(
                position,
                "getint",
                new IntegerType(position),
                new Parameter[] { new Parameter(position, "value", new IntegerType(position))},
                null        // note: the code generator must handle these predefined functions so no body is defined
            );
            Symbol symbol = new Symbol(position, "getint", declaration);
            _symbols.Insert(position, "getint", symbol);

            declaration = new FunctionDeclaration(
                position,
                "putint",
                new IntegerType(position),
                new Parameter[] { new Parameter(position, "value", new IntegerType(position))},
                null        // note: the code generator must handle these predefined functions so no body is defined
            );
            symbol = new Symbol(position, "putint", declaration);
            _symbols.Insert(position, "putint", symbol);

            // enter the predefined constants 'false' and 'true' into the symbol table
            Expression expression = new BooleanExpression(position, false);
            declaration = new ConstDeclaration(position, "false", new BooleanType(position), expression);
            symbol = new Symbol(position, "false", declaration);
            _symbols.Insert(position, "false", symbol);

            expression = new BooleanExpression(position, true);
            declaration = new ConstDeclaration(position, "true", new BooleanType(position), expression);

            _program.visit(this);
        }

        public void visit(AssignCommand that)
        {
            // check that the symbol exists - by trying to look it up
            Symbol symbol = _symbols.Lookup(that.Name);
            if (symbol == null)
                throw new CheckerError(that.Position, "Variable '" + that.Name + "' not found in assignment statement");

            // check that the symbol is indeed a variable
            switch (symbol.Declaration.Kind)
            {
                case SymbolKind.Constant:
                    throw new CheckerError(symbol.Position, "Cannot assign to a constant");

                case SymbolKind.Function:
                    throw new CheckerError(symbol.Position, "Cannot assign to a function");

                case SymbolKind.Variable:
                    break;

                default:
                    throw new CheckerError(symbol.Position, "Unknown symbol kind: " + symbol.Declaration.Kind.ToString());
            }

            // nothing more to check as we support only integers
            // todo: evaluate the type of the rhs and check that the variable has the same type

            that.Expression.visit(this);
        }

        public void visit(BeginCommand that)
        {
            that.Commands.visit(this);
        }

        public void visit(BinaryExpression that)
        {
            that.First.visit(this);
            that.Other.visit(this);
        }

        public void visit(BooleanExpression that)
        {
        }

        public void visit(BooleanType that)
        {
        }

        public void visit(CallCommand that)
        {
            Symbol symbol = _symbols.Lookup(that.Identifier);
            if (symbol == null)
                throw new CheckerError(that.Position, "Unknown function name '" + that.Identifier + "' in call command");

            switch (symbol.Declaration.Kind)
            {
                case SymbolKind.Constant:
                    throw new CheckerError(that.Position, "Cannot invoke constant");

                case SymbolKind.Function:
                    break;

                case SymbolKind.Variable:
                    throw new CheckerError(that.Position, "Cannot invoke variable");

                default:
                    throw new CheckerError(symbol.Position, "Unknown symbol kind: " + symbol.Declaration.Kind.ToString());
            }

            // check that the expected number of parameters is specified
            FunctionDeclaration declaration = (FunctionDeclaration) symbol.Declaration;
            if (that.Arguments.Length != declaration.Parameters.Length)
                throw new CheckerError(that.Position, "Incorrect number of parameters in function call");
            // todo: check that the argument types match the parameter types

            foreach (Expression argument in that.Arguments)
                argument.visit(this);
        }

        public void visit(Commands that)
        {
            foreach (Command command in that.CommandArray)
                command.visit(this);
        }

        public void visit(ConstDeclaration that)
        {
            Symbol symbol = new Symbol(that.Position, that.Identifier, that);
            _symbols.Insert(that.Position, that.Identifier, symbol);
        }

        public void visit(Declarations that)
        {
            foreach (Declaration declaration in that.DeclarationsArray)
                declaration.visit(this);
        }

        public void visit(FunctionDeclaration that)
        {
            Symbol symbol = new Symbol(that.Position, that.Name, that);
            _symbols.Insert(that.Position, that.Name, symbol);

            that.Type.visit(this);
            foreach (Parameter parameter in that.Parameters)
                parameter.visit(this);
            that.Body.visit(this);

        }

        public void visit(FunctionExpression that)
        {
            Symbol symbol = _symbols.Lookup(that.Name);
            if (symbol == null)
                throw new CheckerError(that.Position, "Unknown function name '" + that.Name + "' in function call");

            switch (symbol.Declaration.Kind)
            {
                case SymbolKind.Constant:
                    throw new CheckerError(that.Position, "Cannot invoke constant");

                case SymbolKind.Function:
                    break;

                case SymbolKind.Variable:
                    throw new CheckerError(that.Position, "Cannot invoke variable");

                default:
                    throw new CheckerError(symbol.Position, "Unknown symbol kind: " + symbol.Declaration.Kind.ToString());
            }

            FunctionDeclaration declaration = (FunctionDeclaration) symbol.Declaration;
            if (that.Arguments.Length != declaration.Parameters.Length)
                throw new CheckerError(that.Position, "Incorrect number of parameters in function call");
            // todo: check that the argument types match the parameter types

            foreach (Expression argument in that.Arguments)
                argument.visit(this);
        }

        public void visit(IfCommand that)
        {
            that.Expression.visit(this);
            that.If.visit(this);
            that.Else.visit(this);
        }

        public void visit(IntegerExpression that)
        {
        }

        public void visit(IntegerType that)
        {
            // this method does nothing as long as we have only one type (Integer)
        }

        public void visit(LetCommand that)
        {
            _symbols.EnterScope();
            foreach (Declaration declaration in that.Declarations)
                declaration.visit(this);
            that.Command.visit(this);
            _symbols.LeaveScope();
        }

        public void visit(Parameter that)
        {
        }

        public void visit(Parenthesis that)
        {
            that.Expression.visit(this);
        }

        public void visit(AST.Program that)
        {
            that.Commands.visit(this);
        }

        public void visit(StringExpression that)
        {
        }

        public void visit(StringType that)
        {
        }

        public void visit(UnaryExpression that)
        {
            that.Expression.visit(this);
        }

        public void visit(VarDeclaration that)
        {
            Symbol symbol = new Symbol(that.Position, that.Identifier, that);
            _symbols.Insert(that.Position, that.Identifier, symbol);
        }

        public void visit(VariableExpression that)
        {
        }

        public void visit(WhileCommand that)
        {
            that.Expression.visit(this);
            that.Command.visit(this);
        }
    }
}

