using NRedberry;
using NRedberry.Contexts;
using NRedberry.Indices;
using Shouldly;
using Xunit;

namespace NRedberry.Core.Contexts.Tests;

public sealed class NameDescriptorForTensorFieldTests
{
    [Fact(DisplayName = "Should clone derivative orders")]
    public void ShouldCloneDerivativeOrders()
    {
        // Arrange
        byte type = IndexType.LatinLower.GetType_();
        StructureOfIndices structure = StructureOfIndices.Create(type, 1, true);
        int[] orders = [1, 2];
        var descriptor = new TestTensorFieldDescriptor([structure, structure, structure], 1, orders, "F", false, false, null);

        // Act
        int[] clone = descriptor.GetDerivativeOrders();
        clone[0] = 9;

        // Assert
        descriptor.GetDerivativeOrder(0).ShouldBe(1);
        descriptor.GetDerivativeOrder(1).ShouldBe(2);
        descriptor.GetDerivativeOrders()[0].ShouldBe(1);
    }

    [Fact(DisplayName = "Should build partition mapping for non-derivative fields")]
    public void ShouldBuildPartitionMappingForNonDerivativeFields()
    {
        // Arrange
        byte type = IndexType.LatinLower.GetType_();
        StructureOfIndices main = StructureOfIndices.Create(type, 2, true, false);
        StructureOfIndices arg1 = StructureOfIndices.Create(type, 1, true);
        StructureOfIndices arg2 = StructureOfIndices.Create(type, 1, false);
        var descriptor = new TestTensorFieldDescriptor([main, arg1, arg2], 2, [0, 0], "F", false, false, null);

        // Act
        int[][] mapping = descriptor.GetIndicesPartitionMapping();

        // Assert
        mapping.Length.ShouldBe(3);
        mapping[0].ShouldBe(new[] { 0, 1 });
        mapping[1].ShouldBeEmpty();
        mapping[2].ShouldBeEmpty();
    }

    [Fact(DisplayName = "Should build partition mapping for derivative fields")]
    public void ShouldBuildPartitionMappingForDerivativeFields()
    {
        // Arrange
        byte type = IndexType.LatinLower.GetType_();
        StructureOfIndices main = StructureOfIndices.Create(type, 1, true);
        StructureOfIndices arg = StructureOfIndices.Create(type, 1, false);
        var parent = new TestTensorFieldDescriptor([main, arg], 3, [0], "F", false, false, null);
        StructureOfIndices combined = main.Append(arg);
        var derivative = new TestTensorFieldDescriptor([combined, arg], 4, [1], "F'", false, true, parent);

        // Act
        int[][] mapping = derivative.GetIndicesPartitionMapping();
        mapping[0][0] = 99;

        // Assert
        mapping.Length.ShouldBe(2);
        mapping[0].ShouldBe(new[] { 99 });
        mapping[1].ShouldBe(new[] { 1 });

        int[][] fresh = derivative.GetIndicesPartitionMapping();
        fresh[0].ShouldBe(new[] { 0 });
        fresh[1].ShouldBe(new[] { 1 });
    }

    private sealed class TestTensorFieldDescriptor : NameDescriptorForTensorField
    {
        private readonly bool isDerivative;
        private readonly NameDescriptorForTensorField? parent;

        public TestTensorFieldDescriptor(
            StructureOfIndices[] indexTypeStructures,
            int id,
            int[] orders,
            string name,
            bool isDiracDelta,
            bool isDerivative,
            NameDescriptorForTensorField? parent)
            : base(indexTypeStructures, id, orders, name, isDiracDelta)
        {
            this.isDerivative = isDerivative;
            this.parent = parent ?? this;
        }

        public override NameAndStructureOfIndices[] GetKeys()
        {
            return [new NameAndStructureOfIndices(Name, IndexTypeStructures)];
        }

        public override string GetName(SimpleIndices? indices, OutputFormat format)
        {
            return Name;
        }

        public override NameDescriptorForTensorField GetParent()
        {
            return parent!;
        }

        public override bool IsDerivative()
        {
            return isDerivative;
        }

        public override NameDescriptorForTensorField GetDerivative(params int[] orders)
        {
            return new TestTensorFieldDescriptor(IndexTypeStructures, Id, orders, Name, IsDiracDelta, true, this);
        }
    }
}
