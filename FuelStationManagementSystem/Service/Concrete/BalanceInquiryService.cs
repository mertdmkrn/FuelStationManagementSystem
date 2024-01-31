using FuelStationManagementSystem.Models;
using FuelStationManagementSystem.Repository.Abstract;
using FuelStationManagementSystem.Service.Abstract;

namespace FuelStationManagementSystem.Service.Concrete
{
    public class BalanceInquiryService : IBalanceInquiryService
    {
        private readonly IRepository<Balance> _balanceRepository;

        public BalanceInquiryService(IRepository<Balance> balanceRepository)
        {
            _balanceRepository = balanceRepository;
        }

        public async Task<bool> CheckBalance(string tckn, double amount)
        {
            var balances = await _balanceRepository.GetByConditionsAsync(x => x.CustomerTCKN.Equals(tckn) && x.Amount > 0);

            var totalBalance = balances.Sum(x => x.Amount);

            if(totalBalance < amount) 
                return false;

            var tempBalance = amount;

            foreach (var balance in balances.OrderBy(x => x.Type))
            {
                tempBalance -= balance.Amount;

                if (tempBalance <= 0)
                {
                    balance.Amount -= amount;
                    await _balanceRepository.UpdateAsync(balance);
                    break;
                }
                else
                {
                    await _balanceRepository.DeleteAsync(balance);
                }
            }

            return true;
        }
    }
}
