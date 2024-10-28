namespace ChampionshipManager.Core.Repositories;

using System;
using System.Collections.Generic;
using ChampionshipManager.Core.Domain;
using ChampionshipManager.Core.Repositories.Contracts;

/// <summary>
/// Implements persistence and basic CRUD operations for Matches
/// </summary>
internal class MatchRepository : IMatchRepository
{
    public MatchRepository()
    {
        this.InnerRepository = new MemoryBaseRepository<Match>();
    }

    private IGenericRepository<Match> InnerRepository { get; }

    public void Create(Match entity) => this.InnerRepository.Create(entity);

    public void Delete(Match entity) => this.InnerRepository.Delete(entity);

    public IList<Match> GetAll() => this.InnerRepository.GetAll();

    public Match GetById(Guid id) => this.InnerRepository.GetById(id);

    public void Update(Match entity) => this.InnerRepository.Create(entity);

    public void DeleteAll()
    {
        IList<Match> all = this.GetAll();
        foreach (Match m in all)
        {
            this.Delete(m);
        }
    }
}
