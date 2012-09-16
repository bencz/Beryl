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

        public override void visit(Visitor that)
        {
            that.visit(this);
        }
    }
}

