using System.ComponentModel.DataAnnotations;

namespace ProyectoFinalTecWeb.Entities.Dtos.TripDto
{
    public class UpdateTripDto
    {
        public string Origin { get; set; }
        public string Destiny { get; set; }
        public int Price { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
