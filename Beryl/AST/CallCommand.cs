using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beryl.AST
{
    public class CallCommand: Command
    {
        private string _identifier;
        public string Identifier
        {
            get { return _identifier; }
        }

        private Expression[] _arguments;
        public Expression[] Arguments
        {
            get { return _arguments; }
        }

        public CallCommand(Position position, string identifier, Expression[] arguments):
            base(position)
        {
            _identifier = identifier;

            _arguments = arguments;
            foreach (Expression argument in _arguments)
                argument.Parent = this;
        }

        public override void DumpFields(Indenter stream)
        {
            stream.WriteLine("Name = {0}", _identifier);
            foreach (Expression argument in _arguments)
                stream.WriteLine("Argument = {0,4:D4}", argument.Id);
        }

        public void Encode(System.Text.StringBuilder result)
        {
            // output function or operator name
            result.Append('$');
            result.Append(Mangler.EncodeNamePart(this.Identifier));

            // output parameter types
            result.Append('$');
            foreach (Expression argument in _arguments)
            {
                switch (argument.Type.Kind)
                {
                    case TypeKind.Boolean: result.Append('b'); break;
                    case TypeKind.Integer: result.Append('i'); break;
                    case TypeKind.String : result.Append('s'); break;
                    default:
                        throw new BerylError("Unknown type kind '" + argument.Type.Kind.ToString() + "' encountered");
                }
            }

            // output terminating dollar sign ($)
            result.Append('$');
        }

        public override void visit(Visitor that)
        {
            that.visit(this);
        }
    }
}

