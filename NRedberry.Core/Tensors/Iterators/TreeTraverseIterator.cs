using NRedberry.Core.Utils;

namespace NRedberry.Tensors.Iterators;

/*
 * Original: ./core/src/main/java/cc/redberry/core/tensor/iterator/TreeTraverseIterator.java
 */

public sealed class TreeTraverseIterator<T> where T : Payload<T>
{
    private readonly TraverseGuide _iterationGuide;
    private readonly PayloadFactory<T>? _payloadFactory;
    private LinkedPointer _currentPointer;
    private TraverseState? _lastState;
    private Tensor? _current;

    public TreeTraverseIterator(Tensor tensor, TraverseGuide guide, PayloadFactory<T> payloadFactory)
    {
        ArgumentNullException.ThrowIfNull(tensor);
        ArgumentNullException.ThrowIfNull(guide);

        _currentPointer = new LinkedPointer(null, TensorWrapper.Wrap(tensor), true, payloadFactory);
        _iterationGuide = guide;
        _payloadFactory = payloadFactory;
    }

    public TreeTraverseIterator(Tensor tensor, TraverseGuide guide)
    {
        ArgumentNullException.ThrowIfNull(tensor);
        ArgumentNullException.ThrowIfNull(guide);

        _currentPointer = new LinkedPointer(null, TensorWrapper.Wrap(tensor), true, null);
        _iterationGuide = guide;
    }

    public TreeTraverseIterator(Tensor tensor)
    {
        ArgumentNullException.ThrowIfNull(tensor);

        _currentPointer = new LinkedPointer(null, TensorWrapper.Wrap(tensor), true, null);
        _iterationGuide = TraverseGuide.All;
    }

    public TreeTraverseIterator(Tensor tensor, PayloadFactory<T> payloadFactory)
    {
        ArgumentNullException.ThrowIfNull(tensor);

        _currentPointer = new LinkedPointer(null, TensorWrapper.Wrap(tensor), true, payloadFactory);
        _iterationGuide = TraverseGuide.All;
        _payloadFactory = payloadFactory;
    }

    public TraverseState? Next()
    {
        if (_lastState == TraverseState.Leaving)
        {
            Tensor? cur = null;
            if (_currentPointer.Payload is not null)
            {
                cur = _currentPointer.Payload.OnLeaving(_currentPointer);
            }

            if (cur is not null)
            {
                _current = cur;
            }

            _currentPointer = _currentPointer.PreviousPointer!;
            _currentPointer.Set(_current!);
        }

        Tensor? next;
        while (true)
        {
            next = _currentPointer.Next();
            if (next is null)
            {
                if (_currentPointer.PreviousPointer is null)
                {
                    _lastState = null;
                    return null;
                }

                _current = _currentPointer.GetTensor();

                return _lastState = TraverseState.Leaving;
            }

            TraversePermission permission = _iterationGuide.GetPermission(
                next,
                _currentPointer.Tensor,
                _currentPointer.Position - 1);

            if (permission == TraversePermission.DontShow)
            {
                continue;
            }

            _current = next;
            _currentPointer = new LinkedPointer(_currentPointer, next, permission == TraversePermission.Enter, _payloadFactory);
            return _lastState = TraverseState.Entering;
        }
    }

    public void Set(Tensor tensor)
    {
        if (ReferenceEquals(_current, tensor))
        {
            return;
        }

        ArgumentNullException.ThrowIfNull(tensor);

        _current = tensor;
        _currentPointer.Tensor = tensor;
        _lastState = TraverseState.Leaving;
    }

    public int Depth()
    {
        return _currentPointer.GetDepth();
    }

    public bool IsUnder(IIndicator<Tensor> indicator, int searchDepth)
    {
        ArgumentNullException.ThrowIfNull(indicator);
        return _currentPointer.IsUnder(indicator, searchDepth);
    }

    public Tensor Current()
    {
        return _current!;
    }

    public Tensor Result()
    {
        if (_currentPointer.PreviousPointer is not null)
        {
            throw new InvalidOperationException("Iteration not finished.");
        }

        return _currentPointer.GetTensor()[0];
    }

