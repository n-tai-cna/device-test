
namespace LogRegistration
{
  public class RequestExtendParam
  {
    public LogInformations[] logInformations { get; set; } = new LogInformations[2]{
                new LogInformations(),
                new LogInformations("device id 2" , "log level 2", "log id 2", "log string 2")
                };
  }

  public class LogInformations
  {
    public string deviceId { get; set; } = "device id value";
    public string logLevel { get; set; } = "log level value";
    public string logId { get; set; } = "log id value";
    public string logString { get; set; } = "log string value";

    public LogInformations() { }

    public LogInformations(string deviceId, string logLevel, string logId, string logString)
    {
      this.deviceId = deviceId;
      this.logLevel = logLevel;
      this.logId = logId;
      this.logString = logString;
    }
  }
}