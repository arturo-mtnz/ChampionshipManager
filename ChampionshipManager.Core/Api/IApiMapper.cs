namespace ChampionshipManager.Core.Api;

using Microsoft.AspNetCore.Builder;

internal interface IApiMapper
{
    void RegisterEndpoints(WebApplication app);
}

