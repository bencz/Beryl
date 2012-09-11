using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beryl.AST
{
    public abstract class Declarations: Node
    {
        private Declaration[] _declarations;

        public Declarations(Declaration[] declarations)
        {
            _declarations = declarations;
            foreach (Declaration declaration in _declarations)
                declaration.Parent = this;
        }
    }
}
