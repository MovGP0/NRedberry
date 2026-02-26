using System.Reflection;
using NRedberry.IndexMapping;
using NRedberry.Numbers;
using Xunit;
using TensorFactory = NRedberry.Tensors.Tensors;
using TensorType = NRedberry.Tensors.Tensor;

namespace NRedberry.Core.Tests.Indexmapping;

public sealed class ProviderFunctionsTests
{
    [Theory]
    [InlineData("OddFactory")]
    [InlineData("EvenFactory")]
    [InlineData("Factory")]
    public void FactoryPropertiesShouldBeNonNullAndImplementFactoryInterface(string propertyName)
    {
        IIndexMappingProviderFactory factory = GetFactoryFromProperty(propertyName);

        Assert.NotNull(factory);
        Assert.IsAssignableFrom<IIndexMappingProviderFactory>(factory);
    }

    [Theory]
    [InlineData("CreateOdd", "provider")]
    [InlineData("CreateEven", "provider")]
    [InlineData("Create", "provider")]
    public void CreateMethodsShouldGuardNullProvider(string methodName, string expectedParamName)
    {
        MethodInfo method = GetCreateMethod(methodName);

        ArgumentNullException exception = AssertInnerException<ArgumentNullException>(
            Assert.Throws<TargetInvocationException>(() => method.Invoke(null, [null, Complex.One, Complex.Zero])));

        Assert.Equal(expectedParamName, exception.ParamName);
    }

    [Theory]
    [InlineData("CreateOdd", "from")]
    [InlineData("CreateEven", "from")]
    [InlineData("Create", "from")]
    public void CreateMethodsShouldGuardNullFrom(string methodName, string expectedParamName)
    {
        MethodInfo method = GetCreateMethod(methodName);

        ArgumentNullException exception = AssertInnerException<ArgumentNullException>(
            Assert.Throws<TargetInvocationException>(() => method.Invoke(null, [IndexMappingProviderUtil.EmptyProvider, null, Complex.Zero])));

        Assert.Equal(expectedParamName, exception.ParamName);
    }

    [Theory]
    [InlineData("CreateOdd", "to")]
    [InlineData("CreateEven", "to")]
    [InlineData("Create", "to")]
    public void CreateMethodsShouldGuardNullTo(string methodName, string expectedParamName)
    {
        MethodInfo method = GetCreateMethod(methodName);

        ArgumentNullException exception = AssertInnerException<ArgumentNullException>(
            Assert.Throws<TargetInvocationException>(() => method.Invoke(null, [IndexMappingProviderUtil.EmptyProvider, Complex.One, null])));

        Assert.Equal(expectedParamName, exception.ParamName);
    }

    [Theory]
    [InlineData("OddFactory", "provider")]
    [InlineData("EvenFactory", "provider")]
    [InlineData("Factory", "provider")]
    public void FactoryCreateShouldGuardNullProvider(string propertyName, string expectedParamName)
    {
        IIndexMappingProviderFactory factory = GetFactoryFromProperty(propertyName);

        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => factory.Create(null!, Complex.One, Complex.Zero));

        Assert.Equal(expectedParamName, exception.ParamName);
    }

    [Theory]
    [InlineData("OddFactory", "from")]
    [InlineData("EvenFactory", "from")]
    [InlineData("Factory", "from")]
    public void FactoryCreateShouldGuardNullFrom(string propertyName, string expectedParamName)
    {
        IIndexMappingProviderFactory factory = GetFactoryFromProperty(propertyName);

        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => factory.Create(IndexMappingProviderUtil.EmptyProvider, null!, Complex.Zero));

        Assert.Equal(expectedParamName, exception.ParamName);
    }

    [Theory]
    [InlineData("OddFactory", "to")]
    [InlineData("EvenFactory", "to")]
    [InlineData("Factory", "to")]
    public void FactoryCreateShouldGuardNullTo(string propertyName, string expectedParamName)
    {
        IIndexMappingProviderFactory factory = GetFactoryFromProperty(propertyName);

        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => factory.Create(IndexMappingProviderUtil.EmptyProvider, Complex.One, null!));

        Assert.Equal(expectedParamName, exception.ParamName);
    }

    [Theory]
    [InlineData("CreateOdd")]
    [InlineData("CreateEven")]
    [InlineData("Create")]
    public void CreateMethodsShouldThrowNotImplementedForNonNullInputs(string methodName)
    {
        MethodInfo method = GetCreateMethod(methodName);
        TensorType tensor = CreateCompositeTensor();

        TargetInvocationException exception = Assert.Throws<TargetInvocationException>(
            () => method.Invoke(null, [IndexMappingProviderUtil.EmptyProvider, tensor, tensor]));

        Assert.IsType<NotImplementedException>(exception.InnerException);
    }

    [Theory]
    [InlineData("OddFactory")]
    [InlineData("EvenFactory")]
    [InlineData("Factory")]
    public void FactoryCreateShouldThrowNotImplementedForNonNullInputs(string propertyName)
    {
        IIndexMappingProviderFactory factory = GetFactoryFromProperty(propertyName);
        TensorType tensor = CreateCompositeTensor();

        Assert.Throws<NotImplementedException>(
            () => factory.Create(IndexMappingProviderUtil.EmptyProvider, tensor, tensor));
    }

    private static MethodInfo GetCreateMethod(string methodName)
    {
        Type providerFunctionsType = GetProviderFunctionsType();
        MethodInfo? method = providerFunctionsType.GetMethod(
            methodName,
            BindingFlags.Public | BindingFlags.Static,
            binder: null,
            [typeof(IIndexMappingProvider), typeof(TensorType), typeof(TensorType)],
            modifiers: null);

        Assert.NotNull(method);
        return method!;
    }

    private static IIndexMappingProviderFactory GetFactoryFromProperty(string propertyName)
    {
        Type providerFunctionsType = GetProviderFunctionsType();
        PropertyInfo? property = providerFunctionsType.GetProperty(
            propertyName,
            BindingFlags.Public | BindingFlags.Static);

        Assert.NotNull(property);
        object? value = property!.GetValue(null);
        return Assert.IsAssignableFrom<IIndexMappingProviderFactory>(value);
    }

    private static Type GetProviderFunctionsType()
    {
        Type? providerFunctionsType = typeof(IndexMappings).Assembly.GetType(
            "NRedberry.IndexMapping.ProviderFunctions",
            throwOnError: false);

        Assert.NotNull(providerFunctionsType);
        return providerFunctionsType!;
    }

    private static TException AssertInnerException<TException>(TargetInvocationException exception)
        where TException : Exception
    {
        TException? innerException = exception.InnerException as TException;

        Assert.NotNull(innerException);
        return innerException!;
    }

    private static TensorType CreateCompositeTensor()
    {
        return TensorFactory.Expression(Complex.One, Complex.Zero);
    }
}
