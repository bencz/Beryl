using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beryl.AST
{
    public enum OperatorKind
    {
        Addition,       // +
        Subtraction,    // -
        Multiplication, // *
        Division,       // /
        LessThan,       // <
        GreaterThan,    // >
        Equality,       // =
        Negation        // \
    }
}
