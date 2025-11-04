using NRedberry.Core.Parsers;
using NRedberry.Core.Tensors;
using NRedberry.Core.Transformations.Symmetrization;

namespace NRedberry.Contexts;

public sealed class ParseManager
{
    public IList<IParseTokenTransformer> DefaultParserPreprocessors = new List<IParseTokenTransformer>();
    public IList<ITransformation> DefaultTensorPreprocessors = new List<ITransformation>();

    public Parser Parser { get; }

    public ParseManager(Parser parser)
    {
        Parser = parser;
    }

    public Tensor Parse(string expression, IEnumerable<ITransformation> tensorPreprocessors, IEnumerable<IParseTokenTransformer> nodesPreprocessors)
    {
        var node = Parser.Parse(expression);
        var tronsformedNode = nodesPreprocessors.Aggregate(node, (current, tr) => tr.Transform(current));
        var tensor = tronsformedNode.ToTensor();
        return tensorPreprocessors.Aggregate(tensor, (t, processor) => processor.Transform(t));
    }

    public Tensor Parse(string expression)
    {
        return Parse(expression, DefaultTensorPreprocessors, DefaultParserPreprocessors);
    }
}
