using Microsoft.AspNetCore.Mvc;
using ProyectoFinalTecWeb.Entities.Dtos.TripDto;
using ProyectoFinalTecWeb.Services;

namespace ProyectoFinalTecWeb.Controllers
{
    [ApiController]
    [Route("api/trip")]
    public class TripController : ControllerBase
    {
        private readonly ITripService _service;
        private readonly IPassengerService _passenger;
        private readonly IDriverService _driver;

        public TripController(ITripService service, IPassengerService passenger, IDriverService driver)
        {
            _service = service;
            _passenger = passenger;
            _driver = driver;
        }

        // POST: api/trip
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTripDto dto)
        {
            var id = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetPassenger), new { id }, new { id });
        }

        // GET: api/trip/{id}/passenger
        [HttpGet("{id:Guid}/passenger")]
        public async Task<IActionResult> GetPassenger([FromRoute] Guid id)
        {
            var data = await _passenger.GetOne(id);
            if (data == null) return NotFound();
            return Ok(data);
        }

        // GET: api/trip/{id}/driver
        [HttpGet("{id:Guid}/driver")]
        public async Task<IActionResult> GetDriver([FromRoute] Guid id)
        {
            var data = await _driver.GetOne(id);
            if (data == null) return NotFound();
            return Ok(data);
        }



    }
}
