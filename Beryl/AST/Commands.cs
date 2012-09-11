using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beryl.AST
{
    public class Commands: Command
    {
        Command[] _commands;

        public Commands(Command[] commands)
        {
            _commands = commands;
            foreach (Command command in _commands)
                command.Parent = this;
        }
    }
}
