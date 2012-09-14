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
            // we must explicitly walk all children of all nodes...
            that.Expression.visit(this);
        }

        public void visit(BeginCommand that)
        {
            that.Commands.visit(this);
        }

        public void visit(BinaryExpression that)
        {
        }

        public void visit(CallCommand that)
        {
            that.Expression.visit(this); 
        }

        /* This is the second tree-walking method that is called! */
        public void visit(Commands that)
        {
            foreach (Command command in that.CommandArray)
                command.visit(this);
        }

        public void visit(ConstDeclaration that)
        {
            int value = that.Expression.Evaluate(_symbols);
            Symbol symbol = new Symbol(that.Position, that.Identifier, new  IntegerType(that.Expression.Position), 0);
            _symbols.Insert(that.Position, that.Identifier, symbol);
        }

        public void visit(Declaration that)
        {
        }

        public void visit(Declarations that)
        {
        }

        public void visit(Expression that)
        {
        }

        public void visit(FunctionDeclaration that)
        {
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
        }

        /* This is the first tree-walking method that is called! */
        public void visit(AST.Program that)
        {
            /* process the Program node */
            that.Commands.visit(this);
        }

        public void visit(UnaryExpression that)
        {
        }

        public void visit(VarDeclaration that)
        {
        }

        public void visit(Variable that)
        {
        }

        public void visit(WhileCommand that)
        {
        }
    }
}
