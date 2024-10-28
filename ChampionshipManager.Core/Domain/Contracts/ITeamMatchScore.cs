namespace ChampionshipManager.Core.Domain.Contracts;

internal interface ITeamMatchScore : IEntity, IComparable<ITeamMatchScore>
{
    ITeam TheTeam { get; }
    int Goals { get; set; }
}
