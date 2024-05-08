using GenTree.Domain;
using Microsoft.EntityFrameworkCore;
using WebApi.GenTree.Modules.People;
using WebApi.GenTree.Modules.Relations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<GenTreeContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

PeopleModule.Register(builder.Services);
RelationsModule.Register(builder.Services);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
	using var context = scope.ServiceProvider.GetRequiredService<GenTreeContext>();
	await context.Database.MigrateAsync();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();