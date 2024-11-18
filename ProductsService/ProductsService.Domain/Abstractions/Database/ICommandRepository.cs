using ProductsService.Domain.Entities.Abstractions;

namespace ProductsService.Domain.Abstractions.Database;

public interface ICommandRepository<T> where T : IEntity
{
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
}
