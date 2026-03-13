using NRedberry.Contexts;
using NRedberry.Tensors;
using NRedberry.Transformations.Options;
using NRedberry.Transformations.Symmetrization;

namespace NRedberry.Groovy;

public sealed class DSLTransformationInst<T> : DSLTransformation<T>, TransformationToStringAble, IContextListener
    where T : class, ITransformation
{
    private volatile T? _instance;
    private readonly Lock _syncRoot = new();

    public DSLTransformationInst(T instance)
        : base(instance.GetType())
    {
        _instance = instance ?? throw new ArgumentNullException(nameof(instance));
    }

    public DSLTransformationInst(Type clazz)
        : base(clazz)
    {
        TryRegisterAsContextListener();
    }

    public Tensor Transform(Tensor tensor)
    {
        EnsureInstanceCreated();
        return _instance!.Transform(tensor);
    }

    public override string ToString()
    {
        return ToString(Contexts.CC.DefaultOutputFormat);
    }

    public string ToString(OutputFormat outputFormat)
    {
        EnsureInstanceCreated();

        if (_instance is TransformationToStringAble toStringAble)
        {
            return toStringAble.ToString(outputFormat);
        }

        return _instance!.ToString() ?? string.Empty;
    }

    public void OnCompleted()
    {
    }

    public void OnError(Exception error)
    {
    }

    public void OnNext(ContextEvent value)
    {
        _instance = null;
    }

    private void EnsureInstanceCreated()
    {
        if (_instance is not null)
        {
            return;
        }

        lock (_syncRoot)
        {
            _instance ??= TransformationBuilder.CreateTransformation<T>([]);
        }
    }

    private void TryRegisterAsContextListener()
    {
        Contexts.CC.Current.RegisterListener(this);
    }
}
