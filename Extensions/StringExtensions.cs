namespace Blog6.Extensions
{
  public static class StringExtensions
  {
    public static string NormalizeSlug(this string slug)
    {
      return slug.Replace("@", "-").Replace(".", "-");
    }
  }
}