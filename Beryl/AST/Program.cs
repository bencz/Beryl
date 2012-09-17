using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beryl.AST
{
    public class Program: Node
    {
        private LetCommand _command;
        public LetCommand Command
        {
            get { return _command; }
        }

        public Program(Position position, LetCommand command):
            base(position)
        {
            _command = command;
            _command.Parent = this;
        }

        public override void DumpFields(Indenter stream)
        {
            stream.WriteLine("Command = {0,4:D4}", _command.Id);
        }

        public override void visit(Visitor that)
        {
            that.visit(this);
        }
    }
}
