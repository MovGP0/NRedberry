using System;
using NRedberry.Core.Indices;

namespace NRedberry.Core.Contexts
{
    ///<summary>
    /// Object of this class represents unique type of simple tensor or tensor fields (unique name).
    ///
    /// <p>It holds the information about string name of simple tensor, structure of its indices and arguments
    /// (in case of tensor field). Two simple tensors are considered to have different mathematical nature if and only if
    /// their name descriptors are not equal. Each simple tensor with unique mathematical nature have its own unique integer
    /// identifier, which is hold in the name descriptor. For example, tensors A_mn and A_ij have the same mathematical
    /// origin and thus have the same integer identifier and both have the same name descriptor (the same reference). In
    /// contrast, for example, tensors A_mn and A_i have different mathematical origin and different integer identifiers.</p>
    ///
    /// <p>This class have no public constructors, since Redberry takes care about its creation (see {@link NameManager}).
    /// The only way to receive name descriptor from raw information about tensor is through
    /// {@link NameManager#mapNameDescriptor(String, cc.redberry.core.indices.StructureOfIndices...)}.
    /// In order to receive the descriptor from a simple tensor object, one should use
    /// {@link cc.redberry.core.tensor.SimpleTensor#getNameDescriptor()} method.</p>
    /// </summary>
    public abstract class NameDescriptor
    {
        //first element is simple tensor indexTypeStructure, other appears for tensor fields
        StructureOfIndices[] indexTypeStructures { get; }

        /// <summary>
        /// Unique simple tensor identifier.
        /// </summary>
        public int Id { get; }

        private IndicesSymmetries symmetries { get; }

        protected NameDescriptor(StructureOfIndices[] indexTypeStructures, int id)
        {
            if (indexTypeStructures.Length == 0) throw new ArgumentException();

            Id = id;
            this.indexTypeStructures = indexTypeStructures;
            symmetries = IndicesSymmetries.Create(indexTypeStructures[0]);
        }

        /**
         * Returns symmetries of indices of tensors with this name descriptor
         *
         * @return symmetries of indices of tensors with this name descriptor
         */
        public IndicesSymmetries GetSymmetries()
        {
            return symmetries;
        }

        /**
         * Returns {@code true} if this is a descriptor of tensor field
         *
         * @return {@code true} if this is a descriptor of tensor field
         */
        public bool IsField()
        {
            return indexTypeStructures.Length != 1;
        }

        /**
         * Returns structure of indices of tensors with this name descriptor
         *
         * @return structure of indices of tensors with this name descriptor
         */
        public StructureOfIndices GetStructureOfIndices()
        {
            return indexTypeStructures[0];
        }

        /**
         * Returns structure of indices of tensors with this name descriptor (first element in array) and
         * structures of indices of their arguments (in case of tensor field)
         *
         * @return structure of indices of tensors and their arguments
         */
        public StructureOfIndices[] GetStructuresOfIndices()
        {
            //todo clone() ?
            return indexTypeStructures;
        }

        public abstract NameAndStructureOfIndices[] GetKeys();

        /**
         * Returns string name of tensor. The argument can be {@code null}.
         *
         * @param indices indices (in case of metric or Kronecker) and null in other cases
         * @return string name of tensor
         */
        public abstract string GetName(ISimpleIndices indices);

        public override string ToString()
        {
            return GetName(null) + ":" + indexTypeStructures;
        }

        /**
         * Returns structure of indices of tensors with specified name descriptor
         *
         * @param nd name descriptor
         * @return structure of indices of tensors with specified name descriptor
         */
        public static NameAndStructureOfIndices ExtractKey(NameDescriptor nd)
        {
            return nd.GetKeys()[0];
        }
    }
}
