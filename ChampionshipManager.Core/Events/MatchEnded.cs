namespace ChampionshipManager.Core.Events;

using ChampionshipManager.Core.Domain.Contracts;

internal class MatchEnded
{
    public MatchEnded(IMatch match)
    {
        this.EndedMatch = match;
    }

    public IMatch EndedMatch { get; }
}
