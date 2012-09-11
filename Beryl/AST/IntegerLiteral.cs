using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beryl.AST
{
    public class IntegerLiteral: Expression
    {
        private int _value;

        public IntegerLiteral(int value)
        {
            _value = value;
        }
    }
}
