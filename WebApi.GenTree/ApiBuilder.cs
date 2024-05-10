using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApi.GenTree.Validation;
/// <summary>
/// Осуществляет инициализацию обработчиков апи-запросов
/// </summary>
internal static class ApiBuilder
{
    /// <summary>
    /// Конфигурация сервисов сборщика для обработки апи-запросов
    /// </summary>
    /// <param name="servicePath">Рабочая директория приложения</param>
    /// <param name="services">сервисы построителя приложения</param>
    public static void ConfigureApi(string servicePath, IServiceCollection services)
    {
        ConfigureValidation(services);
        services.AddControllers(opt =>
        {
            opt.AllowEmptyInputInBodyModelBinding = true;
        });
        ConfigureSwagger(services, servicePath);
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
                        .ToList();

                    // возврат 200 со списком ошибок
                    return new OkObjectResult(new
                    {
                        Success = false,
                        Errors = errors
                    });
                };
            });
    }
}