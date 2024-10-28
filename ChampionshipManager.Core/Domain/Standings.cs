namespace ChampionshipManager.Core.Domain;

using System;
using System.Collections.Generic;
using System.Linq;
using ChampionshipManager.Core.Domain.Contracts;

internal class Standings : IStandings
{
    public Standings(List<Team> teams)
    {
        this.Id = Guid.NewGuid();
        _teamScores = teams
            .Select(t => new TeamGroupStageScore(t) as ITeamGroupStageScore)
            .ToList();
    }

    public Standings(List<ITeamGroupStageScore> scores)
    {
        _teamScores = scores;
    }

    public Guid Id { get; }

    private List<ITeamGroupStageScore> _teamScores;

    public List<ITeamGroupStageScore> TeamScores
    {
        get
        {
            _teamScores.Sort();
            for (int i = 0; i < _teamScores.Count; i++ )
            {
                _teamScores[i].Rank = i + 1;
            }
            return _teamScores;
        }
    }

    public List<ITeam> QualifiedTeams
    {
        get
        {
            List<ITeam> qualifiedTeams = new List<ITeam>();

            _teamScores.Sort();
            for (int i = 0; i < 2; i++)
            {
                qualifiedTeams.Add(_teamScores[i].TheTeam);
            }

            return qualifiedTeams;
        }
    }
}
