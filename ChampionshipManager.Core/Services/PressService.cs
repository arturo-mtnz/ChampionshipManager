namespace ChampionshipManager.Core.Services;

using System;
using ChampionshipManager.Core.Events;
using ChampionshipManager.Core.Services.Contracts;

/// <summary>
/// This class observes the matches and championship results and tells the press the breaking news. 
/// 
/// NB! We just write a preset message in console to prove the potential.
/// </summary>
internal class PressService : IPressService
{
    public PressService()
    {
        MediatorProvider<MatchEnded>.Subscribe(InformAboutMatchResults);
        MediatorProvider<GroupStageEnded>.Subscribe(InformAboutGroupResults);
    }

    /// <summary>
    /// Simulates informing the press about ended match results. 
    /// </summary>
    /// <param name="matchEnded">The event containing the information of the match</param>
    private void InformAboutMatchResults(MatchEnded matchEnded)
    {
        Console.WriteLine("Breaking news! Match ended.");
    }

    /// <summary>
    /// Simulates informing the press about ended match results. 
    /// </summary>
    /// <param name="groupPhaseEnded">The event containing the information of the group stage</param>
    private void InformAboutGroupResults(GroupStageEnded groupPhaseEnded)
    {
        Console.WriteLine("Breaking news! Group stage ended.");
    }

}
