using NRedberry.Numbers;
using NRedberry.Tensors;
using TensorFactory = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Tensors;

public sealed class SumTests
{
    [Fact]
    public void ShouldExposeChildrenSizeBuilderAndFactory()
    {
        Sum sum = TensorFactory.Parse("a+b").ShouldBeOfType<Sum>();

        sum.Size.ShouldBe(2);
        sum.GetBuilder().ShouldBeOfType<SumBuilder>();
        sum.GetFactory().ShouldBeSameAs(SumFactory.Factory);
    }

    [Fact]
    public void ShouldWrapStringInsideProductsAndPowers()
    {
        Sum sum = TensorFactory.Parse("a+b").ShouldBeOfType<Sum>();

        sum.ToStringWith<Product>(OutputFormat.Redberry).ShouldBe("(a+b)");
        sum.ToStringWith<Power>(OutputFormat.Redberry).ShouldBe("(a+b)");
    }

    [Fact]
    public void ShouldRemoveAndSelectTerms()
    {
        Sum sum = TensorFactory.Parse("a+b+c").ShouldBeOfType<Sum>();
        string removedTerm = sum[1].ToString(OutputFormat.Redberry);
        string firstTerm = sum[0].ToString(OutputFormat.Redberry);
        string thirdTerm = sum[2].ToString(OutputFormat.Redberry);

        NRedberry.Tensors.Tensor removed = sum.Remove(1);
        NRedberry.Tensors.Tensor selected = sum.Select([0, 2]);

        removed.Size.ShouldBe(2);
        removed.ToString(OutputFormat.Redberry).ShouldContain(firstTerm);
        removed.ToString(OutputFormat.Redberry).ShouldContain(thirdTerm);
        removed.ToString(OutputFormat.Redberry).ShouldNotContain(removedTerm);
        selected.Size.ShouldBe(2);
        selected.ToString(OutputFormat.Redberry).ShouldContain(firstTerm);
        selected.ToString(OutputFormat.Redberry).ShouldContain(thirdTerm);
    }

    [Fact]
    public void ShouldUseSumSpecificSetSemantics()
    {
        Sum sum = TensorFactory.Parse("a+b+c").ShouldBeOfType<Sum>();

        NRedberry.Tensors.Tensor removed = sum.Set(1, Complex.Zero);
        NRedberry.Tensors.Tensor unchanged = sum.Set(0, sum[0]);

        removed.Size.ShouldBe(2);
        unchanged.ShouldBeSameAs(sum);
    }
}
