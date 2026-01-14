using NRedberry.Parsers;
using NRedberry.Tensors;
using NRedberry.Transformations.Symmetrization;

namespace NRedberry.Contexts;

public sealed class ParseManager(Parser parser)
{
    public IList<IParseTokenTransformer> DefaultParserPreprocessors = new List<IParseTokenTransformer>();
    public IList<ITransformation> DefaultTensorPreprocessors = new List<ITransformation>();

    public Parser Parser { get; } = ValidateParser(parser);

    public Tensor Parse(string expression, IEnumerable<ITransformation> tensorPreprocessors, IEnumerable<IParseTokenTransformer> nodesPreprocessors)
    {
        var node = Parser.Parse(expression);
        var transformedNode = nodesPreprocessors.Aggregate(node, (current, tr) => tr.Transform(current));
        var tensor = transformedNode.ToTensor();
        foreach (var processor in tensorPreprocessors)
        {
            if (!ReferenceEquals(processor, tensor))
            {
                tensor = processor.Transform(tensor);
            }
        }

        return tensor;
    }

    public Tensor Parse(string expression, ITransformation[] tensorPreprocessors, IParseTokenTransformer[] nodesPreprocessors)
    {
        var node = Parser.Parse(expression);
        foreach (var tr in nodesPreprocessors)
        {
            node = tr.Transform(node);
        }

        var tensor = node.ToTensor();
        foreach (var tr in tensorPreprocessors)
        {
            if (!ReferenceEquals(tr, tensor))
            {
                tensor = tr.Transform(tensor);
            }
        }

        return tensor;
    }

    public Tensor Parse(string expression, params IParseTokenTransformer[] nodesPreprocessors)
    {
        return Parse(expression, Array.Empty<ITransformation>(), nodesPreprocessors);
    }

    public Tensor Parse(string expression)
    {
        return Parse(expression, DefaultTensorPreprocessors, DefaultParserPreprocessors);
    }

    public void Reset()
    {
        DefaultParserPreprocessors.Clear();
        DefaultTensorPreprocessors.Clear();
    }

    private static Parser ValidateParser(Parser parser)
    {
        ArgumentNullException.ThrowIfNull(parser);
        return parser;
    }
}
