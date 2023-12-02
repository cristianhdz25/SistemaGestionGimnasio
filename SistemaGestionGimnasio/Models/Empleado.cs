using System;
using System.Collections.Generic;

namespace SistemaGestionGimnasio.Models
{
    public partial class Empleado
    {
        public string EmpleadoCedula { get; set; } = null!;
        public string? Nombre { get; set; }
        public string? PrimerApellido { get; set; }
        public string? SegundoApellido { get; set; }
        public string? Teléfono { get; set; }
        public int? Edad { get; set; }
        public bool? Eliminado { get; set; }
        public decimal? Salario { get; set; }
        public string? Horario { get; set; }
    }
}
