using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterficiePersistencia
{
    public class GestorBDTallerException : Exception
    {
        public GestorBDTallerException()
        {
        }

        public GestorBDTallerException(string? message) : base(message)
        {
        }
    }
}
