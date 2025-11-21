using NRedberry.Tensors;

namespace NRedberry.Parsers;

/*
 * Original: ./core/src/main/java/cc/redberry/core/parser/ParseTokenNumber.java
 */

public class ParseTokenNumber(TokenType tokenType, params ParseToken[] content) : ParseToken(tokenType, content)
{
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
