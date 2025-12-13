using ProyectoFinalTecWeb.Entities;
using ProyectoFinalTecWeb.Entities.Dtos.VehicleDto;
using ProyectoFinalTecWeb.Repositories;

namespace ProyectoFinalTecWeb.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly IVehicleRepository _vehicles;

        public VehicleService(IVehicleRepository vehicles)
        {
            _vehicles = vehicles;
        }
        public async Task<Guid> CreateAsync(CreateVehicleDto dto)
        {
            var entity = new Vehicle { Plate = dto.Plate };
            await _vehicles.AddAsync(entity);
            await _vehicles.SaveChangesAsync();
            return entity.Id;
        }

        public async Task DeleteAsync(Guid id)
        {
            Vehicle? vehicle = (await GetAll()).FirstOrDefault(h => h.Id == id);
            if (vehicle == null) return;
            await _vehicles.Delete(vehicle);
        }

        public async Task<IEnumerable<Vehicle>> GetAll()
        {
            return await _vehicles.GetAll();
        }

        public async Task<Vehicle> GetByIdAsync(Guid id)
        {
            return await _vehicles.GetByIdAsync(id);
        }

        public async Task<Vehicle> UpdateAsync(UpdateVehicleDto dto, Guid id)
        {
            Vehicle? vehicle = await GetByIdAsync(id);
            if (vehicle == null) throw new Exception("Vehicle doesnt exist.");

            vehicle.Plate = dto.Plate;

            await _vehicles.Update(vehicle);
            return vehicle;
        }
    }
}
