using System;
using System.Collections;
using System.Collections.Generic;

namespace NRedberry.Core.Combinatorics
{
    public sealed class IntCombinationPermutationGenerator : IIntCombinatorialPort, IIntCombinatorialGenerator
    {
        public int[] Take()
        {
            throw new NotImplementedException();
        }

        public bool MoveNext()
        {
            throw new NotImplementedException();
        }

        void IEnumerator.Reset()
        {
            throw new NotImplementedException();
        }

        public int[] Current { get; }

        object IEnumerator.Current => Current;

        void IIntCombinatorialPort.Reset()
        {
            throw new NotImplementedException();
        }

        public int[] GetReference()
        {
            throw new NotImplementedException();
        }

        public IEnumerator<int[]> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}