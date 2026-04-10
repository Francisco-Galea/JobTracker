using System.Security.Claims;
using JobTracker.Application.UseCases.GetAnalyticsSummary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobTracker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AnalyticsController : ControllerBase
    {
        private readonly GetAnalyticsSummaryHandler handler;

        public AnalyticsController(GetAnalyticsSummaryHandler handler)
        {
            this.handler = handler;
        }

        private Guid GetCurrentUserId() =>
            Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        [HttpGet("summary")]
        public async Task<IActionResult> GetSummary(CancellationToken cancellationToken)
        {
            var query = new GetAnalyticsSummaryQuery(GetCurrentUserId());
            var result = await handler.HandleAsync(query, cancellationToken);
            return Ok(result);
        }
    }
}
