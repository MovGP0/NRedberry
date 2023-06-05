using System;
using System.Diagnostics;
using NRedberry.Core.Indices;
using NRedberry.Core.Tensors;

namespace NRedberry.Core.Parsers;

public sealed class ParseToken
{
    public TokenType TokenType { get; }
    public ParseToken[] Content { get; }
    public ParseToken Parent { get; private set; }

    public ParseToken(TokenType tokenType, params ParseToken[] content)
    {
        TokenType = tokenType;
        Content = content;
        foreach (var node in content)
        {
            node.SetParent(this);
        }
    }

    private void SetParent(ParseToken parent)
    {
        Parent = parent;
    }

    public IIndices GetIndices()
    {
        throw new NotImplementedException();
    }

    public override string ToString()
    {
        throw new NotImplementedException();
    }

    private Tensor[] ContentToTensors()
    {
        throw new NotImplementedException();
    }

    public Tensor ToTensor()
    {
        switch (TokenType)
        {
            case TokenType.Sum:
                return Tensor.Sum(ContentToTensors());

            case TokenType.Power:
                Debug.Assert(Content.Length == 2);
                var tensor0 = Content[0].ToTensor();
                var tensor1 = Content[1].ToTensor();
                return tensor0.Pow(tensor1);

            case TokenType.Product:
                return Tensor.MultiplyAndRenameConflictingDummies(ContentToTensors());

            case TokenType.Expression:
                Debug.Assert(Content.Length == 2);
                return Tensor.Expression(Content[0].ToTensor(), Content[1].ToTensor());

            default:
                throw new ParserException($"Unknown tensor type: {TokenType}");
        }
    }
}