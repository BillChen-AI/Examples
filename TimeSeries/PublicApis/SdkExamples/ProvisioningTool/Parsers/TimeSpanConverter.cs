using System;
using FileHelpers;

namespace ProvisioningTool.Parsers
{
    public class TimeSpanConverter : ConverterBase
    {
        public override object StringToField(string utcOffsetString)
        {
            if (string.IsNullOrWhiteSpace(utcOffsetString))
                return TimeSpan.Zero;

            var trimmedStr = utcOffsetString.Trim();
            if (trimmedStr.StartsWith("+"))
                trimmedStr = trimmedStr.Remove(0, 1);

            return TimeSpan.Parse(trimmedStr);
        }
    }
}
