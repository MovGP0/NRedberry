using NRedberry.IndexMapping;
using NRedberry.Indices;
using NRedberry.Numbers;
using NRedberry.Tensors;
using TensorApi = NRedberry.Tensors.Tensors;

namespace NRedberry.Physics.Feyncalc;

/// <summary>
/// Port of cc.redberry.physics.feyncalc.FeynCalcUtils.
/// </summary>
public static class FeynCalcUtils
{
    public static Expression[] SetMandelstam(string[][] momentums)
    {
        ArgumentNullException.ThrowIfNull(momentums);
        return SetMandelstam(Parse2(momentums));
    }

    public static Expression[] SetMandelstam(Tensor[][] momentums)
    {
        ArgumentNullException.ThrowIfNull(momentums);
        return SetMandelstam(
            momentums,
            TensorApi.Parse("s"),
            TensorApi.Parse("t"),
            TensorApi.Parse("u"));
    }

    public static Expression[] SetMandelstam(Tensor[][] momentums, Tensor s, Tensor t, Tensor u)
    {
        ArgumentNullException.ThrowIfNull(momentums);
        ArgumentNullException.ThrowIfNull(s);
        ArgumentNullException.ThrowIfNull(t);
        ArgumentNullException.ThrowIfNull(u);

        CheckMandelstamInput(momentums, 4);
        if (s.Indices.GetFree().Size() != 0
            || t.Indices.GetFree().Size() != 0
            || u.Indices.GetFree().Size() != 0)
        {
            throw new ArgumentException("Mandelstam variables should be scalar.");
        }

        Expression[] result = new Expression[10];
        int i;
        for (i = 0; i < 4; ++i)
        {
            result[i] = TensorApi.Expression(Square(momentums[i][0]), TensorApi.Pow(momentums[i][1], 2));
        }

        result[i++] = TensorApi.Expression(
            TensorApi.Multiply(Complex.Two, Contract(momentums[0][0], momentums[1][0])),
            TensorApi.Sum(
                s,
                TensorApi.Negate(TensorApi.Sum(
                    TensorApi.Pow(momentums[0][1], 2),
                    TensorApi.Pow(momentums[1][1], 2)))));
        result[i++] = TensorApi.Expression(
            TensorApi.Multiply(Complex.Two, Contract(momentums[2][0], momentums[3][0])),
            TensorApi.Sum(
                s,
                TensorApi.Negate(TensorApi.Sum(
                    TensorApi.Pow(momentums[2][1], 2),
                    TensorApi.Pow(momentums[3][1], 2)))));

        result[i++] = TensorApi.Expression(
            TensorApi.Multiply(Complex.MinusTwo, Contract(momentums[0][0], momentums[2][0])),
            TensorApi.Sum(
                t,
                TensorApi.Negate(TensorApi.Sum(
                    TensorApi.Pow(momentums[0][1], 2),
                    TensorApi.Pow(momentums[2][1], 2)))));
        result[i++] = TensorApi.Expression(
            TensorApi.Multiply(Complex.MinusTwo, Contract(momentums[1][0], momentums[3][0])),
            TensorApi.Sum(
                t,
                TensorApi.Negate(TensorApi.Sum(
                    TensorApi.Pow(momentums[1][1], 2),
                    TensorApi.Pow(momentums[3][1], 2)))));

        result[i++] = TensorApi.Expression(
            TensorApi.Multiply(Complex.MinusTwo, Contract(momentums[0][0], momentums[3][0])),
            TensorApi.Sum(
                u,
                TensorApi.Negate(TensorApi.Sum(
                    TensorApi.Pow(momentums[0][1], 2),
                    TensorApi.Pow(momentums[3][1], 2)))));
        result[i] = TensorApi.Expression(
            TensorApi.Multiply(Complex.MinusTwo, Contract(momentums[1][0], momentums[2][0])),
            TensorApi.Sum(
                u,
                TensorApi.Negate(TensorApi.Sum(
                    TensorApi.Pow(momentums[1][1], 2),
                    TensorApi.Pow(momentums[2][1], 2)))));

        return result;
    }

