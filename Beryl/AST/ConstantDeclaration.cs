using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beryl.AST
{
    public class ConstantDeclaration: Declaration
    {
        private Expression _expression;
        public Expression Expression
        {
            get { return _expression; }
        }

        public ConstantDeclaration(Position position, string name, AST.Type type, Expression expression):
            base(position, name, SymbolKind.Constant, type)
        {
            _expression = expression;
            _expression.Parent = this;
        }

        public override void DumpFields(Indenter stream)
        {
            base.DumpFields(stream);
            stream.WriteLine("Expression = {0,4:D4}", _expression.Id);
        }

        public override void visit(Visitor that)
        {
            that.visit(this);
        }
    }
}

