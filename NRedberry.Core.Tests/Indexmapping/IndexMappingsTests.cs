using System;
using System.Reflection;
using NRedberry.Concurrent;
using NRedberry.IndexMapping;
using NRedberry.Numbers;
using NRedberry.Tensors;
using Xunit;
using TensorType = NRedberry.Tensors.Tensor;

namespace NRedberry.Core.Tests.Indexmapping;

public sealed class IndexMappingsTests
{
    [Fact]
    public void CreatePortShouldThrowNotImplementedException()
    {
        Assert.Throws<NotImplementedException>(() => IndexMappings.CreatePort(Complex.One, Complex.Zero));
    }

    [Fact]
    public void SimpleTensorsPortShouldThrowNotImplementedException()
    {
        SimpleTensor from = null!;
        SimpleTensor to = null!;

        Assert.Throws<NotImplementedException>(() => IndexMappings.SimpleTensorsPort(from, to));
    }

    [Fact]
    public void CreateBijectiveProductPortShouldThrowNotImplementedException()
    {
        TensorType[] from = [Complex.One];
        TensorType[] to = [Complex.Zero];

        Assert.Throws<NotImplementedException>(() => IndexMappings.CreateBijectiveProductPort(from, to));
    }

    [Fact]
    public void GetFirstShouldThrowNotImplementedException()
    {
        Assert.Throws<NotImplementedException>(() => IndexMappings.GetFirst(Complex.One, Complex.Zero));
    }

    [Fact]
    public void MappingExistsShouldThrowNotImplementedException()
    {
        Assert.Throws<NotImplementedException>(() => IndexMappings.MappingExists(Complex.One, Complex.Zero));
    }

    [Fact]
    public void PositiveMappingExistsShouldThrowNotImplementedException()
    {
        Assert.Throws<NotImplementedException>(() => IndexMappings.PositiveMappingExists(Complex.One, Complex.Zero));
    }

    [Fact]
    public void EqualsShouldThrowNotImplementedException()
    {
        Assert.Throws<NotImplementedException>(() => IndexMappings.Equals(Complex.One, Complex.Zero));
    }

    [Fact]
    public void Compare1ShouldThrowNotImplementedException()
    {
        Assert.Throws<NotImplementedException>(() => IndexMappings.Compare1(Complex.One, Complex.Zero));
    }

    [Fact]
    public void IsZeroDueToSymmetryShouldThrowNotImplementedException()
    {
        Assert.Throws<NotImplementedException>(() => IndexMappings.IsZeroDueToSymmetry(Complex.Zero));
    }

    [Fact]
    public void GetAllMappingsShouldThrowNotImplementedException()
    {
        Assert.Throws<NotImplementedException>(() => IndexMappings.GetAllMappings(Complex.One, Complex.Zero));
    }

    [Fact]
    public void InternalCreatePortOfBuffersWithTensorArgumentsShouldThrowNotImplementedException()
    {
        MethodInfo method = GetMethod(
            "CreatePortOfBuffers",
            BindingFlags.NonPublic | BindingFlags.Static,
            typeof(TensorType),
            typeof(TensorType));

        AssertThrowsNotImplemented(method, Complex.One, Complex.Zero);
    }

    [Fact]
    public void InternalCreatePortWithProviderArgumentShouldThrowNotImplementedException()
    {
        MethodInfo method = GetMethod(
            "CreatePort",
            BindingFlags.NonPublic | BindingFlags.Static,
            typeof(IIndexMappingProvider),
            typeof(TensorType),
            typeof(TensorType));

        AssertThrowsNotImplemented(method, IndexMappingProviderUtil.EmptyProvider, Complex.One, Complex.Zero);
    }

    [Fact]
    public void InternalGetFirstBufferShouldThrowNotImplementedException()
    {
        MethodInfo method = GetMethod(
            "GetFirstBuffer",
            BindingFlags.NonPublic | BindingFlags.Static,
            typeof(TensorType),
            typeof(TensorType));

        AssertThrowsNotImplemented(method, Complex.One, Complex.Zero);
    }

    [Fact]
    public void PrivateGetAllMappingsOutputPortOverloadShouldThrowNotImplementedException()
    {
        MethodInfo method = GetMethod(
            "GetAllMappings",
            BindingFlags.NonPublic | BindingFlags.Static,
            typeof(IOutputPort<Mapping>));

        AssertThrowsNotImplemented(method, [null]);
    }

    private static MethodInfo GetMethod(string name, BindingFlags bindingFlags, params Type[] parameterTypes)
    {
        MethodInfo? method = typeof(IndexMappings).GetMethod(name, bindingFlags, null, parameterTypes, null);

        Assert.NotNull(method);
        return method!;
    }

    private static void AssertThrowsNotImplemented(MethodInfo method, params object?[]? arguments)
    {
        TargetInvocationException exception = Assert.Throws<TargetInvocationException>(() => method.Invoke(null, arguments));

        Assert.IsType<NotImplementedException>(exception.InnerException);
    }
}
