namespace NRedberry.Core.Parsers;

/// <summary>
/// Parser of mathematical expressions.
/// </summary>
public sealed class Parser
{
    /// <summary>
    /// Default parser.
    /// </summary>
    public static readonly Parser Default = new(
    //    ParserBrackets.Instance,
    //    ParserSum.Instance,
    //    ParserProduct.Instance,
    //    ParserSimpleTensor.Instance,
    //    ParserTensorField.Instance,
    //    ParserPower.Instance,
    //    ParserNumber.Instance,
    //    ParserFunctions.Instance,
    //    ParserExpression.Instance,
    //    ParserPowerAst.Instance
    );

    private IEnumerable<ITokenParser> TokenParsers { get; }

    /// <summary>
    /// Constructs Parser from a given parsers of AST nodes.
    /// </summary>
    /// <param name="tokenParsers"></param>
    public Parser(params ITokenParser[] tokenParsers)
    {
        TokenParsers = tokenParsers;
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
        if(string.IsNullOrEmpty(expression)) throw new ArgumentNullException(nameof(expression));

        foreach(var tokenParser in TokenParsers)
        {
            var node = tokenParser.ParseToken(expression.Trim(), this);
            if (node is not null)
                return node;
        }

        throw new ParserException($"No appropriate parser for expression: \"{expression}\"");
    }
}