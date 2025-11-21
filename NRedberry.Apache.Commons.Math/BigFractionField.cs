namespace NRedberry.Apache.Commons.Math;

public sealed class BigFractionField: IField<BigFraction>
{
    public BigFraction Zero { get; } = new(0,1);
    public BigFraction One { get; } = new (1,1);

    public Type GetRuntimeClass() => typeof(BigFraction);
}
