namespace NRedberry.Tensors.Iterators;

public interface TraverseGuide
{
    TraversePermission GetPermission(Tensor tensor, Tensor parent, int indexInParent);

    public static readonly TraverseGuide All = new AllTraverseGuide();
    public static readonly TraverseGuide ExceptFunctionsAndFields = new ExceptFunctionsAndFieldsTraverseGuide();
    public static readonly TraverseGuide ExceptFields = new ExceptFieldsTraverseGuide();
}
