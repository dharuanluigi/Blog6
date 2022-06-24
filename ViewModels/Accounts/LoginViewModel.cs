﻿using System.ComponentModel.DataAnnotations;

namespace Blog6.ViewModels.Accounts
{
  public class LoginViewModel
  {
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
  }
}