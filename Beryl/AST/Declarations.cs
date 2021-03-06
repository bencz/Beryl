using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beryl.AST
{
    public abstract class Declarations: Node
    {
        private Declaration[] _declarations;
        public Declaration[] DeclarationsArray
        {
            get { return _declarations; }
        }

        public Declarations(Position position, Declaration[] declarations):
            base(position)
        {
            _declarations = declarations;
            foreach (Declaration declaration in _declarations)
                declaration.Parent = this;
        }

        public override void DumpFields(Indenter stream)
        {
            foreach (Declaration declaration in _declarations)
                stream.WriteLine("Declaration = {0,4:D4}", declaration.Id);
        }

        public override void visit(Visitor that)
        {
            that.visit(this);
        }
    }
}
