using UserService.DAL.Entities;

namespace UserService.DAL.Services.MessageBrocker.ProducerService;

public interface IProducerService
{
    Task ProduceUserCreationAsync(AppUser user, CancellationToken cancellationToken = default);
}
