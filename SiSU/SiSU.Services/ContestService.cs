using SiSU.Infrastructure;
using SiSU.Repository;
using System.Collections.Generic;
using System.Linq;

namespace SiSU.Services
{
    public interface IContestService
    {
        int Create(string name, int tournamentId, List<int> competitorIds);
        void SaveResult(int contestId, bool isDrawn, int? winnerId);
    }

    public class ContestService : IContestService
    {
        private readonly IContestRepository _contestRepo;
        private readonly ILeaderboardService _leaderboardService;
        private readonly ICompetitorRepository _competitorRepository;

        public ContestService(IContestRepository contestRepo, ILeaderboardService leaderboardService, ICompetitorRepository competitorRepository)
        {
            _contestRepo = contestRepo;
            _leaderboardService = leaderboardService;
            _competitorRepository = competitorRepository;
        }

        public int Create(string name, int tournamentId, List<int> competitorIds)
        {
            return _contestRepo.Create(name, tournamentId, competitorIds);
        }

        public void SaveResult(int contestId, bool isDrawn, int? winnerId)
        {
            //validate if contestId is prsent in our system
            var contest = _contestRepo.Get(contestId);
            if (contest == null)
            {
                throw new System.ArgumentException("There are no Contest found with the ContestId");
            }

            //todo: validate if Contest is already decided and saved
                        
            var competitors = _competitorRepository.GetByContest(contestId);
            if (!competitors.Any() || competitors.Count() < 2)
            {
                throw new System.ArgumentException("There are no participant found with the ContestId");
            }

            //validate if winnerId belongs to the Right Contest
            if (winnerId.HasValue)
            {
                if (!competitors.Any(c => c.CompetitorId == winnerId.Value))
                {
                    throw new System.ArgumentException("WinnerId is not right");
                }
            }
            
            var matchResult = new MatchResult(contestId);
            foreach (var competitor in competitors)
            {
                var participant = new Participant(_leaderboardService)
                {
                    ContestId = contestId,
                    CompetitorId = competitor.CompetitorId
                };
                matchResult.AddParticipant(participant);
            }
            
            matchResult.IsDrawn = isDrawn;
            matchResult.WinnerId = winnerId;
            matchResult.Notify();
        }
    }
}
