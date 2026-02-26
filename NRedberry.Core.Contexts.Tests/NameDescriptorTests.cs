using NRedberry;
using NRedberry.Contexts;
using NRedberry.Indices;
using Shouldly;
using Xunit;

namespace NRedberry.Core.Contexts.Tests;

public sealed class NameDescriptorTests
{
    [Fact(DisplayName = "Should throw when index structures are null or empty")]
    public void ShouldThrowWhenIndexStructuresAreNullOrEmpty()
    {
        // Arrange
        StructureOfIndices[] empty = [];

        // Act + Assert
        Should.Throw<ArgumentNullException>(() => _ = new TestNameDescriptor("T", null!, 1));
        Should.Throw<ArgumentException>(() => _ = new TestNameDescriptor("T", empty, 1));
    }

    [Fact(DisplayName = "Should expose structures and field state")]
    public void ShouldExposeStructuresAndFieldState()
    {
        // Arrange
        byte type = IndexType.LatinLower.GetType_();
        StructureOfIndices first = StructureOfIndices.Create(type, 1, true);
        StructureOfIndices second = StructureOfIndices.Create(type, 2, true, false);
        var descriptor = new TestNameDescriptor("T", [first, second], 5);

        // Act + Assert
        descriptor.IsField().ShouldBeTrue();
        descriptor.GetStructureOfIndices().ShouldBe(first);
        descriptor.GetStructuresOfIndices().ShouldBe(descriptor.IndexTypeStructures);
        descriptor.GetArgStructuresOfIndices(0).ShouldBe(second);
    }

    [Fact(DisplayName = "Should return symmetries from structure")]
    public void ShouldReturnSymmetriesFromStructure()
    {
        // Arrange
        byte type = IndexType.LatinLower.GetType_();
        StructureOfIndices first = StructureOfIndices.Create(type, 1, true);
        var descriptor = new TestNameDescriptor("T", [first], 7);

        // Act
        IndicesSymmetries symmetries = descriptor.GetSymmetries();

        // Assert
        symmetries.ShouldBeSameAs(descriptor.Symmetries);
    }

    [Fact(DisplayName = "Should register in name manager once")]
    public void ShouldRegisterInNameManagerOnce()
    {
        // Arrange
        byte type = IndexType.LatinLower.GetType_();
        StructureOfIndices first = StructureOfIndices.Create(type, 1, true);
        var descriptor = new TestNameDescriptor("T", [first], 11);
        var nameManager = new NameManager(1, "d", "g");

        // Act
        descriptor.RegisterInNameManager(nameManager);

        // Assert
        descriptor.NameManager.ShouldBeSameAs(nameManager);

        // Act + Assert
        Should.Throw<InvalidOperationException>(() => descriptor.RegisterInNameManager(new NameManager(2, "d", "g")));
    }

    [Fact(DisplayName = "Should extract key from descriptor")]
    public void ShouldExtractKeyFromDescriptor()
    {
        // Arrange
        byte type = IndexType.LatinLower.GetType_();
        StructureOfIndices first = StructureOfIndices.Create(type, 1, true);
        var descriptor = new TestNameDescriptor("T", [first], 13);

        // Act
        NameAndStructureOfIndices key = NameDescriptor.ExtractKey(descriptor);

        // Assert
        key.Name.ShouldBe("T");
        key.Structure.ShouldBe(descriptor.IndexTypeStructures);
    }

    [Fact(DisplayName = "Should throw when registering with null manager")]
    public void ShouldThrowWhenRegisteringNullManager()
    {
        // Arrange
        byte type = IndexType.LatinLower.GetType_();
        StructureOfIndices first = StructureOfIndices.Create(type, 1, true);
        var descriptor = new TestNameDescriptor("T", [first], 17);

        // Act + Assert
        Should.Throw<ArgumentNullException>(() => descriptor.RegisterInNameManager(null!));
    }

    private sealed class TestNameDescriptor : NameDescriptor
    {
        private readonly string name;
        private readonly NameAndStructureOfIndices[] keys;

        public TestNameDescriptor(string name, StructureOfIndices[] indexTypeStructures, int id)
            : base(indexTypeStructures, id)
        {
            this.name = name;
            keys = [new NameAndStructureOfIndices(name, indexTypeStructures)];
        }

        public override NameAndStructureOfIndices[] GetKeys()
        {
            return keys;
        }

        public override string GetName(SimpleIndices? indices, OutputFormat format)
        {
            return name;
        }
    }
}
