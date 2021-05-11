using System;
using Almex.WincalX.Common.Entity.w0cmtp1;
using Almex.WincalX.Common.Constant.w0cmtp1;
using Almex.WincalX.Common.w0cmtp1;
using Almex.WincalX.Service.w0cmtp1;
using System.Threading;
using log4net;
using System.Collections.Generic;
public class Customer
{
  private static readonly ILog logger = LogManager.GetLogger(typeof(Customer));
  public static WincalXConnector execute(string path)
  {

    WincalXConnector.InitializeConnector(ConfigurationPath: path);

    WincalXConnector Connector = WincalXConnector.GetConnector();

    //Connect Status Changed Handler
    Connector.ConnectionStatusChanged += (Sender, Status) =>
    {
      logger.Info($"Connect {Setting.env} environment: {Status.IsConnect}");
    };

    //Check Received Request
    Connector.ReceivedRequest += (Sender, Message) =>
    {
      MessageInfo messageInfo = Message.MessageInfo;
      DeviceLinkMsgProps args = Message.DeviceLinkMsgProps;
      RequestJob requestJob = messageInfo.RequestJob;
      logger.Info($"\n\n{DateTime.Now} COMMON MESSAGE INFO >>>DEVICE_ID: {Message.DeviceId}, Command: {messageInfo.Command}");
      VOD.BasicParameters basicMsg = Utility.DeserializeFromJsonString<VOD.BasicParameters>(messageInfo.BasicParameters);
      logger.Info($"MESSAGE Basic parameters >>> {messageInfo.BasicParameters}");
      logger.Info($"MESSAGE Extend parameters >>> {messageInfo.ExtendParameters}");
      basicMsg.ResultFlag = 0;
      requestJob.Status = RequestJobConst.RECEIVED;

      switch (messageInfo.Command)
      {
        case "2004": // card issue
        case "2006": // card issue

          Card.Issue.ExtendParam CardIssueExtendParam = new Card.Issue.ExtendParam
          {
            numberOfIssue = 1
          };

          MessageInfo cardIssueResponseMsg = new MessageInfo
          {
            Id = messageInfo.Id,
            Command = messageInfo.Command,
            CreatedTime = Utility.ToUnixTime(DateTime.Now),
            BasicParameters = Utility.SerializeToJSONString(basicMsg),
            ExtendParameters = Utility.SerializeToJSONString(CardIssueExtendParam),
            RequestJob = requestJob
          };

          logger.Info($"MESSAGE Response Basic parameters >>> {messageInfo.BasicParameters}");
          logger.Info($"MESSAGE Response Extend parameters >>> {messageInfo.ExtendParameters}");

          _ = Connector.Response(cardIssueResponseMsg, args); //Response a request from server.

          Thread.Sleep(2000);

          requestJob.Status = RequestJobConst.DONE;

          _ = Connector.Request(cardIssueResponseMsg); //Request to the server.
          break;
        case "2007": // card-read
          basicMsg.RoomNumbers = new List<string>()
                {
                            "0610","0301219"
                };

          Card.Read.ExtendParam CardReadExtendParam = new Card.Read.ExtendParam
          {
            checkinDateTime = "202103301200",
            checkoutDateTime = "202103301230",
            posInformation = new Card.PosInfoExParam
            {
              inType = "$  ",
              cardType = 0,
              systemCode = "getCardPrintingHotelCode",
              roomNumber = "07000"
            }
          };

          MessageInfo cardReadResponseMsg = new MessageInfo
          {
            Id = messageInfo.Id,
            Command = messageInfo.Command,
            CreatedTime = Utility.ToUnixTime(DateTime.Now),
            BasicParameters = Utility.SerializeToJSONString(basicMsg),
            ExtendParameters = Utils.serializeObjectSnakeJson(CardReadExtendParam),
            RequestJob = requestJob
          };

          logger.Info($"MESSAGE Response Basic parameters >>> {messageInfo.BasicParameters}");
          logger.Info($"MESSAGE Response Extend parameters >>> {messageInfo.ExtendParameters}");

          _ = Connector.Response(cardReadResponseMsg, args); //Response a request from server.

          Thread.Sleep(2000);

          requestJob.Status = RequestJobConst.DONE;
          break;
        case "PRINTER_REPORT_REQUEST":
        case "PRINTER_RECEIPT_REQUEST":
        case "2014": // Print Receipt
        case "2015": // Print Report
          Printer.Message printerMessage = Utility.DeserializeFromBSON<Printer.Message>(messageInfo.Message);

          logger.Info($"PRINTER MESSAGE Extend parameters >>> {messageInfo.ExtendParameters}");

          Printer.BasicParameter basicParameter = new Printer.BasicParameter(Message.StoreCode, Message.TerminalId, 0);

          requestJob.Status = RequestJobConst.RECEIVED;

          MessageInfo responseMessage = new MessageInfo
          {
            Id = messageInfo.Id,
            Command = messageInfo.Command,
            CreatedTime = Utility.ToUnixTime(DateTime.Now),
            BasicParameters = Utility.SerializeToJSONString(basicParameter),
            RequestJob = requestJob
          };

          _ = Connector.Response(responseMessage, args); //Response a request from server.

          Thread.Sleep(2000);

          requestJob.Status = RequestJobConst.DONE;

          _ = Connector.Request(responseMessage); //Request to the server. 

          break;
        case "1001":
        case "1002":
        case "1003":
        case "1004":
        case "1005":
        case "1006":
        case "1007":
        case "1008":
        case "1009":
        case "1010":
        case "1011":
        case "1012":
        case "1013":
        case "1014":
        case "1015":
        case "1016":
        case "1017":
          VOD.BasicParameters vodMessage = Utility.DeserializeFromJsonString<VOD.BasicParameters>(messageInfo.BasicParameters);

          logger.Info($"VOD MESSAGE Basic parameters >>> {messageInfo.BasicParameters}");

          vodMessage.ResultFlag = 0;

          requestJob.Status = RequestJobConst.RECEIVED;

          MessageInfo vodResponseMessage = new MessageInfo
          {
            Id = messageInfo.Id,
            Command = messageInfo.Command,
            CreatedTime = Utility.ToUnixTime(DateTime.Now),
            BasicParameters = Utility.SerializeToJSONString(vodMessage),
            RequestJob = requestJob
          };

          _ = Connector.Response(vodResponseMessage, args); //Response a request from server.

          Thread.Sleep(2000);

          requestJob.Status = RequestJobConst.DONE;

          _ = Connector.Request(vodResponseMessage); //Request to the server. 
          break;

        default:
          logger.Info($"MESSAGE >>> ID: {messageInfo.Id}, Command: {messageInfo.Command}, CreateTime: {messageInfo.CreatedTime}");
          break;
      }
    };

    //Handling Error Message
    Connector.ErrorListener += (Sender, Message) =>
    {
      MessageInfo messageInfo = Message.MessageInfo;
      DeviceLinkMsgProps args = Message.DeviceLinkMsgProps;
      logger.Info($"ERROR MESSAGE INFO >>>DEVICE_ID: {Message.DeviceId}, ID: {messageInfo.Id}, Command: {messageInfo.Command}, Message: {messageInfo.Message} ,CreateTime: {messageInfo.CreatedTime}");
      logger.Info($"ERROR REQUEST INFO >>>ID: {messageInfo.RequestJob.Id}, STATUS: {messageInfo.RequestJob.Status}");
    };

    return Connector;
  }
}