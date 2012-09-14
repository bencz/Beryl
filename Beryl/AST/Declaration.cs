using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beryl.AST
{
    public abstract class Declaration: Node
    {
		public Declaration(Position position):
			base(position)
		{
		}
    }
}
