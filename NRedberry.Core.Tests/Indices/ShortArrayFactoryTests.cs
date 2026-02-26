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

        Assert.NotSame(firstBoundary, secondBoundary);
        Assert.NotSame(firstLarge, secondLarge);
        Assert.All(firstBoundary, value => Assert.Equal((short)0, value));
        Assert.All(secondBoundary, value => Assert.Equal((short)0, value));
        Assert.All(firstLarge, value => Assert.Equal((short)0, value));
        Assert.All(secondLarge, value => Assert.Equal((short)0, value));
    }

    [Fact]
    public void GetZeroFilledShortArrayShouldReturnCachedArrayForSmallLengths()
    {
        short[] first = InvokeGetZeroFilledShortArray(5);
        short[] second = InvokeGetZeroFilledShortArray(5);

        Assert.Same(first, second);
        Assert.All(first, value => Assert.Equal((short)0, value));
    }

    [Fact]
    public void GetZeroFilledShortArrayShouldKeepZeroLengthBehaviorStable()
    {
        short[] first = InvokeGetZeroFilledShortArray(0);
        short[] second = InvokeGetZeroFilledShortArray(0);

        Assert.Same(first, second);
        Assert.Empty(first);
    }

    [Fact]
    public void GetZeroFilledShortArrayShouldThrowRuntimeExceptionForNegativeLength()
    {
        TargetInvocationException exception = Assert.Throws<TargetInvocationException>(() => InvokeGetZeroFilledShortArray(-1));

        Assert.NotNull(exception.InnerException);
        Assert.True(
            exception.InnerException is IndexOutOfRangeException or OverflowException,
            $"Unexpected runtime exception type: {exception.InnerException.GetType().FullName}");
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

        Assert.NotNull(method);
        return method!;
    }
}
