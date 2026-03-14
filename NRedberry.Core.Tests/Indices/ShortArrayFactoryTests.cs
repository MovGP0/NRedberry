using System;
using System.Reflection;
using NRedberry.Indices;
using Xunit;

namespace NRedberry.Core.Tests.Indices;

public sealed class ShortArrayFactoryTests
{
    private const string FactoryTypeName = "NRedberry.Indices.ShortArrayFactory";

    private static readonly MethodInfo s_getZeroFilledShortArrayMethod = GetFactoryMethod();

    [Fact]
    public void GetZeroFilledShortArrayShouldReturnNewArrayForLengthGreaterThanOrEqualTo128()
    {
        short[] firstBoundary = InvokeGetZeroFilledShortArray(128);
        short[] secondBoundary = InvokeGetZeroFilledShortArray(128);
        short[] firstLarge = InvokeGetZeroFilledShortArray(129);
        short[] secondLarge = InvokeGetZeroFilledShortArray(129);

        secondBoundary.ShouldNotBeSameAs(firstBoundary);
        secondLarge.ShouldNotBeSameAs(firstLarge);
        Assert.All(firstBoundary, value => value.ShouldBe((short)0));
        Assert.All(secondBoundary, value => value.ShouldBe((short)0));
        Assert.All(firstLarge, value => value.ShouldBe((short)0));
        Assert.All(secondLarge, value => value.ShouldBe((short)0));
    }

    [Fact]
    public void GetZeroFilledShortArrayShouldReturnCachedArrayForSmallLengths()
    {
        short[] first = InvokeGetZeroFilledShortArray(5);
        short[] second = InvokeGetZeroFilledShortArray(5);

        second.ShouldBeSameAs(first);
        Assert.All(first, value => value.ShouldBe((short)0));
    }

    [Fact]
    public void GetZeroFilledShortArrayShouldKeepZeroLengthBehaviorStable()
    {
        short[] first = InvokeGetZeroFilledShortArray(0);
        short[] second = InvokeGetZeroFilledShortArray(0);

        second.ShouldBeSameAs(first);
        first.ShouldBeEmpty();
    }

    [Fact]
    public void GetZeroFilledShortArrayShouldThrowRuntimeExceptionForNegativeLength()
    {
        TargetInvocationException exception = Should.Throw<TargetInvocationException>(() => InvokeGetZeroFilledShortArray(-1));

        exception.InnerException.ShouldNotBeNull();
        exception.InnerException is IndexOutOfRangeException or OverflowException.ShouldBeTrue($"Unexpected runtime exception type: {exception.InnerException.GetType().FullName}");
    }

    private static short[] InvokeGetZeroFilledShortArray(int length)
    {
        return (short[])s_getZeroFilledShortArrayMethod.Invoke(null, [length])!;
    }

    private static MethodInfo GetFactoryMethod()
    {
        Type? type = typeof(IndicesFactory).Assembly.GetType(FactoryTypeName, throwOnError: true);
        MethodInfo? method = type?.GetMethod(
            "GetZeroFilledShortArray",
            BindingFlags.Public | BindingFlags.Static,
            binder: null,
            types: [typeof(int)],
            modifiers: null);

        method.ShouldNotBeNull();
        return method!;
    }
}
