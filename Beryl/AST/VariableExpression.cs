using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beryl.AST
{
    public class VariableExpression: Expression
    {
        private string _name;
        public string Name
        {
            get { return _name; }
        }

        public VariableExpression(Position position, string name):
            base(position)
        {
            _name = name;
        }

        public override void DumpFields(Indenter stream)
        {
            stream.WriteLine("Name = {0}", _name);
        }

        public override int Evaluate(SymbolTable symbols)
        {
            throw new BerylError(this.Position, "Constant expressions not yet implemented");

#if false
            Symbol symbol = symbols.Lookup(_name);
            if (symbol.Declaration.Kind != SymbolKind.Constant)
                throw new CheckerError(this.Position, "Function or variable '" + _name + "' referenced in constant initializer");
            return symbol.Value;
#endif
        }

        public override void visit(Visitor that)
        {
            that.visit(this);
        }
    }
}

