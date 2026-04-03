using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using NRedberry.Tensors;
using NRedberry.Transformations.Symmetrization;
using TensorFactory = NRedberry.Tensors.Tensors;
using TensorType = NRedberry.Tensors.Tensor;

namespace NRedberry.Core.Tests.Extensions;

public static class ShouldlyExtensions
{
    public static void ShouldContractEquals(this string input, string expected)
    {
        TensorType actual = Contract(input);
        TensorType expectedTensor = TensorFactory.Parse(expected);
        TensorUtils.Equals(actual, expectedTensor).ShouldBeTrue();
    }

    public static void ShouldContractEqualsExactly(this string input, string expected)
    {
        TensorType actual = Contract(input);
        TensorType expectedTensor = TensorFactory.Parse(expected);
        TensorUtils.EqualsExactly(actual, expectedTensor).ShouldBeTrue();
    }

    private static TensorType Contract(string tensor)
    {
        return Contract(TensorFactory.Parse(tensor));
    }

    private static TensorType Contract(TensorType tensor)
    {
        return EliminateMetricsTransformation.Eliminate(tensor);
    }

    extension(Should)
    {
        public static void CompleteIn(Action action, string? customMessage = null)
        {
            Should.CompleteIn(action, TimeSpan.FromSeconds(1), customMessage);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SatisfyAllConditions([InstantHandle] params Action[] conditions)
        {
            "Should".ShouldSatisfyAllConditions(conditions);
        }
    }
}
