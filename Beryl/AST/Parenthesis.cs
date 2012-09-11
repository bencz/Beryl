using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beryl.AST
{
    public class Parenthesis: Expression
    {
        private Expression _expression;

        public Parenthesis(Expression expression)
        {
            _expression = expression;
            _expression.Parent = this;
        }
    }
}
