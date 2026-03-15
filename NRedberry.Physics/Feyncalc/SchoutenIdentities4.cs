using System.Collections.Generic;
using NRedberry.Core.Combinatorics;
using NRedberry.IndexMapping;
using NRedberry.Indices;
using NRedberry.Numbers;
using NRedberry.Parsers.Preprocessor;
using NRedberry.Tensors;
using NRedberry.Tensors.Iterators;
using NRedberry.Transformations.Symmetrization;
using TensorCC = NRedberry.Tensors.CC;
using TensorFactory = NRedberry.Tensors.Tensors;

namespace NRedberry.Physics.Feyncalc;

/// <summary>
/// Port of cc.redberry.physics.feyncalc.SchoutenIdentities4.
/// </summary>
public sealed class SchoutenIdentities4 : ITransformation
{
    private readonly SimpleTensor _leviCivita;
    private readonly Tensor[] _schouten1;
    private readonly Tensor[] _schouten2;
    private readonly Tensor[] _schouten3;
    private readonly Tensor[][] _allSchouten;

    private static readonly string[] s_schoutenCombinations1 =
    [
        "-g_{ad}*e_{bcef}",
        "g_{ac}*e_{bdef}",
        "-g_{ab}*e_{cdef}",
        "-g_{af}*e_{bcde}",
        "g_{ae}*e_{bcdf}"
    ];

    private static readonly string[] s_schoutenCombinations2 =
    [
        "e_{bfea}*g_{dc}",
        "-e_{dbfa}*g_{ec}",
        "e_{bcef}*g_{ad}",
        "e_{dbea}*g_{fc}",
        "e_{fdea}*g_{bc}",
        "-e_{bcdf}*g_{ae}",
        "e_{cdef}*g_{ab}",
        "e_{bcde}*g_{af}"
    ];

    private static readonly string[] s_schoutenCombinations3 =
    [
        "-g_{db}*e_{efac}",
        "-g_{de}*e_{bafc}",
        "-g_{cf}*e_{abde}",
        "-g_{da}*e_{febc}",
        "-g_{ac}*e_{bdef}",
        "g_{bc}*e_{adef}",
        "g_{df}*e_{ebac}",
        "g_{ce}*e_{abdf}"
    ];

    public SchoutenIdentities4(SimpleTensor leviCivita)
    {
        ArgumentNullException.ThrowIfNull(leviCivita);

        _leviCivita = leviCivita;
        var tokenTransformer = new ChangeIndicesTypesAndTensorNames(
            TypesAndNamesTransformer.Utils.And(
                TypesAndNamesTransformer.Utils.ChangeName(["e"], [_leviCivita.GetStringName()]),
                TypesAndNamesTransformer.Utils.ChangeType(
                    IndexType.LatinLower,
                    IndicesUtils.GetTypeEnum(_leviCivita.Indices[0]))));
        var parser = TensorCC.Current.ParseManager.Parser;

        _schouten1 = new Tensor[s_schoutenCombinations1.Length];
        for (int i = 0; i < _schouten1.Length; ++i)
        {
            _schouten1[i] = tokenTransformer.Transform(parser.Parse(s_schoutenCombinations1[i])).ToTensor();
        }

        _schouten2 = new Tensor[s_schoutenCombinations2.Length];
        for (int i = 0; i < _schouten2.Length; ++i)
        {
            _schouten2[i] = tokenTransformer.Transform(parser.Parse(s_schoutenCombinations2[i])).ToTensor();
        }

        _schouten3 = new Tensor[s_schoutenCombinations3.Length];
        for (int i = 0; i < _schouten3.Length; ++i)
        {
            _schouten3[i] = tokenTransformer.Transform(parser.Parse(s_schoutenCombinations3[i])).ToTensor();
        }

        _allSchouten = [_schouten1, _schouten2, _schouten3];
    }

