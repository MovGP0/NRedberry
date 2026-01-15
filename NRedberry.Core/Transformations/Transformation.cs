using NRedberry.Tensors;
using NRedberry.Transformations.Symmetrization;

namespace NRedberry.Transformations;

/// <summary>
/// Transformation utilities.
/// </summary>
public static class Transformation
{
    public static ITransformation Identity { get; } = new IdentityTransformation();

    public static Tensor ApplySequentially(Tensor tensor, params ITransformation[] transformations)
    {
        ArgumentNullException.ThrowIfNull(tensor);
        ArgumentNullException.ThrowIfNull(transformations);

        Tensor current = tensor;
        foreach (ITransformation transformation in transformations)
        {
            current = transformation.Transform(current);
        }

        return current;
    }

    public static Tensor ApplyToEachChild(Tensor tensor, ITransformation transformation)
    {
        ArgumentNullException.ThrowIfNull(tensor);
        ArgumentNullException.ThrowIfNull(transformation);

        TensorBuilder? builder = null;
        int size = tensor.Size;
        for (int i = 0; i < size; ++i)
        {
            Tensor original = tensor[i];
            Tensor transformed = transformation.Transform(original);
            if (builder is not null || !ReferenceEquals(transformed, original))
            {
                if (builder is null)
                {
                    builder = tensor.GetBuilder();
                    for (int j = 0; j < i; ++j)
                    {
                        builder.Put(tensor[j]);
                    }
                }

                builder.Put(transformed);
            }
        }

        return builder is null ? tensor : builder.Build();
    }

    public static Tensor ApplyUntilUnchanged(Tensor tensor, params ITransformation[] transformations)
    {
        return ApplyUntilUnchanged(tensor, int.MaxValue, transformations);
    }

    public static Tensor ApplyUntilUnchanged(Tensor tensor, int limit, params ITransformation[] transformations)
    {
        ArgumentNullException.ThrowIfNull(tensor);
        ArgumentNullException.ThrowIfNull(transformations);

        Tensor current = tensor;
        int remaining = limit;
        while (true)
        {
            if (remaining == 0)
            {
                throw new InvalidOperationException($"Steel changed after {limit} tries.");
            }

            Tensor previous = current;
            foreach (ITransformation transformation in transformations)
            {
                current = transformation.Transform(current);
            }

            --remaining;
            if (ReferenceEquals(previous, current))
            {
                return current;
            }
        }
    }
}
