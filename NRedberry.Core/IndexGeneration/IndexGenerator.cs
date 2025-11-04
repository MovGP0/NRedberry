using NotImplementedException = sun.reflect.generics.reflectiveObjects.NotImplementedException;

namespace NRedberry.Core;

public sealed class IndexGenerator : ICloneable, ICloneable<IndexGenerator>
{
    protected Dictionary<byte, IntGenerator> Generators = new();

    /// <summary>
    /// Creates with generator without engaged data.
    /// </summary>
    public IndexGenerator()
    {
    }

    public IndexGenerator(Indices.Indices indices)
        : this(indices.AllIndices.Copy())
    {
    }

    public IndexGenerator(Dictionary<byte, IntGenerator> generators)
    {
        Generators = generators;
    }

    public IndexGenerator(params int[] indices)
    {
        // if (indices.length == 0)
        //     return;
        // for (int i = 0; i < indices.length; ++i)
        //     indices[i] = getNameWithType(indices[i]);
        // Arrays.sort(indices);
        // byte type = getType(indices[0]);
        // indices[0] = getNameWithoutType(indices[0]);
        // int prevIndex = 0;
        // for (int i = 1; i < indices.length; ++i) {
        //     if (getType(indices[i]) != type) {
        //         generators.put(type, new IntGenerator(Arrays.copyOfRange(indices, prevIndex, i)));
        //         prevIndex = i;
        //         type = getType(indices[i]);
        //     }
        //     indices[i] = getNameWithoutType(indices[i]);
        // }
        // generators.put(type, new IntGenerator(Arrays.copyOfRange(indices, prevIndex, indices.length)));
        throw new NotImplementedException();
    }

    public bool Contains(int index)
    {
        // byte type = getType(index);
        // IntGenerator intGen;
        // if ((intGen = generators.get(type)) == null)
        //     return false;
        // return intGen.contains(getNameWithoutType(index));
        throw new NotImplementedException();
    }

    public void MergeFrom(IndexGenerator other)
    {
        // other.generators.forEachEntry(
        //    new TByteObjectProcedure<IntGenerator>() {
        //        @Override
        //        public boolean execute(byte a, IntGenerator b) {
        //            IntGenerator thisGenerator = generators.get(a);
        //            if (thisGenerator == null)
        //                generators.put(a, b.clone());
        //            else
        //                thisGenerator.mergeFrom(b);
        //            return true;
        //        }
        //    }
        // );
        throw new NotImplementedException();
    }

    public int Generate(IndexType type) {
        //return generate(type.Type);
        throw new NotImplementedException();
    }

    public int Generate(byte type)
    {
        //IntGenerator ig = generators.get(type);
        //if (ig == null)
        //    generators.put(type, ig = new IntGenerator());
        //return setType(type, ig.getNext());
        throw new NotImplementedException();
    }

    public IndexGenerator Clone()
    {
        /*
        final TByteObjectHashMap<IntGenerator> newMap = new TByteObjectHashMap<>(generators.size());
        generators.forEachEntry(
            new TByteObjectProcedure<IntGenerator>() {
                @Override
                public boolean execute(byte a, IntGenerator b) {
                    newMap.put(a, b.clone());
                    return true;
                }
            }
        );

        return new IndexGenerator(newMap);
        */
        throw new System.NotImplementedException();
    }

    object ICloneable.Clone() => Clone();
}
