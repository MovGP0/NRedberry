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

        ShouldHaveMethodSignature(iNumberType, nameof(NRedberry.Numbers.INumber<int>.BigIntValue), typeof(BigInteger));
        ShouldHaveMethodSignature(iNumberType, nameof(NRedberry.Numbers.INumber<int>.IntValue), typeof(int));
        ShouldHaveMethodSignature(iNumberType, nameof(NRedberry.Numbers.INumber<int>.LongValue), typeof(long));
        ShouldHaveMethodSignature(iNumberType, nameof(NRedberry.Numbers.INumber<int>.DoubleValue), typeof(double));
        ShouldHaveMethodSignature(iNumberType, nameof(NRedberry.Numbers.INumber<int>.FloatValue), typeof(float));
        ShouldHaveMethodSignature(iNumberType, nameof(NRedberry.Numbers.INumber<int>.GetNumericValue), genericArgument);
        ShouldHaveMethodSignature(iNumberType, nameof(NRedberry.Numbers.INumber<int>.Abs), genericArgument);
    }

    [Fact]
    public void ShouldExposeRequiredAddSubtractDivideMultiplyOverloads()
    {
        var iNumberType = typeof(NRedberry.Numbers.INumber<>);
        var genericArgument = iNumberType.GetGenericArguments().Single();

        ShouldHaveMethodSignature(iNumberType, nameof(NRedberry.Numbers.INumber<int>.Add), genericArgument, typeof(double));
        ShouldHaveMethodSignature(iNumberType, nameof(NRedberry.Numbers.INumber<int>.Add), genericArgument, typeof(int));
        ShouldHaveMethodSignature(iNumberType, nameof(NRedberry.Numbers.INumber<int>.Add), genericArgument, typeof(long));
        ShouldHaveMethodSignature(iNumberType, nameof(NRedberry.Numbers.INumber<int>.Add), genericArgument, typeof(BigInteger));
        ShouldHaveMethodSignature(iNumberType, nameof(NRedberry.Numbers.INumber<int>.Add), genericArgument, typeof(BigFraction));

        ShouldHaveMethodSignature(iNumberType, nameof(NRedberry.Numbers.INumber<int>.Subtract), genericArgument, typeof(double));
        ShouldHaveMethodSignature(iNumberType, nameof(NRedberry.Numbers.INumber<int>.Subtract), genericArgument, typeof(int));
        ShouldHaveMethodSignature(iNumberType, nameof(NRedberry.Numbers.INumber<int>.Subtract), genericArgument, typeof(long));
        ShouldHaveMethodSignature(iNumberType, nameof(NRedberry.Numbers.INumber<int>.Subtract), genericArgument, typeof(BigInteger));
        ShouldHaveMethodSignature(iNumberType, nameof(NRedberry.Numbers.INumber<int>.Subtract), genericArgument, typeof(BigFraction));

        ShouldHaveMethodSignature(iNumberType, nameof(NRedberry.Numbers.INumber<int>.Divide), genericArgument, typeof(double));
        ShouldHaveMethodSignature(iNumberType, nameof(NRedberry.Numbers.INumber<int>.Divide), genericArgument, typeof(int));
        ShouldHaveMethodSignature(iNumberType, nameof(NRedberry.Numbers.INumber<int>.Divide), genericArgument, typeof(long));
        ShouldHaveMethodSignature(iNumberType, nameof(NRedberry.Numbers.INumber<int>.Divide), genericArgument, typeof(BigInteger));
        ShouldHaveMethodSignature(iNumberType, nameof(NRedberry.Numbers.INumber<int>.Divide), genericArgument, typeof(BigFraction));

        ShouldHaveMethodSignature(iNumberType, nameof(NRedberry.Numbers.INumber<int>.Multiply), genericArgument, typeof(double));
        ShouldHaveMethodSignature(iNumberType, nameof(NRedberry.Numbers.INumber<int>.Multiply), genericArgument, typeof(int));
        ShouldHaveMethodSignature(iNumberType, nameof(NRedberry.Numbers.INumber<int>.Multiply), genericArgument, typeof(long));
        ShouldHaveMethodSignature(iNumberType, nameof(NRedberry.Numbers.INumber<int>.Multiply), genericArgument, typeof(BigInteger));
        ShouldHaveMethodSignature(iNumberType, nameof(NRedberry.Numbers.INumber<int>.Multiply), genericArgument, typeof(BigFraction));
    }

    [Fact]
    public void ShouldExposeRequiredPowOverloadsAndPredicateMethods()
    {
        var iNumberType = typeof(NRedberry.Numbers.INumber<>);
        var genericArgument = iNumberType.GetGenericArguments().Single();

        ShouldHaveMethodSignature(iNumberType, nameof(NRedberry.Numbers.INumber<int>.Pow), genericArgument, typeof(double));
        ShouldHaveMethodSignature(iNumberType, nameof(NRedberry.Numbers.INumber<int>.Pow), genericArgument, typeof(BigInteger));
        ShouldHaveMethodSignature(iNumberType, nameof(NRedberry.Numbers.INumber<int>.Pow), genericArgument, typeof(long));
        ShouldHaveMethodSignature(iNumberType, nameof(NRedberry.Numbers.INumber<int>.Pow), genericArgument, typeof(int));

        ShouldHaveMethodSignature(iNumberType, nameof(NRedberry.Numbers.INumber<int>.IsInfinite), typeof(bool));
        ShouldHaveMethodSignature(iNumberType, nameof(NRedberry.Numbers.INumber<int>.IsNaN), typeof(bool));
        ShouldHaveMethodSignature(iNumberType, nameof(NRedberry.Numbers.INumber<int>.IsZero), typeof(bool));
        ShouldHaveMethodSignature(iNumberType, nameof(NRedberry.Numbers.INumber<int>.IsOne), typeof(bool));
        ShouldHaveMethodSignature(iNumberType, nameof(NRedberry.Numbers.INumber<int>.IsMinusOne), typeof(bool));
        ShouldHaveMethodSignature(iNumberType, nameof(NRedberry.Numbers.INumber<int>.IsNumeric), typeof(bool));
        ShouldHaveMethodSignature(iNumberType, nameof(NRedberry.Numbers.INumber<int>.IsInteger), typeof(bool));
        ShouldHaveMethodSignature(iNumberType, nameof(NRedberry.Numbers.INumber<int>.IsNatural), typeof(bool));
    }

    private static void ShouldHaveMethodSignature(
        Type interfaceType,
        string methodName,
        Type expectedReturnType,
        params Type[] parameterTypes)
    {
        var method = interfaceType.GetMethod(methodName, parameterTypes);

        method.ShouldNotBeNull($"Expected method '{methodName}({string.Join(", ", parameterTypes.Select(p => p.Name))})' was not found.");
        method!.ReturnType.ShouldBe(expectedReturnType);
    }
}
