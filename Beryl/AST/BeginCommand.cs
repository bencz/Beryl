using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beryl.AST
{
    public class BeginCommand: Command
    {
        private Commands _commands;
        public Commands Commands
        {
            get { return _commands; }
        }

        public BeginCommand(Commands commands)
        {
            _commands = commands;
            _commands.Parent = this;
        }

        public override void visit(Visitor that)
        {
            that.visit(this);
        }
    }
}
