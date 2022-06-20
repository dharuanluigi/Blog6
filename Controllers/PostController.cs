using Blog.Data;
using Blog6.ViewModels;
using Blog6.ViewModels.Posts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog6.Controllers
{
  [ApiController]
  public class PostController : ControllerBase
  {
    [HttpGet("v1/posts")]
    public async Task<IActionResult> GetAllPostsAsync([FromServices] BlogDataContext context, [FromQuery] int page = 0, [FromQuery] int pageSize = 20)
    {
      try
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
                          .Skip(page * pageSize)
                          .Take(pageSize)
                          .OrderByDescending(p => p.LastUpdateDate)
                          .ToListAsync();

        if (posts is null || posts.Count == 0)
        {
          return NotFound(new ResultViewModel<string>("No posts was found"));
        }

        var result = new ResultPostsViewModel
        {
          Total = posts.Count,
          Page = page,
          PageSize = pageSize,
          Posts = posts
        };

        return Ok(new ResultViewModel<ResultPostsViewModel>(result, null));
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
        return StatusCode(500, new ResultViewModel<string>("Ocurrend an internal server error when try to retrive all posts"));
      }
    }
  }
}