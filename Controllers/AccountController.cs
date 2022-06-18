using Blog.Data;
using Blog.Models;
using Blog6.Extensions;
using Blog6.Services;
using Blog6.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureIdentity.Password;

namespace Blog6.Controllers
{
  [ApiController]
  public class AccountController : ControllerBase
  {
    [HttpPost("v1/account")]
    public async Task<IActionResult> CreateUserAsync([FromBody] CreateUserViewModel createUserViewModel, [FromServices] SendEmailService emailSender, [FromServices] BlogDataContext context)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));
      }

      var user = new User
      {
        Name = createUserViewModel.Name,
        Email = createUserViewModel.Email,
        Slug = createUserViewModel.Email.NormalizeSlug()
      };

      var password = PasswordGenerator.Generate(25);
      user.Password = PasswordHasher.Hash(password);

      try
      {
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();

        emailSender.Send(user.Name, user.Email, "Password access", $"Your password is: <strong>{password}</strong>");

        var result = new ResultUserViewModel
        {
          Email = user.Email,
          Password = password
        };

        return Ok(new ResultViewModel<ResultUserViewModel>(result));
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
        return StatusCode(500, new ResultViewModel<string>("Occurred an error when try to save user in database"));
      }
    }

    [HttpPost("v1/account/login")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginViewModel modelLogin, [FromServices] BlogDataContext context, [FromServices] TokenService tokenService)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));
      }

      var user = await context
                        .Users
                        .AsNoTracking()
                        .Include(u => u.Roles)
                        .FirstOrDefaultAsync(u => modelLogin.Email == u.Email);

      if (user is null)
      {
        return Unauthorized(new ResultViewModel<string>("Incorrect username or password."));
      }

      if (!PasswordHasher.Verify(user.Password, modelLogin.Password))
      {
        return Unauthorized(new ResultViewModel<string>("Incorrect username or password."));
      }

      try
      {
        var token = tokenService.GenerateToken(user);
        return Ok(new ResultViewModel<string>(token, null));
      }
      catch
      {
        return StatusCode(500, new ResultViewModel<string>("An error occurred when try to login"));
      }
    }
  }
}