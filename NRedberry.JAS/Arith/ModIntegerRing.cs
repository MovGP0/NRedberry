using System.Collections;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;

/// <summary>
/// ModIntegerRing factory for ModInteger elements.
/// </summary>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.arith.ModIntegerRing
/// </remarks>
public sealed class ModIntegerRing : ModularRingFactory<ModInteger>, IEnumerable<ModInteger>
{
    public readonly BigInteger Modul;

    public ModIntegerRing(BigInteger m) { throw new NotImplementedException(); }
    public ModIntegerRing(BigInteger m, bool isField) { throw new NotImplementedException(); }

    public BigInteger GetModul() => Modul;
    public BigInteger GetIntegerModul() { throw new NotImplementedException(); }
    public ModInteger Create(BigInteger c) { throw new NotImplementedException(); }
    public ModInteger Create(long c) { throw new NotImplementedException(); }
    public ModInteger Copy(ModInteger c) { throw new NotImplementedException(); }
    public ModInteger GetZERO() { throw new NotImplementedException(); }
    public ModInteger GetONE() { throw new NotImplementedException(); }
    public List<ModInteger> Generators() { throw new NotImplementedException(); }
    public bool IsFinite() { throw new NotImplementedException(); }
    public bool IsCommutative() { throw new NotImplementedException(); }
    public bool IsAssociative() { throw new NotImplementedException(); }
    public bool IsField() { throw new NotImplementedException(); }
    public BigInteger Characteristic() { throw new NotImplementedException(); }
    System.Numerics.BigInteger RingFactory<ModInteger>.Characteristic() { throw new NotImplementedException(); }
    public ModInteger FromInteger(BigInteger a) { throw new NotImplementedException(); }
    ModInteger ElemFactory<ModInteger>.FromInteger(System.Numerics.BigInteger a) { throw new NotImplementedException(); }
    public ModInteger FromInteger(long a) { throw new NotImplementedException(); }
    public ModInteger Random(int n) { throw new NotImplementedException(); }
    public ModInteger Random(int n, Random rnd) { throw new NotImplementedException(); }
    public ModInteger ChineseRemainder(ModInteger c, ModInteger ci, ModInteger a) { throw new NotImplementedException(); }
    public override string ToString() { throw new NotImplementedException(); }
    public IEnumerator<ModInteger> GetEnumerator() { throw new NotImplementedException(); }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
