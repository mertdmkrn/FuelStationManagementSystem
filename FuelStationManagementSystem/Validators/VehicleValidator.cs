using FluentValidation;
using FuelStationManagementSystem.Helpers;
using FuelStationManagementSystem.Models;

namespace FuelStationManagementSystem.Validators
{
    public class VehicleValidator : AbstractValidator<Vehicle>
    {
        public VehicleValidator()
        {
            RuleFor(r => r.Plate)
                .NotEmpty().WithMessage("Plaka girilmeli.")
                .Matches(@"^[0-9]{2}\s?[A-Z]{1,3}\s?[0-9]{2,4}$")
                .WithMessage("Geçersiz plaka formatı. Örnek: '34 AB 123' veya '06ABC45'");

            RuleFor(r => r.CustomerTCKN)
                .NotEmpty().WithMessage("TC Kimlik numarası girilmeli.")
                .Length(11).WithMessage("TC Kimlik Numarası 11 haneli olmalıdır.")
                .Matches(@"^[1-9]{1}[0-9]{9}[0,2,4,6,8]{1}$")
                .WithMessage("Geçersiz TC Kimlik Numarası.");

            RuleFor(r => r.VehicleType)
                .Must(Extensions.BeAValidVehicleType).WithMessage("Geçersiz araç tipi değeri.");

            RuleFor(r => r.FuelType)
                .Must(Extensions.BeAValidFuelType).WithMessage("Geçersiz yakıt tipi değeri.");
        }
    }
}
