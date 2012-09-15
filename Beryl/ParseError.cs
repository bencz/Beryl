using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beryl
{
    public class ParserError : BerylError
    {
        public ParserError(Position position, string message) :
            base(position, message)
        {
        }
    }
}
