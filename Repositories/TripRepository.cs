using Microsoft.EntityFrameworkCore;
using ProyectoFinalTecWeb.Data;
using ProyectoFinalTecWeb.Entities;

namespace ProyectoFinalTecWeb.Repositories
{
    public class TripRepository : ITripRepository
    {
        private readonly AppDbContext _ctx;
        public TripRepository(AppDbContext ctx) => _ctx = ctx;

        public async Task AddAsync(Trip viaje) => await _ctx.Trips.AddAsync(viaje);


        public Task<bool> ExistsAsync(Guid id) =>
            _ctx.Trips.AnyAsync(s => s.Id == id);

        public Task<Trip?> GetTripAsync(Guid id) =>
            _ctx.Trips.FirstOrDefaultAsync(s => s.Id == id);

        public Task<int> SaveChangesAsync() => _ctx.SaveChangesAsync();
    }
}
