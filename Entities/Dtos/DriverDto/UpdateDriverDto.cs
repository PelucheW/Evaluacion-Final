using System.ComponentModel.DataAnnotations;

namespace ProyectoFinalTecWeb.Entities.Dtos.DriverDto
{
    public class UpdateDriverDto
    {
        public string Name { get; set; } = default!;
        public string Licence { get; set; } = default!;
        public string Phone { get; set; }
        public string Email { get; set; } = string.Empty;
    }
}
