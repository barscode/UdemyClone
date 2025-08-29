using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using UdemyClone.Api.Services;

namespace UdemyClone.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MessagesController : ControllerBase
{
    private readonly IMessageService _svc;

    public MessagesController(IMessageService svc)
    {
        _svc = svc;
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> SendMessage(int recipientId, string title, string content)
    {
        var sub = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!int.TryParse(sub, out var senderId))
            return Unauthorized();

        var (ok, err, msg) = await _svc.SendMessageAsync(senderId, recipientId, title, content);
        return ok
            ? Created($"/api/messages/{msg!.Id}", msg)
            : BadRequest(err);
    }
}
