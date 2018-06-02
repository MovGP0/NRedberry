namespace NRedberry.Core.Contexts.Defaults
{
    /// <summary>
    /// A factory interface for <see cref="Context"/> creation.
    /// </summary>
    public interface IContextFactory
    {
        /// <summary>
        /// Creates a context object.
        /// </summary>
        /// <returns>context</returns>
        Context CreateContext();
    }
}