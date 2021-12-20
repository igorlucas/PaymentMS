using Microsoft.AspNetCore.Mvc;
using PaymentMS.Domain.Contracts.Services;
using PaymentMS.Domain.Entities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PaymentMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService) => _customerService = customerService;

        // GET: api/<CustomersController>
        [HttpGet]
        public ActionResult<List<Customer>> GetCustomers() => Ok(_customerService.ReadAll());

        // GET api/<CustomersController>/5
        [HttpGet("{id}")]
        public ActionResult<Customer> GetCustomer(Guid id) => Ok(_customerService.ReadById(id));

        // POST api/<CustomersController>
        [HttpPost]
        public ActionResult<Customer> PostCustomer([FromBody] Customer customer)
        {
            customer = _customerService.Create(customer);
            return CreatedAtAction($"{nameof(GetCustomer)}", new { id = customer.Id }, customer);
        }

        // PUT api/<CustomersController>/5
        [HttpPut("{id}")]
        public IActionResult PutCustomer(Guid id, [FromBody] Customer customer)
        {
            if (id != customer.Id) return BadRequest();
            _customerService.Update(customer);
            return NoContent();
        }

        // DELETE api/<CustomersController>/5
        [HttpDelete("{id}")]    
        public IActionResult DeleteCustomer(Guid id)
        {
            var customer = _customerService.ReadById(id);
            if (customer == null) return BadRequest();
            _customerService.Delete(customer);
            return NoContent();
        }
    }
}