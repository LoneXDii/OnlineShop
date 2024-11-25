using ProductsService.Domain.Entities.Abstractions;

namespace ProductsService.Domain.Abstractions.Database;

public interface ICommandRepository<T> where T : IEntity
{
    Task AddAsync(T entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(T entity, CancellationToken cancellationToken = default);
}
