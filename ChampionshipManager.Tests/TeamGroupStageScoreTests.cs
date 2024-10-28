namespace ChampionshipManager.Tests;

using System;
using Moq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ChampionshipManager.Core.Domain;
using ChampionshipManager.Core.Events;
using ChampionshipManager.Core.Domain.Contracts;
using System.Collections.Immutable;

[TestClass]
public class TeamGroupStageScoreTests
{
    [TestMethod]
    public void Constructor_ShouldInitializeCorrectly()
    {
        string teamName = "Team A";
        int teamStrength = 0;
        ITeam team = new Team(teamName, teamStrength);
        ITeamGroupStageScore teamGroupStageScore = new TeamGroupStageScore(team);

        Assert.AreEqual(team.Id, teamGroupStageScore.TeamId);
        Assert.AreEqual(teamName, teamGroupStageScore.TeamName);
        Assert.AreEqual(0, teamGroupStageScore.Won.Count);
        Assert.AreEqual(0, teamGroupStageScore.Drawed.Count);
        Assert.AreEqual(0, teamGroupStageScore.Lost.Count);
        Assert.AreEqual(0, teamGroupStageScore.For);
        Assert.AreEqual(0, teamGroupStageScore.Against);
        Assert.AreEqual(0, teamGroupStageScore.Points);
        Assert.AreEqual(0, teamGroupStageScore.GoalDifference);
    }

    [TestMethod]
    public void UpdateWithMatchResults_ShouldUpdateScores()
    {
        ITeam team = new Team("Team A", 0);
        ITeam opponent = new Team("Team B", 0);

        Mock<IMatch> matchMock = new Mock<IMatch>() { CallBase = true };

        matchMock.Setup(m => m.Id).Returns(Guid.NewGuid());
        matchMock.Setup(m => m.Teams).Returns(new List<ITeam> { team, opponent }.ToImmutableList());
        matchMock.Setup(m => m.Played).Returns(false);

        matchMock.Setup(m => m.GetWinner()).Returns(team);

        matchMock.Setup(m => m.GetTeamGoals(team, out It.Ref<int>.IsAny, out It.Ref<int>.IsAny))
            .Callback(new GetTeamGoalsCallback((ITeam t, out int scored, out int conceded) =>
            {
                scored = 2;
                conceded = 1;
            }));

        IMatch match = matchMock.Object;
        MatchEnded matchEnded = new MatchEnded(match);
        TeamGroupStageScore teamGroupStageScore = new TeamGroupStageScore(team);

        MethodInfo? methodInfo = typeof(TeamGroupStageScore).GetMethod("UpdateWithMatchResults", BindingFlags.NonPublic | BindingFlags.Instance);
        methodInfo?.Invoke(teamGroupStageScore, new object[] { matchEnded });

        Assert.AreEqual(1, teamGroupStageScore.Won.Count);
        Assert.AreEqual(match, teamGroupStageScore.Won[0]);
        Assert.AreEqual(3, teamGroupStageScore.Points);
        Assert.AreEqual(2, teamGroupStageScore.For);
        Assert.AreEqual(1, teamGroupStageScore.Against);
        Assert.AreEqual(1, teamGroupStageScore.GoalDifference);
    }

   
    [TestMethod]
    public void CompareTo_ShouldReturnMinusOne_WhenThisTeamHasMorePoints()
    {
        ITeam teamA = new Team("Team A", 0);
        ITeam teamB = new Team("Team B", 0);

        TeamGroupStageScore teamAScore = new TeamGroupStageScore(teamA);
        TeamGroupStageScore teamBScore = new TeamGroupStageScore(teamB);

        Mock<IMatch> matchMock = new Mock<IMatch>();

        matchMock.Setup(m => m.Id).Returns(Guid.NewGuid());
        matchMock.Setup(m => m.Teams).Returns(new List<ITeam> { teamA, teamB }.ToImmutableList());
        matchMock.Setup(m => m.Played).Returns(false);

        matchMock.Setup(m => m.GetWinner()).Returns(teamA);
        matchMock.Setup(m => m.GetTeamGoals(teamA, out It.Ref<int>.IsAny, out It.Ref<int>.IsAny))
            .Callback(new GetTeamGoalsCallback((ITeam t, out int scored, out int conceded) =>
            {
                scored = 1;
                conceded = 0;
            }));
        matchMock.Setup(m => m.GetTeamGoals(teamB, out It.Ref<int>.IsAny, out It.Ref<int>.IsAny))
            .Callback(new GetTeamGoalsCallback((ITeam t, out int scored, out int conceded) =>
            {
                scored = 0;
                conceded = 1;
            }));

        IMatch match = matchMock.Object;
        MatchEnded matchEnded = new MatchEnded(match);

        MethodInfo? methodInfo = typeof(TeamGroupStageScore).GetMethod("UpdateWithMatchResults", BindingFlags.NonPublic | BindingFlags.Instance);
        methodInfo?.Invoke(teamAScore, new object[] { matchEnded });
        methodInfo?.Invoke(teamBScore, new object[] { matchEnded });

        int comparisonResult = teamAScore.CompareTo(teamBScore);

        Assert.AreEqual(-1, comparisonResult);
    }

