using Dapper;
using Microsoft.Extensions.Configuration;
using SiSU.Model;
using System.Collections.Generic;
using System.Linq;

namespace SiSU.Repository
{
    public interface IContestRepository
    {
        int Create(string name, int tournamentId, List<int> competitorIds);
        Contest Get(int contestId);
    }

    public class ContestRepository : IContestRepository
    {

        readonly string _connectionString;
        public ContestRepository(IConfiguration iConfiguration)
        {
            _connectionString = iConfiguration.GetSection("ConnectionString").Value;
        }

        public int Create(string name, int tournamentId, List<int> competitorIds)
        {
            var contestId = 0;
            using (var connection = DB.ConnectionFactory(_connectionString))
            {
                connection.Open();
                contestId = connection.Query<int>(@"insert into Contest(Name, TournamentId) values(@name, @tournamentId); select cast(scope_identity() as int);", new { name, tournamentId }).Single();
                foreach (var competitorId in competitorIds)
                {
                    connection.Execute(@"insert into ContestCompetitor(ContestId, CompetitorId) values(@contestId, @competitorId)", new { contestId, competitorId });
                }
            }
            return contestId;
        }

        public Contest Get(int contestId)
        {
            Contest contest;
            using (var connection = DB.ConnectionFactory(_connectionString))
            {
                connection.Open();
                contest = connection.Query<Contest>(@"select Name, TournamentId from Contest where ContestId = @contestId", new { contestId }).Single();
            }
            return contest;
        }


    }
}
