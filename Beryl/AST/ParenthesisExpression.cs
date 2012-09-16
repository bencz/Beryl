using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beryl.AST
{
    public class Parenthesis: Expression
    {
        private Expression _expression;
        public Expression Expression
        {
            get { return _expression; }
        }

        public Parenthesis(Position position, Expression expression):
			base(position)
        {
            _expression = expression;
            _expression.Parent = this;
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
