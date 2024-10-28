namespace ChampionshipManager.Core.Events;
using System.Collections.Generic;
using ChampionshipManager.Core.Domain;

internal class GroupStageEnded
{
    public GroupStageEnded(IEnumerable<TeamGroupStageScore> teamScores)
    {
        this.TeamScores = teamScores;
    }

    IEnumerable<TeamGroupStageScore> TeamScores { get; }

}
