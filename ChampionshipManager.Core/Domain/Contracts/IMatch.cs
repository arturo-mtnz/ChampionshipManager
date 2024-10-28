namespace ChampionshipManager.Core.Domain.Contracts;

using System.Collections.Immutable;

internal interface IMatch : IEntity
{
    IImmutableList<ITeam> Teams { get; }
    bool Played { get; }

    ITeam? GetWinner();
    void GetTeamGoals(ITeam team, out int scored, out int conceded);
    bool Involves(ITeam team);
    void Simulate();
}