    public StackPosition<T> CurrentStackPosition()
    {
        return _currentPointer;
    }

    private sealed record class LinkedPointer : StackPosition<T>
    {
        public int Position { get; private set; }
        public Tensor Tensor { get; set; }
        private Tensor? _current;
        private Tensor? _toSet;
        private TensorBuilder? _builder;
        public LinkedPointer? PreviousPointer { get; }
        private bool _isModified;
        public T? Payload { get; private set; }
        private readonly PayloadFactory<T>? _payloadFactory;

        public LinkedPointer(LinkedPointer? previous, Tensor tensor, bool goInside, PayloadFactory<T>? payloadFactory)
        {
            Tensor = tensor;
            if (!goInside)
            {
                Position = int.MaxValue;
            }

            PreviousPointer = previous;
            _payloadFactory = payloadFactory;
            if (PreviousPointer is not null && _payloadFactory?.AllowLazyInitialization() == false)
            {
                Payload = _payloadFactory.Create(this);
                if (Payload is null)
                {
                    throw new InvalidOperationException("Payload factory returned null payload.");
                }
            }
        }

        public Tensor? Next()
        {
            if (_toSet is not null)
            {
                if (_builder is null)
                {
                    _builder = Tensor.GetBuilder();
                    for (int i = 0; i < Position - 1; ++i)
                    {
                        _builder.Put(Tensor[i]);
                    }
                }

                _builder.Put(_toSet);
                _toSet = null;
            }
            else if (_builder is not null)
            {
                _builder.Put(_current!);
            }

            if (Position >= Tensor.Size)
            {
                _current = null;
                return null;
            }

            _current = Tensor[Position++];
            return _current;
        }

        public Tensor GetTensor()
        {
            if (_builder is not null)
            {
                if (Position != Tensor.Size)
                {
                    throw new InvalidOperationException("Iteration not finished.");
                }

                Tensor = _builder.Build();
                Position = int.MaxValue;
                _builder = null;
            }

            return Tensor;
        }

        public Tensor GetInitialTensor()
        {
            if (Position == int.MaxValue)
            {
                throw new InvalidOperationException("Initial tensor was rebuilt.");
            }

            return Tensor;
        }

        public bool IsModified()
        {
            return _isModified;
        }

        public StackPosition<T> Previous()
        {
            if (PreviousPointer is null || PreviousPointer.PreviousPointer is null)
            {
                return null!;
            }

            return PreviousPointer;
        }

        public T GetPayload()
        {
            if (_payloadFactory is null || PreviousPointer is null)
            {
                return default!;
            }

            if (Payload is null)
            {
                Payload = _payloadFactory.Create(this);
                if (Payload is null)
                {
                    throw new InvalidOperationException("Payload factory returned null payload.");
                }
            }

            return Payload;
        }

        public bool IsPayloadInitialized()
        {
            return Payload is not null;
        }

        private void SetModified()
        {
            _isModified = true;
            PreviousPointer?.SetModified();
        }

        public void Set(Tensor t)
        {
            if (ReferenceEquals(_current, t))
            {
                return;
            }

            _toSet = t;
            SetModified();
        }

        public int GetDepth()
        {
            int depth = -2;
            LinkedPointer? pointer = this;
            while (pointer is not null)
            {
                pointer = pointer.PreviousPointer;
                ++depth;
            }

            return depth;
        }

        public bool IsUnder(IIndicator<Tensor> indicator, int searchDepth)
        {
            LinkedPointer? pointer = this;
            do
            {
                if (indicator.Is(pointer.Tensor))
                {
                    return true;
                }

                pointer = pointer.PreviousPointer;
            } while (pointer is not null && searchDepth-- > 0);

            return false;
        }

        public StackPosition<T> Previous(int level)
        {
            LinkedPointer? pointer = this;
            while (pointer is not null && level-- > 0)
            {
                pointer = pointer.PreviousPointer;
            }

            return pointer!;
        }

        public int CurrentIndex()
        {
            return Position - 1;
        }
    }
}
