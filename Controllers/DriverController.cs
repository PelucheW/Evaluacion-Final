using Microsoft.AspNetCore.Mvc;
using ProyectoFinalTecWeb.Entities;
using ProyectoFinalTecWeb.Entities.Dtos.DriverDto;
using ProyectoFinalTecWeb.Services;

namespace ProyectoFinalTecWeb.Controllers
{
        [ApiController]
        [Route("api/driver")]
        public class DriverController : ControllerBase
        {
            private readonly IDriverService _service;
            private readonly ITripService _trips;
            public DriverController(IDriverService service, ITripService trips)
            {
                _service = service;
                _trips = trips;
            }

            // GET: api/driver
            [HttpGet]
            public async Task<IActionResult> GetAllDriveres()
            {
                IEnumerable<Driver> items = await _service.GetAll();
                return Ok(items);
            }

            // GET: api/driver/{id}
            [HttpGet("{id:guid}")]
            public async Task<IActionResult> GetOne(Guid id)
            {
                var driver = await _service.GetOne(id);
                return Ok(driver);
            }

            // POST: api/driver
            [HttpPost]
            public async Task<IActionResult> Create([FromBody] CreateDriverDto dto)
            {
                var id = await _service.CreateAsync(dto);
                return Created($"api/driver/{id}", new { id });
            }

            // PUT: api/driver/{id}
            [HttpPut("{id:guid}")]
            public async Task<IActionResult> UpdateDriver([FromBody] UpdateDriverDto dto, Guid id)
            {
                if (!ModelState.IsValid) return ValidationProblem(ModelState);
                var driver = await _service.UpdateDriver(dto, id);
                return CreatedAtAction(nameof(GetOne), new { id = driver.Id }, driver);
            }

            // DELETE: api/driver/{id}
            [HttpDelete("{id:guid}")]
            public async Task<IActionResult> DeleteDriver(Guid id)
            {
                if (!ModelState.IsValid) return ValidationProblem(ModelState);
                await _service.DeleteDriver(id);
                return NoContent();
            }


            /*
            [HttpPost("register")]
            public async Task<IActionResult> Register([FromBody] RegisterDriverDto dto)
            {
                var id = await _service.RegisterAsync(dto);
                return CreatedAtAction(nameof(Register), new { id }, null);
            }
            */

        }
}
