using NRedberry.Indices;
using NRedberry.Tensors;
using TensorFactory = NRedberry.Tensors.Tensors;

namespace NRedberry.Core.Tests.Tensors;

public sealed class TensorFieldTests
{
    [Fact]
    public void ShouldExposeArgumentsAndFactories()
    {
        TensorField field = TensorFactory.Parse("f[x]").ShouldBeOfType<TensorField>();

        field.Size.ShouldBe(1);
        field[0].ToString(OutputFormat.Redberry).ShouldBe("x");
        field.IsDerivative().ShouldBeFalse();
        field.IsDiracDelta().ShouldBeFalse();
        field.GetParentField().ShouldBeSameAs(field);
        field.GetBuilder().GetType().Name.ShouldBe("TensorFieldBuilder");
        field.GetFactory()!.GetType().Name.ShouldBe("TensorFieldFactory");
    }

    [Fact]
    public void ShouldReturnClonedArgumentsAndArgumentIndices()
    {
        TensorField field = TensorFactory.Parse("f[x]").ShouldBeOfType<TensorField>();

        NRedberry.Tensors.Tensor[] arguments = field.GetArguments();
        SimpleIndices[] argIndices = field.GetArgIndices();

        arguments.ShouldHaveSingleItem();
        argIndices.ShouldHaveSingleItem();
        field.GetArguments().ShouldNotBeSameAs(arguments);
        field.GetArgIndices().ShouldNotBeSameAs(argIndices);
        field.GetArgIndices(0).Size().ShouldBe(0);
    }

    [Fact]
    public void ShouldRenderAsFieldApplication()
    {
        TensorField field = TensorFactory.Parse("f[x]").ShouldBeOfType<TensorField>();

        field.ToString(OutputFormat.Redberry).ShouldBe("f[x]");
    }
}
