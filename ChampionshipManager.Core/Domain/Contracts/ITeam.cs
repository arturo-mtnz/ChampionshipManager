namespace ChampionshipManager.Core.Domain.Contracts;


internal interface ITeam : IEntity, IEquatable<ITeam>
{
    string Name { get; set; }
    int Strength { get; }

    void SetStregth(int strength);
}
