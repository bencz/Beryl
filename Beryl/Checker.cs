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
            _program.visit(this);
        }

        public void visit(AssignCommand that)
        {
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
			// note: leave the checking of the identifier to the code generator as it knows what symbols it supports
            that.Expression.visit(this); 
        }

        public void visit(Commands that)
        {
            foreach (Command command in that.CommandArray)
                command.visit(this);
        }

        public void visit(ConstDeclaration that)
        {
            int value = that.Expression.Evaluate(_symbols);
            Symbol symbol = new Symbol(that.Position, that.Identifier, new IntegerType(that.Expression.Position), value);
            _symbols.Insert(that.Position, that.Identifier, symbol);
        }

        public void visit(Declarations that)
        {
			foreach (Declaration declaration in that.DeclarationsArray)
				declaration.visit(this);
        }

        public void visit(FunctionDeclaration that)
        {
			foreach (Parameter parameter in that.Parameters)
				parameter.visit(this);
			that.Body.visit(this);
        }

		public void visit(FunctionExpression that)
		{
			foreach (Expression argument in that.Arguments)
				argument.visit(this);
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
			if (that.Type != "integer")
				throw new CheckerError(that.Position, "Unknown type: " + that.Type);
        }

        public void visit(Parenthesis that)
        {
			that.Expression.visit(this);
        }

        public void visit(AST.Program that)
        {
            that.Commands.visit(this);
        }

        public void visit(UnaryExpression that)
        {
			that.Expression.visit(this);
        }

        public void visit(VarDeclaration that)
        {
			if (that.Type != "integer")
				throw new CheckerError(that.Position, "Unknown type: " + that.Type);
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
