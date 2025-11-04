namespace NRedberry.Core.Parsers;

public interface ITokenParser
{
    /// <summary>
    /// Parses the specified string expression and returns the corresponding AST node or {@code null},
    /// string can not be parsed by this parser.
    /// </summary>
    /// <param name="expression">string expression</param>
    /// <param name="parser">recursion</param>
    /// <returns>AST node</returns>
    ParseToken? ParseToken(string expression, Parser parser);

    /// <summary>
    /// The higher the priority, the earlier runs.
    /// </summary>
    int Priority { get; }
}
