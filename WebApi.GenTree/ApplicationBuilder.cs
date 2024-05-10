using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
/// <summary>
/// Осуществляет инициализацию и сборку приложения
/// </summary>
internal static class ApplicationBuilder
{
    /// <summary>
    /// Инициализация сборщика приложения
    /// </summary>
    /// <param name="args">Аргументы запуска приложения</param>
    /// <returns></returns>
    public static (string servicePath, WebApplicationBuilder builder) ConfigureBuilder(string[] args)
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

    /// <summary>
    /// Инициализирует аппликейшн, Регистрирует апи-миддлвары
    /// </summary>
    /// <param name="args">Аргументы запуска приложения</param>
    /// <param name="builder">Сборщик приложения</param>
    /// <returns></returns>
    public static WebApplication BuildApp(string[] args, WebApplicationBuilder builder)
    {
        bool isDebug = Debugger.IsAttached || args.Contains("--debug");

        WebApplication app = builder.Build();

        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApi.GenTree"));

        if (isDebug)
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseStaticFiles();

        app.UseRouting();

        // use added cors for web-ui
        app.UseCors();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        return app;
    }
}
