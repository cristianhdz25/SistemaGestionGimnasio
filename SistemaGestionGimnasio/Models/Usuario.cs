using System;
using System.Collections.Generic;

namespace SistemaGestionGimnasio.Models
{
    public partial class Usuario
    {
        public int Id { get; set; }
        public string NombreUsuario { get; set; }
        public string Contrasenia { get; set; }
        public int TipoUsuario { get; set; }
    }
}
