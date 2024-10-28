namespace ChampionshipManager.Core.Events;

using ChampionshipManager.Core.Domain;

internal class TeamGroupStageScoreCreated
{
    public TeamGroupStageScoreCreated(TeamGroupStageScore score)
    {
        this.TeamGroupStageScore = score;
    }

    public TeamGroupStageScore TeamGroupStageScore { get; } 
}
