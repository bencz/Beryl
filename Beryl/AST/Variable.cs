using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beryl.AST
{
    public class Variable: Expression
    {
        private string _name;
        public string Name
        {
            get { return _name; }
        }

        public Variable(string name)
        {
            _name = name;
        }

        public override int Evaluate(SymbolTable symbols)
        {
            AST.Type type = symbols.Lookup(new Position(), _name);

        }

        public override void visit(Visitor that)
        {
            that.visit(this);
        }
    }
}
