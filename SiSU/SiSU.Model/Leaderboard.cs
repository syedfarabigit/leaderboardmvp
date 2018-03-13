
namespace SiSU.Model
{
    public class Leaderboard
    {
        public int TournamentId {get;set;}
        public string CompetitorName { get; set; }
        public int TotalPlayed { get; set; }
        public int TotalDrawn { get; set; }
        public int TotalWon { get; set; }
        public int TotalLost { get; set; }
        public int TotalPoint=> (TotalWon * 2) + (TotalLost * 0) + (TotalDrawn * 1);
    }
}
