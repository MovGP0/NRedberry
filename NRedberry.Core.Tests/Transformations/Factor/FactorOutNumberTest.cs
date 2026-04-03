using NRedberry.Tensors;
using NRedberry.Transformations.Expand;
using NRedberry.Transformations.Factor;
using TensorFactory = NRedberry.Tensors.Tensors;
using TensorType = NRedberry.Tensors.Tensor;

namespace NRedberry.Core.Tests.Transformations.Factor;

public sealed class FactorOutNumberTest
{
    [Fact]
    public void ShouldFactorOutCommonInteger()
    {
        TensorType tensor = TensorFactory.Parse("2*a - 2*I*b");
        TensorUtils.Equals(tensor, ExpandTransformation.Expand(FactorOutNumber.Instance.Transform(tensor))).ShouldBeTrue();

        tensor = TensorFactory.Parse("2*a - 2*b");
        TensorUtils.Equals(tensor, ExpandTransformation.Expand(FactorOutNumber.Instance.Transform(tensor))).ShouldBeTrue();

        tensor = TensorFactory.Parse("2*a + 2*I*b");
        TensorUtils.Equals(tensor, ExpandTransformation.Expand(FactorOutNumber.Instance.Transform(tensor))).ShouldBeTrue();

        tensor = TensorFactory.Parse("2*I*a - 2*I*b");
        TensorUtils.Equals(tensor, ExpandTransformation.Expand(FactorOutNumber.Instance.Transform(tensor))).ShouldBeTrue();

        tensor = TensorFactory.Parse("2*I*a - 2*I*b + 3*a");
        TensorUtils.Equals(tensor, ExpandTransformation.Expand(FactorOutNumber.Instance.Transform(tensor))).ShouldBeTrue();

        tensor = TensorFactory.Parse("2*I*a - 2*I*b + a");
        TensorUtils.Equals(tensor, ExpandTransformation.Expand(FactorOutNumber.Instance.Transform(tensor))).ShouldBeTrue();

        tensor = TensorFactory.Parse("2*I*a - 22*I*b + 122*a");
        TensorUtils.Equals(tensor, ExpandTransformation.Expand(FactorOutNumber.Instance.Transform(tensor))).ShouldBeTrue();

        tensor = TensorFactory.Parse("1222*I*a - 1222*I*b + 122*a");
        TensorUtils.Equals(tensor, ExpandTransformation.Expand(FactorOutNumber.Instance.Transform(tensor))).ShouldBeTrue();

        tensor = TensorFactory.Parse("1223*I*a - 1225*I*b + 127*a");
        ReferenceEquals(tensor, FactorOutNumber.Instance.Transform(tensor)).ShouldBeTrue();

        tensor = TensorFactory.Parse("1223*I/19*a - 1225*I/17*b + 127/17*a");
        ReferenceEquals(tensor, FactorOutNumber.Instance.Transform(tensor)).ShouldBeTrue();

        tensor = TensorFactory.Parse("12*31*I*a - 12*37*b + 12*39*v");
        TensorUtils.Equals(
            TensorFactory.Parse("12*(31*I*a - 37*b + 39*v)"),
            FactorOutNumber.Instance.Transform(tensor))
            .ShouldBeTrue();

        tensor = TensorFactory.Parse("12*31*I*a - 12*I*37*b + 12*I*39*v");
        TensorUtils.Equals(
            TensorFactory.Parse("12*I*(31*a - 37*b + 39*v)"),
            FactorOutNumber.Instance.Transform(tensor))
            .ShouldBeTrue();

        tensor = TensorFactory.Parse("2*(12*a+12*b) + 24*c");
        TensorUtils.Equals(
            TensorFactory.Parse("24*(a+b+c)"),
            FactorOutNumber.Instance.Transform(tensor))
            .ShouldBeTrue();
    }

    [Fact]
    public void ShouldFactorOutCommonRational()
    {
        TensorType tensor = TensorFactory.Parse("2/13*a - 2/13*I*b");
        TensorUtils.Equals(tensor, ExpandTransformation.Expand(FactorOutNumber.Instance.Transform(tensor))).ShouldBeTrue();

        tensor = TensorFactory.Parse("2/13*a - 2/13*b");
        TensorUtils.Equals(tensor, ExpandTransformation.Expand(FactorOutNumber.Instance.Transform(tensor))).ShouldBeTrue();

        tensor = TensorFactory.Parse("2*a/13 + 2*I*b/13");
        TensorUtils.Equals(tensor, ExpandTransformation.Expand(FactorOutNumber.Instance.Transform(tensor))).ShouldBeTrue();

        tensor = TensorFactory.Parse("2/13*I*a - 2/13*I*b");
        TensorUtils.Equals(tensor, ExpandTransformation.Expand(FactorOutNumber.Instance.Transform(tensor))).ShouldBeTrue();

        tensor = TensorFactory.Parse("2/13*I*a - 2/13*I*b + 3/13*a");
        TensorUtils.Equals(tensor, ExpandTransformation.Expand(FactorOutNumber.Instance.Transform(tensor))).ShouldBeTrue();

        tensor = TensorFactory.Parse("2/13*I*a - 2/13*I*b + a/13");
        TensorUtils.Equals(tensor, ExpandTransformation.Expand(FactorOutNumber.Instance.Transform(tensor))).ShouldBeTrue();

        tensor = TensorFactory.Parse("2/13*I*a - 22/13*I*b + 122/13*a");
        TensorUtils.Equals(tensor, ExpandTransformation.Expand(FactorOutNumber.Instance.Transform(tensor))).ShouldBeTrue();

        tensor = TensorFactory.Parse("1222/13*I*a - 1222/13*I*b + 122/13*a");
        TensorUtils.Equals(tensor, ExpandTransformation.Expand(FactorOutNumber.Instance.Transform(tensor))).ShouldBeTrue();

        tensor = TensorFactory.Parse("1223/13*I*a - 1225/13*I*b + 127/13*a");
        ReferenceEquals(tensor, FactorOutNumber.Instance.Transform(tensor)).ShouldBeTrue();

        tensor = TensorFactory.Parse("1223*I/19*a - 1225*I/17*b + 127/17*a");
        ReferenceEquals(tensor, FactorOutNumber.Instance.Transform(tensor)).ShouldBeTrue();

        tensor = TensorFactory.Parse("12*31*I/13*a - 12*37/13*b + 12*40/13*v");
        TensorUtils.Equals(
            TensorFactory.Parse("12/13*(31*I*a - 37*b + 40*v)"),
            FactorOutNumber.Instance.Transform(tensor))
            .ShouldBeTrue();

        tensor = TensorFactory.Parse("12*31/13*I*a/13 - 12*I*37*b/13 + 12*I*41*v/13");
        TensorUtils.Equals(
            TensorFactory.Parse("12*I*(31/13*a - 37*b + 41*v)/13"),
            FactorOutNumber.Instance.Transform(tensor))
            .ShouldBeTrue();

        tensor = TensorFactory.Parse("2*(12*a+12*b/13) + 24*c");
        TensorUtils.Equals(
            TensorFactory.Parse("24*(a+b/13+c)"),
            FactorOutNumber.Instance.Transform(tensor))
            .ShouldBeTrue();
    }

    [Fact]
    public void ShouldFactorOutImaginaryUnit()
    {
        TensorType tensor = TensorFactory.Parse("-I*a - I*b");
        TensorUtils.Equals(
            TensorFactory.Parse("-I*(a+b)"),
            FactorOutNumber.Instance.Transform(tensor))
            .ShouldBeTrue();
    }
}
