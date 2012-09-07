using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beryl
{
    public class ScannerError: System.Exception
    {
        private Position _position;
        public Position Position
        {
            get { return _position; }
        }

        public ScannerError(Position position, string message) :
            base(message)
        {
            _position = new Position(position);    // make DEEP copy to avoid nasty side effects
        }
    }
}
