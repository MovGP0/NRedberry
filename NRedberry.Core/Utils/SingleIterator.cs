using System;
using System.Collections;
using System.Collections.Generic;

namespace NRedberry.Core.Utils
{
    public class SingleIterator<T> : IEnumerator<T>
    {
        private T element;
        private bool ended = false;

        public SingleIterator(T element)
        {
            this.element = element;
        }

        public T Current
        {
            get
            {
                if (ended)
                    throw new InvalidOperationException();
                return element;
            }
        }

        object? IEnumerator.Current => Current;

        public bool MoveNext()
        {
            if (ended)
                return false;
            ended = true;
            return true;
        }

        public void Reset()
        {
            ended = false;
        }

        public void Dispose()
        {
            // No resources to release
        }
    }
}