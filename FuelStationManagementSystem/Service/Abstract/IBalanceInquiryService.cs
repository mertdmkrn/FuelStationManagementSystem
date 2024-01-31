using FuelStationManagementSystem.Models;
using FuelStationManagementSystem.Repository.Abstract;

namespace FuelStationManagementSystem.Service.Abstract
{
    public interface IBalanceInquiryService
    {
        Task<bool> CheckBalance(string tckn, double amount);
    }
}
