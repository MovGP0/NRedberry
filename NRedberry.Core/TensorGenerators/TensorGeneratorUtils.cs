using NRedberry.Core.Combinatorics;
using NRedberry.IndexMapping;
using NRedberry.Indices;
using NRedberry.Tensors;
using TensorCC = NRedberry.Tensors.CC;

namespace NRedberry.TensorGenerators;

/*
 * Original: ./core/src/main/java/cc/redberry/core/tensorgenerator/TensorGeneratorUtils.java
 */

public static class TensorGeneratorUtils
{
    public static Tensor[] AllStatesCombinations(Tensor tensor)
    {
        ArgumentNullException.ThrowIfNull(tensor);

        Indices.Indices indices = tensor.Indices.GetFree();
        int[] indicesArray = indices.AllIndices.ToArray();
        List<int> metricIndices = [];
        List<int> nonMetricIndices = [];

        for (int i = 0; i < indices.Size(); ++i)
        {
            int index = indices[i];
            if (TensorCC.IsMetric(IndicesUtils.GetType(index)))
            {
                metricIndices.Add(IndicesUtils.GetNameWithType(index));
            }
            else
            {
                nonMetricIndices.Add(index);
            }
        }

        int[] metricInds = metricIndices.ToArray();
        int capacity = metricInds.Length == 0 ? 1 : (int)Math.Pow(2, metricInds.Length);
        List<Tensor> samples = new(capacity);

        for (int i = 0; i <= metricInds.Length; ++i)
        {
            var generator = new IntCombinationsGenerator(metricInds.Length, i);
            List<Tensor> combinationArray = [];
            foreach (int[] combination in generator)
            {
                int[] temp = new int[metricInds.Length];
                Array.Fill(temp, -1);

                for (int j = combination.Length - 1; j >= 0; --j)
                {
                    int index = metricInds[combination[j]];
                    temp[combination[j]] = IndicesUtils.CreateIndex(j, IndicesUtils.GetType(index), true);
                }

                int counter = combination.Length;
                for (int j = 0; j < metricInds.Length; ++j)
                {
                    if (temp[j] == -1)
                    {
                        temp[j] = IndicesUtils.CreateIndex(counter++, IndicesUtils.GetType(metricInds[j]), false);
                    }
                }

                List<int> result = new(nonMetricIndices);
                result.AddRange(temp);
                Tensor renamed = ApplyIndexMapping.Apply(tensor, new Mapping(indicesArray, result.ToArray()));

                bool duplicate = false;
                foreach (Tensor existing in combinationArray)
                {
                    if (TensorUtils.Compare1(existing, renamed) != null)
                    {
                        duplicate = true;
                        break;
                    }
                }

                if (!duplicate)
                {
                    combinationArray.Add(renamed);
                }
            }

            samples.AddRange(combinationArray);
        }

        return samples.ToArray();
    }
}
