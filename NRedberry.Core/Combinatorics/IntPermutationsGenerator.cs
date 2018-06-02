using System;
using System.Collections;
using System.Collections.Generic;

namespace NRedberry.Core.Combinatorics
{
    public sealed class IntPermutationsGenerator : IIntCombinatorialGenerator
    {
        public IEnumerator<int[]> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool MoveNext()
        {
            throw new NotImplementedException();
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }

        public int[] Current { get; }

        object IEnumerator.Current => Current;

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}