namespace SmartCheckIn
{
  public class RequestExtendParam
  {
    public ScRequest scRequest { get; set; }

    public RequestExtendParam(string apiName)
    {
      this.scRequest = new ScRequest(apiName);
    }

    public class ScRequest
    {
      public string apiName { get; set; }

      public ScRequest(string apiName)
      {
        this.apiName = apiName;
      }
    }
  }

}