using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beryl
{
    public class ScannerError: BerylError
    {
        public ScannerError(Position position, string message) :
            base(position, message)
        {
        }
    }
}
