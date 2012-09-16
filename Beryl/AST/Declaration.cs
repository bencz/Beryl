using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beryl.AST
{
    public abstract class Declaration: Node
    {
        private SymbolKind _kind;
        public SymbolKind Kind
        {
            get { return _kind; }
        }

        private AST.Type _type;
        public AST.Type Type
        {
            get { return _type; }
        }

        public Declaration(Position position, SymbolKind kind, AST.Type type):
            base(position)
        {
            _kind = kind;

            _type = type;
            _type.Parent = this;
        }
    }
}
