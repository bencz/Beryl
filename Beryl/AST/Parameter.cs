using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beryl.AST
{
    public class Parameter: Node
    {
        private string _name;
        public string Name
        {
            get { return _name; }
        }

        private AST.Type _type;
        public AST.Type Type
        {
            get { return _type; }
        }

        public Parameter(Position position, string name, AST.Type type):
            base(position)
        {
            _name = name;

            _type = type;
            _type.Parent = this;
        }

        public override void visit(Visitor that)
        {
            that.visit(this);
        }
    }
}
