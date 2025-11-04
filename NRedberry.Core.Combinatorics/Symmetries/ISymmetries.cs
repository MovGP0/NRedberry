namespace NRedberry.Core.Combinatorics.Symmetries;

/// <summary>
/// src\main\java\cc\redberry\core\combinatorics\symmetries\Symmetries.java
/// </summary>
public interface Symmetries : IEnumerable<Symmetry>
{
    int Dimension { get; }
    bool IsEmpty { get; }

    bool Add(Symmetry symmetry); // Note that in C# exceptions are not declared in the method signature
    bool AddUnsafe(Symmetry symmetry);

    List<Symmetry> BasisSymmetries { get; }

    Symmetries Clone();
}
