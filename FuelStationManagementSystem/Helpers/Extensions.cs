using System.Text.RegularExpressions;

namespace FuelStationManagementSystem.Helpers
{
    public static class Extensions
    {
        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value?.Trim());
        }

        public static bool IsNotNullOrEmpty(this string value)
        {
            return !string.IsNullOrEmpty(value);
        }

        public static bool IsNotNullOrEmpty(this Guid? value)
        {
            return value.HasValue && value != Guid.Empty;
        }

        public static string IsNull(this string value, string value2)
        {
            return value.IsNotNullOrEmpty() ? value : value2;
        }

        public static double IsNull(this double value, double value2)
        {
            return value != 0 ? value : value2;
        }

        public static int IsNull(this int value, int value2)
        {
            return value != 0 ? value : value2;
        }

        public static bool IsValidTCIdentityNumber(string tcKimlikNo)
        {
            string regexPattern = @"^(?!00000000000)([1-9][0-9]{9})$";
            return Regex.IsMatch(tcKimlikNo, regexPattern);
        }

        public static bool BeAValidBalanceType(BalanceType balanceType)
        {
            return Enum.IsDefined(typeof(BalanceType), balanceType);
        }

        public static bool BeAValidStatus(CustomerStatus status)
        {
            return Enum.IsDefined(typeof(CustomerStatus), status);
        }

        public static bool BeAValidVehicleType(VehicleType vehicleType)
        {
            return Enum.IsDefined(typeof(VehicleType), vehicleType);
        }

        public static bool BeAValidFuelType(FuelType fuelType)
        {
            return Enum.IsDefined(typeof(FuelType), fuelType);
        }
    }
}
