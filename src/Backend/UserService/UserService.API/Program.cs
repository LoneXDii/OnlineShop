using Hangfire;
using Microsoft.EntityFrameworkCore;
using UserService.API.Configuration;
using UserService.API.Middleware;
using UserService.BLL;
using UserService.DAL;
using UserService.DAL.Database;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("secretconfig.json");

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplication(builder.Configuration)
    .AddInfrastructure(builder.Configuration)
    .AddAuth(builder.Configuration);

builder.Host.UseLogging();

var app = builder.Build();

app.UseMiddleware<LoggingMiddleware>();
app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseHangfireDashboard("/dashboard");

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    await dbContext.Database.MigrateAsync();
}

app.Run();
