using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beryl.AST
{
    public class ParameterDeclaration: Declaration
    {
        public ParameterDeclaration(Position position, string name, AST.Type type):
            base(position, name, SymbolKind.Parameter, type)
        {
        }

        public override void DumpFields(Indenter stream)
        {
            base.DumpFields(stream);
        }

        public override void Encode(System.Text.StringBuilder result)
        {
            this.Type.Encode(result);
        }

        public override void visit(Visitor that)
        {
            that.visit(this);
        }
    }
}

