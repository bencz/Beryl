using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beryl.AST
{
    public class BinaryExpression: Expression
    {
        private Expression _first;
        private Operator _operator;
        private Expression _other;

        public BinaryExpression(Expression first, Operator @operator, Expression other)
        {
            _first = first;
            _first.Parent = this;
            _operator = @operator;
            _other = other;
            _other.Parent = this;
        }
    }
}
