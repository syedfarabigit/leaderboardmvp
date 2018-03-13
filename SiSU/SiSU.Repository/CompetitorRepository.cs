using Dapper;
using Microsoft.Extensions.Configuration;
using SiSU.Model;
using System.Collections.Generic;
using System.Linq;

namespace SiSU.Repository
{
    public interface ICompetitorRepository
    {
        int Create(string name);
        IEnumerable<Competitor> GetByContest(int contestId);
    }

    public class CompetitorRepository : ICompetitorRepository
    {
        readonly string _connectionString;
        public CompetitorRepository(IConfiguration iConfiguration)
        {
            _connectionString = iConfiguration.GetSection("ConnectionString").Value;
        }

        public int Create(string name)
        {
            var competitorId = 0;
            using (var connection = DB.ConnectionFactory(_connectionString))
            {
               connection.Open();
               competitorId = connection.Query<int>(@"insert into Competitor(Name) values(@name); select cast(scope_identity() as int);", new { name }).Single();
            }
            return competitorId;
        }

        public IEnumerable<Competitor> GetByContest(int contestId)
        {
            IEnumerable<Competitor> competitors;
            using (var connection = DB.ConnectionFactory(_connectionString))
            {
                connection.Open();
                competitors = connection.Query<Competitor>(@"select c.CompetitorId, c.Name from ContestCompetitor cc join Competitor c on cc.CompetitorId = c.CompetitorId where cc.ContestId = @contestId", new { contestId });
            }
            return competitors;
        }
    }
}
