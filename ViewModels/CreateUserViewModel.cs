using System.ComponentModel.DataAnnotations;

namespace Blog6.ViewModels
{
  public class CreateUserViewModel
  {
    [Required]
    public string Name { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }
  }
}