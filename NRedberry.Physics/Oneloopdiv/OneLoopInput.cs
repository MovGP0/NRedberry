using System.Text;
using NRedberry;
using NRedberry.Contexts;
using NRedberry.Core.Utils;
using NRedberry.Indices;
using NRedberry.Numbers;
using NRedberry.Tensors;
using NRedberry.Tensors.Iterators;
using NRedberry.Transformations.Expand;
using NRedberry.Transformations.Symmetrization;
using TensorCC = NRedberry.Tensors.CC;
using TensorFactory = NRedberry.Tensors.Tensors;

namespace NRedberry.Physics.Oneloopdiv;

/// <summary>
/// Port of cc.redberry.physics.oneloopdiv.OneLoopInput.
/// </summary>
public sealed class OneLoopInput
{
    private const int HatQuantitiesGeneralCount = 5;
    private const int InputValuesGeneralCount = 6;

    private readonly Expression[] _inputValues;
    private readonly int _operatorOrder;
    private readonly int _matrixIndicesCount;
    private readonly Expression[][] _hatQuantities;
    private readonly Expression[] _kn;
    private readonly Expression _l;
    private readonly int _actualInput;
    private readonly int _actualHatQuantities;
    private readonly ITransformation[] _riemannBackground;
    private readonly Expression _f;
    private readonly Expression _hatF;

    public OneLoopInput(
        int operatorOrder,
        Expression iK,
        Expression k,
        Expression s,
        Expression w,
        Expression n,
        Expression m,
        Expression f)
        : this(operatorOrder, iK, k, s, w, n, m, f, Array.Empty<ITransformation>())
    {
    }

