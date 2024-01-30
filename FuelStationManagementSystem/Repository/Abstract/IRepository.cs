namespace FuelStationManagementSystem.Repository.Abstract
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetByConditionsAsync(Func<T, bool> conditions);
        Task<T> GetByIdAsync(object id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }
}
