using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beryl.AST
{
    public class ConstDeclaration: Declaration
    {
        private string _identifier;
        private Expression _expression;

        public ConstDeclaration(string identifier, Expression expression)
        {
            _identifier = identifier;
            _expression = expression;
            _expression.Parent = this;
        }
    }
}
