using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beryl.AST
{
    public class IntegerType: Type
    {
        public override void visit(Visitor that)
        {
            that.visit(this);
        }
    }
}
