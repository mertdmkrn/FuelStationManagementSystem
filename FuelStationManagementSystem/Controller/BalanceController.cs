using FluentValidation;
using FuelStationManagementSystem.Helpers;
using FuelStationManagementSystem.Model;
using FuelStationManagementSystem.Models;
using FuelStationManagementSystem.Repository.Abstract;
using FuelStationManagementSystem.Validators;
using Microsoft.AspNetCore.Mvc;

namespace FuelStationManagementSystem.Controller
{
    [ApiController]
    [Route("balance")]
    public class BalanceController : ControllerBase
    {
        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<Balance> _balanceRepository;


        public BalanceController(IRepository<Customer> customerRepository, IRepository<Balance> balanceRepository)
        {
            _customerRepository = customerRepository;
            _balanceRepository = balanceRepository;
        }

        /// <summary>
        /// Get All Vehicles
        /// </summary>
        /// <returns></returns>
        [HttpGet("getall")]
        public async Task<ActionResult> GetAll()
        {
            ResponseModel<IEnumerable<Balance>> response = new ResponseModel<IEnumerable<Balance>>();

            response.Data = await _balanceRepository.GetAllAsync();
            response.Message = "Kay�tlar getirildi.";

            return Ok(response);
        }

        /// <summary>
        /// Get Balances By TCKN
        /// </summary>
        /// <returns></returns>
        [HttpGet("get/{tckn}")]
        public async Task<ActionResult> Get(string tckn)
        {
            ResponseModel<IEnumerable<Balance>> response = new ResponseModel<IEnumerable<Balance>>();

            response.Data = await _balanceRepository.GetByConditionsAsync(x => x.CustomerTCKN.Equals(tckn));
            response.Message = "Kay�tlar getirildi.";

            return Ok(response);
        }


        /// <summary>
        /// Save Balance
        /// </summary>
        /// <remarks>
        /// **Sample request body:**
        ///
        ///     { 
        ///        "customerTckn" : "25871117736",
        ///        "amount" : 1000,
        ///        "balanceType": 0
        ///     }
        ///
        /// </remarks>
        /// <returns></returns>
        [HttpPost("save")]
        public async Task<ActionResult> Save([FromBody]Balance balance)
        {
            ResponseModel<Balance> response = new ResponseModel<Balance>();

            var balanceValidator = new BalanceValidator();
            var validationResult = balanceValidator.Validate(balance);

            if (!validationResult.IsValid && validationResult.Errors.Any())
            {
                response.HasError = true;
                validationResult.Errors.ForEach(x => response.ValidationErrors.Add(new ValidationError(x.PropertyName, x.ErrorMessage)));
                response.Message = "Bakiye kayd� yap�lamad�.";
                return BadRequest(response);
            }

            var customer = await _customerRepository.GetByIdAsync(balance.CustomerTCKN);

            if (customer == null)
            {
                response.HasError = true;
                response.Message = "M��teri bulunamad�.";
                return NotFound(response);
            }

            if(customer.Status != CustomerStatus.Onaylandi)
            {
                response.HasError = true;
                response.Message = "M��teri kayd� onaylanmad��� i�in bakiye tan�mlanamad�.";
                return Ok(response);
            }

            await _balanceRepository.AddAsync(balance);
            response.Data = balance;
            response.Message = "Bakiye kayd� ba�ar�l� bir �ekilde eklendi.";

            return Ok(response);
        }

        /// <summary>
        /// Delete Balance
        /// </summary>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            ResponseModel<bool> response = new ResponseModel<bool>();

            var balance = await _balanceRepository.GetByIdAsync(id);

            if (balance == null)
            {
                response.HasError = true;
                response.Message = "Bakiye kayd� bulunamad�.";
                return NotFound(response);
            }

            await _balanceRepository.DeleteAsync(balance);
            response.Data = true;
            response.Message = "Bakiye kayd� bir �ekilde silindi.";

            return Ok(response);
        }
    }
}
