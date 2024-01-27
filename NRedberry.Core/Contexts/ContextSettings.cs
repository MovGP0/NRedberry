using System;
using System.Collections.Generic;
using NRedberry.Core.Parsers;

namespace NRedberry.Contexts;

public sealed class ContextSettings
{
    private string kronecker = "d";
    private string metricName = "g";

    public ContextSettings(
        OutputFormat defaultOutputFormat = OutputFormat.Redberry,
        string kronecker = "g",
        string metricName = "g")
    {
        DefaultOutputFormat = defaultOutputFormat;
        Kronecker = kronecker;
        MetricName = metricName;
    }

    public HashSet<IndexType> MetricTypes { get; } = new();

    public void RemoveMetricIndexType(IndexType type)
    {
        MetricTypes.Remove(type);
    }

    public void AddMetricIndexType(IndexType type)
    {
        MetricTypes.Add(type);
    }

    public OutputFormat DefaultOutputFormat { get; set; }

    public string Kronecker
    {
        get => kronecker;
        set
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (value.Length == 0) throw new ArgumentException("Kronecker tensor name cannot be empty.");
            kronecker = value;
        }
    }

    public string MetricName
    {
        get => metricName;
        set
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (value.Length == 0) throw new ArgumentException("Metric tensor name cannot be empty.");
            metricName = value;
        }
    }

    public long NameManagerSeed { get; set; }

    public IndexConverterManager ConverterManager { get; set; } = IndexConverterManager.Default;

    public Parser Parser { get; set; } = Parser.Default;
}