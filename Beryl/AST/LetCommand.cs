using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beryl.AST
{
    public class LetCommand: Command
    {
        private Declaration[] _declarations;
        private Command _command;

        public LetCommand(Declaration[] declarations, Command command)
        {
            _declarations = declarations;
            foreach (Declaration declaration in _declarations)
                declaration.Parent = this;
            _command = command;
            _command.Parent = this;
        }
    }
}
