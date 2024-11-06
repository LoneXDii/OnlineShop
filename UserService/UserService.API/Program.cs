using UserService.API.Middleware;
using UserService.Application;
using UserService.Infrastructure;
using UserService.Infrastructure.Database.DbInitializer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthorization(opt =>
	{
		opt.AddPolicy("admin", p => p.RequireRole("admin"));
	});

builder.Services.AddApplication()
	.AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
	var DbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
	await DbInitializer.SeedDataAsync();
}

app.Run();
