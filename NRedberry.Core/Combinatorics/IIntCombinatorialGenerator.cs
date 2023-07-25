using System.Collections.Generic;

namespace NRedberry.Core.Combinatorics;

///<summary>
/// Parent interface for combinatoric iterators
///</summary>
public interface IIntCombinatorialGenerator: IEnumerable<int[]>, IEnumerator<int[]>
{
}