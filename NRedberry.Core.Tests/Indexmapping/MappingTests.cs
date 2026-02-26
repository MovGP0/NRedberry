using System;
using System.Runtime.CompilerServices;
using NRedberry.IndexMapping;
using NRedberry.Numbers;
using Xunit;

namespace NRedberry.Core.Tests.Indexmapping;

public sealed class MappingTests
{
    [Fact]
    public void ConstructorFromAndToShouldThrowNotImplementedException()
    {
        int[] from = [1, 2];
        int[] to = [3, 4];

        Assert.Throws<NotImplementedException>(() => new Mapping(from, to));
    }

    [Fact]
    public void ConstructorFromToAndSignShouldThrowNotImplementedException()
    {
        int[] from = [1];
        int[] to = [2];

        Assert.Throws<NotImplementedException>(() => new Mapping(from, to, sign: true));
    }

    [Fact]
    public void IdentityMappingGetterShouldThrowNotImplementedException()
    {
        Assert.Throws<NotImplementedException>(() => Mapping.IdentityMapping);
    }

    [Fact]
    public void TransformShouldThrowNotImplementedException()
    {
        Mapping mapping = CreateUninitializedMapping();

        Assert.Throws<NotImplementedException>(() => mapping.Transform(Complex.One));
    }

    [Fact]
    public void IsEmptyShouldThrowNotImplementedException()
    {
        Mapping mapping = CreateUninitializedMapping();

        Assert.Throws<NotImplementedException>(() => mapping.IsEmpty());
    }

    [Fact]
    public void IsIdentityShouldThrowNotImplementedException()
    {
        Mapping mapping = CreateUninitializedMapping();

        Assert.Throws<NotImplementedException>(() => mapping.IsIdentity());
    }

    [Fact]
    public void GetSignShouldThrowNotImplementedException()
    {
        Mapping mapping = CreateUninitializedMapping();

        Assert.Throws<NotImplementedException>(() => mapping.GetSign());
    }

    [Fact]
    public void AddSignShouldThrowNotImplementedException()
    {
        Mapping mapping = CreateUninitializedMapping();

        Assert.Throws<NotImplementedException>(() => mapping.AddSign(sign: true));
    }

    [Fact]
    public void SizeShouldThrowNotImplementedException()
    {
        Mapping mapping = CreateUninitializedMapping();

        Assert.Throws<NotImplementedException>(() => mapping.Size());
    }

    [Fact]
    public void GetFromNamesShouldThrowNotImplementedException()
    {
        Mapping mapping = CreateUninitializedMapping();

        Assert.Throws<NotImplementedException>(() => mapping.GetFromNames());
    }

    [Fact]
    public void GetToDataShouldThrowNotImplementedException()
    {
        Mapping mapping = CreateUninitializedMapping();

        Assert.Throws<NotImplementedException>(() => mapping.GetToData());
    }

    [Fact]
    public void EqualsShouldThrowNotImplementedException()
    {
        Mapping mapping = CreateUninitializedMapping();

        Assert.Throws<NotImplementedException>(() => mapping.Equals(new object()));
    }

    [Fact]
    public void GetHashCodeShouldThrowNotImplementedException()
    {
        Mapping mapping = CreateUninitializedMapping();

        Assert.Throws<NotImplementedException>(() => mapping.GetHashCode());
    }

    [Fact]
    public void ToStringShouldThrowNotImplementedException()
    {
        Mapping mapping = CreateUninitializedMapping();

        Assert.Throws<NotImplementedException>(() => mapping.ToString());
    }

    [Fact]
    public void ValueOfShouldThrowNotImplementedException()
    {
        Assert.Throws<NotImplementedException>(() => Mapping.ValueOf("[]"));
    }

    private static Mapping CreateUninitializedMapping()
    {
        return (Mapping)RuntimeHelpers.GetUninitializedObject(typeof(Mapping));
    }
}
