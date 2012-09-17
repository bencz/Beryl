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
            Console.Write("{0} = ", that.Name);
            // we must explicitly walk all children of all nodes...
            that.Expression.visit(this);
            Console.WriteLine(";");
        }

        public void visit(BeginCommand that)
        {
            Console.WriteLine("{");
            that.Commands.visit(this);
            Console.WriteLine("}");
        }

        public void visit(BooleanExpression that)
        {
            Console.Write(that.Value ? "true" : "false");
        }

        public void visit(BooleanType that)
        {
            Console.Write("bool");
        }

        public void visit(CallCommand that)
        {
            Console.Write("{0}(", that.Identifier);
            foreach (Expression argument in that.Arguments)
            {
                argument.visit(this);

                if (argument != that.Arguments[that.Arguments.Length - 1])
                    Console.Write(", ");
            }
            Console.WriteLine(");");
#if false
            switch (that.Identifier)
            {
                case "getint":
                    // To get the parameters ??
                    // getint( xx )
                    if (that.Arguments.Length != 1)
                        throw new CoderError(that.Position, "Incorrect number of parameters in function call");
                    VariableExpression argument = that.Arguments[0] as VariableExpression;
                    if (argument == null)
                        throw new CoderError("Variable expected");
                    Console.WriteLine("Arg = {0}", argument.Name);
                    break;

                case "putint":
                    break;
            }

            foreach (Expression argument in that.Arguments)
                argument.visit(this);
#endif
        }

        /* This is the second tree-walking method that is called! */
        public void visit(Commands that)
        {
            foreach (Command command in that.CommandArray)
                command.visit(this);
        }

        public void visit(ConstantDeclaration that)
        {
            Console.Write("const ");
            that.Type.visit(this);
            Console.Write(" {0} = ", that.Name);
            that.Expression.visit(this);
            Console.WriteLine(";");
        }

        public void visit(Declarations that)
        {
            foreach (Declaration declaration in that.DeclarationsArray)
                declaration.visit(this);
        }

        public void visit(FunctionDeclaration that)
        {
            that.Type.visit(this);
            Console.Write(" {0}(", that.Name);
            foreach (ParameterDeclaration parameter in that.Parameters)
            {
                parameter.visit(this);
                if (parameter != that.Parameters[that.Parameters.Length - 1])
                    Console.Write(", ");
            }
            Console.WriteLine(")");
            Console.WriteLine("{");
            Console.Write("return ");
            if (that.Body != null)
                that.Body.visit(this);
            else
                Console.Write("null");
            Console.WriteLine(";");
            Console.WriteLine("}");
        }

        public void visit(FunctionExpression that)
        {
            Console.Write("{0}(", that.Name);
            foreach (Expression argument in that.Arguments)
            {
                argument.visit(this);
                if (argument != that.Arguments[that.Arguments.Length - 1])
                    Console.Write(", ");
            }
            Console.Write(")");
        }

        public void visit(IfCommand that)
        {
            Console.Write("if (");
            that.Expression.visit(this);
            Console.WriteLine(")");
            that.If.visit(this);
            Console.WriteLine("else");
            that.Else.visit(this);
        }

        public void visit(IntegerExpression that)
        {
            Console.Write(that.Value);
        }

        public void visit(IntegerType that)
        {
            Console.Write("int");
        }

        public void visit(LetCommand that)
        {
            Console.WriteLine("{");
            // note: function declarations should be output before the first line
            foreach (Declaration declaration in that.Declarations)
                declaration.visit(this);
            that.Command.visit(this);
            Console.WriteLine("}");
        }

        public void visit(ParameterDeclaration that)
        {
            that.Type.visit(this);
            Console.Write(" {0}", that.Name);
        }

        public void visit(ParenthesisExpression that)
        {
            Console.Write("(");
            that.Expression.visit(this);
            Console.Write(")");
        }

        /* This is the first tree-walking method that is called! */
        public void visit(AST.Program that)
        {
            Console.WriteLine("namespace Foobar");
            that.Command.visit(this);
        }

        public void visit(StringExpression that)
        {
            Console.Write("{0}", that.Value);
        }

        public void visit(StringType that)
        {
            Console.Write("string");
        }

        public void visit(VariableDeclaration that)
        {
            that.Type.visit(this);
            Console.WriteLine(" {0};", that.Name);
        }

        /* VariableExpression perhaps ought to be called IdentifierExpression as it is used for both constants and variables. */
        public void visit(VariableExpression that)
        {
            Console.Write(that.Name);
        }

        public void visit(WhileCommand that)
        {
            Console.Write("while (");
            that.Expression.visit(this);
            Console.WriteLine(")");
            that.Command.visit(this);
        }
    }
}

