using Blog6.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Blog6.Controllers
{
  [ApiController]
  [Route("")]
  public class HomeController : ControllerBase
  {
    [HttpGet("")]
    [ApiKey]
    public IActionResult Get()
    {
      return Ok();
    }
  }
}