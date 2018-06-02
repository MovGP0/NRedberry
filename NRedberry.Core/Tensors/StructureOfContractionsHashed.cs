namespace NRedberry.Core.Tensors
{
    public sealed class StructureOfContractionsHashed
    {
        public static StructureOfContractionsHashed EmptyInstance =
            new StructureOfContractionsHashed(new TensorContraction(-1, new long[0]));

        public TensorContraction FreeContraction { get; }
        private TensorContraction[] Contractions { get; }

        public StructureOfContractionsHashed(TensorContraction freeContraction, params TensorContraction[] contractions)
        {
            FreeContraction = freeContraction;
            Contractions = contractions;
        }

        public TensorContraction this[int i] => Contractions[i];
    }
}