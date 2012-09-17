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
            /** \note The set accessor is \b only used by Checker.visit(Program) to insert the standard environment. */
            set
            {
                if (_declarations != null)
                    throw new BerylError("Cannot redefine children of LetCommand node");
                _declarations = value;
                foreach (Declaration declaration in _declarations)
                    declaration.Parent = this;
            }
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
            if (_declarations != null)
            {
                foreach (Declaration declaration in _declarations)
                    declaration.Parent = this;
            }

            _command = command;
            _command.Parent = this;
        }

        public override void DumpFields(Indenter stream)
        {
            foreach (Declaration declaration in _declarations)
                stream.WriteLine("Declaration = {0,4:D4}", declaration.Id);
            stream.WriteLine("Command = {0,4:D4}", _command.Id);
        }

        public override void visit(Visitor v)
        {
            v.visit(this);
        }
    }
}
