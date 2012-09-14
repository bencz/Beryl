using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beryl.AST
{
    public class LetCommand: Command
    {
        private Declaration[] _declarations;
        public Declaration[] Declarations
        {
            get { return _declarations; }
        }

        private Command _command;
        public Command Command
        {
            get { return _command; }
        }

        public LetCommand(Position position, Declaration[] declarations, Command command):
			base(position)
        {
            _declarations = declarations;
            foreach (Declaration declaration in _declarations)
                declaration.Parent = this;
            _command = command;
            _command.Parent = this;
        }

        public override void visit(Visitor v)
        {
            v.visit(this);
        }
    }
}
