using System;
using System.Collections;
using System.Collections.Generic;

namespace NRedberry.Core.Combinatorics
{
    public sealed class IntCombinationsGenerator : IIntCombinatorialGenerator, IIntCombinatorialPort
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

        void IEnumerator.Reset()
        {
            throw new NotImplementedException();
        }

        public int[] GetReference()
        {
            throw new NotImplementedException();
        }

        public int[] Current { get; }

        object IEnumerator.Current => Current;

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public int[] Take()
        {
            throw new NotImplementedException();
        }

        void IIntCombinatorialPort.Reset()
        {
            throw new NotImplementedException();
        }
    }
}
