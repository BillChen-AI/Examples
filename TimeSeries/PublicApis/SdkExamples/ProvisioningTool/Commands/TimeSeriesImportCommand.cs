using System.IO;
using ProvisioningTool.Importers;

namespace ProvisioningTool.Commands
{
    public class TimeSeriesImportCommand : ProgramCommandBase
    {
        public TimeSeriesImportCommand(Context context) 
            : base(CommandIds.ImportTimeSeries, $"Create reflected time series from csv file '{context.TimeSeriesCsvFileName}'.")
        {
            Context = context;
        }

        public override void Run()
        {
            using (var client = Aquarius.TimeSeries.Client.AquariusClient.CreateConnectedClient(
                Context.ServerHost, Context.LoginUserName, Context.LoginPassword))
            {
                var importer = new TimeSeriesImporter(client);

                Log.Info($"{importer.ImportObjectName} import started.");
                var csvFilePath = Path.Combine(Context.InputFolder, Context.TimeSeriesCsvFileName);

                if (!File.Exists(csvFilePath))
                {
                    Log.Error($"Csv file '{csvFilePath}' does not exist!");
                    return;
                }

                Log.Info($"Importing {importer.ImportObjectName} from {csvFilePath}.");

                importer.ImportFromCsvFile(csvFilePath);

                Log.Info($"Finished importing {importer.ImportObjectName}.");
            }
        }
    }
}
