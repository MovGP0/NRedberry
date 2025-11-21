namespace NRedberry.Tensors.Iterators;

public class AllTraverseGuide : TraverseGuide
{
    public TraversePermission GetPermission(Tensor tensor, Tensor parent, int indexInParent)
    {
        return TraversePermission.Enter;
    }
}
