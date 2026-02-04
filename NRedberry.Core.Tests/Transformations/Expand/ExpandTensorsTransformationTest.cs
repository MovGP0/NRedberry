using NRedberry.Tensors;
using NRedberry.Transformations.Expand;
using NRedberry.Transformations;
using NRedberry.Transformations.Symmetrization;
using TensorFactory = NRedberry.Tensors.Tensors;
using TensorType = NRedberry.Tensors.Tensor;
using Xunit;

namespace NRedberry.Core.Tests.Transformations.Expand;

public sealed class ExpandTensorsTransformationTest
{
    [Fact]
    public void ShouldExpandSingleSum()
    {
        TensorType tensor = TensorFactory.Parse("((a+b)*f_a + (a+c)*t_a)*c*k^a");
        TensorType actual = ExpandTensorsTransformation.Expand(tensor);
        TensorType expected = TensorFactory.Parse("c*(a+b)*f_a*k^a + c*(a+c)*t_a*k^a");
        Assert.True(TensorUtils.Equals(actual, expected));
    }

    [Fact]
    public void ShouldExpandTwoSummands()
    {
        TensorType tensor = TensorFactory.Parse("((a+b)*(f_a + r_a) + (a + c)*t_a)*c*k^a");
        TensorType actual = ExpandTensorsTransformation.Expand(tensor);
        TensorType expected = TensorFactory.Parse("c*(a+b)*f_a*k^a + c*(a+b)*r_a*k^a + c*(a+c)*t_a*k^a");
        Assert.True(TensorUtils.Equals(actual, expected));
    }

    [Fact]
    public void ShouldExpandMultipleFactors()
    {
        TensorType tensor = TensorFactory.Parse("((a+b)*(f_a + r_a) + (a + c)*t_a)*(c+r)*k^a");
        TensorType actual = ExpandTensorsTransformation.Expand(tensor);
        TensorType expected = TensorFactory.Parse("(c+r)*(a+b)*f_a*k^a + (c+r)*(a+b)*r_a*k^a + (c+r)*(a+c)*t_a*k^a");
        Assert.True(TensorUtils.Equals(actual, expected));
    }

    [Fact]
    public void ShouldExpandNestedSums()
    {
        TensorType tensor = TensorFactory.Parse("((a+b)*(c+d)*(f_a + (k+i)*r_a) + (a + c)*t_a)*(c+r)*k^a");
        TensorType actual = ExpandTensorsTransformation.Expand(tensor);
        TensorType expected = TensorFactory.Parse("(c+r)*(a+b)*(c+d)*f_a*k^a + (c+r)*(a+b)*(c+d)*(k+i)*r_a*k^a + (c+r)*(a+c)*t_a*k^a");
        Assert.True(TensorUtils.Equals(actual, expected));
    }

    [Fact]
    public void ShouldExpandTwoSumProducts()
    {
        TensorType tensor = TensorFactory.Parse("((a+b)*(c+d)*(f_a + (k+i)*r_a) + (a + c)*t_a)*(c+r)*((a+b)*k^a + (c+d)*t^a)");
        TensorType actual = ExpandTensorsTransformation.Expand(tensor);
        TensorType expected = TensorFactory.Parse("(a+b)*(c+r)*(a+b)*(c+d)*f_a*k^a + (a+b)*(c+r)*(a+b)*(c+d)*(k+i)*r_a*k^a + (a+b)*(c+r)*(a+c)*t_a*k^a + (c+d)*(c+r)*(a+b)*(c+d)*f_a*t^a + (c+d)*(c+r)*(a+b)*(c+d)*(k+i)*r_a*t^a + (c+d)*(c+r)*(a+c)*t_a*t^a");
        Assert.True(TensorUtils.Equals(actual, expected));
    }

    [Fact]
    public void ShouldMatchExpandTransformation()
    {
        TensorType tensor = TensorFactory.Parse("((a+b)*(c+d)*(f_a + (k+i)*t_a) + (a + c)*t_a)*(c+r)*((a+b)*f^a + (c+d)*t^a)");
        TensorType expanded = ExpandTensorsTransformation.Expand(tensor);
        Assert.True(TensorUtils.Equals(
            ExpandTransformation.Expand(tensor),
            ExpandTransformation.Expand(expanded)));
        TensorType expected = TensorFactory.Parse("(c+r)*(c+d)*(a+b)**2*f_a*f^a + (c+r)*((a+b)*(c+d)*(k+i) + (a + c))*t_a*(a+b)*f^a + (c+r)*(a+b)*(c+d)*f_a*(c+d)*t^a + (c+r)*((a+b)*(c+d)*(k+i) + (a + c))*t_a*(c+d)*t^a");
        Assert.True(TensorUtils.Equals(expanded, expected));
    }

    [Fact]
    public void ShouldMatchExpandWithSubstitutions()
    {
        TensorType tensor = TensorFactory.Parse("((a+b)*(c+d)*(f_a + (k+i)*t_a) + (a + c)*t_a)*(c+r)*((a+b)*f^a + (c+d)*t^a)");
        ITransformation[] subs =
        [
            TensorFactory.ParseExpression("f_a*f^a = 1"),
            TensorFactory.ParseExpression("f_a*t^a = 2"),
            TensorFactory.ParseExpression("t_a*t^a = 3")
        ];

        TensorType expanded = ExpandTensorsTransformation.Expand(tensor, subs);
        Assert.True(TensorUtils.Equals(
            ExpandTransformation.Expand(tensor, subs),
            ExpandTransformation.Expand(expanded)));
        TensorType expected = TensorFactory.Parse("(c+r)*(c+d)*(a+b)**2 + (c+r)*((a+b)*(c+d)*(k+i) + (a + c))*(a+b)*2 + (c+r)*(a+b)*(c+d)*(c+d)*2 + (c+r)*((a+b)*(c+d)*(k+i) + (a + c))*(c+d)*3");
        Assert.True(TensorUtils.Equals(expanded, expected));
    }

