namespace Blog6.ViewModels.Posts
{
  public class ResultPostsViewModel
  {
    public int Total { get; set; }

    public int Page { get; set; }

    public int PageSize { get; set; }

    public IList<ListPostsViewModel> Posts { get; set; }
  }
}