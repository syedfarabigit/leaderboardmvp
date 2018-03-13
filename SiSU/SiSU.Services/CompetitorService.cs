using SiSU.Repository;
using System.Collections.Generic;

namespace SiSU.Services
{
    public interface ICompetitorService
    {
        int Create(string name);
    }

    public class CompetitorService : ICompetitorService
    {
        private ICompetitorRepository _competitorRepo;

        public CompetitorService(ICompetitorRepository competitorRepo)
        {
            _competitorRepo = competitorRepo;
        }


        public int Create(string name)
        {
            return _competitorRepo.Create(name);
        }
    }
}
