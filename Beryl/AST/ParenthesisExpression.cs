using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beryl.AST
{
    public class ParenthesisExpression: Expression
    {
        private Expression _expression;
        public Expression Expression
        {
            get { return _expression; }
        }

        public ParenthesisExpression(Position position, Expression expression):
            base(position)
        {
            _expression = expression;
            _expression.Parent = this;
        }

        public override void DumpFields(Indenter stream)
        {
            stream.WriteLine("Expression = {0,4:D4}", _expression.Id);
        }

        public override int Evaluate(SymbolTable symbols)
        {
            return _expression.Evaluate(symbols);
        }

        public override void visit(Visitor that)
        {
            that.visit(this);
        }
    }
}
