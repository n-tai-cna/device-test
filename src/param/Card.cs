namespace Card
{
  public class PosInfoExParam
  {
    public string inType { get; set; }
    public string systemCode { get; set; }
    public int cardType { get; set; }
    public string roomNumber { get; set; }
    public string paymentMethod { get; set; }
    public string language { get; set; }
    public string inDate { get; set; }
    public string outDate { get; set; }
    public string memberNumber { get; set; }
    public string memberPoint { get; set; }
  }

  namespace Issue
  {
    class ExtendParam
    {
      public int numberOfIssue { get; set; }
    }
  }

  namespace Read
  {
    class ExtendParam
    {
      public string checkinDateTime;
      public string checkoutDateTime;
      public Card.PosInfoExParam posInformation { get; set; } = new Card.PosInfoExParam();
    }
  }

  namespace IssueInfo
  {
    public class RequestExtendParam
    {
      public string roomNumber { get; set; } = "0413";
      public string texNumber { get; set; } = "texNumber";
      public string reserveNumber { get; set; } = "7297";
    }
    public class ResponseExtendParam
    {
      public string roomNumber { get; set; }
      public Card.PosInfoExParam posInformation { get; set; } = new Card.PosInfoExParam();
    }
  }

  namespace QtyNotify
  {
    public class RequestExtendParam : Card.IssueInfo.RequestExtendParam
    {
      public int transactionCategory { get; set; } = 1;
      public int cardResultNumber { get; set; } = 1;
    }
  }
}