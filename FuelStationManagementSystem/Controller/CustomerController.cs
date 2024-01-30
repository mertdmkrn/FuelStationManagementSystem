using Azure;
using FuelStationManagementSystem.Helpers;
using FuelStationManagementSystem.Model;
using FuelStationManagementSystem.Models;
using FuelStationManagementSystem.Repository.Abstract;
using FuelStationManagementSystem.Validators;
using Microsoft.AspNetCore.Mvc;

namespace FuelStationManagementSystem.Controllers
{
    [ApiController]
    [Route("customer")]
    public class CustomerController : ControllerBase
    {
        private readonly IRepository<Customer> _customerRepository;

        public CustomerController(IRepository<Customer> customerRepository)
        {
            _customerRepository = customerRepository;
        }

        [HttpGet("getall")]
        public ActionResult GetAll()
        {
            return Ok(_customerRepository.GetAllAsync());
        }

        [HttpGet("get/{tckn}")]
        public ActionResult Get(string tckn)
        {
            return Ok(_customerRepository.GetByConditionsAsync(x => x.TCKN.Equals(tckn)));
        }

        [HttpPost("save")]
        public async Task<ActionResult> Save([FromBody]Customer customer)
        {
            ResponseModel<Customer> response = new ResponseModel<Customer>();
            var customerValidator = new CustomerValidator();
            var validationResult = customerValidator.Validate(customer);

            if (!validationResult.IsValid && validationResult.Errors.Any())
            {
                response.HasError = true;
                validationResult.Errors.ForEach(x => response.ValidationErrors.Add(new ValidationError(x.PropertyName, x.ErrorMessage)));
                response.Message = "Müþteri eklenemedi.";
                return BadRequest(response);
            }

            await _customerRepository.AddAsync(customer);
            response.Data = customer;
            response.Message = "Müþteri baþarýlý bir þekilde eklendi.";

            return Ok(customer);
        }

        [HttpPut("update")]
        public async Task<ActionResult> Update([FromBody] Customer updateCustomer)
        {
            ResponseModel<Customer> response = new ResponseModel<Customer>();

            if(!Extensions.BeAValidStatus(updateCustomer.Status))
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("Status", "Geçersiz müþteri durumu tipi."));
                response.Message = "Müþteri güncellenemedi.";
                return BadRequest(response);
            }

            var customer = await _customerRepository.GetByIdAsync(updateCustomer.TCKN);

            if(customer == null)
            {
                response.HasError = true;
                response.Message = "Müþteri bulunamadý.";
                return NotFound(response);
            }

            customer.NameSurname = updateCustomer.NameSurname.IsNull(customer.NameSurname);
            customer.Status = updateCustomer.Status;

            await _customerRepository.UpdateAsync(customer);
            response.Data = customer;
            response.Message = "Müþteri baþarýlý bir þekilde güncellendi.";

            return Ok(customer);
        }
    }
}
