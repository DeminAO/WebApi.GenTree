using FluentValidation.AspNetCore;
using GenTree.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.GenTree.Modules.People;
using WebApi.GenTree.Modules.Relations;
using WebApi.GenTree.Validation;

(string servicePath, WebApplicationBuilder builder) = ConfigureBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<GenTreeContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

PeopleModule.Register(builder.Services);
RelationsModule.Register(builder.Services);
builder.Services.AddControllers();

ConfigureValidation(builder.Services);
ConfigureSwagger(builder.Services, servicePath);

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


/// <summary>
/// Инициализация сборщика приложения
/// </summary>
/// <param name="args">Аргументы запуска приложения</param>
/// <returns></returns>
static (string servicePath, WebApplicationBuilder builder) ConfigureBuilder(string[] args)
{
    string servicePath = AppDomain.CurrentDomain.BaseDirectory;

    WebApplicationOptions options = new()
    {
        ContentRootPath = servicePath,
        Args = args
    };

    WebApplicationBuilder builder = WebApplication.CreateBuilder(options);

    builder.Configuration
        .SetBasePath(builder.Environment.ContentRootPath)
        .AddJsonFile("appsettings.json", false, true)
        .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
        .AddEnvironmentVariables();

    return (servicePath, builder);
}
static void ConfigureSwagger(IServiceCollection services, string servicePath)
{
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen(opt =>
    {
        Directory
            .GetFiles(servicePath, "*.xml")
            .ToList()
            .ForEach(f => opt.IncludeXmlComments(f, true));
    });
}
static void ConfigureValidation(IServiceCollection services)
{
    services.AddMvc(conf => conf.Filters.Add(typeof(ExceptionFilter)));

    services
        // регистрация валидации запросов
        .AddFluentValidationAutoValidation()
        // регистрация обработки результатов валидации
        .Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = c =>
            {
                // список ошибок валидации
                var errors = c.ModelState.Values
                    .Where(v => v.Errors.Any())
                    .SelectMany(v => v.Errors)
                    .Select(v => v.ErrorMessage)
                    .Distinct()
                    .ToArray();

                // возврат 200 со списком ошибок
                return new ObjectResult(new ErrorModel(errors));
            };
        });
}