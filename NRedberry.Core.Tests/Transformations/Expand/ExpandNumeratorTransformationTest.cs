using NRedberry.Tensors;
using NRedberry.Transformations.Expand;
using TensorFactory = NRedberry.Tensors.Tensors;
using TensorType = NRedberry.Tensors.Tensor;
using Xunit;

namespace NRedberry.Core.Tests.Transformations.Expand;

public sealed class ExpandNumeratorTransformationTest
{
    [Fact]
    public void ShouldLeaveSimpleNumeratorUnchanged()
    {
        TensorType tensor = TensorFactory.Parse("(a+b)/(c+d)");
        Assert.True(ReferenceEquals(tensor, ExpandNumeratorTransformation.Expand(tensor)));
    }

    [Fact]
    public void ShouldExpandSquaredNumerator()
    {
        TensorType actual = ExpandNumeratorTransformation.Expand(TensorFactory.Parse("(a+b)**2/(c+d)"));
        TensorType expected = TensorFactory.Parse("(a**2+2*a*b+b**2)/(c+d)");
        Assert.True(TensorUtils.Equals(actual, expected));
    }

    [Fact]
    public void ShouldExpandNumeratorWithAdditionalTerm()
    {
        TensorType actual = ExpandNumeratorTransformation.Expand(TensorFactory.Parse("((a+b)**2 + x)/(c+d)"));
        TensorType expected = TensorFactory.Parse("(a**2+2*a*b+b**2+x)/(c+d)");
        Assert.True(TensorUtils.Equals(actual, expected));
    }

    [Fact]
    public void ShouldExpandProductNumerator()
    {
        TensorType actual = ExpandNumeratorTransformation.Expand(TensorFactory.Parse("((a+b)**2 + x)*(a+b)/(c+d)"));
        TensorType expected = TensorFactory.Parse("(a**3+3*a**2*b+3*a*b**2+a*x+b**3+b*x)/(c+d)");
        Assert.True(TensorUtils.Equals(actual, expected));
    }

    [Fact]
    public void ShouldExpandSumOfFractions()
    {
        TensorType actual = ExpandNumeratorTransformation.Expand(TensorFactory.Parse("(a+b)**2/x+(c+d)**2/y"));
        TensorType expected = TensorFactory.Parse("(a**2+2*a*b+b**2)/x+(c**2+2*c*d+d**2)/y");
        Assert.True(TensorUtils.Equals(actual, expected));
    }

    [Fact]
    public void ShouldExpandIndexedNumerators()
    {
        TensorType actual = ExpandNumeratorTransformation.Expand(TensorFactory.Parse("f_m*(a+b)**2/x+g_m*(c+d)**2/y"));
        TensorType expected = TensorFactory.Parse("(f_m*a**2+f_m*2*a*b+f_m*b**2)/x+(g_m*c**2+g_m*2*c*d+g_m*d**2)/y");
        Assert.True(TensorUtils.Equals(actual, expected));
    }

    [Fact]
    public void ShouldExpandNestedIndexedNumerator()
    {
        TensorType actual = ExpandNumeratorTransformation.Expand(TensorFactory.Parse("((a+b)*f_mn+(c+d*(a+b))*(g_mn+l_mn))*(a+b)/a"));
        TensorType expected = TensorFactory.Parse("a**(-1)*((c*b+c*a+b**2*d+a**2*d+2*d*b*a)*g_{mn}+(2*b*a+a**2+b**2)*f_{mn}+(c*b+c*a+b**2*d+a**2*d+2*d*b*a)*l_{mn})");
        Assert.True(TensorUtils.Equals(actual, expected));
    }
}
