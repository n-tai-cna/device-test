using System.Collections.Generic;
using Newtonsoft.Json;

namespace VOD
{
  public class BasicParameters
  {
    [JsonProperty(PropertyName = "store_code")]
    public string StoreCode { get; set; }

    [JsonProperty(PropertyName = "terminal_id")]
    public string TerminalId { get; set; }

    [JsonProperty(PropertyName = "room_numbers")]
    public List<string> RoomNumbers { get; set; }

    [JsonProperty(PropertyName = "result_flag")]
    public int ResultFlag { get; set; }
  }
}