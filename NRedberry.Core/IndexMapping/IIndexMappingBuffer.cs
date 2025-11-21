namespace NRedberry.IndexMapping;

/*
 * Original: ./core/src/main/java/cc/redberry/core/indexmapping/IndexMappingBuffer.java
 */

public interface IIndexMappingBuffer : ICloneable
{
    bool TryMap(int from, int to);

    void AddSign(bool sign);

    void RemoveContracted();

    bool IsEmpty();

    bool GetSign();

    object Export();

    IDictionary<int, IndexMappingBufferRecord> GetMap();
}
