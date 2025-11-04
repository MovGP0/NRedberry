using NRedberry.Core.Parsers;

namespace NRedberry.Contexts;

public sealed class ContextSettings
{
    public ContextSettings()
    {
        DefaultOutputFormat = OutputFormat.Redberry;
        Kronecker = "g";
        MetricName = "g";
    }

    public ContextSettings(
        OutputFormat defaultOutputFormat,
        string kronecker,
        string metricName)
    {
        DefaultOutputFormat = defaultOutputFormat;
        Kronecker = kronecker;
        MetricName = metricName;
    }

    public HashSet<IndexType> MetricTypes { get; } = [];

    public void RemoveMetricIndexType(IndexType type)
    {
        MetricTypes.Remove(type);
    }

    public void AddMetricIndexType(IndexType type)
    {
        MetricTypes.Add(type);
    }

    public OutputFormat DefaultOutputFormat { get; set; }

    private string kronecker = "d";

    public string Kronecker
    {
        get => kronecker;
        set
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            if (value.Length == 0)
                throw new ArgumentException("Kronecker tensor name cannot be empty.");
            kronecker = value;
        }
    }

    private string metricName = "g";

    public string MetricName
    {
        get => metricName;
        set
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            if (value.Length == 0)
                throw new ArgumentException("Metric tensor name cannot be empty.");
            metricName = value;
        }
    }

    public int NameManagerSeed { get; set; }

    public IndexConverterManager ConverterManager { get; set; } = IndexConverterManager.Default;

    public Parser Parser { get; set; } = Parser.Default;
}
