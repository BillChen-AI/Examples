using System;
using Aquarius.TimeSeries.Client.ServiceModels.Provisioning;
using FileHelpers;

namespace ProvisioningTool.Parsers
{
    [IgnoreFirst]
    [DelimitedRecord(",")]
    public class ParsedTimeSeries
    {
        [FieldOrder(1)]
        [FieldNotEmpty]
        [FieldTrim(TrimMode.Both)]
        [FieldQuoted(QuoteMode.OptionalForBoth, MultilineMode.NotAllow)]
        public string LocationIdentifier { get; set; }

        [FieldOrder(2)]
        [FieldNotEmpty]
        [FieldQuoted(QuoteMode.OptionalForBoth, MultilineMode.NotAllow)]
        public TimeSeriesType TimeSeriesType { get; set; }

        [FieldOrder(3)]
        [FieldNotEmpty]
        [FieldTrim(TrimMode.Both)]
        [FieldQuoted(QuoteMode.OptionalForBoth, MultilineMode.NotAllow)]
        public string ParameterId { get; set; }

        [FieldOrder(4)]
        [FieldNotEmpty]
        [FieldTrim(TrimMode.Both)]
        [FieldQuoted(QuoteMode.OptionalForBoth, MultilineMode.NotAllow)]
        public string Label { get; set; }

        [FieldOrder(5)]
        [FieldNotEmpty]
        [FieldTrim(TrimMode.Both)]
        [FieldQuoted(QuoteMode.OptionalForBoth, MultilineMode.NotAllow)]
        public string UnitId { get; set; }

        [FieldOrder(6)]
        [FieldNotEmpty]
        [FieldQuoted(QuoteMode.OptionalForBoth, MultilineMode.NotAllow)]
        public InterpolationType InterpolationType { get; set; }

        [FieldOrder(7)]
        [FieldNotEmpty]
        [FieldQuoted(QuoteMode.OptionalForBoth, MultilineMode.NotAllow)]
        [FieldConverter(typeof(TimeSpanConverter))]
        public TimeSpan UtcOffset { get; set; }

        [FieldOrder(8)]
        [FieldOptional]
        [FieldNullValue(0)]
        [FieldQuoted(QuoteMode.OptionalForBoth, MultilineMode.NotAllow)]
        public int GapToleranceInMinutes { get; set; }

        [FieldOrder(9)]
        [FieldOptional]
        [FieldQuoted(QuoteMode.OptionalForBoth, MultilineMode.NotAllow)]
        public string Description { get; set; }

        [FieldOrder(10)]
        [FieldOptional]
        [FieldQuoted(QuoteMode.OptionalForBoth, MultilineMode.NotAllow)]
        public string Comment { get; set; }

        [FieldOrder(11)]
        [FieldNullValue(false)]
        [FieldTrim(TrimMode.Both)]
        [FieldOptional]
        [FieldQuoted(QuoteMode.OptionalForBoth, MultilineMode.NotAllow)]
        public bool Publish { get; set; }

        [FieldOrder(12)]
        [FieldTrim(TrimMode.Both)]
        [FieldOptional]
        [FieldQuoted(QuoteMode.OptionalForBoth, MultilineMode.NotAllow)]
        public string Method { get; set; }

        [FieldOrder(13)]
        [FieldTrim(TrimMode.Both)]
        [FieldOptional]
        [FieldQuoted(QuoteMode.OptionalForBoth, MultilineMode.NotAllow)]
        public string ComputationIdentifier { get; set; }

        [FieldOrder(14)]
        [FieldTrim(TrimMode.Both)]
        [FieldOptional]
        [FieldQuoted(QuoteMode.OptionalForBoth, MultilineMode.NotAllow)]
        public string ComputationPeriodIdentifier { get; set; }

        [FieldOrder(15)]
        [FieldTrim(TrimMode.Both)]
        [FieldOptional]
        [FieldQuoted(QuoteMode.OptionalForBoth, MultilineMode.NotAllow)]
        public string SubLocationIdentifier { get; set; }
    }
}
