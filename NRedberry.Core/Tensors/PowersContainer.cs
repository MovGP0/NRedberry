using System;
using System.Collections;
using System.Collections.Generic;

namespace NRedberry.Core.Tensors;

/*
 * Original: ./core/src/main/java/cc/redberry/core/tensor/PowersContainer.java
 */

public sealed class PowersContainer : IEnumerable<Tensor>
{
    public PowersContainer()
    {
        throw new NotImplementedException();
    }

    public PowersContainer(int initialCapacity)
    {
        throw new NotImplementedException();
    }

    public bool Sign => throw new NotImplementedException();

    public bool IsEmpty()
    {
        throw new NotImplementedException();
    }

    public int Count => throw new NotImplementedException();

    public void Put(Tensor tensor)
    {
        throw new NotImplementedException();
    }

    public void Merge(PowersContainer container)
    {
        throw new NotImplementedException();
    }

    public IEnumerator<Tensor> GetEnumerator()
    {
        throw new NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
