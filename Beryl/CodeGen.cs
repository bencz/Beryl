using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Beryl.AST;

namespace Beryl
{
    public class CodeGen : Visitor
    {
        private SymbolTable _symbols;
        private AST.Program _program;

        public CodeGen(SymbolTable symbols, AST.Program program)
        {
            _symbols = symbols;
            _program = program;
            _program.visit(this);
        }

        public void visit(AssignCommand that)
        {
            Console.WriteLine("AssignCommand:");

            /* generate code to do the assign command */
            Console.WriteLine("name = {0}", that.Name);

            // we must explicitly walk all children of all nodes...
            that.Expression.visit(this);
        }

        public void visit(BeginCommand that)
        {
            Console.WriteLine("BeginCommand:");
            that.Commands.visit(this);
        }

        public void visit(BinaryExpression that)
        {
        }

        public void visit(CallCommand that)
        {
            Console.WriteLine("CallCommand:");
            that.Expression.visit(this); // yes, but they are the responsibility of the code genereator>

            switch (that.Identifier)
            {
                case "getint":
                    // To get the parameters ??
                    // getint( xx )
                    Variable argument = that.Expression as Variable;
                    if (argument == null)
                        throw new CoderError("Variable expected");
                    Console.WriteLine("Arg = {0}", argument.Name);
                    break;

                case "putint":
                    break;
            }
        }

        /* This is the second tree-walking method that is called! */
        public void visit(Commands that)
        {
            foreach (Command command in that.CommandArray)
                command.visit(this);
        }

        public void visit(ConstDeclaration that)
        {
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

        public void visit(FunctionExpression that)
        {
            foreach (Expression argument in that.Arguments)
                argument.visit(this);
        }

        public void visit(FunctionType that)
        {
            foreach (Parameter parameter in that.Parameters)
                parameter.visit(this);
            that.Body.visit(this);
        }

        public void visit(IfCommand that)
        {
            Console.WriteLine("IfCommand:");
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
            Console.WriteLine("LetCommand:");
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
            Console.WriteLine("WhileCommand:");
        }
    }
}
