
using FuelStationManagementSystem.Model;
using FuelStationManagementSystem.Models;
using FuelStationManagementSystem.Repository.Abstract;
using FuelStationManagementSystem.Service.Abstract;
using FuelStationManagementSystem.Validators;
using Microsoft.AspNetCore.Mvc;

namespace FuelStationManagementSystem.Controller
{
    [ApiController]
    [Route("fueltransaction")]
    public class FuelTransactionController : ControllerBase
    {
        private readonly IRepository<Vehicle> _vehicleRepository;
        private readonly IRepository<Balance> _balanceRepository;
        private readonly IRepository<FuelTransaction> _fuelTransactionRepository;
        private readonly IBalanceInquiryService _balanceInquiryService;


        public FuelTransactionController(IRepository<Vehicle> vehicleRepository, IRepository<Balance> balanceRepository, IRepository<FuelTransaction> fuelTransactionRepository, IBalanceInquiryService balanceInquiryService)
        {
            _vehicleRepository = vehicleRepository;
            _balanceRepository = balanceRepository;
            _fuelTransactionRepository = fuelTransactionRepository;
            _balanceInquiryService = balanceInquiryService;
        }

        /// <summary>
        /// Get All Vehicles
        /// </summary>
        /// <returns></returns>
        [HttpGet("getall")]
        public async Task<ActionResult> GetAll()
        {
            ResponseModel<IEnumerable<FuelTransaction>> response = new ResponseModel<IEnumerable<FuelTransaction>>();

            response.Data = await _fuelTransactionRepository.GetAllAsync();
            response.Message = "Kayýtlar getirildi.";

            return Ok(response);
        }

        /// <summary>
        /// Get Fuel Transactions By Plate
        /// </summary>
        /// <returns></returns>
        [HttpGet("get/{plate}")]
        public async Task<ActionResult> Get(string plate)
        {
            ResponseModel<IEnumerable<FuelTransaction>> response = new ResponseModel<IEnumerable<FuelTransaction>>();

            response.Data = await _fuelTransactionRepository.GetByConditionsAsync(x => x.VehiclePlate.Equals(plate));
            response.Message = "Kayýtlar getirildi.";

            return Ok(response);
        }


        /// <summary>
        /// Save Fuel Transaction
        /// </summary>
        /// <remarks>
        /// **Sample request body:**
        ///
        ///     { 
        ///        "vehiclePlate" : "37 KSK 3737",
        ///        "amount" : 1000
        ///     }
        ///
        /// </remarks>
        /// <returns></returns>
        [HttpPost("save")]
        public async Task<ActionResult> Save([FromBody] FuelTransaction fuelTransaction)
        {
            ResponseModel<FuelTransaction> response = new ResponseModel<FuelTransaction>();
            fuelTransaction.TransactionDate = DateTime.UtcNow;

            var fuelTransactionValidator = new FuelTransactionValidator();
            var validationResult = fuelTransactionValidator.Validate(fuelTransaction);

            if (!validationResult.IsValid && validationResult.Errors.Any())
            {
                response.HasError = true;
                validationResult.Errors.ForEach(x => response.ValidationErrors.Add(new ValidationError(x.PropertyName, x.ErrorMessage)));
                response.Message = "Ýþlem kaydý yapýlamadý.";
                return BadRequest(response);
            }

            var vehicle = await _vehicleRepository.GetByIdAsync(fuelTransaction.VehiclePlate);

            if (vehicle == null)
            {
                response.HasError = true;
                response.Message = "Araç bulunamadý.";
                return NotFound(response);
            }

            bool isEnoughBalance = await _balanceInquiryService.CheckBalance(vehicle.CustomerTCKN, fuelTransaction.Amount);

            if (!isEnoughBalance)
            {
                response.HasError = true;
                response.Message = $"Bakiye yetersiz.";
                return Ok(response);
            }

            await _fuelTransactionRepository.AddAsync(fuelTransaction);
            response.Data = fuelTransaction;
            response.Message = "Ýþlem kaydý baþarýlý bir þekilde eklendi.";

            return Ok(response);
        }
    }
}
