namespace ChampionshipManager.Core.Domain.Contracts;

using System.Collections.Generic;

internal interface IStandings : IEntity
{
    List<ITeamGroupStageScore> TeamScores { get; }

    List<ITeam> QualifiedTeams { get; }
}
