using System.Globalization;
using System.Text;
using System.Threading;

namespace NRedberry.Core.Utils;

/// <summary>
/// Console progress bar with percentage steps.
/// Port of cc.redberry.core.utils.ProgressReporter.
/// </summary>
public sealed class ProgressReporter
{
    private readonly string prefix;
    private readonly long limit;
    private readonly double percentStep;
    private readonly int barLength;
    private readonly NumberFormatInfo formatInfo;
    private long progress;

    private readonly object lockObj = new();
    private double previousShown;

    public ProgressReporter(string prefix, long limit, double percentStep)
    {
        this.prefix = prefix;
        this.limit = limit;
        this.percentStep = percentStep / 100.0;
        barLength = 100;
        formatInfo = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
        formatInfo.NumberDecimalDigits = 2;
    }

    /// <summary>
    /// Advance progress by one item; prints when the configured percent step is reached.
    /// </summary>
    /// <returns>true if a new line was printed; false otherwise.</returns>
    public bool Next()
    {
        long current = Interlocked.Increment(ref progress);
        if (current > limit)
        {
            return false;
        }

        double pr = Round((double)current / limit);
        double target = Round(previousShown + percentStep);
        if (pr >= target)
        {
            lock (lockObj)
            {
                if (pr >= target)
                {
                    previousShown = pr;
                    Print();
                    return true;
                }
            }
        }

        return false;
    }

    private static double Round(double v) => Math.Round(10000.0 * v) / 10000.0;

    private void Print()
    {
        string percentStr = "(" + (100.0 * previousShown).ToString("F2", formatInfo) + "%)";
        var sb = new StringBuilder();
        sb.Append(prefix);
        sb.Append(percentStr);
        sb.Append('[');

        int filled = (int)(previousShown * barLength);
        int i = 0;
        for (; i < filled; ++i)
            sb.Append('=');
        sb.Append('>');
        if (percentStr.Length < barLength - filled)
        {
            sb.Append(percentStr);
            i += percentStr.Length;
        }

        for (; i < barLength; ++i)
            sb.Append(' ');
        sb.Append(']');
        sb.Append('\n');
        Console.Write(sb.ToString());
    }
}
