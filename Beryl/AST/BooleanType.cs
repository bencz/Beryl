using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beryl.AST
{
    public class BooleanType: Type
    {
        public BooleanType(Position position):
            base(position, TypeKind.Boolean)
        {
        }

        public override void DumpFields(Indenter stream)
        {
            base.DumpFields(stream);
        }

        public override void visit(Visitor that)
        {
            that.visit(this);
        }
    }
}

