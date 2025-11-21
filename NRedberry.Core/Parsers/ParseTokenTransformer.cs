namespace NRedberry.Parsers;

public interface IParseTokenTransformer
{
    ParseToken Transform(ParseToken node);
}
