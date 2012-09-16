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

            // enter the predefined functions (getint and putint) into the symbol table
            Position position = new Position("(library)", 0, 0);
            AST.Type type = new FunctionType(
                position,
                new IntegerType(position),
                new Parameter[] { new Parameter(position, "value", new IntegerType(position))},
                null        // note: the code generator must handle these predefined functions so no body is defined
            );
            Symbol symbol = new Symbol(position, SymbolKind.Function, "getint", type);
            _symbols.Insert(position, "getint", symbol);

            type = new FunctionType(
                position,
                new IntegerType(position),
                new Parameter[] { new Parameter(position, "value", new IntegerType(position))},
                null        // note: the code generator must handle these predefined functions so no body is defined
            );
            symbol = new Symbol(position, SymbolKind.Function, "putint", type);
            _symbols.Insert(position, "putint", symbol);

            _program.visit(this);
        }

        public void visit(AssignCommand that)
        {
            // check that the symbol exists - by trying to look it up
            Symbol symbol = _symbols.Lookup(that.Position, that.Name);

            // check that the symbol is indeed a variable
            switch (symbol.Kind)
            {
                case SymbolKind.Constant:
                    throw new CheckerError(symbol.Position, "Cannot assign to a constant");

                case SymbolKind.Function:
                    throw new CheckerError(symbol.Position, "Cannot assign to a function");

                case SymbolKind.Variable:
                    break;

                default:
                    throw new CheckerError(symbol.Position, "Unknown symbol kind: " + symbol.Kind.ToString());
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

        public void visit(CallCommand that)
        {
            Symbol symbol = _symbols.Lookup(that.Position, that.Identifier);
            switch (symbol.Kind)
            {
                case SymbolKind.Constant:
                    throw new CheckerError(that.Position, "Cannot invoke constant");

                case SymbolKind.Function:
                    break;

                case SymbolKind.Variable:
                    throw new CheckerError(that.Position, "Cannot invoke variable");

                default:
                    throw new CheckerError(symbol.Position, "Unknown symbol kind: " + symbol.Kind.ToString());
            }

            // check that the expected number of parameters is specified
            FunctionType type = (FunctionType) symbol.Type;
            if (that.Arguments.Length != type.Parameters.Length)
                throw new CheckerError(that.Position, "Incorrect number of parameters in function call");

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
            int value = that.Expression.Evaluate(_symbols);
            Symbol symbol = new Symbol(that.Position, SymbolKind.Constant, that.Identifier, that.Type, value);
            _symbols.Insert(that.Position, that.Identifier, symbol);
        }

        public void visit(Declarations that)
        {
            foreach (Declaration declaration in that.DeclarationsArray)
                declaration.visit(this);
        }

        public void visit(FunctionDeclaration that)
        {
            Symbol symbol = new Symbol(that.Position, SymbolKind.Function, that.Name, that.Type);
            _symbols.Insert(that.Position, that.Name, symbol);
            that.Type.visit(this);
        }

        public void visit(FunctionExpression that)
        {
            Symbol symbol = _symbols.Lookup(that.Position, that.Name);

            switch (symbol.Kind)
            {
                case SymbolKind.Constant:
                    throw new CheckerError(that.Position, "Cannot invoke constant");

                case SymbolKind.Function:
                    break;

                case SymbolKind.Variable:
                    throw new CheckerError(that.Position, "Cannot invoke variable");

                default:
                    throw new CheckerError(symbol.Position, "Unknown symbol kind: " + symbol.Kind.ToString());
            }

            FunctionType type = (FunctionType) symbol.Type;
            if (that.Arguments.Length != type.Parameters.Length)
                throw new CheckerError(that.Position, "Incorrect number of parameters in function call");

            foreach (Expression argument in that.Arguments)
                argument.visit(this);
        }

        public void visit(FunctionType that)
        {
            that.Type.visit(this);
            foreach (Parameter parameter in that.Parameters)
                parameter.visit(this);
            that.Body.visit(this);
        }

        public void visit(IfCommand that)
        {
            that.Expression.visit(this);
            that.If.visit(this);
            that.Else.visit(this);
        }

        public void visit(IntegerLiteral that)
        {
        }

        public void visit(IntegerType that)
        {
            // this method does nothing as long as we have only one type (Integer)
        }

        public void visit(LetCommand that)
        {
            foreach (Declaration declaration in that.Declarations)
                declaration.visit(this);
            that.Command.visit(this);
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

        public void visit(StringLiteral that)
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
            Symbol symbol = new Symbol(that.Position, SymbolKind.Variable, that.Identifier, that.Type);
            _symbols.Insert(that.Position, that.Identifier, symbol);
        }

        public void visit(Variable that)
        {
        }

        public void visit(WhileCommand that)
        {
            that.Expression.visit(this);
            that.Command.visit(this);
        }
    }
}

