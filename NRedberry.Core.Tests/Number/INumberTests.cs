using System;
using System.Linq;
using System.Numerics;
using NRedberry.Apache.Commons.Math;
using Xunit;

namespace NRedberry.Core.Tests.Number;

public sealed class INumberTests
{
    [Fact]
    public void ShouldExtendIFieldElementOfSameGenericType()
    {
        var iNumberType = typeof(NRedberry.Numbers.INumber<>);
        var genericArgument = iNumberType.GetGenericArguments().Single();
        var expectedBaseInterface = typeof(IFieldElement<>).MakeGenericType(genericArgument);

        iNumberType.GetInterfaces().ShouldContain(expectedBaseInterface);
    }

    [Fact]
    public void ShouldExposeRequiredNumericConversionMethods()
    {
        var iNumberType = typeof(NRedberry.Numbers.INumber<>);
        var genericArgument = iNumberType.GetGenericArguments().Single();

        AssertMethodSignature(iNumberType, nameof(NRedberry.Numbers.INumber<int>.BigIntValue), typeof(BigInteger));
        AssertMethodSignature(iNumberType, nameof(NRedberry.Numbers.INumber<int>.IntValue), typeof(int));
        AssertMethodSignature(iNumberType, nameof(NRedberry.Numbers.INumber<int>.LongValue), typeof(long));
        AssertMethodSignature(iNumberType, nameof(NRedberry.Numbers.INumber<int>.DoubleValue), typeof(double));
        AssertMethodSignature(iNumberType, nameof(NRedberry.Numbers.INumber<int>.FloatValue), typeof(float));
        AssertMethodSignature(iNumberType, nameof(NRedberry.Numbers.INumber<int>.GetNumericValue), genericArgument);
        AssertMethodSignature(iNumberType, nameof(NRedberry.Numbers.INumber<int>.Abs), genericArgument);
    }

    [Fact]
    public void ShouldExposeRequiredAddSubtractDivideMultiplyOverloads()
    {
        var iNumberType = typeof(NRedberry.Numbers.INumber<>);
        var genericArgument = iNumberType.GetGenericArguments().Single();

        AssertMethodSignature(iNumberType, nameof(NRedberry.Numbers.INumber<int>.Add), genericArgument, typeof(double));
        AssertMethodSignature(iNumberType, nameof(NRedberry.Numbers.INumber<int>.Add), genericArgument, typeof(int));
        AssertMethodSignature(iNumberType, nameof(NRedberry.Numbers.INumber<int>.Add), genericArgument, typeof(long));
        AssertMethodSignature(iNumberType, nameof(NRedberry.Numbers.INumber<int>.Add), genericArgument, typeof(BigInteger));
        AssertMethodSignature(iNumberType, nameof(NRedberry.Numbers.INumber<int>.Add), genericArgument, typeof(BigFraction));

        AssertMethodSignature(iNumberType, nameof(NRedberry.Numbers.INumber<int>.Subtract), genericArgument, typeof(double));
        AssertMethodSignature(iNumberType, nameof(NRedberry.Numbers.INumber<int>.Subtract), genericArgument, typeof(int));
        AssertMethodSignature(iNumberType, nameof(NRedberry.Numbers.INumber<int>.Subtract), genericArgument, typeof(long));
        AssertMethodSignature(iNumberType, nameof(NRedberry.Numbers.INumber<int>.Subtract), genericArgument, typeof(BigInteger));
        AssertMethodSignature(iNumberType, nameof(NRedberry.Numbers.INumber<int>.Subtract), genericArgument, typeof(BigFraction));

        AssertMethodSignature(iNumberType, nameof(NRedberry.Numbers.INumber<int>.Divide), genericArgument, typeof(double));
        AssertMethodSignature(iNumberType, nameof(NRedberry.Numbers.INumber<int>.Divide), genericArgument, typeof(int));
        AssertMethodSignature(iNumberType, nameof(NRedberry.Numbers.INumber<int>.Divide), genericArgument, typeof(long));
        AssertMethodSignature(iNumberType, nameof(NRedberry.Numbers.INumber<int>.Divide), genericArgument, typeof(BigInteger));
        AssertMethodSignature(iNumberType, nameof(NRedberry.Numbers.INumber<int>.Divide), genericArgument, typeof(BigFraction));

        AssertMethodSignature(iNumberType, nameof(NRedberry.Numbers.INumber<int>.Multiply), genericArgument, typeof(double));
        AssertMethodSignature(iNumberType, nameof(NRedberry.Numbers.INumber<int>.Multiply), genericArgument, typeof(int));
        AssertMethodSignature(iNumberType, nameof(NRedberry.Numbers.INumber<int>.Multiply), genericArgument, typeof(long));
        AssertMethodSignature(iNumberType, nameof(NRedberry.Numbers.INumber<int>.Multiply), genericArgument, typeof(BigInteger));
        AssertMethodSignature(iNumberType, nameof(NRedberry.Numbers.INumber<int>.Multiply), genericArgument, typeof(BigFraction));
    }

    [Fact]
    public void ShouldExposeRequiredPowOverloadsAndPredicateMethods()
    {
        var iNumberType = typeof(NRedberry.Numbers.INumber<>);
        var genericArgument = iNumberType.GetGenericArguments().Single();

        AssertMethodSignature(iNumberType, nameof(NRedberry.Numbers.INumber<int>.Pow), genericArgument, typeof(double));
        AssertMethodSignature(iNumberType, nameof(NRedberry.Numbers.INumber<int>.Pow), genericArgument, typeof(BigInteger));
        AssertMethodSignature(iNumberType, nameof(NRedberry.Numbers.INumber<int>.Pow), genericArgument, typeof(long));
        AssertMethodSignature(iNumberType, nameof(NRedberry.Numbers.INumber<int>.Pow), genericArgument, typeof(int));

        AssertMethodSignature(iNumberType, nameof(NRedberry.Numbers.INumber<int>.IsInfinite), typeof(bool));
        AssertMethodSignature(iNumberType, nameof(NRedberry.Numbers.INumber<int>.IsNaN), typeof(bool));
        AssertMethodSignature(iNumberType, nameof(NRedberry.Numbers.INumber<int>.IsZero), typeof(bool));
        AssertMethodSignature(iNumberType, nameof(NRedberry.Numbers.INumber<int>.IsOne), typeof(bool));
        AssertMethodSignature(iNumberType, nameof(NRedberry.Numbers.INumber<int>.IsMinusOne), typeof(bool));
        AssertMethodSignature(iNumberType, nameof(NRedberry.Numbers.INumber<int>.IsNumeric), typeof(bool));
        AssertMethodSignature(iNumberType, nameof(NRedberry.Numbers.INumber<int>.IsInteger), typeof(bool));
        AssertMethodSignature(iNumberType, nameof(NRedberry.Numbers.INumber<int>.IsNatural), typeof(bool));
    }

    private static void AssertMethodSignature(
        Type interfaceType,
        string methodName,
        Type expectedReturnType,
        params Type[] parameterTypes)
    {
        var method = interfaceType.GetMethod(methodName, parameterTypes);

        Assert.True(method is not null, $"Expected method '{methodName}({string.Join(", ", parameterTypes.Select(p => p.Name))})' was not found.");
        method!.ReturnType.ShouldBe(expectedReturnType);
    }
}
