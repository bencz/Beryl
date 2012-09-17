using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beryl.AST
{
    public class Commands: Command
    {
        Command[] _commands;
        public Command[] CommandArray
        {
            get { return _commands; }
        }

        public Commands(Position position, Command[] commands):
            base(position)
        {
            _commands = commands;
            foreach (Command command in _commands)
                command.Parent = this;
        }

        public override void DumpFields(Indenter stream)
        {
            foreach (Command command in _commands)
                stream.WriteLine("Command = {0,4:D4}", command.Id);
        }

        public override void visit(Visitor that)
        {
            that.visit(this);
        }
    }
}
