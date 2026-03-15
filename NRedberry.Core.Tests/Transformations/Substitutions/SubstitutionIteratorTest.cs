using NRedberry;
using Shouldly;
using TensorApi = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Transformations.Substitutions;

public sealed class SubstitutionIteratorTest
{
    [Fact]
    public void ShouldIterateAndReplaceWithForbiddenIndices()
    {
        SubstitutionIterator iterator = new(TensorApi.Parse("A_mk*E"));

        while (iterator.Next() is { } current)
        {
            if (current.ToString(OutputFormat.Redberry) != "E")
            {
                continue;
            }

            iterator.GetForbidden().ShouldNotBeEmpty();
            iterator.SafeSet(TensorApi.Parse("H^m_m"));
        }

        string result = iterator.Result().ToString(OutputFormat.Redberry);
        string conflicting = TensorApi.Parse("A_mk*H^m_m").ToString(OutputFormat.Redberry);

        result.ShouldContain("H");
        result.ShouldNotBe(conflicting);
    }

    [Fact]
    public void ShouldIterateProductsWithReplacement()
    {
        SubstitutionIterator iterator = new(TensorApi.Parse("a*b"));

        while (iterator.Next() is { } current)
        {
            if (current.ToString(OutputFormat.Redberry) != "a*b")
            {
                continue;
            }

            iterator.Set(TensorApi.Parse("H^l_l"));
        }

        iterator.Result().ToString(OutputFormat.Redberry)
            .ShouldBe(TensorApi.Parse("H^l_l").ToString(OutputFormat.Redberry));
    }
}
