using NRedberry.Indices;
using NRedberry.Tensors;
using NRedberry.Transformations;
using NRedberry.Transformations.Symmetrization;

namespace NRedberry.Parsers;

/*
 * Original: ./core/src/main/java/cc/redberry/core/parser/ParseTokenDerivative.java
 */

public class ParseTokenDerivative(TokenType tokenType, params ParseToken[] content) : ParseToken(tokenType, content)
{
    private readonly Indices.Indices indices = BuildIndices(content);

    public override Indices.Indices GetIndices()
    {
        return indices;
    }

    public override Tensor ToTensor()
    {
        var vars = new SimpleTensor[Content.Length - 1];
        Tensor temp = Content[0].ToTensor();

        HashSet<int> allowedDummies = TensorUtils.GetAllIndicesNamesT(temp);
        var free = new IndicesBuilder().Append(temp.Indices);
        for (var i = 1; i < Content.Length; ++i)
        {
            temp = Content[i].ToTensor();
            free.Append(temp.Indices.GetInverted());
            allowedDummies.UnionWith(IndicesUtils.GetIndicesNames(temp.Indices));
            if (temp is not SimpleTensor && temp is not TensorField)
            {
                throw new ArgumentException($"Derivative with respect to non simple argument: {temp}");
            }

            vars[i - 1] = (SimpleTensor)temp;
        }

        allowedDummies.ExceptWith(IndicesUtils.GetIndicesNames(free.Indices.GetFree()));
        var differentiate = new DifferentiateTransformation(
            vars,
            new ITransformation[] { ExpandAndEliminateTransformation.Instance }
        );
        Tensor result = differentiate.Transform(Content[0].ToTensor());
        result = ApplyIndexMapping.OptimizeDummies(result);
        HashSet<int> generated = TensorUtils.GetAllDummyIndicesT(result);
        generated.ExceptWith(allowedDummies);

        result = ApplyIndexMapping.RenameDummy(result, generated.ToArray(), allowedDummies.ToArray());
        return result;
    }

    private static Indices.Indices BuildIndices(ParseToken[] content)
    {
        ArgumentNullException.ThrowIfNull(content);

        var builder = new IndicesBuilder();
        builder.Append(content[0].GetIndices().GetFree());
        for (var i = content.Length - 1; i >= 1; --i)
        {
            builder.Append(content[i].GetIndices().GetInverted().GetFree());
        }

        return builder.Indices;
    }
}
