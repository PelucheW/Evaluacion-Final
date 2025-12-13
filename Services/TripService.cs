using ProyectoFinalTecWeb.Entities;
using ProyectoFinalTecWeb.Entities.Dtos.TripDto;
using ProyectoFinalTecWeb.Repositories;

namespace ProyectoFinalTecWeb.Services
{
    public class TripService : ITripService
    {
        private readonly ITripRepository _viajes;
        private readonly IDriverRepository _conductores;
        private readonly IPassengerRepository _pasajeros;

        public TripService(ITripRepository viajes, IDriverRepository conductor, IPassengerRepository pasajero)
        {
            _viajes = viajes;
            _conductores = conductor;
            _pasajeros = pasajero;
        }
        public async Task<Guid> CreateAsync(CreateTripDto dto)
        {
            var viaje = new Trip
            {
                Id = Guid.NewGuid(),
                Origin = dto.Origin,
                Destiny = dto.Destiny,
                EndDate = dto.EndDate,
                StartDate = dto.StartDate,
                Price = dto.Price
                //PassengerId = dto.PassengerId,
                //DriverId = dto.DriverId
            };


            await _viajes.AddAsync(viaje);
            await _viajes.SaveChangesAsync();
            return viaje.Id;
        }
    }
}
