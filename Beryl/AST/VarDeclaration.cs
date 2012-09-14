using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beryl.AST
{
    public class VarDeclaration: Declaration
    {
        private string _identifier;
        public string Identifier
        {
            get { return _identifier; }
        }

        private string _type;
        public string Type
        {
            get { return _type; }
        }

        public VarDeclaration(Position position, string identifier, string type):
			base(position)
        {
            _identifier = identifier;
            _type = type;
        }

        public override void visit(Visitor that)
        {
            that.visit(this);
        }
    }
}
