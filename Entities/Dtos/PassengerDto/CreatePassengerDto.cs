using System.ComponentModel.DataAnnotations;

namespace ProyectoFinalTecWeb.Entities.Dtos.PassengerDto
{
    public class CreatePassengerDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Role { get; set; } = "Passenger";
    }
}
