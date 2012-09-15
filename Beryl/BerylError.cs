using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beryl
{
    public class BerylError : System.Exception
    {
        private Position _position;
        public Position Position
        {
            get { return _position; }
        }

        public BerylError(string message) :
            base(message)
        {
            _position = null;
        }

        public BerylError(Position position, string message) :
            base(message)
        {
            _position = new Position(position);    // make DEEP copy to avoid nasty side effects
        }
    }
}

