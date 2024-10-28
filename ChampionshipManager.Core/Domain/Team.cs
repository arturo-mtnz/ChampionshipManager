namespace ChampionshipManager.Core.Domain;

using System;
using ChampionshipManager.Core.Domain.Contracts;

internal class Team : ITeam
{
    public Team(string name, int strength)
    {
        this.Name = name;
        this.SetStregth(strength);
    }

    public Guid Id { get; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public int Strength { get; private set; }

    /// <summary>
    /// Sets the strength of the team, normalizing the provided value to mod 101 (it may only be from 0 to 100, both inclusive).
    /// </summary>
    /// <param name="strength"></param>
    public void SetStregth(int strength)
    {
        this.Strength = strength % 101;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public override bool Equals(object? obj)
    {
        if (obj is null || obj.GetType() != this.GetType())
        {
            return false;
        }

        return ((Team)obj).Id.Equals(this.Id);
    }

    public bool Equals(ITeam? other)
    {
        if (other is null)
        {
            return false;
        }

        return this.Id.Equals(other.Id);
    }
}
