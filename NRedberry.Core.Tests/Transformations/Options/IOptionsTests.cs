using System.Reflection;
using NRedberry.Transformations.Options;

namespace NRedberry.Core.Tests.Transformations.Options;

public sealed class IOptionsTests
{
    [Fact]
    public void ShouldDeclareTriggerCreateMethod()
    {
        MethodInfo method = typeof(IOptions).GetMethod(nameof(IOptions.TriggerCreate))!;

        method.ReturnType.ShouldBe(typeof(void));
        method.GetParameters().ShouldBeEmpty();
    }
}
