namespace ChampionshipManager.Core.Api;

using System;
using ChampionshipManager.Core.Domain;
using ChampionshipManager.Core.Repositories.Contracts;
using ChampionshipManager.Core.Services.Contracts;
using Microsoft.AspNetCore.Builder;

internal class ApiMapper : IApiMapper
{
    public ApiMapper(
        IGroupStageService groupPhaseService,
        ITeamRepository teamRepository,
        IMatchRepository matchRepository)
    {
        this.GroupPhaseService = groupPhaseService;
        this.TeamRepository = teamRepository;
        this.MatchRepository = matchRepository;
    }

    private IGroupStageService GroupPhaseService { get; }
    private ITeamRepository TeamRepository { get; }
    private IMatchRepository MatchRepository { get; }

    public void RegisterEndpoints(WebApplication app)
    {
        app.MapPost("/api/team", (Team team) =>
            {
                this.TeamRepository.Create(team);
            }
        );

        app.MapGet("/api/team", (Guid id) =>
            {
                return this.TeamRepository.GetById(id);
            }
        );

        app.MapPost("/api/teams", (Team[] teams) =>
        {
            foreach (Team t in teams)
            {
                this.TeamRepository.Create(t);
            }
        }
       );

        app.MapGet("/api/teams", () =>
            {
                return this.TeamRepository.GetAll();
            }
        );

        app.MapPost("/api/matchCreation", () =>
            {
                this.GroupPhaseService.CreateMatches();
            }
        );

        app.MapGet("/api/matches", () =>
            {
                return this.MatchRepository.GetAll();
            }
        );

        app.MapPost("/api/simulation", () =>
            {
                this.GroupPhaseService.Simulate();
            }
        );

        app.MapDelete("/api/simulation", () =>
        {
            this.GroupPhaseService.ResetSimulation();
        }
        );

        app.MapGet("/api/standings", () =>
            {
                return this.GroupPhaseService.GetStandings();
            }
        );

        app.MapGet("/api/teamsQualifiedForKnockoutStage", () =>
        {
            return this.GroupPhaseService.GetQualifiedTeams();
        }
);





        ///*** ENDPOINT EXAMPLES ***/
        //app.MapGet("/api/artu", () =>
        //{
        //    int n = this.GroupPhaseService.Increment();
        //    Console.WriteLine("Esto va a consola, bb");
        //    return Results.Ok(new { Message = $"Hello , I'm Artu! You called this {n} times" });
        //});

        //app.MapPost("/api/another", async (HttpContext context) =>
        //{
        //    return Results.Ok(new { Message = "This is a POST endpoint!" });
        //});
        ///*** END OF ENDPOINT EXAMPLES ***/


        //app.MapPost("/api/", async (HttpContext context) =>
        //{
        //    return Results.Ok(new { Message = "This is a POST endpoint!" });
        //});

    }
}
