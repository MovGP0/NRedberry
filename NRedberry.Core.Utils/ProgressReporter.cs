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
    private readonly string _prefix;
    private readonly long _limit;
    private readonly double _percentStep;
    private readonly int _barLength;
    private readonly NumberFormatInfo _formatInfo;
    private long _progress;

    private readonly object _lock = new();
    private double _previousShown;

    public ProgressReporter(string prefix, long limit, double percentStep)
    {
        _prefix = prefix;
        _limit = limit;
        _percentStep = percentStep / 100.0;
        _barLength = 100;
        _formatInfo = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
        _formatInfo.NumberDecimalDigits = 2;
    }

    /// <summary>
    /// Advance progress by one item; prints when the configured percent step is reached.
    /// </summary>
    /// <returns>true if a new line was printed; false otherwise.</returns>
    public bool Next()
    {
        long current = Interlocked.Increment(ref _progress);
        if (current > _limit)
        {
            return false;
        }

        double pr = Round((double)current / _limit);
        double target = Round(_previousShown + _percentStep);
        if (pr >= target)
        {
            lock (_lock)
            {
                if (pr >= target)
                {
                    _previousShown = pr;
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
        string percentStr = "(" + (100.0 * _previousShown).ToString("F2", _formatInfo) + "%)";
        StringBuilder sb = new();
        sb.Append(_prefix);
        sb.Append(percentStr);
        sb.Append('[');

        int filled = (int)(_previousShown * _barLength);
        int i = 0;
        for (; i < filled; ++i)
        {
            sb.Append('=');
        }

        sb.Append('>');
        if (percentStr.Length < _barLength - filled)
        {
            sb.Append(percentStr);
            i += percentStr.Length;
        }

        for (; i < _barLength; ++i)
        {
            sb.Append(' ');
        }

        sb.Append(']');
        sb.Append('\n');
        Console.Write(sb.ToString());
    }
}
