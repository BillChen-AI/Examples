using System;
using FileHelpers;

namespace ProvisioningTool.Parsers
{
    [IgnoreFirst(1)]
    [DelimitedRecord(",")]
    [IgnoreEmptyLines]
    public class ParsedLocation
    {
        [FieldOrder(1)]
        [FieldNotEmpty]
        [FieldTrim(TrimMode.Both)]
        [FieldQuoted(QuoteMode.OptionalForBoth, MultilineMode.NotAllow)]
        public string LocationIdentifier;

        [FieldOrder(2)]
        [FieldNotEmpty]
        [FieldTrim(TrimMode.Both)]
        [FieldQuoted(QuoteMode.OptionalForBoth, MultilineMode.NotAllow)]
        public string LocationName;

        [FieldOrder(3)]
        [FieldNotEmpty]
        [FieldTrim(TrimMode.Both)]
        [FieldQuoted(QuoteMode.OptionalForBoth, MultilineMode.NotAllow)]
        public string LocationPath;

        [FieldOrder(4)]
        [FieldNotEmpty]
        [FieldTrim(TrimMode.Both)]
        [FieldQuoted(QuoteMode.OptionalForBoth, MultilineMode.NotAllow)]
        public string LocationType;

        [FieldOrder(5)]
        [FieldQuoted(QuoteMode.OptionalForBoth, MultilineMode.NotAllow)]
        [FieldConverter(typeof(TimeSpanConverter))]
        public TimeSpan? UtcOffset;

        [FieldOrder(6)]
        [FieldTrim(TrimMode.Both)]
        [FieldOptional]
        [FieldQuoted(QuoteMode.OptionalForBoth, MultilineMode.AllowForBoth)]
        public string Description;

        [FieldOrder(7)]
        [FieldOptional]
        [FieldQuoted(QuoteMode.OptionalForBoth, MultilineMode.NotAllow)]
        public double? Latitude;

        [FieldOrder(8)]
        [FieldOptional]
        [FieldQuoted(QuoteMode.OptionalForBoth, MultilineMode.NotAllow)]
        public double? Longitude;

        [FieldOrder(9)]
        [FieldOptional]
        [FieldQuoted(QuoteMode.OptionalForBoth, MultilineMode.NotAllow)]
        public double? Elevation;

        [FieldOrder(10)]
        [FieldTrim(TrimMode.Both)]
        [FieldOptional]
        [FieldQuoted(QuoteMode.OptionalForBoth, MultilineMode.NotAllow)]
        public string ElevationUnits;
    }
}
