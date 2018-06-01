using System;
using NRedberry.Core.Indices;

namespace NRedberry.Core.Contexts
{
    /// <inheritdoc />
    /// <summary>
    /// Container for the structure of indices(see { @link cc.redberry.core.indices.StructureOfIndices}) of tensor
    /// and its string name.Two simple tensors are considered to have different mathematical nature if and only if
    /// their { @code IndicesTypeStructureAndName} are not equal.
    /// </summary>
    public sealed class NameAndStructureOfIndices : IEquatable<NameAndStructureOfIndices>
    {
        /// <summary>
        /// Name of tensor.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Structure of tensor indices.
        /// </summary>
        public StructureOfIndices[] Structure { get; }

        ///<summary>
        /// 
        /// </summary>
        /// <param name="name">name of tensor</param>
        /// <param name="structure">structure of tensor indices</param>
        public NameAndStructureOfIndices(string name, StructureOfIndices[] structure)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Structure = structure;
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (!(obj is NameAndStructureOfIndices nameAndStructureOfIndices)) return false;
            return Equals(nameAndStructureOfIndices);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Name != null ? Name.GetHashCode() : 0) * 397)
                       ^ (Structure != null ? Structure.GetHashCode() : 0);
            }
        }

        public bool Equals(NameAndStructureOfIndices other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Name, other.Name)
                   && Equals(Structure, other.Structure);
        }
    }
}
