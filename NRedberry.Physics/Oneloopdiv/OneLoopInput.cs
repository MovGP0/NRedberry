using NRedberry.Core.Tensors;
using NRedberry.Core.Transformations.Symmetrization;

namespace NRedberry.Physics.Oneloopdiv;

/// <summary>
/// Skeleton port of cc.redberry.physics.oneloopdiv.OneLoopInput.
/// </summary>
public sealed class OneLoopInput
{
    private readonly Expression[] inputValues = null!;
    private readonly int operatorOrder;
    private readonly int matrixIndicesCount;
    private readonly Expression[][] hatQuantities = null!;
    private readonly Expression[] kn = null!;
    private readonly Expression l = null!;
    private const int HatQuantitiesGeneralCount = 5;
    private const int InputValuesGeneralCount = 6;
    private readonly int actualInput;
    private readonly int actualHatQuantities;
    private readonly ITransformation[] riemannBackground = null!;
    private readonly Expression f = null!;
    private readonly Expression hatF = null!;

    public OneLoopInput(int operatorOrder, Expression iK, Expression k, Expression s, Expression w, Expression n, Expression m, Expression f)
    {
        throw new NotImplementedException();
    }

    public OneLoopInput(int operatorOrder, Expression iK, Expression k, Expression s, Expression w, Expression n, Expression m, Expression f, ITransformation[] riemannBackground)
    {
        throw new NotImplementedException();
    }

    public Expression GetInputParameter(int index)
    {
        throw new NotImplementedException();
    }

    public Expression[] GetHatQuantities(int index)
    {
        throw new NotImplementedException();
    }

    internal Expression[][] GetHatQuantities()
    {
        throw new NotImplementedException();
    }

    public Expression[] GetKnQuantities()
    {
        throw new NotImplementedException();
    }

    public Expression GetHatF()
    {
        throw new NotImplementedException();
    }

    public Expression GetF()
    {
        throw new NotImplementedException();
    }

    public Expression[] GetNablaS()
    {
        throw new NotImplementedException();
    }

    public Expression GetL()
    {
        throw new NotImplementedException();
    }

    public int GetMatrixIndicesCount()
    {
        throw new NotImplementedException();
    }

    public int GetOperatorOrder()
    {
        throw new NotImplementedException();
    }

    public ITransformation[] GetRiemannBackground()
    {
        throw new NotImplementedException();
    }

    private string GetStringInputName(int index)
    {
        throw new NotImplementedException();
    }

    private string GetStringHatQuantityName(int index)
    {
        throw new NotImplementedException();
    }

    private void CheckConsistency()
    {
        throw new NotImplementedException();
    }
}
