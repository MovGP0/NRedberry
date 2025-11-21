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
        throw new NotImplementedException();
    }

    public ProductData Calc(Tensor tensor)
    {
        throw new NotImplementedException();
    }

    public void Restart()
    {
        throw new NotImplementedException();
    }

    protected abstract ProductData CalcCore(Tensor tensor);
}
