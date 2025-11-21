using System.Collections;
using System.Numerics;
using NRedberry.Transformations.Options;
using NRedberry.Transformations.Symmetrization;
using Complex = NRedberry.Numbers.Complex;

namespace NRedberry.Groovy;

public class DSLTransformation<T>(Type clazz)
    where T : class, ITransformation
{
    public Type Clazz { get; } = clazz ?? throw new ArgumentNullException(nameof(clazz));

    public ITransformation GetAt(IList<object?> args, IDictionary<string, object?> map)
    {
        return TransformationBuilder.CreateTransformation<T>(ToList(args), ToDictionary(map));
    }

    public ITransformation GetAt(IList<object?> args, IList<object?> options)
    {
        return TransformationBuilder.CreateTransformation<T>(ToList(args), ToList(options));
    }

    public ITransformation GetAt(IList<object?> options)
    {
        if (options.Count == 0)
        {
            return TransformationBuilder.CreateTransformation<T>([]);
        }

        var last = options[^1];
        var leading = ToList(options, options.Count - 1);

        if (last is ITransformation transformation)
        {
            var trailing = new List<object?> { transformation };
            return TransformationBuilder.CreateTransformation<T>(leading, trailing);
        }

        if (last is IList lastList)
        {
            return TransformationBuilder.CreateTransformation<T>(leading, ToList(lastList));
        }

        if (last is IDictionary lastDictionary)
        {
            return TransformationBuilder.CreateTransformation<T>(leading, ToDictionary(lastDictionary));
        }

        return TransformationBuilder.CreateTransformation<T>(ToList(options));
    }

    public ITransformation GetAt(IDictionary<string, object?> map)
    {
        return TransformationBuilder.CreateTransformation<T>([], ToDictionary(map));
    }

    public ITransformation GetAt(object? value)
    {
        return TransformationBuilder.CreateTransformation<T>(new List<object?> { ToObject(value) });
    }

    public ITransformation GetAt(string value)
    {
        return TransformationBuilder.CreateTransformation<T>(new List<object?> { ToObject(value) });
    }

    private static List<object?> ToList(IList source)
    {
        var list = new List<object?>(source.Count);
        foreach (var item in source)
        {
            list.Add(ToObject(item));
        }

        return list;
    }

    private static List<object?> ToList(IList<object?> source)
    {
        var list = new List<object?>(source.Count);
        foreach (var item in source)
        {
            list.Add(ToObject(item));
        }

        return list;
    }

    private static List<object?> ToList(IList<object?> source, int count)
    {
        var list = new List<object?>(Math.Max(count, 0));
        for (var i = 0; i < count; i++)
        {
            list.Add(ToObject(source[i]));
        }

        return list;
    }

    private static Dictionary<string, object?> ToDictionary(IDictionary dictionary)
    {
        var map = new Dictionary<string, object?>(dictionary.Count);
        foreach (DictionaryEntry entry in dictionary)
        {
            map[entry.Key.ToString() ?? string.Empty] = ToObject(entry.Value);
        }

        return map;
    }

    private static Dictionary<string, object?> ToDictionary(IDictionary<string, object?> dictionary)
    {
        var map = new Dictionary<string, object?>(dictionary.Count);
        foreach (var pair in dictionary)
        {
            map[pair.Key] = ToObject(pair.Value);
        }

        return map;
    }

    private static object? ToObject(object? value)
    {
        try
        {
            return value switch
            {
                null
                    => null,

                string s
                    => Tensors.Tensors.Parse(s),

                ITransformation transformation
                    => transformation,

                BigInteger bigInteger
                    => new Complex(bigInteger),

                long or int or short or sbyte or ulong or uint or ushort or byte
                    => new Complex(Convert.ToInt64(value, System.Globalization.CultureInfo.InvariantCulture)),

                float or double or decimal
                    => new Complex(Convert.ToDouble(value, System.Globalization.CultureInfo.InvariantCulture)),

                _ => value
            };
        }
        catch
        {
            return value;
        }
    }
}
