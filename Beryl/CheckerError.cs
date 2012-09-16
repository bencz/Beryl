using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beryl
{
    public class CheckerError : BerylError
    {
        public CheckerError(Position position, string message) :
            base(position, message)
        {
        }
    }
}

