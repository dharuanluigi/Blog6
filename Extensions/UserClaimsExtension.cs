using Blog.Models;
using System.Security.Claims;

namespace Blog6.Extensions
{
  public static class UserClaimsExtension
  {
    public static IEnumerable<Claim> GetClaims(this User user)
    {
      var result = new List<Claim>();

      result.Add(new Claim(ClaimTypes.Name, user.Email));

      result.AddRange(user.Roles.Select(r => new Claim(ClaimTypes.Role, r.Name)));

      return result;
    }
  }
}