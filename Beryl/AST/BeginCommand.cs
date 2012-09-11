using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beryl.AST
{
    public class BeginCommand: Command
    {
        private Commands _command;

        public BeginCommand(Commands command)
        {
            _command = command;
            _command.Parent = this;
        }
    }
}
