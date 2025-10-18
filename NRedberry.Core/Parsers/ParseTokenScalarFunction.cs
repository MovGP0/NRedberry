using NRedberry.Core.Tensors;

namespace NRedberry.Core.Parsers;

/*
 * Original: ./core/src/main/java/cc/redberry/core/parser/ParseTokenScalarFunction.java
 */

public class ParseTokenScalarFunction : ParseToken
{
    public ParseTokenScalarFunction(TokenType tokenType, params ParseToken[] content)
        : base(tokenType, content)
    {
    }

    public override Indices.Indices GetIndices()
    {
        throw new NotImplementedException();
    }

    public override string ToString()
    {
        throw new NotImplementedException();
    }

    public override Tensor ToTensor()
    {
        throw new NotImplementedException();
    }
}
