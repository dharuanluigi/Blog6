using Blog.Models;
using System.Security.Claims;

namespace Blog6.Extensions
{
  public static class UserClaimsExtension
  {
    public static IEnumerable<Claim> GetClaims(this User user)
    {
      var result = new List<Claim>
      {
#pragma warning disable CS8604 // Possible null reference argument.
        new Claim(ClaimTypes.Name, user.Email)
      };
#pragma warning restore CS8604 // Possible null reference argument.

#pragma warning disable CS8604 // Possible null reference argument.
      result.AddRange(user.Roles.Select(r => new Claim(ClaimTypes.Role, r.Name)));
#pragma warning restore CS8604 // Possible null reference argument.

      return result;
    }
  }
}