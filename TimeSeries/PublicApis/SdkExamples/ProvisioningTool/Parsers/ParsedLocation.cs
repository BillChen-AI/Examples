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
        [FieldQuoted(QuoteMode.OptionalForBoth, MultilineMode.NotAllow)]
        public string LocationIdentifier;

        [FieldOrder(2)]
        [FieldNotEmpty]
        [FieldQuoted(QuoteMode.OptionalForBoth, MultilineMode.NotAllow)]
        public string LocationName;

        [FieldOrder(3)]
        [FieldNotEmpty]
        [FieldQuoted(QuoteMode.OptionalForBoth, MultilineMode.NotAllow)]
        public string LocationPath;

        [FieldOrder(4)]
        [FieldNotEmpty]
        [FieldQuoted(QuoteMode.OptionalForBoth, MultilineMode.NotAllow)]
        public string LocationType;

        [FieldOrder(5)]
        [FieldQuoted(QuoteMode.OptionalForBoth, MultilineMode.NotAllow)]
        [FieldConverter(typeof(TimeSpanConverter))]
        public TimeSpan? UtcOffset;

        [FieldOrder(6)]
        [FieldQuoted(QuoteMode.OptionalForBoth, MultilineMode.AllowForBoth)]
        public string Description;

        [FieldOrder(7)]
        [FieldQuoted(QuoteMode.OptionalForBoth, MultilineMode.NotAllow)]
        public double? Latitude;

        [FieldOrder(8)]
        [FieldQuoted(QuoteMode.OptionalForBoth, MultilineMode.NotAllow)]
        public double? Longitude;

        [FieldOrder(9)]
        [FieldQuoted(QuoteMode.OptionalForBoth, MultilineMode.NotAllow)]
        public double? Elevation;

        [FieldOrder(10)]
        [FieldQuoted(QuoteMode.OptionalForBoth, MultilineMode.NotAllow)]
        public string ElevationUnits;
    }
}
