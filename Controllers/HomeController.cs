using Microsoft.AspNetCore.Mvc;

namespace Blog6.Controllers
{
  [ApiController]
  [Route("")]
  public class HomeController : ControllerBase
  {
    [HttpGet("")]
    public IActionResult Get()
    {
      return Ok();
    }
  }
}