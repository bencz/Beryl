using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beryl
{
    public class CoderError : BerylError
    {
        public CoderError(string message) :
            base(message)
        {
        }

        public CoderError(Position position, string message) :
            base(position, message)
        {
        }
    }
}

