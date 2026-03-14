using System.Collections;

namespace NRedberry.Core.Utils;

/// <summary>
/// Wraps an <see cref="IEnumerator{T}"/> and reports percentage progress as it iterates.
/// Port of cc.redberry.core.utils.IteratorWithProgress.
/// </summary>
public class IteratorWithProgress<T>(
    IEnumerator<T> innerIterator,
    long totalCount,
    IteratorWithProgress<T>.IConsumer consumer)
    : IEnumerator<T>
{
    private readonly IEnumerator<T> _innerIterator = innerIterator ?? throw new ArgumentNullException(nameof(innerIterator));
    private readonly IConsumer _consumer = consumer ?? throw new ArgumentNullException(nameof(consumer));

    private int _prevPercent = -1;
    private long _currentPosition;

    public bool MoveNext()
    {
        if (!_innerIterator.MoveNext())
        {
            return false;
        }

        _currentPosition++;
        int percent = (int)(100.0 * _currentPosition / totalCount);
        if (percent != _prevPercent)
        {
            _consumer.Consume(percent);
            _prevPercent = percent;
        }

        return true;
    }

    public void Reset()
    {
        _innerIterator.Reset();
        _currentPosition = 0;
        _prevPercent = -1;
    }

    public T Current => _innerIterator.Current;

    object IEnumerator.Current => Current!;

    public void Dispose()
    {
        _innerIterator.Dispose();
    }

    public interface IConsumer
    {
        void Consume(int percent);
    }
}