    public OneLoopInput(
        int operatorOrder,
        Expression iK,
        Expression k,
        Expression s,
        Expression w,
        Expression n,
        Expression m,
        Expression f,
        ITransformation[] riemannBackground)
    {
        _operatorOrder = operatorOrder;
        if (operatorOrder != 2 && operatorOrder != 4)
        {
            throw new ArgumentException("Operator order must be 2 or 4.", nameof(operatorOrder));
        }

        _riemannBackground = riemannBackground ?? Array.Empty<ITransformation>();
        _actualInput = operatorOrder + 2;
        _actualHatQuantities = operatorOrder + 1;

        _inputValues = new Expression[InputValuesGeneralCount];
        _inputValues[0] = iK;
        _inputValues[1] = k;
        _inputValues[2] = s;
        _inputValues[3] = w;
        _inputValues[4] = n;
        _inputValues[5] = m;

        CheckConsistency();

        if (TensorFactory.ParseSimple("R_lmab").SimpleIndices.Symmetries.AvailableForModification())
        {
            AddSymmetry("R_lmab", IndexType.LatinLower, true, 0, 1, 3, 2);
            AddSymmetry("R_lmab", IndexType.LatinLower, false, 2, 3, 0, 1);
        }

        if (TensorFactory.ParseSimple("R_lm").SimpleIndices.Symmetries.AvailableForModification())
        {
            AddSymmetry("R_lm", IndexType.LatinLower, false, 1, 0);
        }

        _l = TensorFactory.Expression(TensorFactory.Parse("L"), new Complex(operatorOrder));
        _hatQuantities = new Expression[HatQuantitiesGeneralCount][];
        _matrixIndicesCount = _inputValues[1][0].Indices.Size() - operatorOrder;

        int[] covariantIndices = new int[operatorOrder];
        int i;
        int j;
        int kIndex;
        for (i = 0; i < operatorOrder; ++i)
        {
            covariantIndices[i] = IndicesUtils.CreateIndex(i, IndexType.LatinLower, true);
        }

        int[] upper = new int[_matrixIndicesCount / 2];
        int[] lower = (int[])upper.Clone();
        for (; i < operatorOrder + _matrixIndicesCount / 2; ++i)
        {
            int index = i - operatorOrder;
            upper[index] = IndicesUtils.CreateIndex(i, IndexType.LatinLower, true);
            lower[index] = IndicesUtils.CreateIndex(i + _matrixIndicesCount / 2, IndexType.LatinLower, false);
        }

        string matrixIndicesString = IndicesUtils.ToString(ArraysUtils.AddAll(upper, lower), OutputFormat.Redberry);

        ITransformation n2 = new SqrSubs(TensorFactory.ParseSimple("n_l"));
        ITransformation n2Transformer = new Transformer(TraverseState.Leaving, new[] { n2 });
        ITransformation[] transformations = new ITransformation[_riemannBackground.Length + 2];
        transformations[0] = EliminateMetricsTransformation.Instance;
        transformations[1] = n2Transformer;
        Array.Copy(_riemannBackground, 0, transformations, 2, _riemannBackground.Length);

        StringBuilder sb;
        Tensor temp;
        string covariantIndicesString;
        for (i = 0; i < _actualHatQuantities; ++i)
        {
            _hatQuantities[i] = new Expression[_operatorOrder + 1 - i];
            covariantIndicesString = IndicesUtils.ToString(
                CopyRange(covariantIndices, 0, covariantIndices.Length - i),
                OutputFormat.Redberry);
            for (j = 0; j < _operatorOrder + 1 - i; ++j)
            {
                sb = new StringBuilder();
                sb.Append(GetStringHatQuantityName(i))
                    .Append(IndicesUtils.ToString(
                        CopyRange(covariantIndices, j, covariantIndices.Length - i - j),
                        OutputFormat.Redberry))
                    .Append(matrixIndicesString)
                    .Append("=iK")
                    .Append(matrixIndicesString)
                    .Append('*')
                    .Append(GetStringInputName(1 + i))
                    .Append(covariantIndicesString)
                    .Append(matrixIndicesString);
                for (kIndex = 0; kIndex < j; ++kIndex)
                {
                    sb.Append("*n")
                        .Append(IndicesUtils.ToString(
                            IndicesUtils.InverseIndexState(covariantIndices[kIndex]),
                            OutputFormat.Redberry));
                }

                temp = TensorFactory.Parse(sb.ToString());
                temp = _inputValues[0].Transform(temp);
                temp = _inputValues[i + 1].Transform(temp);
                temp = ExpandTransformation.Expand(temp, transformations);
                foreach (ITransformation transformation in transformations)
                {
                    temp = transformation.Transform(temp);
                }

                _hatQuantities[i][j] = (Expression)temp;
            }
        }

        for (; i < HatQuantitiesGeneralCount; ++i)
        {
            _hatQuantities[i] = new Expression[1];
            sb = new StringBuilder();
            sb.Append(GetStringHatQuantityName(i)).Append("=0");
            _hatQuantities[i][0] = (Expression)TensorFactory.Parse(sb.ToString());
        }

        _kn = new Expression[_operatorOrder + 1];
        covariantIndicesString = IndicesUtils.ToString(covariantIndices, OutputFormat.Redberry);
        string matrixIndices = IndicesUtils.ToString(ArraysUtils.AddAll(upper, lower), OutputFormat.Redberry);
        for (i = 0; i < _operatorOrder + 1; ++i)
        {
            sb = new StringBuilder();
            sb.Append("Kn")
                .Append(IndicesUtils.ToString(
                    CopyRange(covariantIndices, i, covariantIndices.Length - i),
                    OutputFormat.Redberry))
                .Append(matrixIndices)
                .Append("=K")
                .Append(covariantIndicesString)
                .Append(matrixIndices);
            for (kIndex = 0; kIndex < i; ++kIndex)
            {
                sb.Append("*n")
                    .Append(IndicesUtils.ToString(
                        IndicesUtils.InverseIndexState(covariantIndices[kIndex]),
                        OutputFormat.Redberry));
            }

            temp = TensorFactory.Parse(sb.ToString());
            temp = _inputValues[0].Transform(temp);
            temp = _inputValues[1].Transform(temp);
            temp = ExpandTransformation.Expand(temp, transformations);
            foreach (ITransformation transformation in transformations)
            {
                temp = transformation.Transform(temp);
            }

            _kn[i] = (Expression)temp;
        }

        int[] symmetry = new int[f[0].Indices.Size()];
        symmetry[0] = 1;
        symmetry[1] = 0;
        for (i = 2; i < symmetry.Length; ++i)
        {
            symmetry[i] = i;
        }

        if (f[0] is SimpleTensor fTensor
            && fTensor.SimpleIndices.Symmetries.AvailableForModification())
        {
            fTensor.SimpleIndices.Symmetries.Add(IndexType.LatinLower, true, symmetry);
        }

        _f = f;

        covariantIndicesString = IndicesUtils.ToString(CopyRange(covariantIndices, 0, 2), OutputFormat.Redberry);
        sb = new StringBuilder();
        sb.Append("HATF")
            .Append(covariantIndicesString)
            .Append(matrixIndicesString)
            .Append("=iK")
            .Append(matrixIndicesString)
            .Append("*F")
            .Append(covariantIndicesString)
            .Append(matrixIndicesString);
        Tensor hatF = TensorFactory.Parse(sb.ToString());
        hatF = _f.Transform(hatF);
        hatF = _inputValues[0].Transform(hatF);
        _hatF = (Expression)hatF;
    }

