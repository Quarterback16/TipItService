namespace TipItService.Domain
{
    public class EasySelection
    {
        public string LeagueCode { get; set; } = "NRL";
        public string TeamCode { get; set; }
        public int ExpectedWins { get; set; }
        public int PointsPerWin { get; set; }
    }
}
