using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SiSU.Infrastructure;
using SiSU.Model;
using SiSU.Services;

namespace SiSU.Controllers
{
    [Route("api/competitor")]
    public class CompetitorController : Controller
    {

        ICompetitorService _competitorService;
        public CompetitorController(ICompetitorService competitorService)
        {
            _competitorService = competitorService;
        }

        [Authorize(Policy = nameof(SiSUAuthorizationPolicy.RefreeAccess))]
        [Route("create")]
        [HttpPost]
        public IActionResult Create([FromBody] CreateCompetitorModel model)
        {
            return Ok(new { CompetitorId = _competitorService.Create(model.Name) });
        }
    }
}