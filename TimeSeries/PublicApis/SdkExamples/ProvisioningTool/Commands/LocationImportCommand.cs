using System.IO;
using ProvisioningTool.Importers;

namespace ProvisioningTool.Commands
{
    public class LocationImportCommand : ProgramCommandBase
    {
        public LocationImportCommand(Context context) 
            : base(CommandIds.ImportLocations, $"Get locations from csv file '{context.LocationCsvFileName}' and create/update them in AQTS.")
        {
            Context = context;
        }

        public override void Run()
        {
            using (var client = Aquarius.TimeSeries.Client.AquariusClient.CreateConnectedClient(
                Context.ServerHost, Context.LoginUserName, Context.LoginPassword))
            {
                var importer = new LocationImporter(client);

                Log.Info($"{importer.ImportObjectName} import started.");
                var csvFilePath = Path.Combine(Context.InputFolder, Context.LocationCsvFileName);

                if (!File.Exists(csvFilePath))
                {
                    Log.Error($"Location csv file '{csvFilePath}' does not exist!");
                    return;
                }

                Log.Info($"Importing {importer.ImportObjectName} from {csvFilePath}.");

                importer.ImportFromCsvFile(csvFilePath);

                Log.Info($"Finished importing {importer.ImportObjectName}.");
            }

        }
    }
}
