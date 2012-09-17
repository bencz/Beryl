using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beryl.AST
{
    public class Parameter: Declaration
    {
        private string _name;
        public string Name
        {
            get { return _name; }
        }

        public Parameter(Position position, string name, AST.Type type):
            base(position, SymbolKind.Parameter, type)
        {
            _name = name;
        }

        public override void visit(Visitor that)
        {
            that.visit(this);
        }
    }
}
