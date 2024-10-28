using ChampionshipManager.Core;
using ChampionshipManager.Core.Api;
using ChampionshipManager.Core.Services.Contracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

WebApplicationBuilder webAppBuilder = GetApplicationBuilder();
WebApplication webApp = webAppBuilder.Build();

/* Instantiate required services */
IApiMapper apiMapper = webApp.Services.GetRequiredService<IApiMapper>();
IPressService pressService = webApp.Services.GetRequiredService<IPressService>();

ConfigureWebApp(webApp, apiMapper);
webApp.Run();

static WebApplicationBuilder GetApplicationBuilder()
{
    WebApplicationBuilder webAppBuilder = WebApplication.CreateBuilder();

    /* Register all custom interfaces implementations to make them usable via DI */
    TypeRegistrations.RegisterCustomServices(webAppBuilder);

    webAppBuilder.Services.AddEndpointsApiExplorer();
    webAppBuilder.Services.AddSwaggerGen();

    return webAppBuilder;
}

static void ConfigureWebApp(WebApplication webApp, IApiMapper apiMapper)
{
    webApp.UseSwagger();
    webApp.UseSwaggerUI();

    /* Register endpoints */
    apiMapper.RegisterEndpoints(webApp);
}

