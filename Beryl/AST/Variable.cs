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

        public Variable(Position position, string name):
            base(position)
        {
            _name = name;
        }

        public override int Evaluate(SymbolTable symbols)
        {
            Symbol symbol = symbols.Lookup(_name);
            if (symbol == null)
                throw new CheckerError(this.Position, "Unknown constant '" + _name + "' referenced in constant initializer");
            return symbol.Value;
        }

        public override void visit(Visitor that)
        {
            that.visit(this);
        }
    }
}

