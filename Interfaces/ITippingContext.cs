using System;
using System.Collections.Generic;
using TipItService.Models;

namespace TipItService.Interfaces
{
    public interface ITippingContext : IDisposable
    {
        Dictionary<string, Dictionary<int, List<Game>>> LeagueSchedule { get; set; }
        Dictionary<string, List<Team>> LeagueDict { get; set; }
        int LeagueCount();
        int ScheduledRoundCount(
            string leagueCode);
        int ScheduledGameCount(
            string leagueCode,
            int round);
        void ProcessLeagueSchedule(
            string leagueCode,
            IGameProcessor processor);

        List<Game> MissingResults(
            string leagueCode,
            DateTime queryDate);
    }
}
