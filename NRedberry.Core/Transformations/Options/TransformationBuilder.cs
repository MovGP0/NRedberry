using System.Reflection;
using NRedberry.Transformations.Symmetrization;

namespace NRedberry.Transformations.Options;

public static class TransformationBuilder
{
    public static T BuildOptionsFromMap<T>(IDictionary<string, object?> optionsMap) where T : class
    {
        return (T)BuildOptionsFromMap(typeof(T), optionsMap);
    }

    public static object BuildOptionsFromMap(Type optionsType, IDictionary<string, object?> optionsMap)
    {
        ArgumentNullException.ThrowIfNull(optionsType);
        ArgumentNullException.ThrowIfNull(optionsMap);

        object options = CreateInstance(optionsType);
        foreach (FieldInfo field in GetOptionFields(optionsType))
        {
            OptionAttribute option = field.GetCustomAttribute<OptionAttribute>()!;
            if (!optionsMap.TryGetValue(option.Name, out object? optionValue))
            {
                continue;
            }

            if (optionValue is null)
            {
                continue;
            }

            field.SetValue(options, ConvertValue(optionValue, field.FieldType));
        }

        TriggerOptionsCreated(options);
        return options;
    }

    public static T BuildOptionsFromList<T>(IList<object?> optionsList) where T : class
    {
        return (T)BuildOptionsFromList(typeof(T), optionsList);
    }

    public static object BuildOptionsFromList(Type optionsType, IList<object?> optionsList)
    {
        ArgumentNullException.ThrowIfNull(optionsType);
        ArgumentNullException.ThrowIfNull(optionsList);

        object options = CreateInstance(optionsType);
        Dictionary<int, FieldInfo> fieldsByIndex = GetOptionFields(optionsType)
            .ToDictionary(
                field => field.GetCustomAttribute<OptionAttribute>()!.Index,
                field => field);

        for (int i = 0; i < optionsList.Count; i++)
        {
            if (!fieldsByIndex.TryGetValue(i, out FieldInfo? field))
            {
                throw new InvalidOperationException($"No option field with index {i} was found on {optionsType.FullName}.");
            }

            field.SetValue(options, ConvertValue(optionsList[i], field.FieldType));
        }

        TriggerOptionsCreated(options);
        return options;
    }

    public static TTransformation CreateTransformation<TTransformation>(IReadOnlyList<object?> arguments, IReadOnlyDictionary<string, object?> options)
        where TTransformation : class, ITransformation
    {
        ArgumentNullException.ThrowIfNull(arguments);
        ArgumentNullException.ThrowIfNull(options);

        return (TTransformation)CreateTransformation(typeof(TTransformation), arguments, options);
    }

    public static TTransformation CreateTransformation<TTransformation>(IReadOnlyList<object?> argumentsOrOptions)
        where TTransformation : class, ITransformation
    {
        ArgumentNullException.ThrowIfNull(argumentsOrOptions);

        ConstructorInfo creator = GetCreator(typeof(TTransformation));
        CreatorAttribute creatorAttribute = creator.GetCustomAttribute<CreatorAttribute>()!;
        if (creatorAttribute.HasArgs)
        {
            return (TTransformation)CreateTransformation(typeof(TTransformation), argumentsOrOptions, new Dictionary<string, object?>());
        }

        return (TTransformation)CreateTransformation(typeof(TTransformation), Array.Empty<object?>(), argumentsOrOptions);
    }

    public static TTransformation CreateTransformation<TTransformation>(IReadOnlyList<object?> arguments, IList<object?> options)
        where TTransformation : class, ITransformation
    {
        ArgumentNullException.ThrowIfNull(arguments);
        ArgumentNullException.ThrowIfNull(options);

        return (TTransformation)CreateTransformation(typeof(TTransformation), arguments, options);
    }

    private static void TriggerOptionsCreated(object? options)
    {
        if (options is IOptions created)
        {
            created.TriggerCreate();
        }
    }

    private static object CreateTransformation(Type transformationType, IReadOnlyList<object?> arguments, object optionsSource)
    {
        ArgumentNullException.ThrowIfNull(transformationType);
        ArgumentNullException.ThrowIfNull(arguments);
        ArgumentNullException.ThrowIfNull(optionsSource);

        ConstructorInfo creator = GetCreator(transformationType);
        CreatorAttribute creatorAttribute = creator.GetCustomAttribute<CreatorAttribute>()!;
        ParameterInfo[] parameters = creator.GetParameters();
        int optionsIndex = FindOptionsParameterIndex(parameters);
        object? optionsValue = optionsIndex >= 0
            ? BuildOptions(parameters[optionsIndex].ParameterType, optionsSource)
            : null;

        object?[] initArgs = creatorAttribute.Vararg
            ? BuildVarargArguments(parameters, arguments, optionsIndex, optionsValue)
            : BuildFixedArguments(parameters, arguments, optionsIndex, optionsValue);

        return creator.Invoke(initArgs);
    }

    private static ConstructorInfo GetCreator(Type transformationType)
    {
        ConstructorInfo? creator = transformationType
            .GetConstructors()
            .FirstOrDefault(constructor => constructor.GetCustomAttribute<CreatorAttribute>() is not null);

        return creator ?? throw new InvalidOperationException($"No creator constructor was found on {transformationType.FullName}.");
    }

