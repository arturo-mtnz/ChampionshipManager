namespace ChampionshipManager.Core.Services;

using System.Linq;
using ChampionshipManager.Core.Domain;
using ChampionshipManager.Core.Domain.Contracts;
using ChampionshipManager.Core.Repositories.Contracts;
using ChampionshipManager.Core.Services.Contracts;

/// <summary>
/// Implements the simulation of the group stage of the championship
/// </summary>
internal class GroupStageService : IGroupStageService
{
    public GroupStageService(ITeamRepository teamRepository,
        IMatchRepository matchRepository,
        ITeamGroupStageScoreRepository teamGroupStageScoreRepository)
    {
        this.TeamRepository = teamRepository;
        this.MatchRepository = matchRepository;
        this.TeamGroupStageScoreRepository = teamGroupStageScoreRepository;
    }

    private ITeamRepository TeamRepository { get; }
    private IMatchRepository MatchRepository { get; }
    private ITeamGroupStageScoreRepository TeamGroupStageScoreRepository { get; }

    public void CreateMatches()
    {
        List<Team> allTeams = this.TeamRepository.GetAll().ToList();

        if (allTeams.Count != 4)
        {
            throw new InvalidOperationException("It's only possible to simulate a group stage for 4 teams");
        }

        List<Match> matches = new List<Match>();

        for (int i = 0; i < allTeams.Count; i++)
        {
            for (int j = i + 1; j < allTeams.Count; j++)
            {
                Team homeTeam = allTeams[i];
                Team awayTeam = allTeams[j];

                Match newMatch = new Match(Guid.NewGuid(), homeTeam, awayTeam);
                matches.Add(newMatch);
                this.MatchRepository.Create(newMatch);
            }
        }
    }

    public void Simulate()
    {
        List<Team> allTeams = this.TeamRepository.GetAll().ToList();
        Standings standings = new Standings(allTeams);

        List<Match> allMatches = this.MatchRepository.GetAll().ToList();

        if (allMatches.Count != 6)
        {
            throw new InvalidOperationException("It's only possible to simulate a group stage of 6 matches");
        }

        foreach (IMatch match in allMatches)
        {
            match.Simulate();
        }
    }

    public List<ITeamGroupStageScore> GetStandings()
    {
        IList<ITeamGroupStageScore> scores = this.TeamGroupStageScoreRepository
            .GetAll()
            .ToList<ITeamGroupStageScore>();

        Standings standings = new Standings(scores.ToList());
        return standings.TeamScores;
    }

    public List<ITeam> GetQualifiedTeams()
    {
        IList<ITeamGroupStageScore> scores = this.TeamGroupStageScoreRepository
                   .GetAll()
                   .ToList<ITeamGroupStageScore>();

        Standings standings = new Standings(scores.ToList());
        return standings.QualifiedTeams;
    }

    public void ResetSimulation()
    {
        this.MatchRepository.DeleteAll();
        this.TeamGroupStageScoreRepository.DeleteAll();
    }
}
