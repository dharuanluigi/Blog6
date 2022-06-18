using System.ComponentModel.DataAnnotations;

namespace Blog6.ViewModels.Accounts
{
  public class UploadImageViewModel
  {
    [Required(ErrorMessage = "Invalid image")]
    public string Base64Image { get; set; }
  }
}