using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CowShop.Models
{
    public class Vaca
    {
        public int ID_Vaca { get; set; }
        public string Nombre { get; set; }
        public string Raza { get; set; }
        public int Edad { get; set; }
        public decimal Peso { get; set; }
        public decimal Precio { get; set; }
        public string Estado_Salud { get; set; }
        public int ID_Vendedor { get; set; }
       
    }
}