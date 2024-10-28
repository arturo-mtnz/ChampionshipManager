namespace ChampionshipManager.Core.Repositories.Contracts;

using ChampionshipManager.Core.Domain;

/// <summary>
/// Provides persistence for Matches
/// </summary>
internal interface IMatchRepository : IGenericRepository<Match>
{
    void DeleteAll();
}
