using NRedberry.IndexMapping;

namespace NRedberry.Core.Tests.Indexmapping;

public sealed class IIndexMappingBufferTests
{
    [Fact]
    public void InterfaceShouldInheritICloneable()
    {
        Type type = typeof(IIndexMappingBuffer);

        type.GetInterfaces().ShouldContain(typeof(ICloneable));
    }

    [Fact]
    public void InterfaceShouldDefineRequiredMethodSignatures()
    {
        Type type = typeof(IIndexMappingBuffer);

        type.ShouldHaveMethod(nameof(IIndexMappingBuffer.TryMap), typeof(bool), typeof(int), typeof(int));
        type.ShouldHaveMethod(nameof(IIndexMappingBuffer.AddSign), typeof(void), typeof(bool));
        type.ShouldHaveMethod(nameof(IIndexMappingBuffer.RemoveContracted), typeof(void));
        type.ShouldHaveMethod(nameof(IIndexMappingBuffer.IsEmpty), typeof(bool));
        type.ShouldHaveMethod(nameof(IIndexMappingBuffer.GetSign), typeof(bool));
        type.ShouldHaveMethod(nameof(IIndexMappingBuffer.Export), typeof(object));
        type.ShouldHaveMethod(nameof(IIndexMappingBuffer.GetMap), typeof(IDictionary<int, IndexMappingBufferRecord>));
    }
}
