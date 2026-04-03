using System.Collections.Generic;
using NRedberry;
using NRedberry.Tensors;
using NRedberry.Transformations.Substitutions;
using Shouldly;
using TensorCC = NRedberry.Tensors.CC;
using TensorApi = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Transformations.Substitutions;

public sealed class SubstitutionIteratorTest
{
    [Fact]
    public void ShouldTrackForbiddenIndicesForNestedReplacement()
    {
        ConfigureContext();

        NRedberry.Tensors.Tensor tensor = TensorApi.Parse("A_mk*G^amn_a*(S^k_g*(D^g+Q^gz_z)+N^k_ez^ez)*E");
        SubstitutionIterator iterator = new(tensor);
        HashSet<int> expectedForbidden = TensorUtils.GetAllIndicesNamesT(tensor);
        HashSet<int>? actualForbidden = null;
        int visitedCount = 0;

        while (iterator.Next() is { } current)
        {
            visitedCount++;

            if (current.ToString(OutputFormat.Redberry) != "E")
            {
                continue;
            }

            actualForbidden = [.. iterator.GetForbidden()];
            iterator.SafeSet(TensorApi.Parse("H^l_l"));
        }

        string result = iterator.Result().ToString(OutputFormat.Redberry);

        visitedCount.ShouldBeGreaterThan(0);
        actualForbidden.ShouldNotBeNull();
        actualForbidden.ShouldBe(expectedForbidden, ignoreOrder: true);
        result.ShouldContain("H");
        iterator.Result().Indices.GetFree().EqualsRegardlessOrder(tensor.Indices.GetFree()).ShouldBeTrue();
    }

    [Fact]
    public void ShouldReplaceWholeProductAfterLeavingChildren()
    {
        ConfigureContext();

        SubstitutionIterator iterator = new(TensorApi.Parse("a*b"));
        List<string> visited = [];

        while (iterator.Next() is { } current)
        {
            visited.Add(current.ToString(OutputFormat.Redberry));

            if (current.ToString(OutputFormat.Redberry) != "a*b")
            {
                continue;
            }

            iterator.IsCurrentModified().ShouldBeFalse();
            iterator.Set(TensorApi.Parse("H^l_l"));
            iterator.IsCurrentModified().ShouldBeFalse();
        }

        visited.ShouldBe(["a", "b", "a*b"]);
        iterator.Result().ToString(OutputFormat.Redberry)
            .ShouldBe(TensorApi.Parse("H^l_l").ToString(OutputFormat.Redberry));
    }

    private static void ConfigureContext()
    {
        TensorCC.ResetTensorNames(1423);
        TensorCC.SetDefaultOutputFormat(OutputFormat.Redberry);
    }
}
