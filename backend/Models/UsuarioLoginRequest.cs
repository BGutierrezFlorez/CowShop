using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CowShop.Models
{
    public class UsuarioLoginRequest
    {
        public string Correo { get; set; }
        public string Contrasena { get; set; }
    }
}