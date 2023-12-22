using System;

namespace NRedberry.Core;

public interface ICloneable<out T> : ICloneable
{
    public new T Clone();
}