namespace UserService.DAL.Database.DbInitializer;

public interface IDbInitializer
{
    Task SeedDataAsync();
}