    public Tensor Transform(Tensor tensor)
    {
        ArgumentNullException.ThrowIfNull(tensor);

        FromChildToParentIterator iterator = new(tensor);
        Tensor? current;
main:
        while ((current = iterator.Next()) is not null)
        {
            if (current is not Sum currentSum)
            {
                continue;
            }

            foreach (Tensor[] schouten in _allSchouten)
            {
                if (current.Size < schouten.Length)
                {
                    continue;
                }

                List<int>[] positions = new List<int>[schouten.Length];
                for (int i = 0; i < current.Size; ++i)
                {
                    Mapping0? mapping = BuildMapping(schouten[0], current[i]);
                    if (mapping is null)
                    {
                        continue;
                    }

                    for (int j = 0; j < schouten.Length; ++j)
                    {
                        positions[j] = [];
                    }

                    positions[0].Add(i);

                    bool matchedAll = true;
                    for (int j = 1; j < schouten.Length; ++j)
                    {
                        for (int k = 0; k < current.Size; ++k)
                        {
                            if (TestMapping(mapping, schouten[j], current[k]))
                            {
                                positions[j].Add(k);
                            }
                        }

                        if (positions[j].Count == 0)
                        {
                            matchedAll = false;
                            break;
                        }
                    }

                    if (!matchedAll)
                    {
                        continue;
                    }

                    int[][] positionArrays = new int[schouten.Length][];
                    for (int j = 0; j < schouten.Length; ++j)
                    {
                        positionArrays[j] = positions[j].ToArray();
                    }

                    int[]? distinctPositions = new IntDistinctTuplesPort(positionArrays).Take();
                    if (distinctPositions is not null)
                    {
                        iterator.Set(currentSum.Remove(distinctPositions));
                        goto main;
                    }
                }
            }
        }

        Tensor result = iterator.Result();
        if (!ReferenceEquals(result, tensor))
        {
            return Transform(result);
        }

        return result;
    }

    private static Mapping0? BuildMapping(Tensor epsilon, Tensor part)
    {
        ArgumentNullException.ThrowIfNull(epsilon);
        ArgumentNullException.ThrowIfNull(part);

        if (part is not Product product)
        {
            return null;
        }

        Complex factor = product.Factor;
        Mapping? mapping = IndexMappings.GetFirst(GetDataSubProduct(epsilon), GetDataSubProduct(product));
        if (mapping is null)
        {
            return null;
        }

        return new Mapping0(factor, mapping);
    }

    private static bool TestMapping(Mapping0 mapping, Tensor epsilon, Tensor part)
    {
        ArgumentNullException.ThrowIfNull(mapping);
        ArgumentNullException.ThrowIfNull(epsilon);
        ArgumentNullException.ThrowIfNull(part);

        if (part is not Product product)
        {
            return false;
        }

        Complex factor = product.Factor;
        Tensor epsilonData = GetDataSubProduct(epsilon);
        Tensor dataSubProduct = GetDataSubProduct(product);

        if (IndexMappings.TestMapping(mapping.IndexMapping, epsilonData, dataSubProduct))
        {
            return factor.Equals(mapping.Factor);
        }

        if (IndexMappings.TestMapping(mapping.IndexMapping.AddSign(true), epsilonData, dataSubProduct))
        {
            return factor.Equals(mapping.Factor.Negate());
        }

        return false;
    }

    private static bool Contains(int[] array, int value)
    {
        ArgumentNullException.ThrowIfNull(array);

        foreach (int current in array)
        {
            if (current == value)
            {
                return true;
            }
        }

        return false;
    }

    private static Tensor GetDataSubProduct(Product product)
    {
        return product.Data.Length switch
        {
            0 => Complex.One,
            1 => product.Data[0],
            _ => TensorFactory.Multiply(product.Data)
        };
    }

    private static Tensor GetDataSubProduct(Tensor tensor)
    {
        if (tensor is Product product)
        {
            return GetDataSubProduct(product);
        }

        return tensor;
    }

    private sealed record Mapping0(Complex Factor, Mapping IndexMapping);
}
