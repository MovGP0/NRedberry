using System;
using System.Linq;
using System.Reflection;
using NRedberry.Indices;
using Xunit;

namespace NRedberry.Core.Tests.Indices;

public class IIndexMappingTests
{
    [Fact]
    public void ShouldDeclareExactlyOneMapMethodReturningIntWhenInspectingInterface()
    {
        Type interfaceType = typeof(IIndexMapping);

        MethodInfo[] methods = interfaceType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

        Assert.Single(methods);

        MethodInfo mapMethod = methods.Single();

        Assert.Equal(nameof(IIndexMapping.Map), mapMethod.Name);
        Assert.Equal(typeof(int), mapMethod.ReturnType);

        ParameterInfo[] parameters = mapMethod.GetParameters();
        Assert.Single(parameters);
        Assert.Equal(typeof(int), parameters[0].ParameterType);
    }

    [Fact]
    public void ShouldDeclareNoPropertiesWhenInspectingInterface()
    {
        Type interfaceType = typeof(IIndexMapping);

        PropertyInfo[] properties = interfaceType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

        Assert.Empty(properties);
    }

    [Fact]
    public void ShouldDeclareNoEventsWhenInspectingInterface()
    {
        Type interfaceType = typeof(IIndexMapping);

        EventInfo[] events = interfaceType.GetEvents(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

        Assert.Empty(events);
    }
}
