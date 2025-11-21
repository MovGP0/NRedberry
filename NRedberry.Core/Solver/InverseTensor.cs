using NRedberry.Tensors;
using NRedberry.Transformations.Symmetrization;

namespace NRedberry.Solver;

/*
 * Original: ./core/src/main/java/cc/redberry/core/solver/InverseTensor.java
 */

public sealed class InverseTensor
{
    public InverseTensor(Expression toInverse, Expression equation, Tensor[] samples)
    {
        throw new NotImplementedException();
    }

    public InverseTensor(Expression toInverse, Expression equation, Tensor[] samples, bool symmetricForm, ITransformation[] transformations)
    {
        throw new NotImplementedException();
    }

    public Expression[] GetEquations()
    {
        throw new NotImplementedException();
    }

    public Expression GetGeneralInverseForm()
    {
        throw new NotImplementedException();
    }

    public SimpleTensor[] GetUnknownCoefficients()
    {
        throw new NotImplementedException();
    }

    public ReducedSystem ToReducedSystem()
    {
        throw new NotImplementedException();
    }

    public static Expression FindInverseWithMaple(Expression toInverse, Expression equation, Tensor[] samples, bool symmetricForm, ITransformation[] transformations, string mapleBinDir, string path)
    {
        throw new NotImplementedException();
    }

    public static Expression FindInverseWithMaple(Expression toInverse, Expression equation, Tensor[] samples, bool symmetricForm, bool keepFreeParameters, ITransformation[] transformations, string mapleBinDir, string path)
    {
        throw new NotImplementedException();
    }

    public static Expression FindInverseWithMathematica(Expression toInverse, Expression equation, Tensor[] samples, bool symmetricForm, ITransformation[] transformations, string mathematicaBinDir, string path)
    {
        throw new NotImplementedException();
    }

    public static Expression FindInverseWithMathematica(Expression toInverse, Expression equation, Tensor[] samples, bool symmetricForm, bool keepFreeParameters, ITransformation[] transformations, string mathematicaBinDir, string path)
    {
        throw new NotImplementedException();
    }
}
