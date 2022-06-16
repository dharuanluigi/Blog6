using Blog.Data;
using Blog.Models;
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
      return Ok(await context.Categories.ToListAsync());
    }

    [HttpGet("v1/categories/{id:int}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int id, [FromServices] BlogDataContext context)
    {
      var category = await context.Categories.FirstOrDefaultAsync(c => c.Id == id);

      if (category is null)
      {
        return NotFound();
      }

      return Ok(category);
    }

    [HttpPost("v1/categories")]
    public async Task<IActionResult> PostAsync([FromBody] Category modelCategory, [FromServices] BlogDataContext context)
    {
      await context.Categories.AddAsync(modelCategory);
      await context.SaveChangesAsync();

      return Created($"v1/categories/{modelCategory.Id}", modelCategory);
    }

    [HttpPut("v1/categories/{id:int}")]
    public async Task<IActionResult> PutAsync([FromRoute] int id, [FromBody] Category newCategory, [FromServices] BlogDataContext context)
    {
      var oldCategory = await context.Categories.FirstOrDefaultAsync(c => c.Id == id);

      if (oldCategory is null)
      {
        return NotFound();
      }

      oldCategory.Name = newCategory.Name;
      oldCategory.Slug = newCategory.Slug;

      context.Categories.Update(oldCategory);
      await context.SaveChangesAsync();

      return NoContent();
    }

    [HttpDelete("v1/categories/{id:int}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] int id, [FromServices] BlogDataContext context)
    {
      var category = await context.Categories.FirstOrDefaultAsync(c => c.Id == id);

      if (category is null)
      {
        return NotFound();
      }

      context.Categories.Remove(category);
      await context.SaveChangesAsync();

      return NoContent();
    }
  }
}