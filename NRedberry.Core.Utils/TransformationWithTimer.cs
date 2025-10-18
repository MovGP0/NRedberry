namespace NRedberry.Core.Utils;

/// <summary>
/// Port of cc.redberry.core.utils.TransformationWithTimer.
/// </summary>
public sealed class TransformationWithTimer : IComparable<TransformationWithTimer>
{
    /// <summary>
    /// Gets the wrapped transformation.
    /// </summary>
    public object Transformation { get; } = null!;

    /// <summary>
    /// Gets the optional display name.
    /// </summary>
    public string? Name { get; } = null;

    /// <summary>
    /// Initializes a new instance wrapping the specified transformation.
    /// </summary>
    /// <param name="transformation">The transformation to wrap.</param>
    public TransformationWithTimer(object transformation)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Initializes a new instance with the specified transformation and name.
    /// </summary>
    /// <param name="transformation">The transformation to wrap.</param>
    /// <param name="name">An optional name.</param>
    public TransformationWithTimer(object transformation, string? name)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Returns the invocation count.
    /// </summary>
    /// <returns>The number of recorded invocations.</returns>
    public long Invocations()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Returns the accumulated elapsed time in nanoseconds.
    /// </summary>
    /// <returns>The elapsed time.</returns>
    public long Elapsed()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Returns the accumulated elapsed time in nanoseconds.
    /// </summary>
    /// <returns>The elapsed time in nanoseconds.</returns>
    public long ElapsedNanos()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Returns the accumulated elapsed time in microseconds.
    /// </summary>
    /// <returns>The elapsed time in microseconds.</returns>
    public long ElapsedMicros()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Returns the accumulated elapsed time in milliseconds.
    /// </summary>
    /// <returns>The elapsed time in milliseconds.</returns>
    public long ElapsedMillis()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Returns the accumulated elapsed time in seconds.
    /// </summary>
    /// <returns>The elapsed time in seconds.</returns>
    public long ElapsedSeconds()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Returns the accumulated elapsed time in minutes.
    /// </summary>
    /// <returns>The elapsed time in minutes.</returns>
    public long ElapsedMinutes()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Resets the elapsed time counter.
    /// </summary>
    public void ResetTiming()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Resets the invocation counter.
    /// </summary>
    public void ResetInvocations()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Resets all counters.
    /// </summary>
    public void Reset()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Adds nanoseconds to the elapsed time counter.
    /// </summary>
    /// <param name="amount">Nanoseconds to add.</param>
    public void IncrementNanos(long amount)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Transforms the specified tensor and updates timing statistics.
    /// </summary>
    /// <param name="tensor">Tensor to transform.</param>
    /// <returns>The transformed tensor.</returns>
    public object Transform(object tensor)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public int CompareTo(TransformationWithTimer? other)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public override string ToString()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Returns a formatted string representation.
    /// </summary>
    /// <param name="outputFormat">Desired output format.</param>
    /// <returns>String representation.</returns>
    public string ToString(object outputFormat)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Returns a snapshot of timing statistics.
    /// </summary>
    /// <returns>The statistics entry.</returns>
    public object Stats()
    {
        throw new NotImplementedException();
    }
}
