using System;
using System.Configuration;
using System.Reflection;
using log4net;
using ProvisioningTool.Commands;

namespace ProvisioningTool
{
    class Program
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        static void Main(string[] args)
        {
            try
            {
                Environment.ExitCode = 1;
                
                var context = GetConfig();

                var runner = new ProgramRunner();
                runner.AddCommand(new ParameterImportCommand(context));
                runner.Run(args);

                Environment.ExitCode = 0;
            }
            catch(ProvisioningToolException ex)
            {
                Log.Error($"Program finished with an error: {ex.Message} Please check the log.");
            }
            catch (Exception ex)
            {
                Log.Error("Something went wrong.", ex);
            }
        }

        private static Context GetConfig()
        {
            var appSettings = ConfigurationManager.AppSettings;
            var host = appSettings["aqtsHost"];
            var loginUserName = appSettings["loginUserName"];
            var loginPassword = appSettings["loginPassword"];

            if (string.IsNullOrWhiteSpace(host))
                throw new ArgumentException("Host name is not set in the config file.");
            if (string.IsNullOrWhiteSpace(loginUserName))
                throw new ArgumentException("Login user name is not set in the config file.");
            if (string.IsNullOrWhiteSpace(loginPassword))
                throw new ArgumentException("Login password is not set in the config file.");

            return new Context
            {
                ServerHost = host,
                LoginUserName = loginUserName,
                LoginPassword = loginPassword
            };
        }
    }
}
