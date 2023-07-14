using System;

namespace NRedberry.Core.Indices;

sealed class EmptySimpleIndices : EmptyIndices, ISimpleIndices
{
    // Instance of EmptySimpleIndices
    static readonly EmptySimpleIndices EMPTY_SIMPLE_INDICES_INSTANCE = new EmptySimpleIndices();

    private EmptySimpleIndices() { }

    // Returns EmptySimpleIndices instance
    public ISimpleIndices GetInverted() {
        return this;
    }

    // Returns EmptySimpleIndices instance
    public ISimpleIndices GetFree() {
        return this;
    }

    // Returns EmptySimpleIndices instance
    public ISimpleIndices GetOfType(IndexType type) {
        return this;
    }

    // Returns EmptySimpleIndices instance
    public ISimpleIndices GetUpper() {
        return this;
    }

    // Returns EmptySimpleIndices instance
    public ISimpleIndices GetLower() {
        return this;
    }

    // Doing nothing
    public void SetSymmetries(IndicesSymmetries symmetries) {
        if (symmetries.StructureOfIndices.Count != 0)
            throw new ArgumentException("Symmetries dimensions are not equal to indices size.");
    }

    // Returns empty symmetries.
    public IndicesSymmetries GetSymmetries() {
        return IndicesSymmetries.Empty;
    }

    // Do nothing.
    public ISimpleIndices ApplyIndexMapping(IIndexMapping mapping) {
        return this;
    }

    // Equals method
    public override bool Equals(object obj) {
        return obj is EmptyIndices;
    }

    // Returns equals(indices)
    public bool EqualsWithSymmetries(ISimpleIndices indices) {
        return indices == EMPTY_SIMPLE_INDICES_INSTANCE; //There is only one instance of empty SimpleIndices
    }

    // Returns hash code
    public override int GetHashCode() {
        return 453679;
    }

    public int ContractionsHash() {
        return 0;
    }

    public StructureOfIndices GetStructureOfIndices() {
        return StructureOfIndices.Empty;
    }
}