using NRedberry.Contexts;
using NRedberry.Tensors;
using NRedberry.Transformations.Symmetrization;

namespace NRedberry.Parsers;

/*
 * Original: ./core/src/main/java/cc/redberry/core/parser/ParseTokenExpression.java
 */

public class ParseTokenExpression : ParseToken
{
    public bool Preprocess { get; }

    public ParseTokenExpression(bool preprocess, ParseToken lhs, ParseToken rhs)
        : base(TokenType.Expression, lhs, rhs)
    {
        Preprocess = preprocess;
    }

    public override Indices.Indices GetIndices()
    {
        return Content[0].GetIndices().GetFree();
    }

    public override Tensor ToTensor()
    {
        Tensor expression = NRedberry.Tensors.Tensors.Expression(Content[0].ToTensor(), Content[1].ToTensor());
        if (Preprocess)
        {
            foreach (ITransformation tr in Context.Get().ParseManager.DefaultTensorPreprocessors)
            {
                expression = tr.Transform(expression);
            }

            Context.Get().ParseManager.DefaultTensorPreprocessors.Add((ITransformation)expression);
        }

        return expression;
    }
}
