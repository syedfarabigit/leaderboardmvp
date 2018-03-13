using Microsoft.Extensions.Configuration;
using Dapper;
using SiSU.Infrastructure;
using System.Collections.Generic;
using SiSU.Model;

namespace SiSU.Repository
{
    public interface ILeaderboardRepository
    {
        void Add(int contestId, int competitorId, GameResult gameResult);
        IEnumerable<LeaderboardItem> GetAllByTournamentId(int tournamentId);
        IEnumerable<LeaderboardItem> GetPublicOnlyByTournamentId(int tournamentId);
        void MakePublic(int contestId);
    }

    public class LeaderboardRepository : ILeaderboardRepository
    {
        readonly string _connectionString;
        public LeaderboardRepository(IConfiguration iConfiguration)
        {
            _connectionString = iConfiguration.GetSection("ConnectionString").Value;
        }

        public void Add(int contestId, int competitorId, GameResult gameResult)
        {
            using (var connection = DB.ConnectionFactory(_connectionString))
            {
                connection.Open();
                connection.Execute(@"insert into [Leaderboard](ContestId, CompetitorId, GameResult, IsPublic) values(@contestId, @competitorId, @gameResult, @isPublic)", new { contestId, competitorId, @gameResult = (int)gameResult, @isPublic = false });
            }
        }

        public IEnumerable<LeaderboardItem> GetAllByTournamentId(int tournamentId)
        {
            using (var connection = DB.ConnectionFactory(_connectionString))
            {
                connection.Open();
                var sql = @"select lb.LeaderboardId, c.TournamentId, c.ContestId, comp.CompetitorId, c.Name as ContestName, comp.Name as CompetitorName, lb.GameResult from Leaderboard lb join Contest c on lb.ContestId = c.ContestId join Competitor comp on lb.CompetitorId = comp.CompetitorId  where c.TournamentId = @tournamentId";
                return connection.Query<LeaderboardItem>(sql, new { tournamentId });
            }
        }

        public IEnumerable<LeaderboardItem> GetPublicOnlyByTournamentId(int tournamentId)
        {
            using (var connection = DB.ConnectionFactory(_connectionString))
            {
                connection.Open();
                var sql = @"select lb.LeaderboardId, c.TournamentId, c.ContestId, comp.CompetitorId, c.Name as ContestName, comp.Name as CompetitorName, lb.GameResult from Leaderboard lb join Contest c on lb.ContestId = c.ContestId join Competitor comp on lb.CompetitorId = comp.CompetitorId  where c.TournamentId = @tournamentId and lb.IsPublic = 1";
                return connection.Query<LeaderboardItem>(sql, new { tournamentId });
            }
        }

        public void MakePublic(int contestId)
        {
            using (var connection = DB.ConnectionFactory(_connectionString))
            {
                connection.Open();
                var sql = @"update Leaderboard set IsPublic = 1 where ContestId = @contestId";
                connection.Execute(sql, new { contestId });
            }
        }
    }
}
