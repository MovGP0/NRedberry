using System.Text.RegularExpressions;

namespace NRedberry.Parsers;

/// <summary>
/// Parser of mathematical expressions.
/// </summary>
public sealed class Parser
{
    /// <summary>
    /// Default parser.
    /// </summary>
    private static readonly ITokenParser[] DefaultTokenParsers =
    [
        ParserBrackets.Instance,
        ParserSum.Instance,
        ParserProduct.Instance,
        ParserSimpleTensor.Instance,
        ParserTensorField.Instance,
        ParserDerivative.Instance,
        ParserPower.Instance,
        ParserNumber.Instance,
        ParserFunctions.Instance,
        ParserExpression.Instance
    ];

    public static readonly Parser Default = new((ITokenParser[])DefaultTokenParsers.Clone());

    private readonly ITokenParser[] tokenParsers;

    public bool AllowSameVariance { get; set; }

    /// <summary>
    /// Constructs Parser from a given parsers of AST nodes.
    /// </summary>
    /// <param name="tokenParsers"></param>
    public Parser(params ITokenParser[] tokenParsers)
    {
        ArgumentNullException.ThrowIfNull(tokenParsers);

        this.tokenParsers = tokenParsers;
        Array.Sort(tokenParsers, NodeParserComparator.Instance);
    }

    /// <summary>
    /// Parse string expression into AST.
    /// </summary>
    /// <param name="expression">string expression</param>
    /// <returns>AST</returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ParserException"></exception>
    public ParseToken Parse(string expression)
    {
        ArgumentNullException.ThrowIfNull(expression);

        expression = DeleteComments(expression);
        if (expression.Length == 0)
        {
            throw new ArgumentException("Empty expression.", nameof(expression));
        }

        foreach (var tokenParser in tokenParsers)
        {
            var node = tokenParser.ParseToken(expression.Trim(), this);
            if (node is not null)
                return node;
        }

        throw new ParserException($"No appropriate parser for expression: \"{expression}\"");
    }

    private static string DeleteComments(string expression)
    {
        return Regex.Replace(expression, "//.*|(\"(?:\\\\[^\"]|\\\\\"|.)*?\")|(?s)/\\*.*?\\*/", string.Empty)
            .Replace("\n", string.Empty, StringComparison.Ordinal);
    }
}
