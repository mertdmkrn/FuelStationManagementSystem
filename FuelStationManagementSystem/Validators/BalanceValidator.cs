using FluentValidation;
using FuelStationManagementSystem.Helpers;
using FuelStationManagementSystem.Models;
using System.Reflection;

namespace FuelStationManagementSystem.Validators
{
    public class BalanceValidator : AbstractValidator<Balance>
    {
        public BalanceValidator()
        {
            RuleFor(r => r.CustomerTCKN)
                .NotEmpty().WithMessage("TC Kimlik numarası girilmeli.")
                .Length(11).WithMessage("TC Kimlik Numarası 11 haneli olmalıdır.")
                .Matches(@"^(?!00000000000)([1-9][0-9]{9})$")
                .WithMessage("Geçersiz TC Kimlik Numarası.");

            RuleFor(r => r.Type)
                .Must(Extensions.BeAValidBalanceType).WithMessage("Geçersiz bakiye tipi değeri.");

            RuleFor(r => r.Amount)
                .GreaterThan(0).WithMessage("İşlem tutarı girimelidir.");
        }


    }
}
