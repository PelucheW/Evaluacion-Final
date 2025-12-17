using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoFinalTecWeb.Entities;
using ProyectoFinalTecWeb.Entities.Dtos.VehicleDto;
using ProyectoFinalTecWeb.Repositories;
using ProyectoFinalTecWeb.Services;
using System.Security.Cryptography;

namespace ProyectoFinalTecWeb.Controllers
{
    [ApiController]
    [Route("api/vehicle")]
    public class VehicleController : ControllerBase
    {
        private readonly IVehicleService _service;
        private readonly IDriverService _drivers;
        private readonly IVehicleRepository _vehicles;
        private readonly IDriverVehicleService _driverVehicleService;

        public VehicleController(IVehicleService service, IDriverService drivers, IVehicleRepository vehicles, IDriverVehicleService driverVehicleService)
        {
            _service = service;
            _drivers = drivers;
            _vehicles = vehicles;
            _driverVehicleService = driverVehicleService;
        }

        // POST: api/vehicle
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateVehicleDto dto)
        {
            var id = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id }, new { id });
        }

        // GET: api/vehicle
        [HttpGet]
        [Authorize(Policy = "DriverOnly")]
        public async Task<IActionResult> GetAllVehicles()
        {
            IEnumerable<VehicleDto> items = await _service.GetAll();
            return Ok(items);
        }

        // GET: api/vehicle/{id}
        [HttpGet("{id:guid}")]
        [Authorize(Policy = "DriverOnly")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var data = await _service.GetByIdAsync(id);
            if (data == null) return NotFound();
            return Ok(data);
        }

        // PUT: api/vehicle/{id}
        [HttpPut("{id:guid}")]
        [Authorize(Policy = "DriverOnly")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateVehicleDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            var vehicle = await _service.UpdateAsync(dto, id);
            return CreatedAtAction(nameof(GetById), new { id = vehicle.Id }, vehicle);
        }

        // DELETE: api/vehicle/{id}
        [HttpDelete("{id:guid}")]
        [Authorize(Policy = "DriverOnly")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            await _service.DeleteAsync(id);
            return NoContent();
        }

        // POST: api/vehicle/{vehicleId}/drivers/{driverId}
        [HttpPost("{vehicleId:guid}/drivers/{driverId:guid}")]
        [Authorize(Policy = "DriverOnly")]
        public async Task<IActionResult> AssignDriver(Guid vehicleId, Guid driverId)
        {
            try
            {
                await _driverVehicleService.AssignDriverToVehicle(vehicleId, driverId);
                return Ok(new { message = "Driver assigned to vehicle successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // DELETE: api/vehicle/{vehicleId}/drivers/{driverId}
        [HttpDelete("{vehicleId:guid}/drivers/{driverId:guid}")]
        [Authorize(Policy = "DriverOnly")]
        public async Task<IActionResult> RemoveDriver(Guid vehicleId, Guid driverId)
        {
            try
            {
                await _driverVehicleService.RemoveDriverFromVehicle(vehicleId, driverId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // GET: api/vehicle/{vehicleId}/drivers
        [HttpGet("{vehicleId:guid}/drivers")]
        public async Task<IActionResult> GetDriversByVehicle(Guid vehicleId)
        {
            var drivers = await _driverVehicleService.GetDriversByVehicle(vehicleId);
            return Ok(drivers);
        }

    }
    }
