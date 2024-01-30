using FuelStationManagementSystem.Helpers;
using FuelStationManagementSystem.Model;
using FuelStationManagementSystem.Models;
using FuelStationManagementSystem.Repository.Abstract;
using FuelStationManagementSystem.Validators;
using Microsoft.AspNetCore.Mvc;

namespace FuelStationManagementSystem.Controller
{
    [ApiController]
    [Route("vehicle")]
    public class VehicleController : ControllerBase
    {
        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<Vehicle> _vehicleRepository;

        public VehicleController(IRepository<Customer> customerRepository, IRepository<Vehicle> vehicleRepository)
        {
            _customerRepository = customerRepository;
            _vehicleRepository = vehicleRepository;
        }

        /// <summary>
        /// Get All Vehicles
        /// </summary>
        /// <returns></returns>
        [HttpGet("getall")]
        public async Task<ActionResult> GetAll()
        {
            ResponseModel<IEnumerable<Vehicle>> response = new ResponseModel<IEnumerable<Vehicle>>();

            response.Data = await _vehicleRepository.GetAllAsync();
            response.Message = "Kayýtlar getirildi.";

            return Ok(response);
        }

        /// <summary>
        /// Get Vehicle By Plate
        /// </summary>
        /// <returns></returns>
        [HttpGet("get/{plate}")]
        public async Task<ActionResult> Get(string plate)
        {
            ResponseModel<Vehicle> response = new ResponseModel<Vehicle>();

            response.Data = await _vehicleRepository.GetByIdAsync(plate);
            response.Message = "Kayýt getirildi.";

            return Ok(response);
        }


        /// <summary>
        /// Save Vehicle
        /// </summary>
        /// <remarks>
        /// **Sample request body:**
        ///
        ///     { 
        ///        "plate" : "37 KSK 3737",
        ///        "customerTckn" : "25871117736",
        ///        "vehicleType": 0,
        ///        "fuelType": 3
        ///     }
        ///
        /// </remarks>
        /// <returns></returns>
        [HttpPost("save")]
        public async Task<ActionResult> Save([FromBody]Vehicle vehicle)
        {
            ResponseModel<Vehicle> response = new ResponseModel<Vehicle>();

            var vehicleValidator = new VehicleValidator();
            var validationResult = vehicleValidator.Validate(vehicle);

            if (!validationResult.IsValid && validationResult.Errors.Any())
            {
                response.HasError = true;
                validationResult.Errors.ForEach(x => response.ValidationErrors.Add(new ValidationError(x.PropertyName, x.ErrorMessage)));
                response.Message = "Araç eklenemedi.";
                return BadRequest(response);
            }

            var customer = await _customerRepository.GetByIdAsync(vehicle.CustomerTCKN);

            if (customer == null)
            {
                response.HasError = true;
                response.Message = "Müþteri bulunamadý.";
                return NotFound(response);
            }

            if(customer.Status == CustomerStatus.Reddedildi)
            {
                response.HasError = true;
                response.Message = "Müþteri kaydý reddedildiði için araç kaydý yapýlamadý.";
                return Ok(response);
            }

            await _vehicleRepository.AddAsync(vehicle);
            response.Data = vehicle;
            response.Message = "Araç baþarýlý bir þekilde eklendi.";

            return Ok(response);
        }


        /// <summary>
        /// Update Customer
        /// </summary>
        /// <remarks>
        /// **Sample request body:**
        ///
        ///     { 
        ///        "plate" : "37 KSK 3737",
        ///        "vehicleType": 1,
        ///        "fuelType": 3
        ///     }
        ///
        /// </remarks>
        /// <returns></returns>
        [HttpPut("update")]
        public async Task<ActionResult> Update([FromBody]Vehicle updateVehicle)
        {
            ResponseModel<Vehicle> response = new ResponseModel<Vehicle>();

            if(!Extensions.BeAValidVehicleType(updateVehicle.VehicleType))
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("VehicleType", "Geçersiz araç tipi."));
            }

            if (!Extensions.BeAValidFuelType(updateVehicle.FuelType))
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("FuelType", "Geçersiz yakýt tipi."));
            }

            if (response.HasError)
            {
                response.Message = "Araç güncellenemedi.";
                return BadRequest(response);
            }

            var vehicle = await _vehicleRepository.GetByIdAsync(updateVehicle.Plate);

            if(vehicle == null)
            {
                response.HasError = true;
                response.Message = "Araç bulunamadý.";
                return NotFound(response);
            }

            vehicle.VehicleType = updateVehicle.VehicleType;
            vehicle.FuelType = updateVehicle.FuelType;

            await _vehicleRepository.UpdateAsync(vehicle);
            response.Data = vehicle;
            response.Message = "Araç baþarýlý bir þekilde güncellendi.";

            return Ok(response);
        }

        /// <summary>
        /// Delete Vehicle
        /// </summary>
        /// <returns></returns>
        [HttpDelete("delete/{plate}")]
        public async Task<ActionResult> Delete(string plate)
        {
            ResponseModel<bool> response = new ResponseModel<bool>();

            if (plate.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("plate", "Plaka numarasý girilmeli."));
                response.Message = "Araç silinemedi.";
                return BadRequest(response);
            }

            var vehicle = await _vehicleRepository.GetByIdAsync(plate);

            if (vehicle == null)
            {
                response.HasError = true;
                response.Message = "Araç bulunamadý.";
                return NotFound(response);
            }

            await _vehicleRepository.DeleteAsync(vehicle);
            response.Data = true;
            response.Message = "Araç baþarýlý bir þekilde silindi.";

            return Ok(response);
        }
    }
}
