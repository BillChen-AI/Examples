using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Aquarius.TimeSeries.Client;
using FileHelpers;
using log4net;
using ProvisioningTool.Parsers;
using ServiceStack;

namespace ProvisioningTool.Importers
{
    public abstract class ImporterBase<TParser, TSource, TTarget>
        where TParser : IParser<TSource>, new()
        where TSource : class
    {
        private readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public ImportResult<TSource, TTarget> ImportResult { get; }
        public abstract string ImportObjectName { get; }

        protected readonly IAquariusClient ServiceClient;

        protected ImporterBase(IAquariusClient serviceClient)
        {
            ImportResult = new ImportResult<TSource, TTarget>();

            ServiceClient = serviceClient;
        }

        protected abstract string GetId(TSource item);

        public virtual void ImportFromCsvFile(string filePath)
        {
            var parsedItems = ParseFile(filePath);

            ImportResult.ParsedItemTotal = parsedItems.Count;

            var itemsToUpdate = GetItemsToUpdate(parsedItems).ToList();
            var parsedItemsToUpdate = itemsToUpdate.Select(item => item.ParsedItem).ToList();

            var itemsToCreate = parsedItems.Except(parsedItemsToUpdate).ToList();

            ImportResult.ItemsToCreate.AddRange(itemsToCreate);
            ImportResult.ItemsToUpdate.AddRange(parsedItemsToUpdate);

            try
            {
                CreateMany(itemsToCreate);
                UpdateMany(itemsToUpdate);
            }
            finally
            {
                _log.Info($"Total {ImportObjectName} parsed: {ImportResult.ParsedItemTotal}. Inserted: {ImportResult.CreatedItems.Count}/{ImportResult.ItemsToCreate.Count}. " +
                          $"Updated: {ImportResult.UpdatedItems.Count}/{ImportResult.ItemsToUpdate.Count}. " +
                          $"Create failed: {ImportResult.CreateFailedItems.Count}. Update failed: {ImportResult.UpdateFailedItems.Count}");
                SaveFailedItems();
            }
        }

        private List<TSource> ParseFile(string filePath)
        {
            try
            {
                return new TParser().ParseFromFile(filePath).ToList();
            }
            catch (Exception ex)
            {
                var errorMessage = $"Error parsing file '{filePath}' for {ImportObjectName}.";

                _log.Error(errorMessage, ex);

                throw new ProvisioningToolException(errorMessage);
            }
        }

        private void UpdateMany(IEnumerable<UpdateItem<TSource>> itemsToUpdate)
        {
            foreach (var itemToUpdate in itemsToUpdate)
            {
                try
                {
                    var updatedItem = Update(itemToUpdate);

                    ImportResult.UpdatedItems.Add(updatedItem);
                }
                catch (WebServiceException ex)
                {
                    _log.Error($"Failed to update {ImportObjectName} with id '{GetId(itemToUpdate.ParsedItem)}'.", ex);
                    ImportResult.UpdateFailedItems.Add(itemToUpdate.ParsedItem);
                }
            }
        }

        protected abstract IEnumerable<UpdateItem<TSource>> GetItemsToUpdate(List<TSource> parsedItems);

        protected abstract TTarget Create(TSource itemToCreate);
        protected abstract TTarget Update(UpdateItem<TSource> itemToUpdate);

        private void CreateMany(IEnumerable<TSource> itemsToCreate)
        {
            foreach (var sourceItem in itemsToCreate)
            {
                try
                {
                    var createdItem = Create(sourceItem);
                    ImportResult.CreatedItems.Add(createdItem);
                }
                catch (WebServiceException ex)
                {
                    _log.Error($"Failed to create {ImportObjectName} with id '{GetId(sourceItem)}'.", ex);
                    ImportResult.CreateFailedItems.Add(sourceItem);
                }
            }
        }

        private void SaveFailedItems()
        {
            const string outputFolder = "Output";

            var timeStamp = $"{DateTime.Now:yyyy-MM-ddTHH-mm-ss}";

            if (ImportResult.CreateFailedItems.Any())
            {
                var filePath = Path.Combine(outputFolder, $"{ImportObjectName}CreateFailed_{timeStamp}.csv");
                WriteItemsToFile(ImportResult.CreateFailedItems, filePath);
                _log.Info($"{ImportResult.CreateFailedItems.Count} create failed items saved: {filePath}.");
            }

            if (ImportResult.UpdateFailedItems.Any())
            {
                var filePath = Path.Combine(outputFolder, $"{ImportObjectName}UpdateFailed_{timeStamp}.csv");
                WriteItemsToFile(ImportResult.UpdateFailedItems, filePath);
                _log.Info($"{ImportResult.UpdateFailedItems.Count} update failed items saved: {filePath}.");
            }
        }

        private void WriteItemsToFile<T>(List<T> items, string filePath)
            where T : class
        {
            var engine = new DelimitedFileEngine<T>();
            engine.Options.Delimiter = ",";

            engine.HeaderText = engine.GetFileHeader();

            CreateFolderIfNotExists(Path.GetDirectoryName(filePath));
            engine.WriteFile(filePath, items);
        }

        private void CreateFolderIfNotExists(string directoryName)
        {
            if (Directory.Exists(directoryName))
                return;

            Directory.CreateDirectory(directoryName);
        }
    }
}
