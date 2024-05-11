using System;

namespace TipItService.Domain
{
    public class LeagueTeam
    {
        public string Code{ get; }
        public string Name { get; }
        public string League { get;  }
        private LeagueTeam(
            string code,
            string name,
            string league)
        {
            Code = code;
            Name = name;
            League = league;
        }

        public static LeagueTeam From(
            LeagueCode leagueCode,
            string teamCode)
        {
            if (string.IsNullOrEmpty(teamCode))
            {
                throw new ArgumentException(
                    "No team code", nameof(teamCode));
            }
            switch (leagueCode.Code)
            {
                case "NRL":
                    return new LeagueTeam(
                        teamCode, 
                        NrlNameFor(teamCode),
                        leagueCode.Code);
                case "AFL":
                    return new LeagueTeam(
                        teamCode, 
                        AflNameFor(teamCode),
                        leagueCode.Code);
                default:
                    throw new ArgumentException(
                        "Invalid league code", 
                        nameof(leagueCode));
            }
        }

        private static string NrlNameFor(
            string teamCode)
        {
            return teamCode;
        }

        private static string AflNameFor(
            string teamCode)
        {
            return teamCode;
        }

        public string JsonCode() =>
        
            JsonCode(Code,League);
        

        public static string JsonCode(
            string teamCode,
            string leagueCode)
        {
            // what the team is called in the JSON results file
            if (leagueCode == "NRL")
            {
                switch (teamCode)
                {
                    case "MANL":
                        return "Sea Eagles";
                    case "SSYD":
                        return "Rabbitohs";
                    case "SYDR":
                        return "Roosters";
                    case "BRIS":
                        return "Broncos";
                    case "CANB":
                        return "Raiders";
                    case "NEWC":
                        return "Knights";
                    case "NZW":
                        return "Warriors";
                    case "SHRK":
                        return "Sharks";
                    case "MELB":
                        return "Storm";
                    case "PENR":
                        return "Panthers";
                    case "PARR":
                        return "Eels";
                    case "CANT":
                        return "Bulldogs";
                    case "TITN":
                        return "Titans";
                    case "DRAG":
                        return "Dragons";
                    case "DOLP":
                        return "Dolphins";
                    case "NQLD":
                        return "Cowboys";
                    case "BULL":
                        return "Bulldogs";
                    case "WTIG":
                        return "Wests Tigers";

                    default:
                        return $"{teamCode} invalid";
                }
            }
            else if (leagueCode == "AFL")
            {
                switch (teamCode)
                {
                    case "BL":
                        return "Brisbane Lions";
                    case "FRE":
                        return "Fremantle";
                    case "WB":
                        return "Western Bulldogs";
                    case "ESS":
                        return "Essendon";
                    case "GWS":
                        return "GWS Giants";
                    case "STK":
                        return "St Kilda";
                    case "CARL":
                        return "Carlton";
                    case "ADEL":
                        return "Adelaide Crows";
                    case "PORT":
                        return "Port Adelaide";
                    case "GCFC":
                        return "Gold Coast Suns";
                    case "HAW":
                        return "Hawthorn";
                    case "GEEL":
                        return "Geelong Cats";
                    case "NMFC":
                        return "North Melbourne";
                    case "WCE":
                        return "West Coast Eagles";
                    case "RICH":
                        return "Richmond";
                    case "MELB":
                        return "Melbourne";
                    case "COLL":
                        return "Collingwood";
                    case "SYD":
                        return "Sydney Swans";
                    default:
                        return $"{teamCode} invalid";
                }
            }
            else
                return "Unknown league code";
        }

        public override string ToString() => JsonCode();
    }
}
