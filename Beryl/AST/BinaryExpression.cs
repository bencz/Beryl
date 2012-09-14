using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beryl.AST
{
    public class BinaryExpression: Expression
    {
        private Expression _first;
        public Expression First
        {
            get { return _first; }
        }

        private Operator _operator;
        public Operator Operator
        {
            get { return _operator; }
        }

        private Expression _other;
        public Expression Other
        {
            get { return _other; }
        }

        public BinaryExpression(Expression first, Operator @operator, Expression other)
        {
            _first = first;
            _first.Parent = this;
            _operator = @operator;
            _other = other;
            _other.Parent = this;
        }

        public override int Evaluate(SymbolTable symbols)
        {
            int first = _first.Evaluate(symbols);
            int other = _other.Evaluate(symbols);

            switch (_operator)
            {
                case Operator.Addition:
                    return first + other;

                case Operator.Difference:
                    return (first != other) ? 1 : 0;

                case Operator.Division:
                    if (other == 0)
                        throw new System.DivideByZeroException();
                    return first / other;

                case Operator.Equality:
                    return (first == other) ? 1 : 0;

                case Operator.GreaterThan:
                    return (first > other) ? 1 : 0;

                case Operator.LessThan:
                    return (first < other) ? 1 : 0;

                case Operator.Multiplication:
                    return first * other;

                case Operator.Subtraction:
                    return first - other;

                default:
                    throw new System.Exception("Unknown Operator value: " + _operator.ToString());
            }
        }

        public override void visit(Visitor that)
        {
            that.visit(this);
        }
    }
}
