using UdemyClone.Api.DTOs;
using UdemyClone.Api.Domain;

namespace UdemyClone.Api.Services;

public interface ICouponService
{
    Task<(bool Success, string? Error, Kupon? Coupon)> CreateCouponAsync(CreateCouponRequest req);
}
