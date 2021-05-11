namespace SmartCheckInRequest
{
  public class RequestInformation
  {
    public int RequestCode { set; get; }
    public string TenpoCode { set; get; }
    public string TerminalCode { set; get; }
    public string LanguageCode { set; get; }
  }

  public class BaseXmlRequest
  {
    public RequestInformation RequestInformation { set; get; }
  }

  public class GetReserveNoRequest : BaseXmlRequest
  {
    public SearchDataClass SearchData { set; get; }

    public class SearchDataClass
    {
      public string PmsRsvNoForSearch { set; get; }
      public string PhoneNumber { set; get; }
      public string NameKana { set; get; }
      public string AgentCode { set; get; }
      public string AgentRsvNo { set; get; }
      public string RoomNo { set; get; }
      public string MemberNumber { set; get; }
    }
  }
}