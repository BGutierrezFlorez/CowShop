using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CowShop.Models
{
    public class Membresia
    {
        public int ID_Membresia { get; set; }
        public string Nombre_Membresia { get; set; }
        public int Valor_Membresia { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public bool Estado { get; set; }

        // Constructor para valores por defecto
        public Membresia()
        {
            FechaCreacion = DateTime.Now;
            FechaModificacion = DateTime.Now;
            Estado = true;
        }
    }
}