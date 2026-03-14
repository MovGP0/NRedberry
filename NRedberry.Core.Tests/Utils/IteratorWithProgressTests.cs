using NRedberry.Core.Utils;
using Xunit;

namespace NRedberry.Core.Tests.Utils;

public sealed class IteratorWithProgressTests
{
    [Fact]
    public void ShouldReportProgressAsItemsAdvance()
    {
        RecordingProgressConsumer consumer = new();
        using IteratorWithProgress<int> iterator =
            new(new List<int> { 1, 2, 3, 4 }.GetEnumerator(), 4, consumer);

        List<int> values = [];
        while (iterator.MoveNext())
        {
            values.Add(iterator.Current);
        }

        values.ShouldBe(new[] { 1, 2, 3, 4 });
        consumer.Values.ShouldBe(new[] { 25, 50, 75, 100 });
    }

    [Fact]
    public void ShouldResetUnderlyingState()
    {
        RecordingProgressConsumer consumer = new();
        using IteratorWithProgress<int> iterator =
            new(new List<int> { 10, 20 }.GetEnumerator(), 2, consumer);

        iterator.MoveNext().ShouldBeTrue();
        iterator.Reset();
        iterator.MoveNext().ShouldBeTrue();
        iterator.Current.ShouldBe(10);
    }
}

public sealed class RecordingProgressConsumer : IteratorWithProgress<int>.IConsumer
{
    public List<int> Values { get; } = [];

    public void Consume(int percent)
    {
        Values.Add(percent);
    }
}
