using NRedberry.Indices;
using NRedberry.Numbers;

namespace NRedberry.Parsers;

/*
 * Original: ./core/src/main/java/cc/redberry/core/parser/ParserProduct.java
 */

public sealed class ParserProduct : ParserOperator
{
    public static ParserProduct Instance { get; } = new();

    private ParserProduct()
        : base('*', '/')
    {
    }

    public override ParseToken? ParseToken(string expression, Parser parser)
    {
        ParseToken? node = base.ParseToken(expression, parser);
        if (node is null || !parser.AllowSameVariance)
        {
            return node;
        }

        HashSet<int> indices = [];
        foreach (ParseToken c in node.Content)
        {
            Indices.Indices free = c.GetIndices().GetFree();
            for (var i = 0; i < free.Size(); i++)
            {
                var ind = free[i];
                if (indices.Contains(ind))
                {
                    RevertIndex(c, ind);
                }
                else
                {
                    indices.Add(ind);
                }
            }
        }

        return node;
    }

    public override int Priority => 999;

    protected override ParseToken Compile(IReadOnlyList<ParseToken> nodes)
    {
        return new ParseToken(TokenType.Product, nodes.ToArray());
    }

    protected override ParseToken InverseOperation(ParseToken tensor)
    {
        return new ParseToken(TokenType.Power, tensor, new ParseTokenNumber(Complex.MinusOne));
    }

    protected override bool TestOperator(char[] expressionChars, int position)
    {
        return !((position + 1 < expressionChars.Length && expressionChars[position + 1] == '*')
            || (position - 1 >= 0 && expressionChars[position - 1] == '*'));
    }

    private static void RevertIndex(ParseToken token, int index)
    {
        if (token is ParseTokenSimpleTensor pToken)
        {
            SimpleIndices indices = pToken.Indices;
            for (var i = 0; i < indices.Size(); i++)
            {
                if (indices[i] == index)
                {
                    var inds = indices.AllIndices.ToArray();
                    inds[i] = IndicesUtils.InverseIndexState(index);
                    pToken.Indices = IndicesFactory.CreateSimple(null, inds);
                    break;
                }
            }
        }
        else if (token.TokenType is TokenType.Product or TokenType.Trace or TokenType.Sum)
        {
            foreach (ParseToken c in token.Content)
            {
                RevertIndex(c, index);
            }
        }
    }
}
