namespace NRedberry.Core.Parsers;

public interface IParseTokenTransformer
{
    ParseToken Transform(ParseToken node);
}
