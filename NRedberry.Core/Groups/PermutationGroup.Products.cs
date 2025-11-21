namespace NRedberry.Groups;

public sealed partial class PermutationGroup
{
    public PermutationGroup DirectProduct(PermutationGroup group)
    {
        return CreatePermutationGroupFromBSGS(AlgorithmsBase.DirectProduct(GetBSGS(), group.GetBSGS()));
    }
}
