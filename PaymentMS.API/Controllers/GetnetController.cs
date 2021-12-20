using GetnetProvider.Commands;
using GetnetProvider.Commands.CommandRequests;
using GetnetProvider.Commands.CommandResponses;
using GetnetProvider.Models;
using GetnetProvider.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PaymentMS.Domain.Entities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PaymentMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetnetController : ControllerBase
    {
        private readonly IGetnetCustomerService _getnetCustomerService;
        private readonly GetnetSettings _getnetSettings;

        public GetnetController(IGetnetCustomerService getnetCustomerService, IOptions<GetnetSettings> settingsOptions)
        {
            _getnetCustomerService = getnetCustomerService;
            _getnetSettings = settingsOptions.Value;
        }

        // GET: api/getnet/customers
        [HttpGet("customers")]
        public async Task<ActionResult<ServiceCommandResponse<ReadCustomersResponse>>> GetCutomers()
        {
            var queryString = HttpContext.Request.QueryString.ToString();  
            var readCustomersResponse = await _getnetCustomerService.ReadAsync(queryString);
            return Ok(readCustomersResponse);
        }

        // GET api/getnet/customers/5
        [HttpGet("customers/{id}")]
        public async Task<IActionResult> GetCutomer(string id)
        {
            var readCustomerResponse = await _getnetCustomerService.ReadByCustomerIdAsync(id);
            return Ok(readCustomerResponse);
        }

        // POST api/getnet/cutomers
        [HttpPost("customers")]
        public async Task<IActionResult> PostCustomer([FromBody] Customer customer)
        {
            var sellerId = _getnetSettings.SellerId;
            var observation = $"Customer created at: {DateTimeOffset.Now}";
            var createCustomerRequest = new CreateCustomerRequest(sellerId, observation, customer);
            var createCustomerResponse = await _getnetCustomerService.CreateAsync(createCustomerRequest);
            return Ok(createCustomerResponse);
        }
    }
}