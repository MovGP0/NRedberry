namespace NRedberry.Core.Tensors.Iterators;

public interface ITreeIterator
{
    /// <summary>
    /// Returns the next tensor in tree or null if iteration is finished
    /// </summary>
    /// <returns>
    /// the next tensor in tree or null if iteration is finished
    /// </returns>
    Tensor Next();

    /// <summary>
    /// Set current tensor in tree.
    /// </summary>
    /// <param name="tensor">tensor</param>
    void Set(Tensor tensor);

    /// <summary>
    /// Returns the result after iteration finished (or stopped). After this step the
    /// iterator becomes broken.
    /// </summary>
    /// <returns>
    /// the result after iteration finished (or stopped)
    /// </returns>
    Tensor Result();

    /// <summary>
    /// Returns the current depth in the tree.
    /// </summary>
    /// <returns>
    /// current depth in the tree
    /// </returns>
    int Depth { get; }
}
