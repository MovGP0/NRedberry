using NRedberry.Contexts;
using NRedberry.Indices;

namespace NRedberry.Parsers.Preprocessor;

/*
 * Original: ./core/src/main/java/cc/redberry/core/parser/preprocessor/ChangeIndicesTypesAndTensorNames.java
 */

public sealed class ChangeIndicesTypesAndTensorNames : IParseTokenTransformer
{
    private readonly TypesAndNamesTransformer transformer;

    public ChangeIndicesTypesAndTensorNames(TypesAndNamesTransformer transformer)
    {
        ArgumentNullException.ThrowIfNull(transformer);

        this.transformer = transformer;
    }

    public ParseToken Transform(ParseToken node)
    {
        ArgumentNullException.ThrowIfNull(node);

        switch (node.TokenType)
        {
            case TokenType.SimpleTensor:
            {
                var simpleTensor = (ParseTokenSimpleTensor)node;
                NameAndStructureOfIndices descriptor = simpleTensor.GetIndicesTypeStructureAndName();
                return new ParseTokenSimpleTensor(
                    TransformIndices(simpleTensor.Indices, descriptor),
                    transformer.NewName(descriptor.Name, descriptor));
            }

            case TokenType.TensorField:
            {
                var tensorField = (ParseTokenTensorField)node;
                ParseToken[] newContent = TransformContent(tensorField.Content);
                var newArgsIndices = new SimpleIndices[tensorField.ArgumentsIndices.Length];
                for (int i = newArgsIndices.Length - 1; i >= 0; --i)
                {
                    newArgsIndices[i] = IndicesFactory.CreateSimple(null, newContent[i].GetIndices());
                }

                NameAndStructureOfIndices descriptor = tensorField.GetIndicesTypeStructureAndName();
                return new ParseTokenTensorField(
                    TransformIndices(tensorField.Indices, descriptor),
                    transformer.NewName(descriptor.Name, descriptor),
                    newContent,
                    newArgsIndices);
            }

            case TokenType.Number:
                return node;

            case TokenType.ScalarFunction:
            {
                var functionToken = (ParseTokenScalarFunction)node;
                return new ParseTokenScalarFunction(functionToken.Function, TransformContent(node.Content));
            }

            case TokenType.Expression:
            {
                ParseToken[] content = TransformContent(node.Content);
                return new ParseTokenExpression(((ParseTokenExpression)node).Preprocess, content[0], content[1]);
            }

            default:
                return new ParseToken(node.TokenType, TransformContent(node.Content));
        }
    }

    private ParseToken[] TransformContent(ParseToken[] content)
    {
        var newContent = new ParseToken[content.Length];
        for (int i = content.Length - 1; i >= 0; --i)
        {
            newContent[i] = Transform(content[i]);
        }

        return newContent;
    }

    private SimpleIndices TransformIndices(SimpleIndices old, NameAndStructureOfIndices oldDescriptor)
    {
        int size = old.Size();
        var newIndices = new int[size];
        for (int i = size - 1; i >= 0; --i)
        {
            int index = old[i];
            index = IndicesUtils.SetType(transformer.NewType(IndicesUtils.GetTypeEnum(index), oldDescriptor), index);
            newIndices[i] = transformer.NewIndex(index, oldDescriptor);
        }

        return IndicesFactory.CreateSimple(null, newIndices);
    }
}
