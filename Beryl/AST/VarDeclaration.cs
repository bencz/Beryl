using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beryl.AST
{
    public class VarDeclaration: Declaration
    {
        private string _identifier;
        private string _type;

        public VarDeclaration(string identifier, string type)
        {
            _identifier = identifier;
            _type = type;
        }
    }
}
