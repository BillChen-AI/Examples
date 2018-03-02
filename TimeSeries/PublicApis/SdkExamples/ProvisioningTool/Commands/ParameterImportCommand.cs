﻿using System.IO;
using ProvisioningTool.Importers;

namespace ProvisioningTool.Commands
{
    public class ParameterImportCommand : ProgramCommandBase
    {
        public ParameterImportCommand(Context context)
            : base(CommandIds.ImportParameters, "Insert/Update parameters from the specified csv file.")
        {
            Context = context;
        }

        public override void Run()
        {
            using (var client = Aquarius.TimeSeries.Client.AquariusClient.CreateConnectedClient(
                Context.ServerHost, Context.LoginUserName, Context.LoginPassword))
            {
                var importer = new ParameterImporter(client);
                Log.Info($"{importer.ImportObjectName} import started.");

                var csvFilePath = Path.Combine(Context.InputFolder, Context.ParameterCsvFileName);

                if (!File.Exists(csvFilePath))
                {
                    Log.Error($"{importer.ImportObjectName} csv file '{csvFilePath}' does not exist!");
                    return;
                }

                Log.Info($"Importing {importer.ImportObjectName} from {csvFilePath}.");

                importer.ImportFromCsvFile(csvFilePath);

                Log.Info($"Finished importing {importer.ImportObjectName}.");
            }
        }
    }
}
