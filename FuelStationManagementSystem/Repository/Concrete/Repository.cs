using FuelStationManagementSystem.Repository.Abstract;
using Microsoft.EntityFrameworkCore;

namespace FuelStationManagementSystem.Repository.Concrete
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly FuelStationDbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(FuelStationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetByConditionsAsync(Func<T, bool> conditions)
        {
            return await _dbSet.AsNoTracking().Where(conditions).AsQueryable().ToListAsync();
        }

        public async Task<T> GetByIdAsync(object id)
        {
            return await _dbSet.FindAsync(id);
        }
        public async Task AddAsync(T entity)
        {
             await _dbSet.AddAsync(entity);
             await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
