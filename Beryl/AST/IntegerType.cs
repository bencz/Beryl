using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beryl.AST
{
    public class IntegerType: Type
    {
        public IntegerType(Position position):
            base(position, TypeKind.Integer)
        {
        }

        public override void DumpFields(Indenter stream)
        {
            base.DumpFields(stream);
        }

        public override void Encode(System.Text.StringBuilder result)
        {
            result.Append('i');
        }

        public override void visit(Visitor that)
        {
            that.visit(this);
        }
    }
}
