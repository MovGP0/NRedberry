using NRedberry.Core.Tensors.Functions;

namespace NRedberry.Core.Tensors.Iterators;

public class ExceptFunctionsAndFieldsTraverseGuide : TraverseGuide
{
    public TraversePermission GetPermission(Tensor tensor, Tensor parent, int indexInParent)
    {
        if (tensor is ScalarFunction)
            return TraversePermission.DontShow;

        if (tensor is TensorField)
            return TraversePermission.ShowButNotEnter;

        return TraversePermission.Enter;
    }
}
