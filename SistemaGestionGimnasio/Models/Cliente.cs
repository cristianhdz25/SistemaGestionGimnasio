using System;
using System.Collections.Generic;

namespace SistemaGestionGimnasio.Models
{
    public partial class Cliente
    {
        public Cliente()
        {
            Membresia = new HashSet<Membresium>();
        }

        public string ClienteCedula { get; set; } = null!;
        public string? Nombre { get; set; }
        public string? PrimerApellido { get; set; }
        public string? SegundoApellido { get; set; }
        public string? Teléfono { get; set; }
        public bool Activo { get; set; }
        public bool Eliminado { get; set; }

        public virtual ICollection<Membresium> Membresia { get; set; }
    }
}
