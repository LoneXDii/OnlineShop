using Hangfire;
using Microsoft.AspNetCore.SignalR;
using SupportService.API.Configuration;
using SupportService.API.Filters;
using SupportService.API.Hubs;
using SupportService.Application;
using SupportService.Domain.Abstractions;
using SupportService.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSignalR(opt =>
{
    opt.AddFilter<HubExceptionsFilter>();
});

builder.Services.AddSingleton<HubExceptionsFilter>();

builder.Services.AddInfrastructure(builder.Configuration)
    .AddApplication(builder.Configuration)
    .AddAuth(builder.Configuration);

builder.Host.UseLogging();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.MapHub<ChatHub>("chat-hub");

app.UseHangfireDashboard("/dashboard");

using (var scope = app.Services.CreateScope())
{
    var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

    await unitOfWork.EnableMigrationsAsync();
}

app.Run();
