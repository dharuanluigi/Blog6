using System.ComponentModel.DataAnnotations;

namespace Blog6.ViewModels.Categories
{
  public class EditorCategoryViewModel
  {
    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string Slug { get; set; } = string.Empty;
  }
}