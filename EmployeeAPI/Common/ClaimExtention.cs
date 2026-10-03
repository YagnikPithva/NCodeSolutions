namespace EmployeeAPI.Common;

using System.Security.Claims;

public static class ClaimsExtensions
{
    public static int? GetInt(this ClaimsPrincipal user, string claimType) =>
        int.TryParse(user.FindFirstValue(claimType), out var v) ? v : null;
}
