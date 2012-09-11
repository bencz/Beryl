using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beryl.AST
{
    public class UnaryExpression: Expression
    {
        private Operator _operator;
        private Expression _expression;

        public UnaryExpression(Operator @operator, Expression expression)
        {
            _operator = @operator;
            _expression = expression;
            _expression.Parent = this;
        }
    }
}
