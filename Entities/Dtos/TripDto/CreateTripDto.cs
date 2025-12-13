using System.ComponentModel.DataAnnotations;

namespace ProyectoFinalTecWeb.Entities.Dtos.TripDto
{
    public class CreateTripDto
    {
        [Required]
        public string Origin { get; set; }
        [Required]
        public string Destiny { get; set; }
        [Required]
        public int Price { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
    }
}
