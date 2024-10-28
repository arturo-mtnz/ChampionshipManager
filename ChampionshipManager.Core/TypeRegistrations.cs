namespace ChampionshipManager.Core;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using ChampionshipManager.Core.Api;
using ChampionshipManager.Core.Repositories.Contracts;
using ChampionshipManager.Core.Repositories;
using ChampionshipManager.Core.Services;
using ChampionshipManager.Core.Services.Contracts;

/// <summary>
/// Static class which provides intefaces' implementations registration.
/// </summary>
internal static class TypeRegistrations
{
    /// <summary>
    /// Registers implementatons of interfaces which need to be used for DI.
    /// </summary>
    /// <param name="webAppBuilder">The WebApplicationBuilder object where we are registering the implementations</param>
    public static void RegisterCustomServices(WebApplicationBuilder webAppBuilder)
    {
        /*** Registration of implementated interfaces ***/
        /* Web API */
        webAppBuilder.Services.AddSingleton<IApiMapper, ApiMapper>();
        /* Services */
        webAppBuilder.Services.AddSingleton<IGroupStageService, GroupStageService>();
        webAppBuilder.Services.AddSingleton<IPressService, PressService>();
        /* Repos */
        webAppBuilder.Services.AddSingleton<IMatchRepository, MatchRepository>();
        webAppBuilder.Services.AddSingleton<ITeamRepository, TeamRepository>();
        webAppBuilder.Services.AddSingleton<ITeamGroupStageScoreRepository, TeamGroupStageScoreRepository>();
    }
}
