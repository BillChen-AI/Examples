using System.Collections.Generic;

namespace ProvisioningTool.Parsers
{
    public interface IParser<out TOut>
    {
        IEnumerable<TOut> ParseFromFile(string filePath);
    }
}
