using NRedberry.Core.Tensors.Functions;

namespace NRedberry.Core.Tensors.Iterators;

public interface TraverseGuide
{
    TraversePermission GetPermission(Tensor tensor, Tensor parent, int indexInParent);

    public static readonly TraverseGuide All = new AllTraverseGuide();
    public static readonly TraverseGuide ExceptFunctionsAndFields = new ExceptFunctionsAndFieldsTraverseGuide();
    public static readonly TraverseGuide ExceptFields = new ExceptFieldsTraverseGuide();
}

public class AllTraverseGuide : TraverseGuide
{
    public TraversePermission GetPermission(Tensor tensor, Tensor parent, int indexInParent)
    {
        return TraversePermission.Enter;
    }
}

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

public class ExceptFieldsTraverseGuide : TraverseGuide
{
    public TraversePermission GetPermission(Tensor tensor, Tensor parent, int indexInParent)
    {
        if (tensor is TensorField)
            return TraversePermission.ShowButNotEnter;

        return TraversePermission.Enter;
    }
}