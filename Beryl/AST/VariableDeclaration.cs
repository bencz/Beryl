using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beryl.AST
{
    public class VariableDeclaration: Declaration
    {
        public VariableDeclaration(Position position, string name, AST.Type type):
            base(position, name, SymbolKind.Variable, type)
        {
        }

        public override void visit(Visitor that)
        {
            that.visit(this);
        }
    }
}

