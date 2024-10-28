namespace ChampionshipManager.Core.Repositories.Contracts;

using ChampionshipManager.Core.Domain;

internal interface ITeamGroupStageScoreRepository : IGenericRepository<TeamGroupStageScore>
{
    void DeleteAll();
}
