using Hangfire;
using ProductsService.API;
using ProductsService.API.Interceptors;
using ProductsService.API.Middleware;
using ProductsService.API.Services;
using ProductsService.Application;
using ProductsService.Domain.Abstractions.Database;
using ProductsService.Infrastructure;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddGrpc(options =>
{
    options.Interceptors.Add<ExceptionsInterceptor>();
});
builder.Services.AddGrpcReflection();

builder.Services.AddInfrastructure(builder.Configuration)
    .AddApplication(builder.Configuration)
    .AddAuthentication(builder.Configuration);

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapGrpcReflectionService();
app.MapGrpcService<ProductsGrpcService>();

app.UseHangfireDashboard("/dashboard");

using (var scope = app.Services.CreateScope())
{
    var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

    await unitOfWork.EnableMigrationsAsync();
}

app.Run();
