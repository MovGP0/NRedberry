using System;
using NRedberry.Physics.Feyncalc;
using TensorFactory = NRedberry.Tensors.Tensors;

namespace NRedberry.Physics.Tests.Feyncalc;

public sealed class UnitarySimplifyTransformationTest
{
    public void Test1()
    {
        // TODO: GeneralIndicesInsertion is not yet ported; insertion rules skipped.
        // TODO: setAntiSymmetric/setSymmetric are not yet ported.

        UnitarySimplifyTransformation tr = new(
            TensorFactory.ParseSimple("T_A"),
            TensorFactory.ParseSimple("f_ABC"),
            TensorFactory.ParseSimple("d_ABC"),
            TensorFactory.ParseSimple("n"));

        Console.WriteLine(tr.Transform(TensorFactory.Parse("T_A*T^A")));
    }
}