    public static Expression[] SetMandelstam5(string[][] momentums)
    {
        ArgumentNullException.ThrowIfNull(momentums);
        return SetMandelstam5(Parse2(momentums));
    }

    public static Expression[] SetMandelstam5(Tensor[][] momentums)
    {
        ArgumentNullException.ThrowIfNull(momentums);
        return SetMandelstam5(
            momentums,
            TensorApi.Parse("s"),
            TensorApi.Parse("t1"),
            TensorApi.Parse("t2"),
            TensorApi.Parse("u1"),
            TensorApi.Parse("u2"));
    }

    public static Expression[] SetMandelstam5(Tensor[][] momentums, Tensor s, Tensor t1, Tensor t2, Tensor u1, Tensor u2)
    {
        ArgumentNullException.ThrowIfNull(momentums);
        ArgumentNullException.ThrowIfNull(s);
        ArgumentNullException.ThrowIfNull(t1);
        ArgumentNullException.ThrowIfNull(t2);
        ArgumentNullException.ThrowIfNull(u1);
        ArgumentNullException.ThrowIfNull(u2);

        CheckMandelstamInput(momentums, 5);
        if (s.Indices.GetFree().Size() != 0
            || t1.Indices.GetFree().Size() != 0
            || t2.Indices.GetFree().Size() != 0
            || u1.Indices.GetFree().Size() != 0
            || u2.Indices.GetFree().Size() != 0)
        {
            throw new ArgumentException("Mandelstam variables should be scalar.");
        }

        Expression[] result = new Expression[15];
        int i;
        for (i = 0; i < 5; ++i)
        {
            result[i] = TensorApi.Expression(Square(momentums[i][0]), TensorApi.Pow(momentums[i][1], 2));
        }

        Tensor m1Squared = TensorApi.Pow(momentums[0][1], 2);
        Tensor m2Squared = TensorApi.Pow(momentums[1][1], 2);
        Tensor m3Squared = TensorApi.Pow(momentums[2][1], 2);
        Tensor m4Squared = TensorApi.Pow(momentums[3][1], 2);
        Tensor m5Squared = TensorApi.Pow(momentums[4][1], 2);

        result[i++] = TensorApi.Expression(
            Contract(momentums[0][0], momentums[1][0]),
            Half(TensorApi.Sum(TensorApi.Negate(m1Squared), TensorApi.Negate(m2Squared), s)));
        result[i++] = TensorApi.Expression(
            Contract(momentums[2][0], momentums[0][0]),
            Half(TensorApi.Sum(m1Squared, m3Squared, TensorApi.Negate(t1))));
        result[i++] = TensorApi.Expression(
            Contract(momentums[3][0], momentums[0][0]),
            Half(TensorApi.Sum(m1Squared, TensorApi.Negate(t2), m4Squared)));
        result[i++] = TensorApi.Expression(
            Contract(momentums[4][0], momentums[0][0]),
            Half(TensorApi.Sum(TensorApi.Negate(m1Squared), TensorApi.Negate(m2Squared), TensorApi.Negate(m3Squared), s, t2, TensorApi.Negate(m4Squared), t1)));
        result[i++] = TensorApi.Expression(
            Contract(momentums[2][0], momentums[1][0]),
            Half(TensorApi.Sum(TensorApi.Negate(u1), m2Squared, m3Squared)));
        result[i++] = TensorApi.Expression(
            Contract(momentums[3][0], momentums[1][0]),
            Half(TensorApi.Sum(TensorApi.Negate(u2), m2Squared, m4Squared)));
        result[i++] = TensorApi.Expression(
            Contract(momentums[4][0], momentums[1][0]),
            Half(TensorApi.Sum(u1, TensorApi.Negate(m1Squared), u2, TensorApi.Negate(m2Squared), TensorApi.Negate(m3Squared), s, TensorApi.Negate(m4Squared))));
        result[i++] = TensorApi.Expression(
            Contract(momentums[2][0], momentums[3][0]),
            Half(TensorApi.Sum(TensorApi.Negate(u1), TensorApi.Negate(u2), TensorApi.Multiply(Complex.Two, m1Squared), m3Squared, TensorApi.Negate(s), TensorApi.Negate(t2), m4Squared, TensorApi.Multiply(Complex.Two, m2Squared), TensorApi.Negate(t1), m5Squared)));
        result[i++] = TensorApi.Expression(
            Contract(momentums[2][0], momentums[4][0]),
            Half(TensorApi.Sum(TensorApi.Negate(m1Squared), u2, TensorApi.Negate(m2Squared), TensorApi.Negate(m3Squared), s, t2, TensorApi.Negate(m4Squared), TensorApi.Negate(m5Squared))));
        result[i] = TensorApi.Expression(
            Contract(momentums[4][0], momentums[3][0]),
            Half(TensorApi.Sum(u1, TensorApi.Negate(m1Squared), TensorApi.Negate(m2Squared), TensorApi.Negate(m3Squared), s, TensorApi.Negate(m4Squared), t1, TensorApi.Negate(m5Squared))));

        return result;
    }

