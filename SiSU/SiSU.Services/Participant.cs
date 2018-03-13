using SiSU.Infrastructure;

namespace SiSU.Services
{
    public class Participant : IParticipant
    {
        private readonly ILeaderboardService _leaderboardService;
        public int ContestId;
        public int CompetitorId;

        public Participant(ILeaderboardService leaderboardService)
        {
            _leaderboardService = leaderboardService;
        }

        public void Update(bool isDrawn, int? winnerId)
        {
            GameResult gameResult;
            if (isDrawn)
            {
                gameResult = GameResult.Drawn;
            }
            else
            {
                gameResult = winnerId == CompetitorId ? GameResult.Won : GameResult.Lost;
            }
            _leaderboardService.Add(ContestId, CompetitorId, gameResult);
        }
    }
}
