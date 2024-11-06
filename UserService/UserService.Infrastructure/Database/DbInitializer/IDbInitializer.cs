namespace UserService.Infrastructure.Database.DbInitializer;

public interface IDbInitializer
{
	Task SeedDataAsync();
}