    [TestMethod]
    public void CompareTo_ShouldReturnOne_WhenThisTeamHasFewerPoints()
    {
        ITeam teamA = new Team("Team A", 0);
        ITeam teamB = new Team("Team B", 0);

        TeamGroupStageScore teamAScore = new TeamGroupStageScore(teamA);
        TeamGroupStageScore teamBScore = new TeamGroupStageScore(teamB);

        Mock<IMatch> matchMock = new Mock<IMatch>();

        matchMock.Setup(m => m.Id).Returns(Guid.NewGuid());
        matchMock.Setup(m => m.Teams).Returns(new List<ITeam> { teamA, teamB }.ToImmutableList());
        matchMock.Setup(m => m.Played).Returns(false);

        matchMock.Setup(m => m.GetWinner()).Returns(teamB);
        matchMock.Setup(m => m.GetTeamGoals(teamA, out It.Ref<int>.IsAny, out It.Ref<int>.IsAny))
            .Callback(new GetTeamGoalsCallback((ITeam t, out int scored, out int conceded) =>
            {
                scored = 0;
                conceded = 1;
            }));
        matchMock.Setup(m => m.GetTeamGoals(teamB, out It.Ref<int>.IsAny, out It.Ref<int>.IsAny))
            .Callback(new GetTeamGoalsCallback((ITeam t, out int scored, out int conceded) =>
            {
                scored = 1;
                conceded = 0;
            }));

        IMatch match = matchMock.Object;
        MatchEnded matchEnded = new MatchEnded(match);

        MethodInfo? methodInfo = typeof(TeamGroupStageScore).GetMethod("UpdateWithMatchResults", BindingFlags.NonPublic | BindingFlags.Instance);
        methodInfo?.Invoke(teamAScore, new object[] { matchEnded });
        methodInfo?.Invoke(teamBScore, new object[] { matchEnded });

        int comparisonResult = teamAScore.CompareTo(teamBScore);

        Assert.AreEqual(1, comparisonResult);
    }

    [TestMethod]
    public void CompareTo_ShouldReturnMinusOne_WhenTeamsHaveSamePointsButThisTeamHasBetterGoalDifference()
    {
        ITeam teamA = new Team("Team A", 0);
        ITeam teamB = new Team("Team B", 0);
        ITeam teamC = new Team("Team C", 0);

        TeamGroupStageScore teamAScore = new TeamGroupStageScore(teamA);
        TeamGroupStageScore teamBScore = new TeamGroupStageScore(teamB);

        MethodInfo? methodInfo = typeof(TeamGroupStageScore).GetMethod("UpdateWithMatchResults", BindingFlags.NonPublic | BindingFlags.Instance);

        Mock<IMatch> matchMockA = new Mock<IMatch>();

        matchMockA.Setup(m => m.Id).Returns(Guid.NewGuid());
        matchMockA.Setup(m => m.Teams).Returns(new List<ITeam> { teamA, teamC }.ToImmutableList());
        matchMockA.Setup(m => m.Played).Returns(false);

        matchMockA.Setup(m => m.GetWinner()).Returns(teamA);
        matchMockA.Setup(m => m.GetTeamGoals(teamA, out It.Ref<int>.IsAny, out It.Ref<int>.IsAny))
            .Callback(new GetTeamGoalsCallback((ITeam t, out int scored, out int conceded) =>
            {
                scored = 3;
                conceded = 0;
            }));
        matchMockA.Setup(m => m.GetTeamGoals(teamC, out It.Ref<int>.IsAny, out It.Ref<int>.IsAny))
            .Callback(new GetTeamGoalsCallback((ITeam t, out int scored, out int conceded) =>
            {
                scored = 0;
                conceded = 3;
            }));
        IMatch matchA = matchMockA.Object;
        MatchEnded matchEndedA = new MatchEnded(matchA);
        methodInfo?.Invoke(teamAScore, new object[] { matchEndedA });

        Mock<IMatch> matchMockB = new Mock<IMatch>();

        matchMockB.Setup(m => m.Id).Returns(Guid.NewGuid());
        matchMockB.Setup(m => m.Teams).Returns(new List<ITeam> { teamB, teamC }.ToImmutableList());
        matchMockB.Setup(m => m.Played).Returns(false);

        matchMockB.Setup(m => m.GetWinner()).Returns(teamB);
        matchMockB.Setup(m => m.GetTeamGoals(teamB, out It.Ref<int>.IsAny, out It.Ref<int>.IsAny))
            .Callback(new GetTeamGoalsCallback((ITeam t, out int scored, out int conceded) =>
            {
                scored = 1;
                conceded = 0;
            }));
        matchMockB.Setup(m => m.GetTeamGoals(teamC, out It.Ref<int>.IsAny, out It.Ref<int>.IsAny))
            .Callback(new GetTeamGoalsCallback((ITeam t, out int scored, out int conceded) =>
            {
                scored = 0;
                conceded = 1;
            }));
        IMatch matchB = matchMockB.Object;
        MatchEnded? matchEndedB = new MatchEnded(matchB);
        methodInfo?.Invoke(teamBScore, new object[] { matchEndedB });

        int comparisonResult = teamAScore.CompareTo(teamBScore);

        Assert.AreEqual(-1, comparisonResult);
    }

