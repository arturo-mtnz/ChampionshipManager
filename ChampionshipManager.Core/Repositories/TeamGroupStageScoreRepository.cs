namespace ChampionshipManager.Core.Repositories;

using System;
using System.Collections.Generic;
using ChampionshipManager.Core.Domain;
using ChampionshipManager.Core.Events;
using ChampionshipManager.Core.Repositories.Contracts;

internal class TeamGroupStageScoreRepository : ITeamGroupStageScoreRepository
{
    public TeamGroupStageScoreRepository()
    {
        this.InnerRepository = new MemoryBaseRepository<TeamGroupStageScore>();

        MediatorProvider<TeamGroupStageScoreCreated>.Subscribe(c => this.Create(c.TeamGroupStageScore));
    }

    private IGenericRepository<TeamGroupStageScore> InnerRepository { get; }

    public void Create(TeamGroupStageScore entity) => this.InnerRepository.Create(entity);

    public void Delete(TeamGroupStageScore entity) => this.InnerRepository.Delete(entity);

    public IList<TeamGroupStageScore> GetAll() => this.InnerRepository.GetAll();

    public TeamGroupStageScore GetById(Guid id) => this.InnerRepository.GetById(id);

    public void Update(TeamGroupStageScore entity) => this.InnerRepository.Create(entity);

    public void DeleteAll()
    {
        IList<TeamGroupStageScore> all = this.GetAll();
        foreach (TeamGroupStageScore s in all)
        {
            this.Delete(s);
        }
    }
}
