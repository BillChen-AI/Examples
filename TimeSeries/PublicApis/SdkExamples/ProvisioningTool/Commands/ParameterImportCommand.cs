using System.IO;
using System.Reflection;
using log4net;
using ProvisioningTool.Importers;

namespace ProvisioningTool.Commands
{
    public class ParameterImportCommand : ProgramCommandBase
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly Context _context;

        public ParameterImportCommand(Context context)
            : base(CommandIds.ImportParameters, "Insert/Update parameters from the specified csv file.")
        {
            _context = context;
        }

        public override void Run()
        {
            using (var client = Aquarius.TimeSeries.Client.AquariusClient.CreateConnectedClient(
                _context.ServerHost, _context.LoginUserName, _context.LoginPassword))
            {
                Log.Info("Parameter import started.");
                var importer = new ParameterImporter(client);
                var csvFilePath = Path.Combine(_context.InputFolder, _context.ParameterCsvFileName);

                if (!File.Exists(csvFilePath))
                {
                    Log.Error($"Parameter csv file '{csvFilePath}' does not exist!");
                    return;
                }

                Log.Info($"Importing parameters from {csvFilePath}.");

                importer.ImportFromCsvFile(csvFilePath);

                Log.Info("Finished importing parameters.");
            }
        }
    }
}
