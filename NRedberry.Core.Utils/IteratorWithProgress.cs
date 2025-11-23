using System.Collections;

namespace NRedberry.Core.Utils;

/// <summary>
/// Wraps an <see cref="IEnumerator{T}"/> and reports percentage progress as it iterates.
/// Port of cc.redberry.core.utils.IteratorWithProgress.
/// </summary>
public class IteratorWithProgress<T> : IEnumerator<T>
{
    private readonly IEnumerator<T> innerIterator;
    private readonly long totalCount;
    private readonly IConsumer consumer;

    private int prevPercent = -1;
    private long currentPosition;

    public IteratorWithProgress(IEnumerator<T> innerIterator, long totalCount, IConsumer consumer)
    {
        this.innerIterator = innerIterator ?? throw new ArgumentNullException(nameof(innerIterator));
        this.totalCount = totalCount;
        this.consumer = consumer ?? throw new ArgumentNullException(nameof(consumer));
    }

    public bool MoveNext()
    {
        if (!innerIterator.MoveNext())
            return false;

        ++currentPosition;
        int percent = (int)(100.0 * currentPosition / totalCount);
        if (percent != prevPercent)
        {
            consumer.Consume(percent);
            prevPercent = percent;
        }

        return true;
    }

    public void Reset()
    {
        innerIterator.Reset();
        currentPosition = 0;
        prevPercent = -1;
    }

    public T Current => innerIterator.Current;

    object IEnumerator.Current => Current!;

    public void Dispose() => innerIterator.Dispose();

    public interface IConsumer
    {
        void Consume(int percent);
    }
}
