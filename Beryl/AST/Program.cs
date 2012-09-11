using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beryl.AST
{
    public class Program: Node
    {
        private Commands _command;

        public Program(Commands command)
        {
            _command = command;
            _command.Parent = this;
        }
    }
}
