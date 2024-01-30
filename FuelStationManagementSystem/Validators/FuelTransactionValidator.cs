using FluentValidation;
using FuelStationManagementSystem.Helpers;
using FuelStationManagementSystem.Models;
using System.Reflection;

namespace FuelStationManagementSystem.Validators
{
    public class FuelTransactionValidator : AbstractValidator<FuelTransaction>
    {
        public FuelTransactionValidator()
        {
            RuleFor(r => r.VehiclePlate)
                .NotEmpty().WithMessage("Plaka girilmeli.")
                .Matches(@"^[0-9]{2}\s?[A-Z]{1,3}\s?[0-9]{2,4}$")
                .WithMessage("Geçersiz plaka formatı. Örnek: '34 AB 123' veya '06ABC45'");

            RuleFor(r => r.TransactionDate)
                .NotEmpty().WithMessage("İşlem tarihi girilmeli.");

            RuleFor(r => r.Amount)
                .GreaterThan(0).WithMessage("İşlem tutarı girimelidir.");
        }
    }
}
