using System;
using Almex.WincalX.Common.Constant.w0cmtp1;
using Almex.WincalX.Common.w0cmtp1;
using Almex.WincalX.Service.w0cmtp1;
using log4net;

public class Provider
{
  private static readonly ILog logger = LogManager.GetLogger(typeof(Provider));
  public static void execute(WincalXConnector Connector)
  {
    Utils utils = new Utils(Connector);
    while (true)
    {
      logger.Info("Input Case: ");
      string input = Console.ReadLine();
      Console.Out.WriteLine("");

      MessageInfo requestMessage = new MessageInfo
      {
        Id = Guid.NewGuid().ToString(),
        CreatedTime = Utility.ToUnixTime(DateTime.Now),
        BasicParameters = Utils.serializeObjectSnakeJson(new BasicParam()),
      };

      switch (input)
      {
        case "simple":
          logger.Info("Input case simple: ");
          string inputSimple = Console.ReadLine();
          logger.Info("Input The Message: ");
          string input2 = Console.ReadLine();
          if (inputSimple == "print")
          {
            requestMessage.Command = input;
            requestMessage.RequestJob = utils.createRequestJob(input2, RequestJobConst.DONE);
          }
          else
          {
            requestMessage.Command = "SIMPLE_MESSAGE_WITH_RESPONSE";
            requestMessage.RequestJob = utils.createRequestJob(Guid.NewGuid().ToString(), RequestJobConst.NEW);
          }
          requestMessage.Message = Utility.SerializeToBSON(new SimpleMessageInfo(input2));
          utils.requestAndResponse(requestMessage);
          break;
        default:
        case "3009":
          Console.Out.WriteLine("master_data_request");
          utils.requestAndResponse(requestMessage, input, null);
          break;
        case "3010":
          Console.Out.WriteLine("Log_Registration");
          utils.requestAndResponse(requestMessage, input, new LogRegistration.RequestExtendParam());
          break;
        case "4008":
          Console.Out.WriteLine("card_issuance");
          utils.requestAndResponse(requestMessage, input, new Card.IssueInfo.RequestExtendParam());
          break;
        case "4009":
          Console.Out.WriteLine("card_qty_notify");
          utils.requestAndResponse(requestMessage, input, new Card.QtyNotify.RequestExtendParam());
          break;
        case "Cancel":
          //Cancel Comsumer.
          Connector.CancelConsumer();
          break;
        case "exit":
          //Disconnect.
          Connector.Disconnect();
          break;
      }
    }
  }
}