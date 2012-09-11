using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beryl.AST
{
    public class Variable: Expression
    {
        private string _name;

        public Variable(string name)
        {
            _name = name;
        }
    }
}
