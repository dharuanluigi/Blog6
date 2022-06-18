using Blog.Data;
using Blog.Models;
using Blog6.Extensions;
using Blog6.ViewModels;
using Blog6.ViewModels.Categories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog6.Controllers
{
  [ApiController]
  public class CategoryController : ControllerBase
  {
    [HttpGet("v1/categories")]
    public async Task<IActionResult> GetAllAsync([FromServices] BlogDataContext context)
    {
      try
      {
        var categories = await context.Categories.ToListAsync();
        return Ok(new ResultViewModel<IList<Category>>(categories));
      }
      catch
      {
        return StatusCode(500, new ResultViewModel<IList<Category>>("Error when try to get categories in database"));
      }
    }

    [HttpGet("v1/categories/{id:int}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int id, [FromServices] BlogDataContext context)
    {
      try
      {
        var category = await context.Categories.FirstOrDefaultAsync(c => c.Id == id);

        if (category is null)
        {
          return NotFound(new ResultViewModel<Category>("Category not found."));
        }

        return Ok(new ResultViewModel<Category>(category));
      }
      catch
      {
        return StatusCode(500, new ResultViewModel<Category>("An error occurred when try to get the category by id internal server error"));
      }
    }

    [HttpPost("v1/categories")]
    public async Task<IActionResult> PostAsync([FromBody] EditorCategoryViewModel categoryViewModel, [FromServices] BlogDataContext context)
    {
      try
      {
        if (!ModelState.IsValid)
        {
          return BadRequest(new ResultViewModel<Category>(ModelState.GetErrors()));
        }

        var category = new Category
        {
          Name = categoryViewModel.Name,
          Slug = categoryViewModel.Slug.ToLower(),
          Posts = null
        };

        await context.Categories.AddAsync(category);
        await context.SaveChangesAsync();

        return Created($"v1/categories/{category.Id}", new ResultViewModel<Category>(category));
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
        return StatusCode(500, new ResultViewModel<Category>("An error ocurrend when try to insert a category"));
      }
    }

    [HttpPut("v1/categories/{id:int}")]
    public async Task<IActionResult> PutAsync([FromRoute] int id, [FromBody] EditorCategoryViewModel newCategory, [FromServices] BlogDataContext context)
    {
      try
      {
        var oldCategory = await context.Categories.FirstOrDefaultAsync(c => c.Id == id);

        if (oldCategory is null)
        {
          return NotFound(new ResultViewModel<Category>("Category not found."));
        }

        oldCategory.Name = newCategory.Name;
        oldCategory.Slug = newCategory.Slug;

        context.Categories.Update(oldCategory);
        await context.SaveChangesAsync();

        return NoContent();
      }
      catch
      {
        return StatusCode(500, new ResultViewModel<Category>("Occurred an error when try to update the specified category"));
      }
    }

    [HttpDelete("v1/categories/{id:int}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] int id, [FromServices] BlogDataContext context)
    {
      try
      {
        var category = await context.Categories.FirstOrDefaultAsync(c => c.Id == id);

        if (category is null)
        {
          return NotFound(new ResultViewModel<Category>("Category not found."));
        }

        context.Categories.Remove(category);
        await context.SaveChangesAsync();

        return NoContent();
      }
      catch (Exception e)
      {
        return StatusCode(500, new ResultViewModel<Category>("Occurred an error when try to delete the specified category"));
      }
    }
  }
}