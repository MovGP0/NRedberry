﻿using System.Collections;
using System.Collections.Generic;

namespace NRedberry.Core.Combinatorics
{
    public abstract class IntCombinatorialGenerator : IEnumerable<int>, IEnumerator<int>
    {
        public IEnumerator<int> GetEnumerator()
        {
            return this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public abstract bool MoveNext();

        public abstract void Reset();

        public abstract int Current { get; }

        object IEnumerator.Current => Current;

        public abstract void Dispose();
    }
}
