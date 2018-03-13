using SiSU.Infrastructure;
using SiSU.Model;
using SiSU.Repository;
using System.Collections.Generic;
using System.Linq;

namespace SiSU.Services
{
    public interface ILeaderboardService
    {
        void Add(int contestId, int competitorId, GameResult gameResult);
        IEnumerable<Leaderboard> GetAllByTournamentId(int tournamentId);
        IEnumerable<Leaderboard> GetPublicOnlyByTournamentId(int tournamentId);
        void MakePublic(int contestId);
    }

    public class LeaderboardService : ILeaderboardService
    {
        private ILeaderboardRepository _leaderboardRepo;

        public LeaderboardService(ILeaderboardRepository leaderboardRepo)
        {
            _leaderboardRepo = leaderboardRepo;
        }


        public void Add(int contestId, int competitorId, GameResult gameResult)
        {
            _leaderboardRepo.Add(contestId, competitorId, gameResult);
        }


        public IEnumerable<Leaderboard> GetAllByTournamentId(int tournamentId)
        {
            var leaderboardItems = _leaderboardRepo.GetAllByTournamentId(tournamentId);
            return GenerateLeaderboard(leaderboardItems);
        }

        public IEnumerable<Leaderboard> GetPublicOnlyByTournamentId(int tournamentId)
        {
            var leaderboardItems = _leaderboardRepo.GetPublicOnlyByTournamentId(tournamentId);
            return GenerateLeaderboard(leaderboardItems);
        }

        public void MakePublic(int contestId)
        {
            _leaderboardRepo.MakePublic(contestId);
        }

        private IEnumerable<Leaderboard> GenerateLeaderboard(IEnumerable<LeaderboardItem> leaderboardItems)
        {
            var query = from li in leaderboardItems
                        group li by new { tournamentId = li.TournamentId, compId = li.CompetitorId, compName = li.CompetitorName } into grp
                        select new Leaderboard
                        {
                            TournamentId = grp.Key.tournamentId,
                            CompetitorName = grp.Key.compName,
                            TotalPlayed = grp.Count(),
                            TotalDrawn = grp.Count(g => g.GameResult == GameResult.Drawn),
                            TotalLost = grp.Count(g => g.GameResult == GameResult.Lost),
                            TotalWon = grp.Count(g => g.GameResult == GameResult.Won),
                        };

            return query.ToList();
        }
    }
}
