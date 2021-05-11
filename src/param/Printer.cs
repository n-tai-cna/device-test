using Newtonsoft.Json;

namespace Printer
{
  public class Message
  {
    [JsonProperty(PropertyName = "print_data")]
    public byte[] PrintData { get; set; }
  }

  public class BasicParameter
  {
    public string storeCode { get; set; }
    public string terminalId { get; set; }
    public int resultFlag { get; set; }

    public BasicParameter(string storeCode, string terminalId, int resultFlag)
    {
      this.storeCode = storeCode;
      this.terminalId = terminalId;
      this.resultFlag = resultFlag;
    }
  }
}