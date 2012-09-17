using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beryl.AST
{
    public class BooleanExpression: Expression
    {
        private bool _value;
        public bool Value
        {
            get { return _value; }
        }

        public BooleanExpression(Position position, bool value):
            base(position)
        {
            _value = value;
        }

        public override void DumpFields(Indenter stream)
        {
            stream.WriteLine("Value = {0}", _value ? "true" : "false");
        }

        public override int Evaluate(SymbolTable symbols)
        {
            throw new CheckerError(this.Position, "Cannot initialize booleans just yet...");
        }

        public override void visit(Visitor that)
        {
            that.visit(this);
        }
    }
}

