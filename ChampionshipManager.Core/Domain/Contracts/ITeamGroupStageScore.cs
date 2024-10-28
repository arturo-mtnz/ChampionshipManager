namespace ChampionshipManager.Core.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

internal interface ITeamGroupStageScore : IEntity, IComparable<ITeamGroupStageScore>
{
    [JsonIgnore]
    ITeam TheTeam { get; }

    [JsonIgnore]
    IList<IMatch> Won { get; }

    [JsonIgnore]
    IList<IMatch> Drawed { get; }

    [JsonIgnore]
    IList<IMatch> Lost { get; }

    [JsonIgnore]
    IEnumerable<IMatch> AllMatches { get; }
    Guid TeamId { get; }
    string TeamName { get; }
    int Points { get; }
    int For { get; }
    int Against { get; }
    int GoalDifference { get; }
    int Rank { get; set; }
}
