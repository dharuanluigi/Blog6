using System.ComponentModel.DataAnnotations;

namespace Blog6.ViewModels.Accounts
{
  public class CreateUserViewModel
  {
    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
  }
}