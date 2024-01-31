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

        /// <summary>
        /// Get All Customers
        /// </summary>
        /// <returns></returns>
        [HttpGet("getall")]
        public async Task<ActionResult> GetAll()
        {
            ResponseModel<IEnumerable<Customer>> response = new ResponseModel<IEnumerable<Customer>>();

            response.Data = await _customerRepository.GetAllAsync();
            response.Message = "Kayýtlar getirildi.";

            return Ok(response);
        }

        /// <summary>
        /// Get Customer By TCKN
        /// </summary>
        /// <returns></returns>
        [HttpGet("get/{tckn}")]
        public async Task<ActionResult> Get(string tckn)
        {
            ResponseModel<Customer> response = new ResponseModel<Customer>();

            response.Data = await _customerRepository.GetByIdAsync(tckn);
            response.Message = "Kayýt getirildi.";

            return Ok(response);
        }


        /// <summary>
        /// Save Customer
        /// </summary>
        /// <remarks>
        /// **Sample request body:**
        ///
        ///     { 
        ///        "tckn" : "25871117736",
        ///        "namesurname" : "mertdmkrn37@gmail.com",
        ///        "address": "Ýstanbul/Kaðýthane"
        ///     }
        ///
        /// </remarks>
        /// <returns></returns>
        [HttpPost("save")]
        public async Task<ActionResult> Save([FromBody]Customer customer)
        {
            ResponseModel<Customer> response = new ResponseModel<Customer>();
            customer.Status = CustomerStatus.OnayBekliyor;

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

            return Ok(response);
        }


        /// <summary>
        /// Update Customer
        /// </summary>
        /// <remarks>
        /// **Sample request body:**
        ///
        ///     { 
        ///        "tckn" : "25871117736",
        ///        "status": 1
        ///     }
        ///
        /// </remarks>
        /// <returns></returns>
        [HttpPut("update")]
        public async Task<ActionResult> Update([FromBody] Customer updateCustomer)
        {
            ResponseModel<Customer> response = new ResponseModel<Customer>();

            if(!Extensions.BeAValidStatus(updateCustomer.Status))
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("Status", "Geçersiz müþteri durumu."));
                response.Message = "Müþteri silinemedi.";
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
            customer.Address = updateCustomer.Address.IsNull(customer.Address);
            customer.Status = updateCustomer.Status;

            await _customerRepository.UpdateAsync(customer);
            response.Data = customer;
            response.Message = "Müþteri baþarýlý bir þekilde güncellendi.";

            return Ok(response);
        }

        /// <summary>
        /// Delete Customer
        /// </summary>
        /// <returns></returns>
        [HttpDelete("delete/{tckn}")]
        public async Task<ActionResult> Delete(string tckn)
        {
            ResponseModel<bool> response = new ResponseModel<bool>();

            if (tckn.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("tckn", "TC Kimlik numarasý girilmeli."));
                response.Message = "Müþteri güncellenemedi.";
                return BadRequest(response);
            }

            var customer = await _customerRepository.GetByIdAsync(tckn);

            if (customer == null)
            {
                response.HasError = true;
                response.Message = "Müþteri bulunamadý.";
                return NotFound(response);
            }

            await _customerRepository.DeleteAsync(customer);
            response.Data = true;
            response.Message = "Müþteri baþarýlý bir þekilde silindi.";

            return Ok(response);
        }
    }
}
