namespace ChampionshipManager.Core.Domain;

using System.Collections.Generic;
using System.Text.Json.Serialization;
using ChampionshipManager.Core.Domain.Contracts;
using ChampionshipManager.Core.Events;

internal class TeamGroupStageScore : ITeamGroupStageScore
{
    public TeamGroupStageScore(ITeam team)
    {
        this.Id = Guid.NewGuid();

        this.TheTeam = team;

        this.Won = new List<IMatch>();
        this.Drawed = new List<IMatch>();
        this.Lost = new List<IMatch>();

        MediatorProvider<MatchEnded>.Subscribe(e => e.EndedMatch.Involves(team), UpdateWithMatchResults);
        MediatorProvider<TeamGroupStageScoreCreated>.Publish(new TeamGroupStageScoreCreated(this));
    }

    [JsonIgnore]
    public ITeam TheTeam { get; }

    [JsonIgnore]
    public IList<IMatch> Won { get; }

    [JsonIgnore]
    public IList<IMatch> Drawed { get; }

    [JsonIgnore]
    public IList<IMatch> Lost { get; }

    [JsonIgnore]
    public IEnumerable<IMatch> AllMatches
    {
        get
        {
            return this.Won
                .Union(this.Drawed)
                .Union(this.Lost);
        }
    }

    public Guid TeamId => this.TheTeam.Id;

    public string TeamName => this.TheTeam.Name;

    public int Points
    {
        get
        {
            const int WinPoints = 3;
            const int DrawPoints = 1;
            const int LossPoints = 0;

            return this.Won.Count * WinPoints + 
                this.Drawed.Count * DrawPoints +
                this.Lost.Count * LossPoints;
        }
    }

    public int For { get; private set; }

    public int Against { get; private set; }

    public int GoalDifference
    {
        get
        {
            return this.For - this.Against;
        }
    }

    public int Rank { get; set; }

    [JsonIgnore]
    public Guid Id { get; }

    private void UpdateWithMatchResults(MatchEnded ended)
    {
        IMatch match = ended.EndedMatch;
        ITeam? winner = match.GetWinner();

        if (winner is null)
        {
            this.Drawed.Add(match);
        }
        else if (winner.Equals(this.TheTeam))
        {
            this.Won.Add(match);
        }
        else
        {
            this.Lost.Add(match);
        }

        match.GetTeamGoals(this.TheTeam, out int scored, out int conceded);
        this.For += scored;
        this.Against += conceded;
    }

    #region IComparable

    /// <summary>
    /// Compared the result this team had with another team in order to untie the rankings.
    /// </summary>
    /// <param name="otherTeam">The other team</param>
    /// <returns>True if this team defeated the other head to head, false otherwise. In case of draw we order it arbitrarily but consistently</returns>
    private bool BeatsHeadToHead(ITeam otherTeam)
    {
        if (otherTeam is null)
        {
            return true;
        }

        IMatch? decidingMatch = this
            .AllMatches
            .Where(m => m.Teams.Any(t => t.Equals(otherTeam)))
            .FirstOrDefault();

        ITeam? winner = decidingMatch?.GetWinner();
        if (winner is null)
        {
            /* We break the tie with a pre-fixed rule */
            return this.TheTeam.GetHashCode() > otherTeam.GetHashCode();
        }

        if (winner.Equals(this.TheTeam))
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Returns 
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public int CompareTo(ITeamGroupStageScore? other)
    {
        if (other is null)
        {
            return -1;
        }

        if (this.Points != other.Points)
        {
            return this.Points > other.Points ? -1 : 1;
        }

        if (this.GoalDifference != other.GoalDifference)
        {
            return this.GoalDifference > other.GoalDifference ? -1 : 1;
        }

        if (this.For != other.For)
        {
            return this.For > other.For ? -1 : 1;
        }

        if (this.Against != other.Against)
        {
            return this.Against < other.Against ? -1 : 1;
        }

        return this.BeatsHeadToHead(other.TheTeam) ? -1 : 1;
    }
    #endregion
}
