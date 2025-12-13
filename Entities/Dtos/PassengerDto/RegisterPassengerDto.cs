namespace ProyectoFinalTecWeb.Entities.Dtos.PassengerDto
{
    public class RegisterPassengerDto
    {
        public string Name { get; init; }
        public string Email { get; init; }
        public string Password { get; init; }
        public string Role { get; set; } = "Passenger";
    }
}
