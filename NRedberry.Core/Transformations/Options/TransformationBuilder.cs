using NRedberry.Core.Transformations.Symmetrization;

namespace NRedberry.Core.Transformations.Options;

/// <summary>
/// Skeleton port of cc.redberry.core.transformations.options.TransformationBuilder.
/// </summary>
public static class TransformationBuilder
{
    public static T BuildOptionsFromMap<T>(IDictionary<string, object?> optionsMap) where T : class
    {
        throw new NotImplementedException();
    }

    public static object BuildOptionsFromMap(Type optionsType, IDictionary<string, object?> optionsMap)
    {
        throw new NotImplementedException();
    }

    public static T BuildOptionsFromList<T>(IList<object?> optionsList) where T : class
    {
        throw new NotImplementedException();
    }

    public static object BuildOptionsFromList(Type optionsType, IList<object?> optionsList)
    {
        throw new NotImplementedException();
    }

    public static TTransformation CreateTransformation<TTransformation>(IReadOnlyList<object?> arguments, IReadOnlyDictionary<string, object?> options)
        where TTransformation : class, ITransformation
    {
        throw new NotImplementedException();
    }

    public static TTransformation CreateTransformation<TTransformation>(IReadOnlyList<object?> argumentsOrOptions)
        where TTransformation : class, ITransformation
    {
        throw new NotImplementedException();
    }

    public static TTransformation CreateTransformation<TTransformation>(IReadOnlyList<object?> arguments, IList<object?> options)
        where TTransformation : class, ITransformation
    {
        throw new NotImplementedException();
    }

    private static void TriggerOptionsCreated(object? options)
    {
        throw new NotImplementedException();
    }
}
