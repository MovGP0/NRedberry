using System.Reflection;
using NRedberry.IndexMapping;
using NRedberry.Numbers;
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

        factory.ShouldNotBeNull();
        factory.ShouldBeAssignableTo<IIndexMappingProviderFactory>();
    }

    [Theory]
    [InlineData("CreateOdd", "provider")]
    [InlineData("CreateEven", "provider")]
    [InlineData("Create", "provider")]
    public void CreateMethodsShouldGuardNullProvider(string methodName, string expectedParamName)
    {
        MethodInfo method = GetCreateMethod(methodName);

        ArgumentNullException exception = AssertInnerException<ArgumentNullException>(
            Should.Throw<TargetInvocationException>(() => method.Invoke(null, [null, Complex.One, Complex.Zero])));

        exception.ParamName.ShouldBe(expectedParamName);
    }

    [Theory]
    [InlineData("CreateOdd", "from")]
    [InlineData("CreateEven", "from")]
    [InlineData("Create", "from")]
    public void CreateMethodsShouldGuardNullFrom(string methodName, string expectedParamName)
    {
        MethodInfo method = GetCreateMethod(methodName);

        ArgumentNullException exception = AssertInnerException<ArgumentNullException>(
            Should.Throw<TargetInvocationException>(() => method.Invoke(null, [IndexMappingProviderUtil.EmptyProvider, null, Complex.Zero])));

        exception.ParamName.ShouldBe(expectedParamName);
    }

    [Theory]
    [InlineData("CreateOdd", "to")]
    [InlineData("CreateEven", "to")]
    [InlineData("Create", "to")]
    public void CreateMethodsShouldGuardNullTo(string methodName, string expectedParamName)
    {
        MethodInfo method = GetCreateMethod(methodName);

        ArgumentNullException exception = AssertInnerException<ArgumentNullException>(
            Should.Throw<TargetInvocationException>(() => method.Invoke(null, [IndexMappingProviderUtil.EmptyProvider, Complex.One, null])));

        exception.ParamName.ShouldBe(expectedParamName);
    }

    [Theory]
    [InlineData("OddFactory", "provider")]
    [InlineData("EvenFactory", "provider")]
    [InlineData("Factory", "provider")]
    public void FactoryCreateShouldGuardNullProvider(string propertyName, string expectedParamName)
    {
        IIndexMappingProviderFactory factory = GetFactoryFromProperty(propertyName);

        ArgumentNullException exception = Should.Throw<ArgumentNullException>(() => factory.Create(null!, Complex.One, Complex.Zero));

        exception.ParamName.ShouldBe(expectedParamName);
    }

    [Theory]
    [InlineData("OddFactory", "from")]
    [InlineData("EvenFactory", "from")]
    [InlineData("Factory", "from")]
    public void FactoryCreateShouldGuardNullFrom(string propertyName, string expectedParamName)
    {
        IIndexMappingProviderFactory factory = GetFactoryFromProperty(propertyName);

        ArgumentNullException exception = Should.Throw<ArgumentNullException>(() => factory.Create(IndexMappingProviderUtil.EmptyProvider, null!, Complex.Zero));

        exception.ParamName.ShouldBe(expectedParamName);
    }

    [Theory]
    [InlineData("OddFactory", "to")]
    [InlineData("EvenFactory", "to")]
    [InlineData("Factory", "to")]
    public void FactoryCreateShouldGuardNullTo(string propertyName, string expectedParamName)
    {
        IIndexMappingProviderFactory factory = GetFactoryFromProperty(propertyName);

        ArgumentNullException exception = Should.Throw<ArgumentNullException>(() => factory.Create(IndexMappingProviderUtil.EmptyProvider, Complex.One, null!));

        exception.ParamName.ShouldBe(expectedParamName);
    }

    [Theory]
    [InlineData("CreateOdd")]
    [InlineData("CreateEven")]
    [InlineData("Create")]
    public void CreateMethodsShouldReturnProviderForNonNullInputs(string methodName)
    {
        MethodInfo method = GetCreateMethod(methodName);
        TensorType tensor = CreateCompositeTensor();

        object? provider = method.Invoke(null, [IndexMappingProviderUtil.EmptyProvider, tensor, tensor]);

        provider.ShouldNotBeNull();
        provider.ShouldBeAssignableTo<IIndexMappingProvider>();
    }

    [Theory]
    [InlineData("OddFactory")]
    [InlineData("EvenFactory")]
    [InlineData("Factory")]
    public void FactoryCreateShouldReturnProviderForNonNullInputs(string propertyName)
    {
        IIndexMappingProviderFactory factory = GetFactoryFromProperty(propertyName);
        TensorType tensor = CreateCompositeTensor();

        IIndexMappingProvider provider = factory.Create(IndexMappingProviderUtil.EmptyProvider, tensor, tensor);

        provider.ShouldNotBeNull();
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

        method.ShouldNotBeNull();
        return method!;
    }

    private static IIndexMappingProviderFactory GetFactoryFromProperty(string propertyName)
    {
        Type providerFunctionsType = GetProviderFunctionsType();
        PropertyInfo? property = providerFunctionsType.GetProperty(
            propertyName,
            BindingFlags.Public | BindingFlags.Static);

        property.ShouldNotBeNull();
        object? value = property!.GetValue(null);
        return value.ShouldBeAssignableTo<IIndexMappingProviderFactory>();
    }

    private static Type GetProviderFunctionsType()
    {
        Type? providerFunctionsType = typeof(IndexMappings).Assembly.GetType(
            "NRedberry.IndexMapping.ProviderFunctions",
            throwOnError: false);

        providerFunctionsType.ShouldNotBeNull();
        return providerFunctionsType!;
    }

    private static TException AssertInnerException<TException>(TargetInvocationException exception)
        where TException : Exception
    {
        TException? innerException = exception.InnerException as TException;

        innerException.ShouldNotBeNull();
        return innerException!;
    }

    private static TensorType CreateCompositeTensor()
    {
        return TensorFactory.Expression(Complex.One, Complex.Zero);
    }
}