    [Fact(Skip = "RandomTensor is not yet ported.")]
    public void ShouldMatchExpandOnRandomTensors()
    {
    }

    [Fact(Skip = "RandomTensor is not yet ported.")]
    public void ShouldMatchExpandOnRandomTensorsWithSubstitutions()
    {
    }

    [Fact]
    public void ShouldMatchExpandAfterSubstitutions()
    {
        TensorType tensor = TensorFactory.Parse("(2*(c+a)-164*a)*(f_{a}+t_{a})*f^{a}");
        ITransformation[] subs =
        [
            TensorFactory.ParseExpression("f_a*f^a = a"),
            TensorFactory.ParseExpression("f_a*t^a = b"),
            TensorFactory.ParseExpression("t_a*t^a = c")
        ];
        TensorType expanded = ExpandTensorsTransformation.Expand(tensor, subs);
        Assert.True(TensorUtils.Equals(
            ExpandTransformation.Expand(tensor, subs),
            ExpandTransformation.Expand(expanded)));
    }

    [Fact]
    public void ShouldRemainSymbolicAfterExpansion()
    {
        TensorType tensor = TensorFactory.Parse("-31*(69*c*f_{a}*t^{a}+c*a)*(-7*b*f^{b}*t_{b}+c*f^{b}*f_{b})");
        ITransformation[] subs =
        [
            TensorFactory.ParseExpression("f_a*f^a = a"),
            TensorFactory.ParseExpression("f_a*t^a = b"),
            TensorFactory.ParseExpression("t_a*t^a = c")
        ];
        TensorType expanded = ExpandTensorsTransformation.Expand(tensor, subs);
        Assert.True(TensorUtils.IsSymbolic(expanded));
    }

    [Fact]
    public void ShouldRemainSymbolicAfterNestedExpansion()
    {
        TensorType tensor = TensorFactory.Parse("-2*(b+a)*f^{b}*f^{a}*t_{b}*(-89*a*t_{a}-26*b*f_{a})");
        ITransformation[] subs =
        [
            TensorFactory.ParseExpression("f_a*f^a = a"),
            TensorFactory.ParseExpression("f_a*t^a = b"),
            TensorFactory.ParseExpression("t_a*t^a = c")
        ];
        TensorType expanded = ExpandTensorsTransformation.Expand(tensor, subs);
        Assert.True(TensorUtils.IsSymbolic(expanded));
        TensorType expandedExpected = ExpandTransformation.Expand(
            ExpandTransformation.Expand(
                Transformation.ApplySequentially(tensor, subs), subs));
        Assert.True(TensorUtils.Equals(ExpandTransformation.Expand(expandedExpected), ExpandTransformation.Expand(expanded)));
    }

    [Fact]
    public void ShouldMatchExpandAfterMultipleSubstitutions()
    {
        TensorType tensor = TensorFactory.Parse("-80*(-94*a*b-37*b*f^{c}*f_{c})*(-58*c*f_{a}+t^{d}*f_{d}*f_{a})*(t^{b}*f_{b}*f^{a}+c*a*b*f^{a})");
        ITransformation[] subs =
        [
            TensorFactory.ParseExpression("f_a*f^a = a"),
            TensorFactory.ParseExpression("f_a*t^a = b"),
            TensorFactory.ParseExpression("t_a*t^a = c")
        ];
        TensorType expanded = ExpandTensorsTransformation.Expand(tensor, subs);
        Assert.True(TensorUtils.IsSymbolic(expanded));
        TensorType expected = ExpandTransformation.Expand(
            Transformation.ApplySequentially(
                ExpandTransformation.Expand(
                    Transformation.ApplySequentially(tensor, subs), subs),
                subs));
        Assert.True(TensorUtils.Equals(ExpandTransformation.Expand(expected), ExpandTransformation.Expand(expanded)));
    }

    [Fact]
    public void ShouldExpandRepeatedSums()
    {
        TensorType tensor = TensorFactory.Parse("2*((a+b)*(a_i*a^i + b_i*b^i) + (c+d)*(a_i*a^i + b_i*b^i))*((a+b)*(a_i*a^i + b_i*b^i) + (c+d)*(a_i*a^i + b_i*b^i))");
        TensorType actual = ExpandTensorsTransformation.Expand(tensor);
        TensorType expected = TensorFactory.Parse("4*(d+b+c+a)**2*a_{i}*a^{i}*b_{a}*b^{a}+2*(d+b+c+a)**2*a_{i}*a^{i}*a_{a}*a^{a}+2*(d+b+c+a)**2*b_{i}*b^{i}*b_{a}*b^{a}");
        Assert.True(TensorUtils.Equals(actual, expected));
    }

    [Fact(Skip = "Performance test skipped.")]
    public void ShouldBenchmarkExpansion()
    {
    }
}
