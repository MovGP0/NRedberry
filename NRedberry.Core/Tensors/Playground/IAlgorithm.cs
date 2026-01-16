using System.Diagnostics;

namespace NRedberry.Tensors.Playground;

/*
 * Original: ./core/src/main/java/cc/redberry/core/tensor/playground/IAlgorithm.java
 */

public abstract class IAlgorithm(string name)
{
    public string Name { get; } = name;

    public long Timing { get; protected set; }

    public long TimingMillis()
    {
        return Timing * 1000 / Stopwatch.Frequency;
    }

    public ProductData Calc(Tensor tensor)
    {
        ArgumentNullException.ThrowIfNull(tensor);

        long start = Stopwatch.GetTimestamp();
        ProductData data = CalcCore(tensor);
        Timing += Stopwatch.GetTimestamp() - start;
        return data;
    }

    public void Restart()
    {
        Timing = 0;
    }

    protected abstract ProductData CalcCore(Tensor tensor);
}
