using System.Collections;
using NRedberry.Tensors;
using NRedberry.Tensors.Iterators;

namespace NRedberry.TensorGenerators;

public sealed class GeneratedTensor
{
    public GeneratedTensor(SimpleTensor[] coefficients, Tensor generatedTensor)
    {
        ArgumentNullException.ThrowIfNull(coefficients);
        ArgumentNullException.ThrowIfNull(generatedTensor);

        Coefficients = coefficients;
        Tensor = generatedTensor;
    }

    public SimpleTensor[] Coefficients { get; }
    public Tensor Tensor { get; }
}

public sealed class SymbolsGenerator : IEnumerator<SimpleTensor>
{
    private string Name { get; }
    private int Count { get; set; }
    private string[] UsedNames { get; }

    public SymbolsGenerator(string name, params Tensor[] forbiddenTensors)
    {
        CheckName(name);
        Name = name;

        var set = new HashSet<string>();
        FromChildToParentIterator iterator;
        foreach (Tensor f in forbiddenTensors)
        {
            iterator = new FromChildToParentIterator(f);
            Tensor c;
            while ((c = iterator.Next()) != null)
            {
                if (TensorUtils.IsSymbol(c))
                    set.Add(c.ToString());
            }
        }

        var usedNames = new string[set.Count];
        var i = -1;
        foreach (var str in set)
        {
            usedNames[++i] = str;
        }

        UsedNames = usedNames.Order().ToArray();
    }

    public SymbolsGenerator(string name)
    {
        CheckName(name);
        Name = name;
        UsedNames = [];
    }

    private static void CheckName(string name)
    {
        if (string.IsNullOrEmpty(name))
            throw new ArgumentException("Empty string is illegal.");
    }

    public bool MoveNext()
    {
        ++Count;
        return true;
    }

    public void Reset()
    {
        Count = 0;
    }

    public SimpleTensor Current => Tensors.Tensors.ParseSimple(Name + Count);

    object IEnumerator.Current => Current;

    public void Dispose()
    {
    }
}
