namespace ChampionshipManager.Core.Domain;

using System;
using System.Collections.Immutable;
using MathNet.Numerics.Distributions;
using ChampionshipManager.Core.Domain.Contracts;
using ChampionshipManager.Core.Events;

internal class Match : IMatch
{
    public Match(Guid id, ITeam homeTeam, ITeam awayTeam)
    {
        this.Id = id;
        this.Teams = [homeTeam, awayTeam];
        this.MatchScores = [new TeamMatchScore(homeTeam), new TeamMatchScore(awayTeam)];

        this.Played = false;
    }

    public Guid Id { get; }
    public IImmutableList<ITeam> Teams { get; }
    public bool Played { get; private set; }

    private List<ITeamMatchScore> MatchScores { get; }

    public ITeam? GetWinner()
    {
        if (!this.Played || this.MatchScores[0].Goals == this.MatchScores[1].Goals)
        {
            return null;
        }

        this.MatchScores.Sort();

        return this.MatchScores
            .First()
            .TheTeam;
    }

    public void GetTeamGoals(ITeam team, out int scored, out int conceded)
    {
        scored = 0;
        conceded = 0;

        ITeamMatchScore score0 = this.MatchScores[0];
        ITeamMatchScore score1 = this.MatchScores[1];

        if (score0.TheTeam.Equals(team)) 
        {
            scored = score0.Goals;
            conceded = score1.Goals;
        }
        else
        {
            scored = score1.Goals;
            conceded = score0.Goals;
        }
    }

    public bool Involves(ITeam team)
    {
        return this.Teams.Contains(team);
    }

    public void Simulate()
    {
        if (this.Played)
        {
            return;
        }

        foreach(TeamMatchScore teamScore in this.MatchScores)
        {
            SetScore(teamScore);
        }

        this.Played = true;
        MediatorProvider<MatchEnded>.Publish(new MatchEnded(this));
    }

    private static void SetScore(TeamMatchScore teamMatchScore)
    {
        int scored = GetScoredGoals(teamMatchScore.TheTeam.Strength);
        teamMatchScore.Goals = scored;
    }

    private static int GetScoredGoals(int teamStrength)
    {
        const double defaultMeanGoals = 2.2;

        if (teamStrength < 0 || teamStrength > 100)
        {
            throw new ArgumentException("Value must be between 0 and 100", nameof(teamStrength));
        }

        double meanGoals = defaultMeanGoals + GetDeviation(teamStrength);
        Poisson poissonDistributionOfGoals = new Poisson(meanGoals);
        return poissonDistributionOfGoals.Sample();
    }

    private static double GetDeviation(int teamStrength)
    {
        /* Deviation of 0.01 per unit of strength differing from 50 */
        return ((double)teamStrength - 50.0) / 100.0;
    }
}
