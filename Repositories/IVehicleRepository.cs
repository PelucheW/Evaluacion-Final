using ProyectoFinalTecWeb.Entities;

namespace ProyectoFinalTecWeb.Repositories
{
    public interface IVehicleRepository
    {
        Task AddAsync(Vehicle vehicle);
        Task<Vehicle?> GetByIdAsync(Guid id);
        Task<bool> PlateExistsAsync(string plate);
        Task<int> SaveChangesAsync();
        Task<IEnumerable<Vehicle>> GetAll();
        Task Update(Vehicle vehicle);
        Task Delete(Vehicle vehicle);
    }
}
