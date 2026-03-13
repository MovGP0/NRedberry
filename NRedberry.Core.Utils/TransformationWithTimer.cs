using System.Diagnostics;
using System.Threading;

namespace NRedberry.Core.Utils;

/// <summary>
/// Adapter interface for timed transformations in the utility layer.
/// </summary>
public interface ITimerTransformation
{
    object Transform(object input);

    string ToString(object outputFormat);
}

/// <summary>
/// Port of cc.redberry.core.utils.TransformationWithTimer.
/// </summary>
public sealed class TransformationWithTimer : IComparable<TransformationWithTimer>
{
    private long _elapsedNanos;
    private long _invocations;

    /// <summary>
    /// Gets the wrapped transformation.
    /// </summary>
    public object Transformation { get; }

    /// <summary>
    /// Gets the optional display name.
    /// </summary>
    public string? Name { get; }

    /// <summary>
    /// Initializes a new instance wrapping the specified transformation.
    /// </summary>
    /// <param name="transformation">The transformation to wrap.</param>
    public TransformationWithTimer(object transformation)
        : this(transformation, null)
    {
    }

    /// <summary>
    /// Initializes a new instance with the specified transformation and name.
    /// </summary>
    /// <param name="transformation">The transformation to wrap.</param>
    /// <param name="name">An optional name.</param>
    public TransformationWithTimer(object transformation, string? name)
    {
        ArgumentNullException.ThrowIfNull(transformation);

        Transformation = transformation;
        Name = name;
    }

    /// <summary>
    /// Returns the invocation count.
    /// </summary>
    /// <returns>The number of recorded invocations.</returns>
    public long Invocations()
    {
        return Interlocked.Read(ref _invocations);
    }

    /// <summary>
    /// Returns the accumulated elapsed time in nanoseconds.
    /// </summary>
    /// <returns>The elapsed time.</returns>
    public long Elapsed()
    {
        return ElapsedNanos();
    }

    /// <summary>
    /// Returns the accumulated elapsed time in nanoseconds.
    /// </summary>
    /// <returns>The elapsed time in nanoseconds.</returns>
    public long ElapsedNanos()
    {
        return Interlocked.Read(ref _elapsedNanos);
    }

    /// <summary>
    /// Returns the accumulated elapsed time in microseconds.
    /// </summary>
    /// <returns>The elapsed time in microseconds.</returns>
    public long ElapsedMicros()
    {
        return ElapsedNanos() / 1_000L;
    }

    /// <summary>
    /// Returns the accumulated elapsed time in milliseconds.
    /// </summary>
    /// <returns>The elapsed time in milliseconds.</returns>
    public long ElapsedMillis()
    {
        return ElapsedNanos() / 1_000_000L;
    }

    /// <summary>
    /// Returns the accumulated elapsed time in seconds.
    /// </summary>
    /// <returns>The elapsed time in seconds.</returns>
    public long ElapsedSeconds()
    {
        return ElapsedNanos() / 1_000_000_000L;
    }

    /// <summary>
    /// Returns the accumulated elapsed time in minutes.
    /// </summary>
    /// <returns>The elapsed time in minutes.</returns>
    public long ElapsedMinutes()
    {
        return ElapsedSeconds() / 60L;
    }

    /// <summary>
    /// Resets the elapsed time counter.
    /// </summary>
    public void ResetTiming()
    {
        Interlocked.Exchange(ref _elapsedNanos, 0);
    }

    /// <summary>
    /// Resets the invocation counter.
    /// </summary>
    public void ResetInvocations()
    {
        Interlocked.Exchange(ref _invocations, 0);
    }

    /// <summary>
    /// Resets all counters.
    /// </summary>
    public void Reset()
    {
        ResetTiming();
        ResetInvocations();
    }

    /// <summary>
    /// Adds nanoseconds to the elapsed time counter.
    /// </summary>
    /// <param name="amount">Nanoseconds to add.</param>
    public void IncrementNanos(long amount)
    {
        if (amount < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount));
        }

        Interlocked.Add(ref _elapsedNanos, amount);
    }

    /// <summary>
    /// Transforms the specified payload and updates timing statistics.
    /// </summary>
    /// <param name="tensor">Input payload.</param>
    /// <returns>The transformed payload.</returns>
    public object Transform(object tensor)
    {
        ArgumentNullException.ThrowIfNull(tensor);

        object transformed;
        long start = Stopwatch.GetTimestamp();
        try
        {
            transformed = Transformation switch
            {
                ITimerTransformation timedTransformation => timedTransformation.Transform(tensor),
                Func<object, object> transform => transform(tensor),
                _ => throw new InvalidOperationException(
                    "Wrapped object must implement ITimerTransformation or be a Func<object, object>.")
            };
        }
        finally
        {
            long elapsedTicks = Stopwatch.GetTimestamp() - start;
            IncrementNanos(ToNanoseconds(elapsedTicks));
            Interlocked.Increment(ref _invocations);
        }

        return transformed;
    }

    /// <inheritdoc />
    public int CompareTo(TransformationWithTimer? other)
    {
        if (other is null)
        {
            return 1;
        }

        return string.Compare(ToString(), other.ToString(), StringComparison.Ordinal);
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return ToString(string.Empty);
    }

    /// <summary>
    /// Returns a formatted string representation.
    /// </summary>
    /// <param name="outputFormat">Desired output format.</param>
    /// <returns>String representation.</returns>
    public string ToString(object outputFormat)
    {
        if (Name is not null)
        {
            return Name;
        }

        return Transformation switch
        {
            ITimerTransformation timedTransformation => timedTransformation.ToString(outputFormat),
            _ => Transformation.ToString() ?? string.Empty
        };
    }

    /// <summary>
    /// Returns a snapshot of timing statistics.
    /// </summary>
    /// <returns>The statistics entry.</returns>
    public object Stats()
    {
        return new TimingStatistics.StatEntry(ElapsedNanos(), Invocations());
    }

    private static long ToNanoseconds(long elapsedTicks)
    {
        return elapsedTicks * 1_000_000_000L / Stopwatch.Frequency;
    }
}
