using Aquarius.TimeSeries.Client.ServiceModels.Provisioning;
using FileHelpers;

namespace ProvisioningTool.Parsers
{
    [IgnoreFirst(1)]
    [DelimitedRecord(",")]
    [IgnoreEmptyLines]
    public sealed class ParsedParameter
    {
        [FieldOrder(1)]
        [FieldNotEmpty]
        [FieldQuoted(QuoteMode.OptionalForBoth, MultilineMode.NotAllow)]
        public string ParameterId;

        [FieldOrder(2)]
        [FieldNotEmpty]
        [FieldQuoted(QuoteMode.OptionalForBoth, MultilineMode.NotAllow)]
        public string Identifier;

        [FieldNotEmpty]
        [FieldOrder(3)]
        [FieldQuoted(QuoteMode.OptionalForBoth, MultilineMode.NotAllow)]
        public string DisplayName;

        [FieldOrder(4)]
        [FieldNotEmpty]
        [FieldQuoted(QuoteMode.OptionalForBoth, MultilineMode.NotAllow)]
        public string UnitGroupIdentifier;

        [FieldOrder(5)]
        [FieldNotEmpty]
        [FieldQuoted(QuoteMode.OptionalForBoth, MultilineMode.NotAllow)]
        public string UnitIdentifier;

        [FieldOrder(6)]
        [FieldNotEmpty]
        [FieldQuoted(QuoteMode.OptionalForBoth, MultilineMode.NotAllow)]
        public InterpolationType InterpolationType;

        [FieldOrder(7)]
        [FieldQuoted(QuoteMode.OptionalForBoth, MultilineMode.NotAllow)]
        public double? MinValue;

        [FieldOrder(8)]
        [FieldQuoted(QuoteMode.OptionalForBoth, MultilineMode.NotAllow)]
        public double? MaxValue;

        [FieldOrder(9)]
        [FieldQuoted(QuoteMode.OptionalForBoth, MultilineMode.NotAllow)]
        public string RoundingSpec;
    }
}
