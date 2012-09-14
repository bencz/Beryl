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

        private string _type;
        public string Type
        {
            get { return _type; }
        }

        public Parameter(string name, string type)
        {
            _name = name;
            _type = type;
        }

        public override void visit(Visitor that)
        {
            that.visit(this);
        }
    }
}
