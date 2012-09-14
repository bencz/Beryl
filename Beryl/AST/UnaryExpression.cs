using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beryl.AST
{
    public class UnaryExpression: Expression
    {
        private Operator _operator;
        public Operator Operator
        {
            get { return _operator; }
        }

        private Expression _expression;
        public Expression Expression
        {
            get { return _expression; }
        }

        public UnaryExpression(Operator @operator, Expression expression)
        {
            _operator = @operator;
            _expression = expression;
            _expression.Parent = this;
        }

        public override int Evaluate(SymbolTable symbols)
        {
            int result = _expression.Evaluate(symbols);
            if (_operator == Operator.Subtraction)
                result = -result;
            return result;
        }

        public override void visit(Visitor that)
        {
            that.visit(this);
        }
    }
}
