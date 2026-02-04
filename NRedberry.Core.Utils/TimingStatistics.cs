using System.Globalization;
using System.Text;

namespace NRedberry.Core.Utils;

/// <summary>
/// Port of cc.redberry.core.utils.TimingStatistics.
/// </summary>
public sealed class TimingStatistics
{
    private readonly SortedDictionary<TransformationWithTimer, StatEntry> _set = new();

    public void Track(params TransformationWithTimer[] transformations)
    {
        Track((IEnumerable<TransformationWithTimer>)transformations);
    }

    public void Track(IEnumerable<TransformationWithTimer> transformations)
    {
        foreach (TransformationWithTimer transformation in transformations)
        {
            _set[transformation] = Empty;
            transformation.Reset();
        }
    }

    public void ResetAll()
    {
        foreach (TransformationWithTimer transformation in _set.Keys)
        {
            transformation.Reset();
        }
    }

    public void Merge(TimingStatistics stats)
    {
        ArgumentNullException.ThrowIfNull(stats);

        lock (_set)
        {
            foreach (KeyValuePair<TransformationWithTimer, StatEntry> entry in stats._set)
            {
                TransformationWithTimer tr = entry.Key;
                TransformationWithTimer? existing = FindFloorKey(_set, tr);
                if (existing is null || existing.CompareTo(tr) != 0)
                {
                    _set[tr] = entry.Value;
                    tr.Reset();
                }
                else
                {
                    _set[existing] = _set[existing].Add(entry.Value);
                }
            }
        }
    }

    public static string ToStringStatistics(
        SortedDictionary<TransformationWithTimer, StatEntry> data,
        long div,
        string unit)
    {
        string totalStr = "Total";
        int longestString = totalStr.Length;
        foreach (TransformationWithTimer tr in data.Keys)
        {
            longestString = Math.Max(tr.ToString().Length, longestString);
        }

        StringBuilder sb = new();
        long totalTiming = 0;
        long totalInvocations = 0;
        foreach (KeyValuePair<TransformationWithTimer, StatEntry> entry in data)
        {
            string label = entry.Key.ToString();
            label += EmptyString(longestString - label.Length);
            long timing = entry.Value.Elapsed / div;
            long invocations = entry.Value.Invocations;
            totalTiming += timing;
            totalInvocations += invocations;
            sb.Append(label)
                .Append(FormatNumber(timing))
                .Append(unit)
                .Append(" (")
                .Append(FormatNumber(invocations))
                .Append(" invocations)")
                .Append('\n');
        }

        totalStr += EmptyString(longestString - totalStr.Length);
        sb.Append(totalStr)
            .Append(FormatNumber(totalTiming))
            .Append(unit)
            .Append(" (")
            .Append(FormatNumber(totalInvocations))
            .Append(" invocations)");

        return sb.ToString();
    }

    public string ToStringNanos()
    {
        CollectStatistics();
        return ToStringStatistics(_set, 1, "ns");
    }

    public string ToStringMicros()
    {
        CollectStatistics();
        return ToStringStatistics(_set, 1_000, "us");
    }

    public string ToStringMillis()
    {
        CollectStatistics();
        return ToStringStatistics(_set, 1_000_000, "ms");
    }

    public string ToStringSeconds()
    {
        CollectStatistics();
        return ToStringStatistics(_set, 1_000_000_000, "s");
    }

    public override string ToString()
    {
        return ToStringMillis();
    }

    private void CollectStatistics()
    {
        foreach (KeyValuePair<TransformationWithTimer, StatEntry> entry in _set.ToArray())
        {
            TransformationWithTimer tr = entry.Key;
            StatEntry stats = (StatEntry)tr.Stats();
            _set[tr] = entry.Value.Add(stats);
            tr.Reset();
        }
    }

    private static TransformationWithTimer? FindFloorKey(
        SortedDictionary<TransformationWithTimer, StatEntry> data,
        TransformationWithTimer key)
    {
        TransformationWithTimer? candidate = null;
        foreach (TransformationWithTimer current in data.Keys)
        {
            if (current.CompareTo(key) > 0)
            {
                break;
            }

            candidate = current;
        }

        return candidate;
    }

    private static string EmptyString(int length)
    {
        char[] arr = new char[length + 3];
        Array.Fill(arr, ' ');
        arr[length + 1] = ':';
        return new string(arr);
    }

    private static string FormatNumber(long value)
    {
        return value.ToString("N0", CultureInfo.InvariantCulture);
    }

    private static readonly StatEntry Empty = new(0, 0);

    public sealed class StatEntry
    {
        public StatEntry(long elapsed, long invocations)
        {
            Elapsed = elapsed;
            Invocations = invocations;
        }

        public long Elapsed { get; }

        public long Invocations { get; }

        public StatEntry Add(StatEntry other)
        {
            return new StatEntry(Elapsed + other.Elapsed, Invocations + other.Invocations);
        }
    }
}
