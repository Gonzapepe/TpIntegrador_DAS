
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOM
{
    public class Rol
    {
        public int ID { get; set; }
        public string Detalle { get; set; }
        public override string ToString()
        {
            return Detalle;
        }
    }
}
