using System.Reflection;
using NRedberry.Indices;

namespace NRedberry.Core.Tests.Indices;

public class IIndexMappingTests
{
    [Fact]
    public void ShouldDeclareExactlyOneMapMethodReturningIntWhenInspectingInterface()
    {
        Type interfaceType = typeof(IIndexMapping);

        MethodInfo[] methods = interfaceType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

        methods.ShouldHaveSingleItem();

        MethodInfo mapMethod = methods.Single();

        mapMethod.Name.ShouldBe(nameof(IIndexMapping.Map));
        mapMethod.ReturnType.ShouldBe(typeof(int));

        ParameterInfo[] parameters = mapMethod.GetParameters();
        parameters.ShouldHaveSingleItem();
        parameters[0].ParameterType.ShouldBe(typeof(int));
    }

    [Fact]
    public void ShouldDeclareNoPropertiesWhenInspectingInterface()
    {
        Type interfaceType = typeof(IIndexMapping);

        PropertyInfo[] properties = interfaceType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

        properties.ShouldBeEmpty();
    }

    [Fact]
    public void ShouldDeclareNoEventsWhenInspectingInterface()
    {
        Type interfaceType = typeof(IIndexMapping);

        EventInfo[] events = interfaceType.GetEvents(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

        events.ShouldBeEmpty();
    }
}
