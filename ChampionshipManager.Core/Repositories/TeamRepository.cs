namespace ChampionshipManager.Core.Repositories;

using System;
using System.Collections.Generic;
using ChampionshipManager.Core.Domain;
using ChampionshipManager.Core.Repositories.Contracts;

/// <summary>
/// Implements persistence and basic CRUD operations for Teams
/// </summary>
internal class TeamRepository : ITeamRepository
{
    public TeamRepository()
    {
        this.InnerRepository = new MemoryBaseRepository<Team>();
    }

    private IGenericRepository<Team> InnerRepository { get; }

    public void Create(Team entity) => this.InnerRepository.Create(entity);

    public void Delete(Team entity) => this.InnerRepository.Delete(entity);

    public IList<Team> GetAll() => this.InnerRepository.GetAll();

    public Team GetById(Guid id) => this.InnerRepository.GetById(id);

    public void Update(Team entity) => this.InnerRepository.Create(entity);
}
