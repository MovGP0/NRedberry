namespace NRedberry.Tensors.Iterators;

public class ExceptFieldsTraverseGuide : TraverseGuide
{
    public TraversePermission GetPermission(Tensor tensor, Tensor parent, int indexInParent)
    {
        if (tensor is TensorField)
            return TraversePermission.ShowButNotEnter;

        return TraversePermission.Enter;
    }
}
