using Blog.Data;
using Blog.Models;
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

    [HttpGet("v1/posts/{id:int}")]
    public async Task<IActionResult> GetAPostByIdAsync([FromRoute] int id, [FromServices] BlogDataContext context)
    {
      try
      {
        var post = await context
          .Posts
          .AsNoTracking()
          .Include(p => p.Author)
            .ThenInclude(a => a.Roles)
          .Include(p => p.Category)
          .FirstOrDefaultAsync(p => p.Id == id);

        if (post is null)
        {
          return NotFound(new ResultViewModel<string>("No post was found"));
        }

        return Ok(new ResultViewModel<Post>(post));
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
        return StatusCode(500, new ResultViewModel<string>("Ocurred an internal server error when try to retrive a post by id"));
      }
    }

    [HttpGet("v1/posts/category/{category}")]
    public async Task<IActionResult> GetPostsByCategoryAsync([FromRoute] string category, [FromServices] BlogDataContext context, [FromQuery] int page = 0, [FromQuery] int pageSize = 20)
    {
      try
      {
        var posts = await context
          .Posts
          .AsNoTracking()
          .Include(p => p.Author)
          .Include(p => p.Category)
          .Where(p => p.Category.Name == category)
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
          return NotFound(new ResultViewModel<string>($"No posts was found by informed category: {category}"));
        }

        var result = new ResultPostsViewModel
        {
          Total = posts.Count,
          Page = page,
          PageSize = pageSize,
          Posts = posts
        };

        return Ok(new ResultViewModel<ResultPostsViewModel>(result));
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
        return StatusCode(500, new ResultViewModel<string>("Occurred an internal server error when try to get posts by category"));
      }
    }
  }
}