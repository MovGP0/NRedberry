namespace NRedberry.Core.Tensors.Iterators;

public enum TraversePermission
{
    /// <summary>
    /// Allows iterator to enter the tensor
    /// </summary>
    Enter,
    /// <summary>
    /// Does not allows iterator to enter the tensor, but bind iterator to show
    /// it
    /// </summary>
    ShowButNotEnter,
    /// <summary>
    /// Do not show and do not enter the tensor
    /// </summary>
    DontShow
}
