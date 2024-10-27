using Microsoft.AspNetCore.Mvc;
using fleetpanda.dataaccess.Repositories.Abstractions;
using fleetpanda.common;
using fleetpanda.dataaccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace fleetpanda.webui.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TargetCustomersController(ICustomerRepository repository) : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository = repository;

        [HttpGet]
        public async Task<ActionResult<List<Customer>>> GetCustomersAtTarget()
        {
            List<Customer> data = [];
            Response response = await _customerRepository.GetCustomersAtTargetAsync();
            if (response.Success)
                data = response.Data as List<Customer>;
            if (data is null)
                return NoContent();
            return Ok(data);
        }

        [HttpPost]
        public async Task<ActionResult> SyncTables()
        {
            var resp = await _customerRepository.SyncCustomersToTargetAsync();
            if (resp.Success) return Ok("Success");
            return BadRequest("Something went wrong");
            
        }

        [HttpGet("locations")]

        public async Task<ActionResult> GetCustomerLocations()
        {
            List<Location> data = [];
            Response response = await _customerRepository.GetCustomerLocationsAsync();
            if (response.Success)
                data = response.Data as List<Location>;
            if (data is null)
                return NoContent();
            return Ok(data);
        }

   }
}
