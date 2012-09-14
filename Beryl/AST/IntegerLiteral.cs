using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beryl.AST
{
    public class IntegerLiteral: Expression
    {
        private int _value;
        public int Value
        {
            get { return _value; }
        }

        public IntegerLiteral(int value)
        {
            _value = value;
        }

        public override int Evaluate(SymbolTable symbols)
        {
            return _value;
        }

        public override void visit(Visitor that)
        {
            that.visit(this);
        }
    }
}
