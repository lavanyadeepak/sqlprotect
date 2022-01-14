using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using log4net;
using log4net.Appender;
using log4net.Config;

namespace EncryptDatabaseObjects
{
    internal class Program
    {
        private static readonly ILog log =
    LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        static void Main(string[] args)
        {
            XmlConfigurator.Configure();
            log.Info("Starting Encryption Application");

            string strConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            if (string.IsNullOrEmpty(strConnectionString))
                log.Error("No database was specified to encrypt");
            else
            {
                EncryptObjects o = new EncryptObjects();
                o.ConnectionStrings = strConnectionString.Split('§');
                o.StartEncrypt();
            }
            log.Info("Completing Encryption Application");
        }
    }
}