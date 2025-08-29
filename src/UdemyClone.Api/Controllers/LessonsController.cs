using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using UdemyClone.Api.Services;

namespace UdemyClone.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LessonsController : ControllerBase
{
    private readonly ILessonCompletionService _svc;

    public LessonsController(ILessonCompletionService svc)
    {
        _svc = svc;
    }

    [Authorize]
    [HttpPost("{lessonId:int}/complete")]
    public async Task<IActionResult> Complete(int lessonId)
    {
        var sub = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!int.TryParse(sub, out var userId)) return Unauthorized();
        var (ok, err, comp) = await _svc.MarkCompletedAsync(userId, lessonId);
        return ok
            ? Created($"/api/lessons/{lessonId}/complete/{comp!.Id}", comp)
            : BadRequest(err);
    }
}
