using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SiSU.Infrastructure;
using SiSU.Model;
using SiSU.Services;

namespace SiSU.Controllers
{
    [Route("api/leaderboard")]
    public class LeaderboardController : Controller
    {

        private readonly ILeaderboardService _leaderboardService;

        public LeaderboardController(ILeaderboardService leaderboardService)
        {
            _leaderboardService = leaderboardService;
        }

        [Route("get/{tournamentId}")]
        [HttpGet]
        public IActionResult Get(int tournamentId)
        {
            return Ok(new { Leaderboard = _leaderboardService.GetPublicOnlyByTournamentId(tournamentId) }) ;
        }


        [Authorize(Policy = nameof(SiSUAuthorizationPolicy.MemberAccess))]
        [Route("get-all/{tournamentId}")]
        [HttpGet]
        public IActionResult GetAll(int tournamentId)
        {
            return Ok(new { Leaderboard = _leaderboardService.GetAllByTournamentId(tournamentId) });
        }


        [Authorize(Policy = nameof(SiSUAuthorizationPolicy.RefreeAccess))]
        [Route("make-public")]
        [HttpPost]
        public IActionResult MakePublic([FromBody]  MakeLeaderboardPublicModel model)
        {
            _leaderboardService.MakePublic(model.ContestId);
            return Ok(new { Message ="Leader board for the contest made public" });
        }
    }
}