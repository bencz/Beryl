using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beryl.AST
{
    public class StringType: Type
    {
        public StringType(Position position):
            base(position, TypeKind.String)
        {
        }

        public override void DumpFields(Indenter stream)
        {
            base.DumpFields(stream);
        }

        public override void Encode(System.Text.StringBuilder result)
        {
            result.Append('s');
        }

        public override void visit(Visitor that)
        {
            that.visit(this);
        }
    }
}

