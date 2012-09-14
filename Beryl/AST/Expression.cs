using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beryl.AST
{
    public abstract class Expression: Node
    {
        public Expression(Position position):
			base(position)
        {
        }

        public abstract int Evaluate(SymbolTable symbols);
    }
}
