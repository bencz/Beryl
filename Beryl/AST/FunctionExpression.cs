using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beryl.AST
{
    public class FunctionExpression: Expression
    {
        private string _name;
        public string Name
        {
            get { return _name; }
        }

        private Expression[] _arguments;
        public Expression[] Arguments
        {
            get { return _arguments; }
        }

        public FunctionExpression(Position position, string name, Expression[] arguments):
            base(position)
        {
            _name = name;
            _arguments = arguments;
            foreach (Expression argument in _arguments)
                argument.Parent = this;
        }

        public override void DumpFields(Indenter stream)
        {
            stream.WriteLine("Name = {0}", _name);
            foreach (Expression argument in _arguments)
                stream.WriteLine("Argument = {0,4:D4}", argument.Id);
        }

        public void Encode(System.Text.StringBuilder result)
        {
            // output function or operator name
            result.Append('$');
            result.Append(Mangler.EncodeNamePart(this.Name));

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

        public override int Evaluate(SymbolTable symbols)
        {
            throw new CheckerError(this.Position, "Functions cannot be part of a constant expression: " + _name);
        }

        public override void visit(Visitor that)
        {
            that.visit(this);
        }
    }
}

