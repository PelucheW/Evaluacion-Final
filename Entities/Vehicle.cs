namespace ProyectoFinalTecWeb.Entities
{

    public class Vehicle
    {
        public Guid Id { get; set; }
        public string Plate { get; set; }

        // N:M vechicle -> driver
        public Guid DriverId { get; set; }
        public Driver Driver { get; set; } = default!;

        // 1:1 vehicle-> model
        public Guid ModelId { get; set; }
        public Model Model { get; set; } = default!;

    }
}
