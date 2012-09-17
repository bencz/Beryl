using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beryl.AST
{
    public class StringExpression: Expression
    {
        private string _value;
        public string Value
        {
            get { return _value; }
        }

        public StringExpression(Position position, string value):
            base(position)
        {
            _value = value;
        }

        public override void DumpFields(Indenter stream)
        {
            stream.WriteLine("Value = \"{0}\"", _value);
        }

        public override int Evaluate(SymbolTable symbols)
        {
            throw new CheckerError(this.Position, "Cannot use string in constant initializer expression");
        }

        public override void visit(Visitor that)
        {
            that.visit(this);
        }
    }
}
