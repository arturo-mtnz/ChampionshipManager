namespace ChampionshipManager.Core.Repositories.Contracts;

using System;
using System.Collections.Generic;
using ChampionshipManager.Core.Domain.Contracts;

/// <summary>
/// Provides persistence for IEntities
/// </summary>
internal interface IGenericRepository<T> where T : IEntity
{
    IList<T> GetAll();
    T GetById(Guid id);
    void Update(T entity);
    void Create(T entity);
    void Delete(T entity);
}
