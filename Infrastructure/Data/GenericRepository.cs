using Core.Entities;
using Core.Interfaces;
using Core.Secifications;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class GenericRepository<T> : IGenericRepository<T> where T: BaseEntity
    {
        private readonly StoreContext context;
        public GenericRepository(StoreContext context)
        {
            this.context = context;
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await context.Set<T>().FindAsync(id);
        }

        public async Task<IReadOnlyList<T>> ListAllAsync()
        {
            return await context.Set<T>().ToListAsync();
        }

        public async Task<T> GetEntityWithSpec(
            ISpecification<T> spec)
        {
            return await ApplySpecification(spec)
                .FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyList<T>> ListAsync(
            ISpecification<T> spec)
        {
            return await ApplySpecification(spec)
                .ToListAsync();
        }

        private IQueryable<T> ApplySpecification(
            ISpecification<T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(
                context.Set<T>().AsQueryable(), spec);
        }
    }
}