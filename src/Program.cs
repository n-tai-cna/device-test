using Almex.WincalX.Common.w0cmtp1;
using Almex.WincalX.Service.w0cmtp1;
using log4net.Config;

namespace Citynow.Test
{
  class Program
  {
    static void Main(string[] args)
    {

      XmlConfigurator.Configure();

      string path = Setting.env == "local" ? ".\\src\\WincalXConfigLocal.json" : null;

      WincalXConnector Connector = Customer.execute(path);

      _ = Connector.Connect(new ClientInfo
      {
        StoreCode = Setting.storeCode,
        Email = "dll-dev@usen-next.jp",
        Password = "almex-dll"
      }).Result;

      Provider.execute(Connector);

    }

  }

}
