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

        private AST.Type _type;
        public AST.Type Type
        {
            get { return _type; }
        }

        public VarDeclaration(Position position, string identifier, AST.Type type):
            base(position)
        {
            _identifier = identifier;

            _type = type;
            _type.Parent = this;
        }

        public override void visit(Visitor that)
        {
            that.visit(this);
        }
    }
}

