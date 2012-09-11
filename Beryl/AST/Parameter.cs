using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beryl.AST
{
    public class Parameter: Node
    {
        private string _name;
        private string _type;

        public Parameter(string name, string type)
        {
            _name = name;
            _type = type;
        }
    }
}
