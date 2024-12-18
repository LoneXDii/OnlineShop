namespace UserService.DAL.Services.MessageBrocker.ProducerService;

public interface IProducerService
{
    Task ProduceAsync(CancellationToken cancellationToken=default);
}
