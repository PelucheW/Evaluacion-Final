using ProyectoFinalTecWeb.Entities;
using ProyectoFinalTecWeb.Entities.Dtos.PassengerDto;

namespace ProyectoFinalTecWeb.Services
{
    public interface IPassengerService
    {
        //Authentication
        Task<string> RegisterAsync(RegisterPassengerDto dto);

        //CRUD
        Task<Guid> CreateAsync(CreatePassengerDto dto);
        Task<IEnumerable<Passenger>> GetAll();
        Task<Passenger> GetOne(Guid id);
        Task<Passenger> UpdatePassenger(UpdatePassengerDto dto, Guid id);
        Task DeletePassenger(Guid id);
    }
}
