using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beryl.AST
{
    public abstract class Type: Node
    {
        private TypeKind _kind;
        public TypeKind Kind
        {
            get { return _kind; }
        }

        public Type(Position position, TypeKind kind) :
            base(position)
        {
            _kind = kind;
        }

        public override void DumpFields(Indenter stream)
        {
            stream.WriteLine("Kind = {0}", _kind.ToString());
        }

        public abstract void Encode(System.Text.StringBuilder result);
    }
}