    public Expression GetInputParameter(int index)
    {
        return _inputValues[index];
    }

    public Expression[] GetHatQuantities(int index)
    {
        return (Expression[])_hatQuantities[index].Clone();
    }

    internal Expression[][] GetHatQuantities()
    {
        return _hatQuantities;
    }

    public Expression[] GetKnQuantities()
    {
        return (Expression[])_kn.Clone();
    }

    public Expression GetHatF()
    {
        return _hatF;
    }

    public Expression GetF()
    {
        return _f;
    }

    public Expression[] GetNablaS()
    {
        if (_operatorOrder < 1)
        {
            return Array.Empty<Expression>();
        }

        Expression[] nablaS = new Expression[GetHatQuantities(1).Length];
        for (int i = 0; i < nablaS.Length; ++i)
        {
            string indices = GetHatQuantities(1)[i][0].Indices.ToString(OutputFormat.Redberry);
            StringBuilder sb = new StringBuilder()
                .Append("NABLAS_{l_{9}}")
                .Append(indices)
                .Append("=0");
            nablaS[i] = (Expression)TensorFactory.Parse(sb.ToString());
        }

        return nablaS;
    }

    public Expression GetL()
    {
        return _l;
    }

    public int GetMatrixIndicesCount()
    {
        return _matrixIndicesCount;
    }

    public int GetOperatorOrder()
    {
        return _operatorOrder;
    }

    public ITransformation[] GetRiemannBackground()
    {
        return _riemannBackground;
    }

    private static void AddSymmetry(string tensor, IndexType type, bool sign, params int[] permutation)
    {
        var simple = TensorFactory.ParseSimple(tensor);
        simple.SimpleIndices.Symmetries.Add(type, sign, permutation);
    }

    private string GetStringInputName(int index)
    {
        return index switch
        {
            0 => "iK",
            1 => "K",
            2 => "S",
            3 => "W",
            4 => "N",
            5 => "M",
            _ => throw new ArgumentException()
        };
    }

    private string GetStringHatQuantityName(int index)
    {
        return index switch
        {
            0 => "HATK",
            1 => "HATS",
            2 => "HATW",
            3 => "HATN",
            4 => "HATM",
            _ => throw new ArgumentException()
        };
    }

    private void CheckConsistency()
    {
        int i;
        for (i = 0; i < _actualInput; ++i)
        {
            if (_inputValues[i] is null || _inputValues[i][0] is not SimpleTensor st)
            {
                throw new ArgumentException();
            }

            NameDescriptor nd = TensorCC.GetNameDescriptor(st.Name);
            if (!string.Equals(nd.GetName(null, OutputFormat.Redberry), GetStringInputName(i), StringComparison.Ordinal))
            {
                throw new ArgumentException();
            }
        }

        for (; i < InputValuesGeneralCount; ++i)
        {
            if (_inputValues[i] is not null)
            {
                throw new ArgumentException();
            }
        }

        SimpleIndices indices = (SimpleIndices)_inputValues[1][0].Indices;
        StructureOfIndices structureOfIndices = indices.StructureOfIndices;
        if (structureOfIndices.GetTypeData(IndexType.LatinLower.GetType_()).Length != structureOfIndices.Size)
        {
            throw new ArgumentException("Only Latin lower indices are legal.");
        }

        int matrixIndicesCount = indices.Size() - _operatorOrder;
        if (matrixIndicesCount % 2 != 0)
        {
            throw new ArgumentException();
        }

        if (_inputValues[0][0].Indices.Size() != matrixIndicesCount)
        {
            throw new ArgumentException();
        }

        for (i = 1; i < _actualInput; ++i)
        {
            structureOfIndices = ((SimpleIndices)_inputValues[i][0].Indices).StructureOfIndices;
            if (structureOfIndices.GetTypeData(IndexType.LatinLower.GetType_()).Length != structureOfIndices.Size)
            {
                throw new ArgumentException("Only Latin lower indices are legal.");
            }

            if (structureOfIndices.Size + i - 1 != _operatorOrder + matrixIndicesCount)
            {
                throw new ArgumentException();
            }
        }
    }

    private static int[] CopyRange(int[] source, int start, int length)
    {
        int[] result = new int[length];
        Array.Copy(source, start, result, 0, length);
        return result;
    }
}
