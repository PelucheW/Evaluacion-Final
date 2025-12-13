using ProyectoFinalTecWeb.Entities;
using ProyectoFinalTecWeb.Entities.Dtos.TripDto;

namespace ProyectoFinalTecWeb.Services
{
    public interface ITripService
    {
        Task<Guid> CreateAsync(CreateTripDto dto);

        //Task<ViajePasajeroDto?> GetPasajeroAsync(int id);
    }
}
