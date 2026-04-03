using NRedberry.Core.Combinatorics.Symmetries;

namespace NRedberry.Core.Combinatorics.Tests.Symmetries;

internal sealed class TestDummySymmetries(int dimension, params List<Symmetry> basis)
    : DummySymmetries(dimension, basis)
{
    public override IEnumerator<Symmetry> GetEnumerator()
    {
        return Basis.GetEnumerator();
    }
}