    private static void CheckMandelstamInput(Tensor[][] momentums, int expected)
    {
        ArgumentNullException.ThrowIfNull(momentums);

        if (momentums.Length != expected)
        {
            throw new ArgumentException("Unexpected number of momentum entries.", nameof(momentums));
        }

        for (int i = 0; i < expected; ++i)
        {
            Tensor[] pair = momentums[i];
            if (pair.Length != 2)
            {
                throw new ArgumentException("Each momentum entry must contain a momentum and a mass.", nameof(momentums));
            }

            if (pair[0].Indices.Size() != 1)
            {
                throw new ArgumentException("Momentum tensors must have one free index.", nameof(momentums));
            }

            if (pair[1].Indices.Size() != 0)
            {
                throw new ArgumentException("Mass tensors must be scalar.", nameof(momentums));
            }
        }
    }

    private static Tensor[][] Parse2(string[][] momentums)
    {
        ArgumentNullException.ThrowIfNull(momentums);

        Tensor[][] parsed = new Tensor[momentums.Length][];
        for (int i = 0; i < parsed.Length; ++i)
        {
            ArgumentNullException.ThrowIfNull(momentums[i]);
            parsed[i] = new Tensor[momentums[i].Length];
            for (int j = 0; j < parsed[i].Length; ++j)
            {
                parsed[i][j] = TensorApi.Parse(momentums[i][j]);
            }
        }

        return parsed;
    }

    private static Tensor Contract(Tensor a, Tensor b)
    {
        ArgumentNullException.ThrowIfNull(a);
        ArgumentNullException.ThrowIfNull(b);
        return TensorApi.MultiplyAndRenameConflictingDummies(a, InvertIndices(b));
    }

    private static Tensor Square(Tensor tensor)
    {
        ArgumentNullException.ThrowIfNull(tensor);
        return TensorApi.MultiplyAndRenameConflictingDummies(tensor, InvertIndices(tensor));
    }

    private static Tensor InvertIndices(Tensor tensor)
    {
        ArgumentNullException.ThrowIfNull(tensor);

        Indices.Indices free = tensor.Indices.GetFree();
        Mapping mapping = new(free.AllIndices.ToArray(), free.GetInverted().AllIndices.ToArray());
        return ApplyIndexMapping.Apply(tensor, mapping);
    }

    private static Tensor Half(Tensor tensor)
    {
        ArgumentNullException.ThrowIfNull(tensor);
        return TensorApi.Multiply(Complex.OneHalf, tensor);
    }
}
