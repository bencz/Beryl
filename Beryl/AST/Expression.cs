using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beryl.AST
{
    public abstract class Expression: Node
    {
        /** \note This field is \b not linked into the tree; it is a shortcut without a parent. */
        private AST.Type _type;
        public AST.Type Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public Expression(Position position):
            base(position)
        {
        }

        public abstract int Evaluate(SymbolTable symbols);
    }
}
