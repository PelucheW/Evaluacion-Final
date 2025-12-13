using System.ComponentModel.DataAnnotations;

namespace ProyectoFinalTecWeb.Entities.Dtos.VehicleDto
{
    public class CreateVehicleDto
    {
        [Required]
        public string Plate { get; set; }
    }
}
