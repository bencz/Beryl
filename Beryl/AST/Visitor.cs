﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beryl.AST
{
    public interface Visitor
    {
        void visit(AssignCommand that);
        void visit(BeginCommand that);
        void visit(BinaryExpression that);
        void visit(CallCommand that);
        void visit(Commands that);
        void visit(ConstDeclaration that);
        void visit(Declarations that);
        void visit(FunctionDeclaration that);
		void visit(FunctionExpression that);
        void visit(IfCommand that);
        void visit(IntegerLiteral that);
        void visit(IntegerType that);
        void visit(LetCommand that);
        void visit(Parameter that);
        void visit(Parenthesis that);
        void visit(Program that);
        void visit(UnaryExpression that);
        void visit(VarDeclaration that);
        void visit(Variable that);
        void visit(WhileCommand that);
    }
}