
using SiSU.Infrastructure;

namespace SiSU.Model
{
    public class LeaderboardItem
    {
        public int LeaderboardId { get; set; }
        public int TournamentId { get; set; }
        public int ContestId { get; set; }
        public int CompetitorId { get; set; }
        public GameResult GameResult { get; set; }
        public string ContestName { get; set; }
        public string CompetitorName { get; set; }
        public int Point
        {
            get
            {
                if (GameResult == GameResult.Drawn)
                    return 1;
                if (GameResult == GameResult.Lost)
                    return 0;
                return 2;
            }
        }
    }
}
