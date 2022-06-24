namespace Blog6.ViewModels
{
  public class ResultViewModel<T>
  {
    public T Data { get; set; }

    public List<string> Errors { get; } = new();

    public ResultViewModel(T data, List<string> errors)
    {
      Data = data;
      Errors = errors;
    }

    public ResultViewModel(T data)
    {
      Data = data;
    }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public ResultViewModel(List<string> errors)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
      Errors = errors;
    }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public ResultViewModel(string error)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
      Errors.Add(error);
    }
  }
}