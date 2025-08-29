using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using UdemyClone.Api.Application.UseCases;
using UdemyClone.Api.DTOs;

namespace UdemyClone.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CouponsController : ControllerBase
{
    private readonly CreateCouponUseCase _useCase;

    public CouponsController(CreateCouponUseCase useCase)
    {
        _useCase = useCase;
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateCoupon([FromBody] CreateCouponRequest req)
    {
        var sub = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!int.TryParse(sub, out var instructorId))
            return Unauthorized();

        var (success, error, coupon) = await _useCase.ExecuteAsync(instructorId, req);
        return success
            ? Created($"/api/coupons/{coupon!.Id}", coupon)
            : BadRequest(error);
    }
}
