using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SiSU.Infrastructure;
using SiSU.Model;
using SiSU.Services;

namespace SiSU.Controllers
{
    [Route("api/contest")]
    public class ContestController : Controller
    {

        IContestService _contestService;
        public ContestController(IContestService contestService)
        {
            _contestService = contestService;
        }

        [Authorize(Policy = nameof(SiSUAuthorizationPolicy.RefreeAccess))]
        [Route("create")]
        [HttpPost]
        public IActionResult Create([FromBody] CreateContestModel model)
        {
            return Ok(new { ContestId = _contestService.Create(model.Name, model.TournamentId, model.CompetitorIds) });
        }

        [Authorize(Policy = nameof(SiSUAuthorizationPolicy.RefreeAccess))]
        [Route("save-result")]
        [HttpPost]
        public IActionResult SaveResult([FromBody] ContestResultModel model)
        {
            _contestService.SaveResult(model.ContestId, model.IsDrawn, model.WinnerId);
            return Ok(new {Message = "Result saved" });
        }
    }
}