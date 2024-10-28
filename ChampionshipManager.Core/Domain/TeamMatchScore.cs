namespace ChampionshipManager.Core.Domain;

using System;
using ChampionshipManager.Core.Domain.Contracts;

internal class TeamMatchScore : ITeamMatchScore
{
    public TeamMatchScore(ITeam team)
    {
        const int startingScore = 0;

        this.Id = Guid.NewGuid();
        this.TheTeam = team;
        this.Goals = startingScore;
    }

    public ITeam TheTeam { get; }

    public int Goals { get; set; }

    public Guid Id { get; }

    public int CompareTo(ITeamMatchScore? other)
    {
        if (other is null || other.Goals < this.Goals)
        {
            return -1;
        }
        
        if (other.Goals > this.Goals)
        {
            return 1;
        }

        return 0;
    }
}
