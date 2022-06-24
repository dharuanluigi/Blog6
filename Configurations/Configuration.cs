namespace Blog6.Configurations
{
  public static class Configuration
  {
    public static string JwtKey { get; set; } = string.Empty;

    public static string ApiKeyName { get; set; } = string.Empty;

    public static string ApiKey { get; set; } = string.Empty;

    public static SmtpConfiguration Smtp { get; set; } = null!;
  }
}