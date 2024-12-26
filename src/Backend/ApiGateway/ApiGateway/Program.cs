using ApiGateway;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);


builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
builder.Services.AddOcelot();
builder.Services.AddSignalR();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureAuthentication(builder.Configuration);

builder.Services.AddSwaggerForOcelot(builder.Configuration);

builder.Host.UseLogging();

var app = builder.Build();

app.UseSwaggerForOcelotUI(options =>
{
    options.PathToSwaggerGenerator = "/swagger/docs";
});

app.UseAuthentication();
app.UseAuthorization();

app.UseWebSockets();
await app.UseOcelot();

app.Run();
