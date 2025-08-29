using Microsoft.AspNetCore.Mvc;
using UdemyClone.Api.DTOs;
using UdemyClone.Api.Services;

namespace UdemyClone.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EnrollmentsController : ControllerBase
{
    private readonly IEnrollmentService _svc;

    public EnrollmentsController(IEnrollmentService svc)
    {
        _svc = svc;
    }

    [HttpPost]
    public async Task<IActionResult> Enroll([FromBody] EnrollRequest req)
    {
        var (ok, err, enr) = await _svc.EnrollAsync(req.UserId, req.CourseId);
        return ok
            ? Created($"/api/enrollments/{enr!.Id}", enr)
            : BadRequest(err);
    }
}
