using System;
using System.Collections.Generic;

namespace SistemaGestionGimnasio.Models
{
    public partial class Membresium
    {
        public int Id { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public string TipoMembresia { get; set; }
        public decimal Precio { get; set; }
        public string ClienteCedula { get; set; }

        public virtual Cliente ClienteCedulaNavigation { get; set; }
    }
}
