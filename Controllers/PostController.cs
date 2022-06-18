using Blog.Data;
using Blog6.ViewModels.Posts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog6.Controllers
{
  [ApiController]
  public class PostController : ControllerBase
  {
    [HttpGet("v1/posts")]
    public async Task<IActionResult> GetAllPostsAsync([FromServices] BlogDataContext context)
    {
      var posts = await context
                          .Posts
                          .AsNoTracking()
                          .Include(p => p.Category)
                          .Include(p => p.Author)
                          .Select(p => new ListPostsViewModel
                          {
                            Id = p.Id,
                            Title = p.Title,
                            Slug = p.Slug,
                            LastUpdateDate = p.LastUpdateDate,
                            Category = p.Category.Name,
                            Author = p.Author.Name
                          })
                          .ToListAsync();
      return Ok(posts);
    }
  }
}