using System;
using System.Collections.Generic;
using System.Text;

namespace MongoPie
{
    public class MongoPieException :Exception
    {
        public MongoPieException() : base()
        {

        }

        public MongoPieException(string message) : base(message)
        {

        }

        public MongoPieException(string message, Exception inner):base(message, inner)
        {

        }
    }
}
