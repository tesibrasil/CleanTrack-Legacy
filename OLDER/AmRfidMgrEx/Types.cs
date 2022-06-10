namespace amrfidmgrex
{
  public class Types
  {
    public enum Result
    {
      Unknown,
      Error,
      Success,
      Timeout,
    }

    public class Info
    {
      public Types.Result Result { get; set; }

      public string Description { get; set; }

      public int IdStepType { get; set; }
    }

    public delegate void CompleteDelegate(Types.Info info);
  }
}
