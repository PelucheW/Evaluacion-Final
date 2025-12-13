using ProyectoFinalTecWeb.Entities;
using ProyectoFinalTecWeb.Entities.Dtos.VehicleDto;

namespace ProyectoFinalTecWeb.Services
{
    public interface IVehicleService
    {
        Task<Guid> CreateAsync(CreateVehicleDto dto);
        Task<Vehicle> GetByIdAsync(Guid id);
        Task<Vehicle> UpdateAsync(UpdateVehicleDto dto, Guid id);
        Task DeleteAsync(Guid id);
        Task<IEnumerable<Vehicle>> GetAll();

        /*
         Task<Guid> CreateAsync(CreateConductorDto dto);
        Task<IEnumerable<Conductor>> GetAll();
        Task<Conductor> GetOne(Guid id);
        Task<Conductor> UpdateConductor(UpdateConductorDto dto, Guid id);
        Task DeleteConductor(Guid id);
         */

    }
}
