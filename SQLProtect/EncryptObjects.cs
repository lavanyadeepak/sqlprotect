using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Repository.Hierarchy;
using System.Reflection;

namespace EncryptDatabaseObjects
{
    public class EncryptObjects
    {
        public string[] ConnectionStrings { get; set; }
        public List<KeyValuePair<string, string>> OperationSummary { get; set; }

        private SqlConnectionInfo sc = new SqlConnectionInfo();

        private Server srv = new Server();

        private ServerConnection objServerConnection = new ServerConnection();

        private Database db = new Database();

        private List<KeyValuePair<string, string>> erringObjects = new List<KeyValuePair<string, string>>();

        private bool IsDatabaseReady = false;
        private static readonly ILog log =
    LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


        public EncryptObjects()
        {
            XmlConfigurator.Configure();
        }

        public void EstablishConnection(string operatingConnectionString)
        {
            log.Debug("Processing for Connection String: " + operatingConnectionString);
        
            SqlConnectionStringBuilder sb = new SqlConnectionStringBuilder(operatingConnectionString);

            sc.ServerName = sb.DataSource;
            sc.DatabaseName = sb.InitialCatalog;

            sc.UseIntegratedSecurity = false;
            sc.UserName = sb.UserID;
            sc.Password = sb.Password;


            try // Check to see if server connection details are ok.
            {
                log.Debug("Attempting to connect to the server");
                srv = new Server(objServerConnection);
                if (srv is null)
                {
                    log.Error("The connection to the server failed for unknown reasons");
                }
                else
                {
                    log.Debug("The server connection was successful");
                }
            }
            catch (Exception SqlConnectionException)
            {
                log.Fatal(SqlConnectionException.Message,SqlConnectionException);
            }


            try // Check to see if database exists.
            {
                db = srv.Databases[sc.DatabaseName];
                if (db == null)
                {
                    log.Error("The database "+ db.Name + " was not found on the server "+sc.ServerName);
                    IsDatabaseReady = false;
                }
                else
                {
                    log.Info("The database " + db.Name + " was found ready for encryption tasks on the server " + sc.ServerName);
                    IsDatabaseReady = true;
                }
            }
            catch (Exception SqlConnectionException)
            {
                log.Fatal(SqlConnectionException.Message, SqlConnectionException);
            }
        }

        public void StartEncrypt()
        {
            log.Debug("Number of Connection Strings Received " + ConnectionStrings.Length);
            foreach (string operatingConnectionString in ConnectionStrings)
            {
                EstablishConnection(operatingConnectionString);
                if (IsDatabaseReady)
                {
                    EncryptStoredProcedures();
                    EncryptTriggers();
                    EncryptUserDefinedFunctions();
                    EncryptViews();
                }
            }
            log.Debug("Encryption Task Completed Successfully");
        }

        public void EncryptStoredProcedures()
        {
            for (int i = 0; i < db.StoredProcedures.Count; i++)
            {
                StoredProcedure sp;
                sp = new StoredProcedure();
                sp = db.StoredProcedures[i];
                if (!sp.IsSystemObject)// Exclude System stored procedures
                {
                    if (!sp.IsEncrypted) // Exclude already encrypted stored procedures
                    {
                        log.Debug("Encrypting " + sp.Name);

                        try
                        {
                            sp.TextMode = false;
                            sp.IsEncrypted = true;
                            sp.TextMode = true;
                            sp.Alter();

                            sp = null;
                        }
                        catch (Exception FailedProcException)
                        {
                            log.Error(FailedProcException.Message, FailedProcException);
                        }
                    }
                    else
                    {
                        log.Warn(sp.Name + " is already encrypted");
                    }
                }
                else
                {
                    log.Warn(sp.Name + " is skipped because it is a system object");
                }
            }
        }

        public void EncryptViews()
        {
            for (int i = 0; i < db.Views.Count; i++)
            {
                View sp;
                sp = new View();
                sp = db.Views[i];
                if (!sp.IsSystemObject)// Exclude System stored procedures
                {
                    if (!sp.IsEncrypted) // Exclude already encrypted stored procedures
                    {
                        log.Debug("Encrypting " + sp.Name);

                        try
                        {
                            sp.TextMode = false;
                            sp.IsEncrypted = true;
                            sp.TextMode = true;
                            sp.Alter();

                            sp = null;
                        }
                        catch (Exception FailedProcException)
                        {
                            log.Error(FailedProcException.Message, FailedProcException);
                        }
                    }
                    else
                    {
                        log.Warn(sp.Name + " is already encrypted");
                    }
                }
                else
                {
                    log.Warn(sp.Name + " is skipped because it is a system object");
                }
            }


        }

        public void EncryptUserDefinedFunctions()
        {
            for (int i = 0; i < db.UserDefinedFunctions.Count; i++)
            {
                UserDefinedFunction sp;
                sp = new UserDefinedFunction();
                sp = db.UserDefinedFunctions[i];
                if (!sp.IsSystemObject)// Exclude System stored procedures
                {
                    if (!sp.IsEncrypted) // Exclude already encrypted stored procedures
                    {
                        log.Debug("Encrypting " + sp.Name);

                        try
                        {
                            sp.TextMode = false;
                            sp.IsEncrypted = true;
                            sp.TextMode = true;
                            sp.Alter();

                            sp = null;
                        }
                        catch (Exception FailedProcException)
                        {
                            log.Error(FailedProcException.Message, FailedProcException);
                        }
                    }
                    else
                    {
                        log.Warn(sp.Name + " is already encrypted");
                    }
                }
                else
                {
                    log.Warn(sp.Name + " is skipped because it is a system object");
                }
            }
        }

        public void EncryptTriggers()
        {
            for (int i = 0; i < db.Triggers.Count; i++)
            {
                DatabaseDdlTrigger sp;
                sp = new DatabaseDdlTrigger();
                sp = db.Triggers[i];
                if (!sp.IsSystemObject)// Exclude System stored procedures
                {
                    if (!sp.IsEncrypted) // Exclude already encrypted stored procedures
                    {
                        log.Debug("Encrypting " + sp.Name);

                        try
                        {
                            sp.TextMode = false;
                            sp.IsEncrypted = true;
                            sp.TextMode = true;
                            sp.Alter();

                            sp = null;
                        }
                        catch (Exception FailedProcException)
                        {
                            log.Error(FailedProcException.Message, FailedProcException);
                        }
                    }
                    else
                    {
                       log.Warn(sp.Name + " is already encrypted");
                    }
                }
                else
                {
                    log.Warn(sp.Name + " is skipped because it is a system object");
                }
            }
        }
    }
}