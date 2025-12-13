using System.ComponentModel.DataAnnotations;

namespace ProyectoFinalTecWeb.Entities.Dtos.PassengerDto
{
    public class UpdatePassengerDto
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; } = string.Empty;
    }
}
