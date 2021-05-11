using System;
using Almex.WincalX.Common.Constant.w0cmtp1;
using Almex.WincalX.Common.w0cmtp1;
using Almex.WincalX.Service.w0cmtp1;
using Newtonsoft.Json;
using log4net;
using Newtonsoft.Json.Serialization;

class Utils
{
  private static readonly ILog logger = LogManager.GetLogger(typeof(Utils));
  private WincalXConnector connector;

  public Utils(WincalXConnector connector)
  {
    this.connector = connector;
  }

  public MessageInfo requestAndResponse(MessageInfo requestMessage)
  {
    requestMessage.RequestJob = createRequestJob(Guid.NewGuid().ToString(), RequestJobConst.NEW);
    logger.Info($"BASIC PARAM >>> {requestMessage.BasicParameters}");
    logger.Info($"EXTEND PARAM >>> {requestMessage.ExtendParameters}");
    if (requestMessage.Message != null)
    {
      logger.Info($"MESSAGE >>> {Utility.DeserializeFromBSON<Object>(requestMessage.Message)}");
    }

    MessageInfo responseMessage = connector.RequestAndResponse(requestMessage);
    logger.Info(responseMessage);

    if (responseMessage != null)
    {
      logger.Info($"BASIC PARAM RESPONSE {responseMessage.BasicParameters}");
      logger.Info($"EXTEND PARAM RESPONSE {responseMessage.ExtendParameters}");
      if (responseMessage.Message != null)
      {
        logger.Info($"MESSAGE >>> {Utility.DeserializeFromBSON<Object>(responseMessage.Message)}");
      }
    }
    Console.Out.WriteLine("-----------------------------------------------------------------\n");

    return responseMessage;
  }

  public MessageInfo requestAndResponse(MessageInfo requestMessage, string command, Object extendParameters)
  {
    requestMessage.Command = command;
    requestMessage.ExtendParameters = Utils.serializeObjectSnakeJson(extendParameters);
    Console.Out.WriteLine(requestMessage.Command);
    return this.requestAndResponse(requestMessage);
  }

  public MessageInfo requestAndResponse<T>(MessageInfo requestMessage, string command, Object extendParameters, string requestFilePath) where T : SmartCheckInRequest.BaseXmlRequest
  {
    System.IO.StreamReader file = new System.IO.StreamReader(requestFilePath);
    System.Xml.Serialization.XmlSerializer reader = new System.Xml.Serialization.XmlSerializer(typeof(T));
    T request = (T)reader.Deserialize(file);
    file.Close();
    requestMessage.Message = Utility.SerializeToBSON(request);
    return this.requestAndResponse(requestMessage, command, extendParameters);
  }

  public RequestJob createRequestJob(string Id, string Status)
  {
    logger.Info($"CREATE REQUEST JOB >>> Id: {Id}, STATUS: {Status}");

    return new RequestJob
    {
      Id = Id,
      CreatedTime = Utility.ToUnixTime(DateTime.Now),
      Status = Status
    };
  }

  public static string serializeObjectSnakeJson(object o)
  {
    return JsonConvert.SerializeObject(o, new JsonSerializerSettings
    {
      ContractResolver = new DefaultContractResolver
      {
        NamingStrategy = new SnakeCaseNamingStrategy()
      },
      Formatting = Formatting.Indented
    });
  }
}