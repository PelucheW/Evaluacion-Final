
using ProyectoFinalTecWeb.Entities;
using ProyectoFinalTecWeb.Entities.Dtos.DriverDto;
using ProyectoFinalTecWeb.Repositories;

namespace ProyectoFinalTecWeb.Services
{
    public class DriverService : IDriverService
    {
        private readonly IDriverRepository _drivers;
        private readonly IConfiguration _configuration;

        public DriverService(IDriverRepository drivers, IConfiguration configuration)
        {
            _drivers = drivers;
            _configuration = configuration;
        }


        public async Task<Guid> CreateAsync(CreateDriverDto dto)
        {
            var entity = new Driver { Name = dto.Name, Licence = dto.Licence, Phone = dto.Phone, Email = dto.Email };
            await _drivers.AddAsync(entity);
            await _drivers.SaveChangesAsync();
            return entity.Id;
        }

        public async Task DeleteDriver(Guid id)
        {
            Driver? conductor = (await GetAll()).FirstOrDefault(h => h.Id == id);
            if (conductor == null) return;
            await _drivers.Delete(conductor);
        }

        public async Task<IEnumerable<Driver>> GetAll()
        {
            return await _drivers.GetAll();
        }

        public async Task<Driver> GetOne(Guid id)
        {
            return await _drivers.GetOne(id);
        }

        
        public async Task<string> RegisterAsync(RegisterDriverDto dto)
        {
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            var conductor = new Driver
            {
                Email = dto.Email,
                Name = dto.Name
            };
            await _drivers.AddAsync(conductor);
            return conductor.Id.ToString();
        }
        
        public async Task<Driver> UpdateDriver(UpdateDriverDto dto, Guid id)
        {
            Driver? conductor = await GetOne(id);
            if (conductor == null) throw new Exception("Driver doesnt exist.");

            conductor.Name = dto.Name;
            conductor.Licence = dto.Licence;
            conductor.Phone = dto.Phone;
            conductor.Email = dto.Email;

            await _drivers.Update(conductor);
            return conductor;
        }
    }
}
