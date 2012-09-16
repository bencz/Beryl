using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beryl.AST
{
    public class VarDeclaration: Declaration
    {
        private string _identifier;
        public string Identifier
        {
            get { return _identifier; }
        }

        public VarDeclaration(Position position, string identifier, AST.Type type):
            base(position, SymbolKind.Variable, type)
        {
            _identifier = identifier;
        }

        public override void visit(Visitor that)
        {
            that.visit(this);
        }
    }
}

