using FluentValidation;
using FuelStationManagementSystem.Helpers;
using FuelStationManagementSystem.Models;
using System.Reflection;

namespace FuelStationManagementSystem.Validators
{
    public class CustomerValidator : AbstractValidator<Customer>
    {
        public CustomerValidator()
        {
            RuleFor(r => r.TCKN)
                .NotEmpty().WithMessage("TC Kimlik numarası girilmeli.")
                .Length(11).WithMessage("TC Kimlik Numarası 11 haneli olmalıdır.")
                .Matches(@"^[1-9]{1}[0-9]{9}[0,2,4,6,8]{1}$")
                .WithMessage("Geçersiz TC Kimlik Numarası.");

            RuleFor(r => r.NameSurname)
                .NotEmpty().WithMessage("Müşteri adı girilmeli.")
                .MaximumLength(100).WithMessage("Müşteri adı 100 karakterden fazla girilmemelidir.");

            RuleFor(r => r.Address)
                .NotEmpty().WithMessage("Müşteri adresi girilmeli")
                .MaximumLength(200).WithMessage("Müşteri adresi 200 karakterden fazla girilmemelidir.");

            RuleFor(r => r.Status)
                .Must(Extensions.BeAValidStatus).WithMessage("Geçersiz durum değeri.");
        }
    }
}