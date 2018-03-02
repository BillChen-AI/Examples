using System.Collections.Generic;
using System.Text;
using FileHelpers;

namespace ProvisioningTool.Parsers
{
    public abstract class CsvFileParserBase<TParsed> : IParser<TParsed> 
        where TParsed : class
    {
        public Encoding Encoding { get; set; }

        protected virtual IEnumerable<TParsed> ReadFromFile(string filePath)
        {
            var engine = Encoding == null
                ? new FileHelperEngine<TParsed>()
                : new DelimitedFileEngine<TParsed>(Encoding);
            
            return engine.ReadFile(filePath);
        }

        public virtual IEnumerable<TParsed> ParseFromFile(string filePath)
        {
            return ReadFromFile(filePath);
        }
    }
}
