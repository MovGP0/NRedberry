using System;
using System.Collections.Generic;

namespace NRedberry.Core.Tensors
{
    public sealed class BasicTensorIterator : IEnumerator<Tensor>
    {
        private Tensor Tensor { get; }
        private int Position { get; set; } = -1;
        private int Size => Tensor.Size;

        public BasicTensorIterator(Tensor tensor)
        {
            Tensor = tensor ?? throw new ArgumentNullException(nameof(tensor));
        }

        public object Current => Tensor[Position];

        Tensor IEnumerator<Tensor>.Current => Tensor[Position];

        public bool MoveNext()
        {
            if (!HasNext()) return false;
            ++Position;
            return true;
        }

        private bool HasNext()
        {
            return Position < Size - 1;
        }

        public void Reset()
        {
            Position = -1;
        }

        #region IDisposable
        private bool IsDisposed { get; set; }

        public void Dispose()
        {
            if (!IsDisposed) Dispose(true);
        }

        public void Dispose(bool disposing)
        {
            IsDisposed = true;
        }

        ~BasicTensorIterator()
        {
            if (!IsDisposed) Dispose(false);
        }
        #endregion
    }
}
