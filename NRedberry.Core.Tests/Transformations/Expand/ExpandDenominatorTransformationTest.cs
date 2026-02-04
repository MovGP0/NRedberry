using NRedberry.Tensors;
using NRedberry.Transformations.Expand;
using TensorFactory = NRedberry.Tensors.Tensors;
using TensorType = NRedberry.Tensors.Tensor;
using Xunit;

namespace NRedberry.Core.Tests.Transformations.Expand;

public sealed class ExpandDenominatorTransformationTest
{
    [Fact]
    public void ShouldLeaveSimpleDenominatorUnchanged()
    {
        TensorType tensor = TensorFactory.Parse("(a+b)/(c+d)");
        Assert.True(ReferenceEquals(tensor, ExpandDenominatorTransformation.Expand(tensor)));
    }

    [Fact]
    public void ShouldLeaveSimpleDenominatorPowerUnchanged()
    {
        TensorType tensor = TensorFactory.Parse("(a+b)**2/(c+d)");
        TensorType result = ExpandDenominatorTransformation.Expand(tensor);
        Assert.True(ReferenceEquals(tensor, result));
    }

    [Fact]
    public void ShouldExpandSquaredDenominator()
    {
        TensorType actual = ExpandDenominatorTransformation.Expand(TensorFactory.Parse("(a+b)**2/(c+d)**2"));
        TensorType expected = TensorFactory.Parse("(a+b)**2/(c**2+2*c*d+d**2)");
        Assert.True(TensorUtils.Equals(actual, expected));
    }

    [Fact]
    public void ShouldExpandNestedDenominator()
    {
        TensorType actual = ExpandDenominatorTransformation.Expand(TensorFactory.Parse("(x+(a+b)**2)/(c+d)**2"));
        TensorType expected = TensorFactory.Parse("(x+(a+b)**2)/(c**2+2*c*d+d**2)");
        Assert.True(TensorUtils.Equals(actual, expected));
    }

    [Fact]
    public void ShouldExpandDenominatorWithScalarFactor()
    {
        TensorType actual = ExpandDenominatorTransformation.Expand(TensorFactory.Parse("f*(x+(a+b)**2)/(c+d)**2"));
        TensorType expected = TensorFactory.Parse("f*(x+(a+b)**2)/(c**2+2*c*d+d**2)");
        Assert.True(TensorUtils.Equals(actual, expected));
    }

    [Fact]
    public void ShouldExpandDenominatorWithIndexlessFactor()
    {
        TensorType actual = ExpandDenominatorTransformation.Expand(TensorFactory.Parse("f*(x+(a+b)**2)/((c+d)**2*k)"));
        TensorType expected = TensorFactory.Parse("f*(x+(a+b)**2)/(k*c**2+2*k*c*d+k*d**2)");
        Assert.True(TensorUtils.Equals(actual, expected));
    }

    [Fact]
    public void ShouldExpandDenominatorWithMultipleIndexlessFactors()
    {
        TensorType actual = ExpandDenominatorTransformation.Expand(TensorFactory.Parse("f*(x+(a+b)**2)/((c+d)**2*k*i)"));
        TensorType expected = TensorFactory.Parse("f*(x+(a+b)**2)/(k*c**2*i+2*k*c*d*i+k*d**2*i)");
        Assert.True(TensorUtils.Equals(actual, expected));
    }

    [Fact]
    public void ShouldExpandSimpleProductDenominator()
    {
        TensorType actual = ExpandDenominatorTransformation.Expand(TensorFactory.Parse("1/((a+b)*c)"));
        TensorType expected = TensorFactory.Parse("1/(a*c + b*c)");
        Assert.True(TensorUtils.Equals(actual, expected));
    }

    [Fact]
    public void ShouldExpandIndexedDenominator()
    {
        TensorType actual = ExpandDenominatorTransformation.Expand(TensorFactory.Parse("(a+b)**2/(k_i*(a^i+b^i))**2"));
        TensorType expected = TensorFactory.Parse("(a+b)**2/(k_i*a^i*k_j*a^j+2*k_j*a^j*k_i*b^i+k_i*b^i*k_j*b^j)");
        Assert.True(TensorUtils.Equals(actual, expected));
    }

    [Fact]
    public void ShouldExpandNegativePowerDenominator()
    {
        TensorType actual = ExpandDenominatorTransformation.Expand(TensorFactory.Parse("(a + b/(1 + c)**2)**(-2)"));
        TensorType expected = TensorFactory.Parse("(a**2 + b**2/(1 + c)**4 + (2*a*b)/(1 + c)**2)**(-1)");
        Assert.True(TensorUtils.Equals(actual, expected));
    }

    [Fact]
    public void ShouldExpandNestedNegativePowerDenominator()
    {
        TensorType actual = ExpandDenominatorTransformation.Expand(TensorFactory.Parse("(b*(c + d) + (e + f)/a)**(-2)"));
        TensorType expected = TensorFactory.Parse("(b**2*c**2 + 2*b**2*c*d + b**2*d**2 + (2*b*c*e)/a + (2*b*d*e)/a + e**2/a**2 + (2*b*c*f)/a + (2*b*d*f)/a + (2*e*f)/a**2 + f**2/a**2)**(-1)");
        Assert.True(TensorUtils.Equals(actual, expected));
    }
}
