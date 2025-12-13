using Microsoft.AspNetCore.Mvc;
using ProyectoFinalTecWeb.Entities;
using ProyectoFinalTecWeb.Entities.Dtos.PassengerDto;
using ProyectoFinalTecWeb.Services;

namespace ProyectoFinalTecWeb.Controllers
{
    [ApiController]
    [Route("api/passenger")]
    public class PassengerController : ControllerBase
    {
        private readonly IPassengerService _service;
        private readonly ITripService _trips;
        public PassengerController(IPassengerService service, ITripService trips)
        {
            _service = service;
            _trips = trips;
        }

        // GET: api/passenger
        [HttpGet]
        public async Task<IActionResult> GetAllPassengers()
        {
            IEnumerable<Passenger> items = await _service.GetAll();
            return Ok(items);
        }

        // GET: api/passenger/{id}
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetOne(Guid id)
        {
            var passenger = await _service.GetOne(id);
            return Ok(passenger);
        }

        // POST: api/passenger
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePassengerDto dto)
        {
            var id = await _service.CreateAsync(dto);
            return Created($"api/passenger/{id}", new { id });
        }

        // PUT: api/passenger/{id}
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdatePassenger([FromBody] UpdatePassengerDto dto, Guid id)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            var passenger = await _service.UpdatePassenger(dto, id);
            return CreatedAtAction(nameof(GetOne), new { id = passenger.Id }, passenger);
        }

        // DELETE: api/passenger/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeletePassenger(Guid id)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            await _service.DeletePassenger(id);
            return NoContent();
        }


        /*
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterPassengerDto dto)
        {
            var id = await _service.RegisterAsync(dto);
            return CreatedAtAction(nameof(Register), new { id }, null);
        }
        */

    }
}
