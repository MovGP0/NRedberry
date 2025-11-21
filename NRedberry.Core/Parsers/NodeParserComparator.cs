namespace NRedberry.Parsers;

/*
 * Original: ./core/src/main/java/cc/redberry/core/parser/NodeParserComparator.java
 */

internal sealed class NodeParserComparator : IComparer<ITokenParser>
{
    public static NodeParserComparator Instance { get; } = new();

    private NodeParserComparator()
    {
    }

    public int Compare(ITokenParser? x, ITokenParser? y)
    {
        throw new NotImplementedException();
    }
}
