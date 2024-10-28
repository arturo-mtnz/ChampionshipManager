namespace ChampionshipManager.Core.Services.Contracts;

using ChampionshipManager.Core.Domain.Contracts;

/// <summary>
/// Service used for simulation of the Group Stage setup and run.
/// </summary>
internal interface IGroupStageService
{
    /// <summary>
    /// Creates matches from all the possible crossings of all the teams in the repostory
    /// </summary>
    void CreateMatches();

    /// <summary>
    /// Simulates the matches execution
    /// </summary>
    void Simulate();

    /// <summary>
    /// Rests the matches and scores.
    /// </summary>
    void ResetSimulation();

    /// <summary>
    /// Get the ranking and standings
    /// </summary>
    /// <returns></returns>
    List<ITeamGroupStageScore> GetStandings();

    /// <summary>
    /// Gets the teams qualified for the knockout stage of the Championship.
    /// </summary>
    /// <returns></returns>
    List<ITeam> GetQualifiedTeams();
}