    private static int FindOptionsParameterIndex(ParameterInfo[] parameters)
    {
        int optionsIndex = -1;
        for (int i = 0; i < parameters.Length; i++)
        {
            if (parameters[i].GetCustomAttribute<OptionsAttribute>() is null)
            {
                continue;
            }

            if (optionsIndex >= 0)
            {
                throw new InvalidOperationException("Only one [Options] parameter is supported.");
            }

            optionsIndex = i;
        }

        return optionsIndex;
    }

    private static object?[] BuildFixedArguments(
        ParameterInfo[] parameters,
        IReadOnlyList<object?> arguments,
        int optionsIndex,
        object? optionsValue)
    {
        int requiredArgumentCount = optionsIndex >= 0 ? parameters.Length - 1 : parameters.Length;
        if (arguments.Count != requiredArgumentCount)
        {
            throw new ArgumentException($"Expected {requiredArgumentCount} arguments but got {arguments.Count}.", nameof(arguments));
        }

        object?[] initArgs = new object?[parameters.Length];
        int argumentIndex = 0;
        for (int i = 0; i < parameters.Length; i++)
        {
            if (i == optionsIndex)
            {
                initArgs[i] = optionsValue;
                continue;
            }

            initArgs[i] = ConvertValue(arguments[argumentIndex], parameters[i].ParameterType);
            argumentIndex++;
        }

        return initArgs;
    }

    private static object?[] BuildVarargArguments(
        ParameterInfo[] parameters,
        IReadOnlyList<object?> arguments,
        int optionsIndex,
        object? optionsValue)
    {
        if (optionsIndex >= 0)
        {
            if (parameters.Length != 2)
            {
                throw new InvalidOperationException("Vararg creators with options must have exactly two parameters.");
            }

            int arrayIndex = optionsIndex == 0 ? 1 : 0;
            return BuildVarargArguments(parameters, arguments, arrayIndex, optionsIndex, optionsValue);
        }

        if (parameters.Length != 1)
        {
            throw new InvalidOperationException("Vararg creators without options must have exactly one parameter.");
        }

        return BuildVarargArguments(parameters, arguments, 0, -1, null);
    }

    private static object?[] BuildVarargArguments(
        ParameterInfo[] parameters,
        IReadOnlyList<object?> arguments,
        int arrayIndex,
        int optionsIndex,
        object? optionsValue)
    {
        Type arrayType = parameters[arrayIndex].ParameterType;
        if (!arrayType.IsArray)
        {
            throw new InvalidOperationException("Vararg creator parameter must be an array type.");
        }

        Array array = Array.CreateInstance(arrayType.GetElementType()!, arguments.Count);
        for (int i = 0; i < arguments.Count; i++)
        {
            array.SetValue(ConvertValue(arguments[i], arrayType.GetElementType()!), i);
        }

        object?[] initArgs = new object?[parameters.Length];
        initArgs[arrayIndex] = array;
        if (optionsIndex >= 0)
        {
            initArgs[optionsIndex] = optionsValue;
        }

        return initArgs;
    }

    private static object BuildOptions(Type optionsType, object optionsSource)
    {
        return optionsSource switch
        {
            IReadOnlyDictionary<string, object?> readOnlyDictionary => BuildOptionsFromMap(optionsType, new Dictionary<string, object?>(readOnlyDictionary)),
            IDictionary<string, object?> dictionary => BuildOptionsFromMap(optionsType, dictionary),
            IList<object?> list => BuildOptionsFromList(optionsType, list),
            IReadOnlyList<object?> readOnlyList => BuildOptionsFromList(optionsType, readOnlyList.ToList()),
            _ => throw new ArgumentException($"Unsupported options source type: {optionsSource.GetType().FullName}.", nameof(optionsSource))
        };
    }

    private static object CreateInstance(Type type)
    {
        return Activator.CreateInstance(type)
            ?? throw new InvalidOperationException($"Unable to create an instance of {type.FullName}.");
    }

    private static IEnumerable<FieldInfo> GetOptionFields(Type optionsType)
    {
        return optionsType.GetFields(BindingFlags.Instance | BindingFlags.Public)
            .Where(field => field.GetCustomAttribute<OptionAttribute>() is not null);
    }

    private static object? ConvertValue(object? value, Type targetType)
    {
        if (value is null)
        {
            if (targetType.IsValueType && Nullable.GetUnderlyingType(targetType) is null)
            {
                throw new InvalidOperationException($"Cannot assign null to {targetType.FullName}.");
            }

            return null;
        }

        if (targetType.IsInstanceOfType(value))
        {
            return value;
        }

        Type? nullableUnderlyingType = Nullable.GetUnderlyingType(targetType);
        Type effectiveTargetType = nullableUnderlyingType ?? targetType;
        if (effectiveTargetType.IsEnum)
        {
            return value is string text
                ? Enum.Parse(effectiveTargetType, text, ignoreCase: false)
                : Enum.ToObject(effectiveTargetType, value);
        }

        return Convert.ChangeType(value, effectiveTargetType);
    }
}
