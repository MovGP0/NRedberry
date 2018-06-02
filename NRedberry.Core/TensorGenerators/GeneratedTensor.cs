using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NRedberry.Core.Tensors;
using NRedberry.Core.Tensors.Iterators;

namespace NRedberry.Core.TensorGenerators
{
    public sealed class GeneratedTensor
    {
        public SimpleTensor[] Coefficients { get; }
        public Tensor Tensor { get; }

        public GeneratedTensor(SimpleTensor[] coefficients, Tensor tensor)
        {
            Coefficients = coefficients;
            Tensor = tensor;
        }
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
                    if (TensorUtils.IsSymbol(c))
                        set.Add(c.ToString());
            }

            var usedNames = new string[set.Count];
            var i = -1;
            foreach (var str in set)
            {
                usedNames[++i] = str;
            }

            UsedNames = usedNames.OrderBy(n => n).ToArray();
        }

        public SymbolsGenerator(string name)
        {
            CheckName(name);
            Name = name;
            UsedNames = new string[0];
        }

        private static void CheckName(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentException("Empty string is illegal.");
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
}
