namespace Blog6.Configurations
{
  public static class Configuration
  {
    public static string JwtKey { get; set; }

    public static string ApiKeyName { get; set; }

    public static string ApiKey { get; set; }

    public static SmtpConfiguration Smtp { get; set; }
  }
}