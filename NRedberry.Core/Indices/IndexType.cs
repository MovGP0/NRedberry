namespace NRedberry.Core.Indices
{
    /// <summary>
    /// This {@code enum} is a container of the information on all available index types and appropriate converters. This
    /// {@code enum} is scanning at the initialization of {@link ContextSettings} and all the values are putting in the
    /// Context as default indices types.
    /// </summary>
    public enum IndexType
    {
        LatinLower,
        LatinUpper,
        GreekLower,
        GreekUpper,
        Matrix1,
        Matrix2,
        Matrix3,
        Matrix4
    }
}