    [TestMethod]
    public void CompareTo_ShouldReturnMinusOne_WhenTeamsAreEqualButThisTeamBeatsOtherHeadToHead()
    {
        ITeam teamA = new Team("Team A", 0);
        ITeam teamB = new Team("Team B", 0);
        ITeam teamC = new Team("Team C", 0);

        TeamGroupStageScore teamAScore = new TeamGroupStageScore(teamA);
        TeamGroupStageScore teamBScore = new TeamGroupStageScore(teamB);

        MethodInfo? methodInfo = typeof(ITeamGroupStageScore).GetMethod("UpdateWithMatchResults", BindingFlags.NonPublic | BindingFlags.Instance);

        Mock<IMatch> matchMockAB = new Mock<IMatch>();

        matchMockAB.Setup(m => m.Id).Returns(Guid.NewGuid());
        matchMockAB.Setup(m => m.Teams).Returns(new List<ITeam> { teamA, teamB }.ToImmutableList());
        matchMockAB.Setup(m => m.Played).Returns(false);

        matchMockAB.Setup(m => m.GetWinner()).Returns(teamA);
        matchMockAB.Setup(m => m.GetTeamGoals(teamA, out It.Ref<int>.IsAny, out It.Ref<int>.IsAny))
           .Callback(new GetTeamGoalsCallback((ITeam t, out int scored, out int conceded) =>
           {
               scored = 1;
               conceded = 0;
           }));
        matchMockAB.Setup(m => m.GetTeamGoals(teamB, out It.Ref<int>.IsAny, out It.Ref<int>.IsAny))
           .Callback(new GetTeamGoalsCallback((ITeam t, out int scored, out int conceded) =>
           {
               scored = 0;
               conceded = 1;
           }));
        IMatch matchAB = matchMockAB.Object;
        MatchEnded matchEndedAB = new MatchEnded(matchAB);
        methodInfo?.Invoke(teamAScore, new object[] { matchEndedAB });
        methodInfo?.Invoke(teamBScore, new object[] { matchEndedAB });

        Mock<IMatch> matchMockAC = new Mock<IMatch>();

        matchMockAB.Setup(m => m.Id).Returns(Guid.NewGuid());
        matchMockAB.Setup(m => m.Teams).Returns(new List<ITeam> { teamA, teamC }.ToImmutableList());
        matchMockAB.Setup(m => m.Played).Returns(false);

        matchMockAC.Setup(m => m.GetWinner()).Returns(teamA);
        matchMockAC.Setup(m => m.GetTeamGoals(teamA, out It.Ref<int>.IsAny, out It.Ref<int>.IsAny))
           .Callback(new GetTeamGoalsCallback((ITeam t, out int scored, out int conceded) =>
           {
               scored = 2;
               conceded = 0;
           }));
        matchMockAC.Setup(m => m.GetTeamGoals(teamC, out It.Ref<int>.IsAny, out It.Ref<int>.IsAny))
           .Callback(new GetTeamGoalsCallback((ITeam t, out int scored, out int conceded) =>
           {
               scored = 0;
               conceded = 2;
           }));
        IMatch matchAC = matchMockAC.Object;
        MatchEnded matchEndedAC = new MatchEnded(matchAC);
        methodInfo?.Invoke(teamAScore, new object[] { matchEndedAC });


        Mock<IMatch> matchMockBC = new Mock<IMatch>();

        matchMockAB.Setup(m => m.Id).Returns(Guid.NewGuid());
        matchMockAB.Setup(m => m.Teams).Returns(new List<ITeam> { teamB, teamC }.ToImmutableList());
        matchMockAB.Setup(m => m.Played).Returns(false);

        matchMockBC.Setup(m => m.GetWinner()).Returns(teamB);
        matchMockBC.Setup(m => m.GetTeamGoals(teamB, out It.Ref<int>.IsAny, out It.Ref<int>.IsAny))
           .Callback(new GetTeamGoalsCallback((ITeam t, out int scored, out int conceded) =>
           {
               scored = 2;
               conceded = 0;
           }));
        matchMockBC.Setup(m => m.GetTeamGoals(teamC, out It.Ref<int>.IsAny, out It.Ref<int>.IsAny))
           .Callback(new GetTeamGoalsCallback((ITeam t, out int scored, out int conceded) =>
           {
               scored = 0;
               conceded = 2;
           }));
        IMatch matchBC = matchMockBC.Object;
        MatchEnded matchEndedBC = new MatchEnded(matchBC);
        methodInfo?.Invoke(teamBScore, new object[] { matchEndedBC });

        int comparisonResult = teamAScore.CompareTo(teamBScore);

        Assert.AreEqual(-1, comparisonResult);
    }

    private delegate void GetTeamGoalsCallback(ITeam team, out int scored, out int conceded);
}
