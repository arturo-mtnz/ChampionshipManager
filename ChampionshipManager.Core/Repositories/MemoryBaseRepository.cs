namespace ChampionshipManager.Core.Repositories;

using System;
using System.Collections.Generic;
using ChampionshipManager.Core.Domain.Contracts;
using ChampionshipManager.Core.Repositories.Contracts;


/// <summary>
/// Simulates in memory, specifically in a single dictionary, the persistence of objects of the type IEntity.
/// </summary>
/// <typeparam name="T">The type of objects to be stored. Must implement IEntity</typeparam>
internal class MemoryBaseRepository<T> : IGenericRepository<T> where T : IEntity
{
    private IDictionary<Guid, T> Entities = new Dictionary<Guid, T>();

    public void Create(T entity)
    {
        this.Entities.Add(entity.Id, entity);
    }

    public void Delete(T entity)
    {
        this.Entities.Remove(entity.Id);
    }

    public IList<T> GetAll()
    {
        return this.Entities.Values.ToList();
    }

    public T GetById(Guid id)
    {
        return this.Entities[id];
    }

    public void Update(T entity)
    {
        this.Entities[entity.Id] = entity;
    }

}
