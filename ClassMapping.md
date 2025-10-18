# NRedberry to Redberry Class Mapping

This document lists each NRedberry class (.cs) alongside the Redberry source (.java) it was ported from. Paths are relative and use the `./` prefix as requested.

Table sorted by NRedberry path:

| NRedberry class | Redberry source |
| --- | --- |
| ./NRedberry.Apache.Commons.Math/BigFraction.cs | (no match) |
| ./NRedberry.Core/Transformations/Expand/AbstractExpandTransformation.cs | ./core/src/main/java/cc/redberry/core/transformations/expand/AbstractExpandTransformation.java |
| ./NRedberry.Core/Transformations/Expand/ExpandAllTransformation.cs | ./core/src/main/java/cc/redberry/core/transformations/expand/ExpandAllTransformation.java |
| ./NRedberry.Core/Transformations/Expand/ExpandDenominatorTransformation.cs | ./core/src/main/java/cc/redberry/core/transformations/expand/ExpandDenominatorTransformation.java |
| ./NRedberry.Core/Transformations/Expand/ExpandNumeratorTransformation.cs | ./core/src/main/java/cc/redberry/core/transformations/expand/ExpandNumeratorTransformation.java |
| ./NRedberry.Core/Transformations/Expand/ExpandPort.cs | ./core/src/main/java/cc/redberry/core/transformations/expand/ExpandPort.java |
| ./NRedberry.Core/Transformations/Expand/ExpandTransformation.cs | ./core/src/main/java/cc/redberry/core/transformations/expand/ExpandTransformation.java |
| ./NRedberry.Core/Transformations/Expand/ExpandUtils.cs | ./core/src/main/java/cc/redberry/core/transformations/expand/ExpandUtils.java |
| ./NRedberry.Core/Transformations/Factor/FactorTransformation.cs | ./core/src/main/java/cc/redberry/core/transformations/factor/FactorTransformation.java |
| ./NRedberry.Core/Transformations/Factor/JasFactor.cs | ./core/src/main/java/cc/redberry/core/transformations/factor/JasFactor.java |
| ./NRedberry.Core/Transformations/Fractions/GetDenominatorTransformation.cs | ./core/src/main/java/cc/redberry/core/transformations/fractions/GetDenominatorTransformation.java |
| ./NRedberry.Core/Transformations/Fractions/GetNumeratorTransformation.cs | ./core/src/main/java/cc/redberry/core/transformations/fractions/GetNumeratorTransformation.java |
| ./NRedberry.Core/Transformations/Fractions/NumeratorDenominator.cs | ./core/src/main/java/cc/redberry/core/transformations/fractions/NumeratorDenominator.java |
| ./NRedberry.Core/Transformations/Fractions/TogetherTransformation.cs | ./core/src/main/java/cc/redberry/core/transformations/fractions/TogetherTransformation.java |
| ./NRedberry.Core/Transformations/Substitutions/BijectionContainer.cs | (no match) |
| ./NRedberry.Core/Transformations/Transformation.cs | ./core/src/main/java/cc/redberry/core/transformations/Transformation.java |
| ./NRedberry.Core/Transformations/Substitutions/InconsistentSubstitutionException.cs | ./core/src/main/java/cc/redberry/core/transformations/substitutions/InconsistentSubstitutionException.java |
| ./NRedberry.Core/Transformations/Substitutions/PrimitiveProductSubstitution.cs | ./core/src/main/java/cc/redberry/core/transformations/substitutions/PrimitiveProductSubstitution.java |
| ./NRedberry.Core/Transformations/Substitutions/PrimitiveSimpleTensorSubstitution.cs | ./core/src/main/java/cc/redberry/core/transformations/substitutions/PrimitiveSimpleTensorSubstitution.java |
| ./NRedberry.Core/Transformations/Substitutions/PrimitiveSubstitution.cs | ./core/src/main/java/cc/redberry/core/transformations/substitutions/PrimitiveSubstitution.java |
| ./NRedberry.Core/Transformations/Substitutions/PrimitiveSumSubstitution.cs | ./core/src/main/java/cc/redberry/core/transformations/substitutions/PrimitiveSumSubstitution.java |
| ./NRedberry.Core/Transformations/Substitutions/PrimitiveTensorFieldSubstitution.cs | ./core/src/main/java/cc/redberry/core/transformations/substitutions/PrimitiveTensorFieldSubstitution.java |
| ./NRedberry.Core/Transformations/Substitutions/ProductsBijectionsPort.cs | ./core/src/main/java/cc/redberry/core/transformations/substitutions/ProductsBijectionsPort.java |
| ./NRedberry.Core/Transformations/Substitutions/SubstitutionIterator.cs | ./core/src/main/java/cc/redberry/core/transformations/substitutions/SubstitutionIterator.java |
| ./NRedberry.Core/Transformations/Substitutions/SubstitutionTransformation.cs | ./core/src/main/java/cc/redberry/core/transformations/substitutions/SubstitutionTransformation.java |
| ./NRedberry.Core/Transformations/Substitutions/SumBijectionPort.cs | ./core/src/main/java/cc/redberry/core/transformations/substitutions/SumBijectionPort.java |
| ./NRedberry.Core/Transformations/Symmetrization/CollectNonScalarsTransformation.cs | ./core/src/main/java/cc/redberry/core/transformations/CollectNonScalarsTransformation.java |
| ./NRedberry.Core/Transformations/Symmetrization/CollectScalarFactorsTransformation.cs | ./core/src/main/java/cc/redberry/core/transformations/CollectScalarFactorsTransformation.java |
| ./NRedberry.Core/Transformations/Symmetrization/ComplexConjugateTransformation.cs | ./core/src/main/java/cc/redberry/core/transformations/ComplexConjugateTransformation.java |
| ./NRedberry.Core/Transformations/Symmetrization/DifferentiateTransformation.cs | ./core/src/main/java/cc/redberry/core/transformations/DifferentiateTransformation.java |
| ./NRedberry.Core/Transformations/Symmetrization/EliminateFromSymmetriesTransformation.cs | (no match) |
| ./NRedberry.Core/Transformations/Substitutions/IndexlessBijectionsPort.cs | ./core/src/main/java/cc/redberry/core/transformations/substitutions/IndexlessBijectionsPort.java |
| ./NRedberry.Core/Transformations/Symmetrization/EliminateMetricsTransformation.cs | ./core/src/main/java/cc/redberry/core/transformations/EliminateMetricsTransformation.java |
| ./NRedberry.Core/Transformations/IdentityTransformation.cs | (no match) |
| ./NRedberry.Core/Tensors/Iterators/TraverseState.cs | ./core/src/main/java/cc/redberry/core/tensor/iterator/TraverseState.java |
| ./NRedberry.Core/Tensors/Functions/ArcCosFactory.cs | (no match) |
| ./NRedberry.Core/Tensors/Functions/ArcCot.cs | ./core/src/main/java/cc/redberry/core/tensor/functions/ArcCot.java |
| ./NRedberry.Core/Tensors/Functions/ArcCotFactory.cs | (no match) |
| ./NRedberry.Core/Tensors/Functions/ArcSin.cs | ./core/src/main/java/cc/redberry/core/tensor/functions/ArcSin.java |
| ./NRedberry.Core/Tensors/Functions/ArcSinFactory.cs | (no match) |
| ./NRedberry.Core/Tensors/Functions/ArcTan.cs | ./core/src/main/java/cc/redberry/core/tensor/functions/ArcTan.java |
| ./NRedberry.Core/Tensors/Functions/ArcTanFactory.cs | (no match) |
| ./NRedberry.Core/Tensors/Functions/ContextManager.cs | ./core/src/main/java/cc/redberry/core/context/ContextManager.java |
| ./NRedberry.Core/Tensors/Functions/Cos.cs | ./core/src/main/java/cc/redberry/core/tensor/functions/Cos.java |
| ./NRedberry.Core/Tensors/Functions/CosFactory.cs | (no match) |
| ./NRedberry.Core/Tensors/Functions/Cot.cs | ./core/src/main/java/cc/redberry/core/tensor/functions/Cot.java |
| ./NRedberry.Core/Tensors/Functions/CotFactory.cs | (no match) |
| ./NRedberry.Core/Tensors/Functions/Exp.cs | ./core/src/main/java/cc/redberry/core/tensor/functions/Exp.java |
| ./NRedberry.Core/Tensors/Functions/ExpFactory.cs | (no match) |
| ./NRedberry.Core/Tensors/Iterators/TreeIteratorAbstract.cs | ./core/src/main/java/cc/redberry/core/tensor/iterator/TreeIteratorAbstract.java |
| ./NRedberry.Core/Tensors/Functions/Log.cs | ./core/src/main/java/cc/redberry/core/tensor/functions/Log.java |
| ./NRedberry.Core/Tensors/Functions/ScalarFunction.cs | ./core/src/main/java/cc/redberry/core/tensor/functions/ScalarFunction.java |
| ./NRedberry.Core/Tensors/Functions/ScalarFunctionBuilder.cs | ./core/src/main/java/cc/redberry/core/tensor/functions/ScalarFunctionBuilder.java |
| ./NRedberry.Core/Tensors/Functions/ScalarFunctionFactory.cs | ./core/src/main/java/cc/redberry/core/tensor/functions/ScalarFunctionFactory.java |
| ./NRedberry.Core/Tensors/Functions/Sin.cs | ./core/src/main/java/cc/redberry/core/tensor/functions/Sin.java |
| ./NRedberry.Core/Tensors/Functions/SinFactory.cs | (no match) |
| ./NRedberry.Core/Tensors/Functions/Tan.cs | ./core/src/main/java/cc/redberry/core/tensor/functions/Tan.java |
| ./NRedberry.Core/Tensors/Functions/TanFactory.cs | (no match) |
| ./NRedberry.Core/Tensors/Iterators/AllTraverseGuide.cs | (no match) |
| ./NRedberry.Core/Tensors/Iterators/ExceptFieldsTraverseGuide.cs | (no match) |
| ./NRedberry.Core/Tensors/Iterators/ExceptFunctionsAndFieldsTraverseGuide.cs | (no match) |
| ./NRedberry.Core/Tensors/Iterators/FromChildToParentIterator.cs | ./core/src/main/java/cc/redberry/core/tensor/iterator/FromChildToParentIterator.java |
| ./NRedberry.Core/Tensors/Iterators/ITreeIterator.cs | (no match) |
| ./NRedberry.Core/Tensors/Iterators/TraverseGuide.cs | ./core/src/main/java/cc/redberry/core/tensor/iterator/TraverseGuide.java |
| ./NRedberry.Core/Tensors/Iterators/TraversePermission.cs | ./core/src/main/java/cc/redberry/core/tensor/iterator/TraversePermission.java |
| ./NRedberry.Core/Tensors/Functions/LogFactory.cs | (no match) |
| ./NRedberry.Core/Transformations/Symmetrization/ExpandAndEliminateTransformation.cs | ./core/src/main/java/cc/redberry/core/transformations/ExpandAndEliminateTransformation.java |
| ./NRedberry.Core/Transformations/Symmetrization/ToNumericTransformation.cs | ./core/src/main/java/cc/redberry/core/transformations/ToNumericTransformation.java |
| ./NRedberry.Core/Transformations/Symmetrization/Transformation.cs | ./core/src/main/java/cc/redberry/core/transformations/Transformation.java |
| ./NRedberry.Core.Entities/IndexType.cs | ./core/src/main/java/cc/redberry/core/indices/IndexType.java |
| ./NRedberry.Core.Entities/INumber.cs | (no match) |
| ./NRedberry.Core.Entities/Numeric.cs | ./core/src/main/java/cc/redberry/core/number/Numeric.java |
| ./NRedberry.Core.Entities/OutputFormat.cs | ./core/src/main/java/cc/redberry/core/context/OutputFormat.java |
| ./NRedberry.Core.Entities/Rational.cs | ./core/src/main/java/cc/redberry/core/number/Rational.java |
| ./NRedberry.Core.Entities/RationalExtensions.cs | (no match) |
| ./NRedberry.Core.Entities/Real.cs | ./core/src/main/java/cc/redberry/core/number/Real.java |
| ./NRedberry.Core.Entities/RealField.cs | ./core/src/main/java/cc/redberry/core/number/RealField.java |
| ./NRedberry.Core.Entities/TypeData.cs | (no match) |
| ./NRedberry.Core.Entities/UpperLowerIndices.cs | (no match) |
| ./NRedberry.Core.Exceptions/InconsistentGeneratorsException.cs | ./core/src/main/java/cc/redberry/core/combinatorics/InconsistentGeneratorsException.java, ./core/src/main/java/cc/redberry/core/groups/permutations/InconsistentGeneratorsException.java |
| ./NRedberry.Core.Exceptions/IndexConverterException.cs | ./core/src/main/java/cc/redberry/core/context/IndexConverterException.java |
| ./NRedberry.Core.Utils/Arrays.cs | (no match) |
| ./NRedberry.Core.Utils/ArraysUtils.cs | ./core/src/main/java/cc/redberry/core/utils/ArraysUtils.java |
| ./NRedberry.Core.Entities/IIndexSymbolConverter.cs | (no match) |
| ./NRedberry.Core.Utils/ArraysUtils.QuickSort.cs | (no match) |
| ./NRedberry.Core.Utils/ArrayUtils.InsertionSort.cs | (no match) |
| ./NRedberry.Core.Utils/BinaryToStringConverter.cs | (no match) |
| ./NRedberry.Core.Utils/BitArrayExtensions.cs | (no match) |
| ./NRedberry.Core.Utils/ByteBackedBitArray.cs | (no match) |
| ./NRedberry.Core.Utils/DefaultToStringConverter.cs | (no match) |
| ./NRedberry.Core.Utils/EnumerableEx.cs | (no match) |
| ./NRedberry.Core.Utils/HashFunctions.cs | ./core/src/main/java/cc/redberry/core/utils/HashFunctions.java |
| ./NRedberry.Core.Utils/HexToStringConverter.cs | (no match) |
| ./NRedberry.Core.Utils/IBitArray.cs | (no match) |
| ./NRedberry.Core.Utils/Indicator.cs | ./core/src/main/java/cc/redberry/core/utils/Indicator.java |
| ./NRedberry.Core.Utils/IntArray.cs | ./core/src/main/java/cc/redberry/core/utils/IntArray.java |
| ./NRedberry.Core.Utils/IntArrayList.cs | ./core/src/main/java/cc/redberry/core/utils/IntArrayList.java |
| ./NRedberry.Core.Utils/IntTimSort.cs | ./core/src/main/java/cc/redberry/core/utils/IntTimSort.java |
| ./NRedberry.Core.Utils/IToStringConverter.cs | (no match) |
| ./NRedberry.Core.Utils/ArraysUtils.Swap.cs | (no match) |
| ./NRedberry.Core.Entities/BitArrayExtensions.cs | (no match) |
| ./NRedberry.Core.Entities/BitArrayEqualityComparer.cs | (no match) |
| ./NRedberry.Core.Combinatorics/Symmetries/SymmetriesImpl.cs | (no match) |
| ./NRedberry.Core/Transformations/Symmetrization/TransformationCollection.cs | ./core/src/main/java/cc/redberry/core/transformations/TransformationCollection.java |
| ./NRedberry.Core/Transformations/Symmetrization/TransformationException.cs | ./core/src/main/java/cc/redberry/core/transformations/TransformationException.java |
| ./NRedberry.Core/Transformations/Symmetrization/Transformer.cs | ./core/src/main/java/cc/redberry/core/transformations/Transformer.java |
| ./NRedberry.Core/Utils/TensorHashCalculator.cs | (no match) |
| ./NRedberry.Core.Combinatorics/Combinatorics.cs | ./core/src/main/java/cc/redberry/core/combinatorics/Combinatorics.java |
| ./NRedberry.Core.Combinatorics/IIntCombinatorialGenerator.cs | (no match) |
| ./NRedberry.Core.Combinatorics/IIntCombinatorialPort.cs | (no match) |
| ./NRedberry.Core.Combinatorics/InconsistentGeneratorsException.cs | ./core/src/main/java/cc/redberry/core/combinatorics/InconsistentGeneratorsException.java, ./core/src/main/java/cc/redberry/core/groups/permutations/InconsistentGeneratorsException.java |
| ./NRedberry.Core.Combinatorics/IntCombinationPermutationGenerator.cs | ./core/src/main/java/cc/redberry/core/combinatorics/IntCombinationPermutationGenerator.java |
| ./NRedberry.Core.Combinatorics/IntCombinationsGenerator.cs | ./core/src/main/java/cc/redberry/core/combinatorics/IntCombinationsGenerator.java |
| ./NRedberry.Core.Combinatorics/IntCombinatorialGenerator.cs | ./core/src/main/java/cc/redberry/core/combinatorics/IntCombinatorialGenerator.java |
| ./NRedberry.Core.Combinatorics/IntDistinctTuplesPort.cs | ./core/src/main/java/cc/redberry/core/combinatorics/IntDistinctTuplesPort.java |
| ./NRedberry.Core.Combinatorics/IntPermutationsGenerator.cs | ./core/src/main/java/cc/redberry/core/combinatorics/IntPermutationsGenerator.java |
| ./NRedberry.Core.Combinatorics/IntPermutationsSpanGenerator.cs | (no match) |
| ./NRedberry.Core.Combinatorics/IntTuplesPort.cs | ./core/src/main/java/cc/redberry/core/combinatorics/IntTuplesPort.java |
| ./NRedberry.Core.Combinatorics/IOutputPortUnsafe.cs | (no match) |
| ./NRedberry.Core.Combinatorics/Permutation.cs | ./core/src/main/java/cc/redberry/core/groups/permutations/Permutation.java |
| ./NRedberry.Core.Combinatorics/Symmetries/SymmetriesFactory.cs | (no match) |
| ./NRedberry.Core.Combinatorics/Symmetries/ISymmetries.cs | (no match) |
| ./NRedberry.Core.Combinatorics/Symmetries/IntPriorityPermutationsGenerator.cs | ./core/src/main/java/cc/redberry/core/combinatorics/IntPriorityPermutationsGenerator.java |
| ./NRedberry.Core.Combinatorics/Symmetries/FullSymmetries.cs | (no match) |
| ./NRedberry.Core.Combinatorics/Symmetries/EmptySymmetries.cs | (no match) |
| ./NRedberry.Core.Combinatorics/Symmetries/DummySymmetries.cs | (no match) |
| ./NRedberry.Core/Tensors/Functions/ArcCos.cs | ./core/src/main/java/cc/redberry/core/tensor/functions/ArcCos.java |
| ./NRedberry.Core.Combinatorics/Symmetries/AbstractSymmetries.cs | (no match) |
| ./NRedberry.Core.Combinatorics/Extensions/BitArrayExtensions.cs | (no match) |
| ./NRedberry.Core.Combinatorics/UnsafeCombinatorics.cs | (no match) |
| ./NRedberry.Core.Combinatorics/Symmetry.cs | (no match) |
| ./NRedberry.Core.Combinatorics/PermutationsSpanIterator.cs | (no match) |
| ./NRedberry.Core.Combinatorics/PermutationsGenerator.cs | (no match) |
| ./NRedberry.Core.Combinatorics/PermutationPriorityTuple.cs | (no match) |
| ./NRedberry.Core.Combinatorics/Extensions/EnumeratorExtensions.cs | (no match) |
| ./NRedberry.Core/Tensors/TensorWrapper.cs | ./core/src/main/java/cc/redberry/core/tensor/TensorWrapper.java |
| ./NRedberry.Core/Tensors/TensorUtils.cs | ./core/src/main/java/cc/redberry/core/utils/TensorUtils.java |
| ./NRedberry.Core/Tensors/Tensors.cs | ./core/src/main/java/cc/redberry/core/tensor/Tensors.java |
| ./NRedberry.Core/Contexts/Defaults/IndexWithStrokeConverter.cs | ./core/src/main/java/cc/redberry/core/context/defaults/IndexWithStrokeConverter.java |
| ./NRedberry.Core/Contexts/Defaults/LatinLowerCaseConverter.cs | ./core/src/main/java/cc/redberry/core/context/defaults/LatinLowerCaseConverter.java |
| ./NRedberry.Core/Contexts/Defaults/LatinUpperCaseConverter.cs | ./core/src/main/java/cc/redberry/core/context/defaults/LatinUpperCaseConverter.java |
| ./NRedberry.Core/Contexts/Defaults/SymbolArrayConverter.cs | (no match) |
| ./NRedberry.Core/Graphs/GraphType.cs | ./core/src/main/java/cc/redberry/core/graph/GraphType.java |
| ./NRedberry.Core/Graphs/GraphUtils.cs | ./core/src/main/java/cc/redberry/core/graph/GraphUtils.java |
| ./NRedberry.Core/Graphs/PrimitiveSubgraph.cs | ./core/src/main/java/cc/redberry/core/graph/PrimitiveSubgraph.java |
| ./NRedberry.Core/Graphs/PrimitiveSubgraphPartition.cs | ./core/src/main/java/cc/redberry/core/graph/PrimitiveSubgraphPartition.java |
| ./NRedberry.Core/Groups/AlgorithmsBacktrack.cs | ./core/src/main/java/cc/redberry/core/groups/permutations/AlgorithmsBacktrack.java |
| ./NRedberry.Core/Groups/AlgorithmsBase.cs | ./core/src/main/java/cc/redberry/core/groups/permutations/AlgorithmsBase.java |
| ./NRedberry.Core/Groups/BacktrackSearch.cs | ./core/src/main/java/cc/redberry/core/groups/permutations/BacktrackSearch.java |
| ./NRedberry.Core/Groups/BacktrackSearchPayload.cs | ./core/src/main/java/cc/redberry/core/groups/permutations/BacktrackSearchPayload.java |
| ./NRedberry.Core/Groups/BacktrackSearchTestFunction.cs | ./core/src/main/java/cc/redberry/core/groups/permutations/BacktrackSearchTestFunction.java |
| ./NRedberry.Core/Groups/BruteForcePermutationIterator.cs | ./core/src/main/java/cc/redberry/core/groups/permutations/BruteForcePermutationIterator.java |
| ./NRedberry.Core/Contexts/Defaults/IndexConverterExtender.cs | ./core/src/main/java/cc/redberry/core/context/defaults/IndexConverterExtender.java |
| ./NRedberry.Core/Groups/BSGSCandidateElement.cs | ./core/src/main/java/cc/redberry/core/groups/permutations/BSGSCandidateElement.java |
| ./NRedberry.Core/Groups/InducedOrdering.cs | ./core/src/main/java/cc/redberry/core/groups/permutations/InducedOrdering.java |
| ./NRedberry.Core/Groups/InducedOrderingOfPermutations.cs | ./core/src/main/java/cc/redberry/core/groups/permutations/InducedOrderingOfPermutations.java |
| ./NRedberry.Core/Groups/Permutation.cs | ./core/src/main/java/cc/redberry/core/groups/permutations/Permutation.java |
| ./NRedberry.Core/Groups/PermutationGroup.cs | ./core/src/main/java/cc/redberry/core/groups/permutations/PermutationGroup.java |
| ./NRedberry.Core/Groups/PermutationOneLineAbstract.cs | ./core/src/main/java/cc/redberry/core/groups/permutations/PermutationOneLineAbstract.java |
| ./NRedberry.Core/Groups/PermutationOneLineByte.cs | ./core/src/main/java/cc/redberry/core/groups/permutations/PermutationOneLineByte.java |
| ./NRedberry.Core/Groups/PermutationOneLineInt.cs | ./core/src/main/java/cc/redberry/core/groups/permutations/PermutationOneLineInt.java |
| ./NRedberry.Core/Groups/PermutationOneLineShort.cs | ./core/src/main/java/cc/redberry/core/groups/permutations/PermutationOneLineShort.java |
| ./NRedberry.Core/Groups/Permutations.cs | ./core/src/main/java/cc/redberry/core/groups/permutations/Permutations.java |
| ./NRedberry.Core/Groups/Permutations.LengthsOfCycles.cs | (no match) |
| ./NRedberry.Core/Groups/RandomPermutation.cs | ./core/src/main/java/cc/redberry/core/groups/permutations/RandomPermutation.java |
| ./NRedberry.Core/Groups/SchreierVector.cs | ./core/src/main/java/cc/redberry/core/groups/permutations/SchreierVector.java |
| ./NRedberry.Core/IndexGeneration/IndexGenerator.cs | ./core/src/main/java/cc/redberry/core/indexgenerator/IndexGenerator.java |
| ./NRedberry.Core/IndexGeneration/IntGenerator.cs | ./core/src/main/java/cc/redberry/core/indexgenerator/IntGenerator.java |
| ./NRedberry.Core/Groups/BSGSElement.cs | ./core/src/main/java/cc/redberry/core/groups/permutations/BSGSElement.java |
| ./NRedberry.Core/Contexts/Defaults/IContextFactory.cs | (no match) |
| ./NRedberry.Core/Contexts/Defaults/GreekLaTeXUpperCaseConverter.cs | ./core/src/main/java/cc/redberry/core/context/defaults/GreekLaTeXUpperCaseConverter.java |
| ./NRedberry.Core/Contexts/Defaults/GreekLaTeXLowerCaseConverter.cs | ./core/src/main/java/cc/redberry/core/context/defaults/GreekLaTeXLowerCaseConverter.java |
| ./NRedberry.Apache.Commons.Math/IField.cs | (no match) |
| ./NRedberry.Apache.Commons.Math/IFieldElement.cs | (no match) |
| ./NRedberry.Core/Globals.cs | (no match) |
| ./NRedberry.Core/ICloneable`1.cs | (no match) |
| ./NRedberry.Core/Concurrent/IOutputPort.cs | (no match) |
| ./NRedberry.Core/Concurrent/IOutputPortUnsafe.cs | (no match) |
| ./NRedberry.Core/Concurrent/PortEnumerator.cs | (no match) |
| ./NRedberry.Core/Concurrent/Singleton.cs | (no match) |
| ./NRedberry.Core/Contexts/ArrayExtensions.cs | (no match) |
| ./NRedberry.Core/Contexts/CC.cs | ./core/src/main/java/cc/redberry/core/context/CC.java |
| ./NRedberry.Core/Contexts/Context.cs | ./core/src/main/java/cc/redberry/core/context/Context.java |
| ./NRedberry.Core/Contexts/ContextEvent.cs | ./core/src/main/java/cc/redberry/core/context/ContextEvent.java |
| ./NRedberry.Core/Contexts/ContextSettings.cs | ./core/src/main/java/cc/redberry/core/context/ContextSettings.java |
| ./NRedberry.Core/Contexts/IContextListener.cs | (no match) |
| ./NRedberry.Core/Contexts/IndexConverterManager.cs | ./core/src/main/java/cc/redberry/core/context/IndexConverterManager.java |
| ./NRedberry.Core/Contexts/IndexSymbolConverter.cs | ./core/src/main/java/cc/redberry/core/context/IndexSymbolConverter.java |
| ./NRedberry.Core/Contexts/LongBackedBitArray.cs | (no match) |
| ./NRedberry.Core/Contexts/Defaults/DefaultContextSettings.cs | ./core/src/main/java/cc/redberry/core/context/defaults/DefaultContextSettings.java |
| ./NRedberry.Core/Contexts/Defaults/DefaultContextFactory.cs | ./core/src/main/java/cc/redberry/core/context/defaults/DefaultContextFactory.java |
| ./NRedberry.Core/Contexts/ULongExtensions.cs | (no match) |
| ./NRedberry.Core/Contexts/ParseManager.cs | ./core/src/main/java/cc/redberry/core/parser/ParseManager.java |
| ./NRedberry.Core/Contexts/OutputFormat.cs | ./core/src/main/java/cc/redberry/core/context/OutputFormat.java |
| ./NRedberry.Core/Contexts/NameManager.cs | ./core/src/main/java/cc/redberry/core/context/NameManager.java |
| ./NRedberry.Core/Indices/AbstractIndices.cs | ./core/src/main/java/cc/redberry/core/indices/AbstractIndices.java |
| ./NRedberry.Core/Contexts/NameDescriptorForTensorFieldImpl.cs | ./core/src/main/java/cc/redberry/core/context/NameDescriptorForTensorFieldImpl.java |
| ./NRedberry.Core/Contexts/NameDescriptorForTensorField.cs | ./core/src/main/java/cc/redberry/core/context/NameDescriptorForTensorField.java |
| ./NRedberry.Core/Contexts/NameDescriptorForSimpleTensor.cs | ./core/src/main/java/cc/redberry/core/context/NameDescriptorForSimpleTensor.java |
| ./NRedberry.Core/Contexts/NameDescriptorForMetricAndKronecker.cs | ./core/src/main/java/cc/redberry/core/context/NameDescriptorForMetricAndKronecker.java |
| ./NRedberry.Core/Contexts/NameDescriptor.cs | ./core/src/main/java/cc/redberry/core/context/NameDescriptor.java |
| ./NRedberry.Core/Contexts/NameAndStructureOfIndices.cs | ./core/src/main/java/cc/redberry/core/context/NameAndStructureOfIndices.java |
| ./NRedberry.Core/Contexts/LongExtensions.cs | (no match) |
| ./NRedberry.Core/Contexts/NameDescriptorForTensorFieldDerivative.cs | ./core/src/main/java/cc/redberry/core/context/NameDescriptorForTensorFieldDerivative.java |
| ./NRedberry.Core.Utils/MathUtils.cs | ./core/src/main/java/cc/redberry/core/utils/MathUtils.java |
| ./NRedberry.Core/Indices/EmptyIndices.cs | ./core/src/main/java/cc/redberry/core/indices/EmptyIndices.java |
| ./NRedberry.Core/Indices/IIndexMapping.cs | (no match) |
| ./NRedberry.Core/Tensors/ComplexSumBuilder.cs | (no match) |
| ./NRedberry.Core/Tensors/FactorNode.cs | ./core/src/main/java/cc/redberry/core/tensor/FactorNode.java |
| ./NRedberry.Core/Tensors/IndexMapper.cs | (no match) |
| ./NRedberry.Core/Tensors/MultiTensor.cs | ./core/src/main/java/cc/redberry/core/tensor/MultiTensor.java |
| ./NRedberry.Core/Tensors/OutputFormat.cs | ./core/src/main/java/cc/redberry/core/context/OutputFormat.java |
| ./NRedberry.Core/Tensors/Power.cs | ./core/src/main/java/cc/redberry/core/tensor/Power.java, ./core/src/main/java/cc/redberry/core/transformations/factor/jasfactor/edu/jas/structure/Power.java |
| ./NRedberry.Core/Tensors/PowerBuilder.cs | ./core/src/main/java/cc/redberry/core/tensor/PowerBuilder.java |
| ./NRedberry.Core/Tensors/PowerFactory.cs | ./core/src/main/java/cc/redberry/core/tensor/PowerFactory.java |
| ./NRedberry.Core/Tensors/Product.cs | ./core/src/main/java/cc/redberry/core/tensor/Product.java |
| ./NRedberry.Core/Tensors/ProductContent.cs | ./core/src/main/java/cc/redberry/core/tensor/ProductContent.java |
| ./NRedberry.Core/Tensors/ProductFactory.cs | ./core/src/main/java/cc/redberry/core/tensor/ProductFactory.java |
| ./NRedberry.Core/Tensors/SimpleTensor.cs | ./core/src/main/java/cc/redberry/core/tensor/SimpleTensor.java |
| ./NRedberry.Core/Tensors/SimpleTensorBuilder.cs | (no match) |
| ./NRedberry.Core/Tensors/SimpleTensorExtensions.cs | (no match) |
| ./NRedberry.Core/Tensors/CC.cs | ./core/src/main/java/cc/redberry/core/context/CC.java |
| ./NRedberry.Core/Tensors/SimpleTensorFactory.cs | (no match) |
| ./NRedberry.Core/Tensors/SplitNumbers.cs | (no match) |
| ./NRedberry.Core/Tensors/StructureOfContractions.cs | ./core/src/main/java/cc/redberry/core/tensor/StructureOfContractions.java |
| ./NRedberry.Core/Tensors/StructureOfContractionsHashed.cs | (no match) |
| ./NRedberry.Core/Tensors/Sum.cs | ./core/src/main/java/cc/redberry/core/tensor/Sum.java |
| ./NRedberry.Core/Tensors/SumBuilder.cs | ./core/src/main/java/cc/redberry/core/tensor/SumBuilder.java |
| ./NRedberry.Core/Tensors/SumFactory.cs | ./core/src/main/java/cc/redberry/core/tensor/SumFactory.java |
| ./NRedberry.Core/Tensors/Tensor.cs | ./core/src/main/java/cc/redberry/core/tensor/Tensor.java |
| ./NRedberry.Core/Tensors/TensorArrayUtils.cs | (no match) |
| ./NRedberry.Core/Tensors/TensorBuilder.cs | ./core/src/main/java/cc/redberry/core/tensor/TensorBuilder.java |
| ./NRedberry.Core/Tensors/TensorContraction.cs | ./core/src/main/java/cc/redberry/core/tensor/TensorContraction.java |
| ./NRedberry.Core/Tensors/TensorException.cs | ./core/src/main/java/cc/redberry/core/tensor/TensorException.java |
| ./NRedberry.Core/Tensors/TensorExtensions.cs | (no match) |
| ./NRedberry.Core/Tensors/TensorFactory.cs | ./core/src/main/java/cc/redberry/core/tensor/TensorFactory.java |
| ./NRedberry.Core/Tensors/TensorField.cs | ./core/src/main/java/cc/redberry/core/tensor/TensorField.java |
| ./NRedberry.Core/Tensors/Split.cs | ./core/src/main/java/cc/redberry/core/tensor/Split.java |
| ./NRedberry.Core/Tensors/BasicTensorIterator.cs | ./core/src/main/java/cc/redberry/core/tensor/BasicTensorIterator.java |
| ./NRedberry.Core/Tensors/ApplyIndexMapping.cs | ./core/src/main/java/cc/redberry/core/tensor/ApplyIndexMapping.java |
| ./NRedberry.Core/Tensors/AbstractSumBuilder.cs | ./core/src/main/java/cc/redberry/core/tensor/AbstractSumBuilder.java |
| ./NRedberry.Core/Indices/InconsistentIndicesException.cs | ./core/src/main/java/cc/redberry/core/indices/InconsistentIndicesException.java |
| ./NRedberry.Core/Indices/IndexType.cs | ./core/src/main/java/cc/redberry/core/indices/IndexType.java |
| ./NRedberry.Core/Indices/Indices.cs | ./core/src/main/java/cc/redberry/core/indices/Indices.java |
| ./NRedberry.Core/Indices/IndicesArraysUtils.cs | (no match) |
| ./NRedberry.Core/Indices/IndicesBuilder.cs | ./core/src/main/java/cc/redberry/core/indices/IndicesBuilder.java |
| ./NRedberry.Core/Indices/IndicesFactory.cs | ./core/src/main/java/cc/redberry/core/indices/IndicesFactory.java |
| ./NRedberry.Core/Indices/IndicesSymmetries.cs | ./core/src/main/java/cc/redberry/core/indices/IndicesSymmetries.java |
| ./NRedberry.Core/Indices/IndicesUtils.cs | ./core/src/main/java/cc/redberry/core/indices/IndicesUtils.java |
| ./NRedberry.Core/Indices/SimpleIndices.cs | ./core/src/main/java/cc/redberry/core/indices/SimpleIndices.java |
| ./NRedberry.Core/Indices/SimpleIndicesAbstract.cs | (no match) |
| ./NRedberry.Core/Indices/SimpleIndicesIsolated.cs | ./core/src/main/java/cc/redberry/core/indices/SimpleIndicesIsolated.java |
| ./NRedberry.Core/Indices/SimpleIndicesOfTensor.cs | ./core/src/main/java/cc/redberry/core/indices/SimpleIndicesOfTensor.java |
| ./NRedberry.Core/Indices/SortedIndices.cs | ./core/src/main/java/cc/redberry/core/indices/SortedIndices.java |
| ./NRedberry.Core/Indices/StructureOfIndices.cs | ./core/src/main/java/cc/redberry/core/indices/StructureOfIndices.java |
| ./NRedberry.Core/Maths/MathUtils.cs | ./core/src/main/java/cc/redberry/core/utils/MathUtils.java |
| ./NRedberry.Core/Numbers/Complex.cs | ./core/src/main/java/cc/redberry/core/number/Complex.java, ./core/src/main/java/cc/redberry/core/transformations/factor/jasfactor/edu/jas/poly/Complex.java |
| ./NRedberry.Core/Numbers/ComplexBuilder.cs | (no match) |
| ./NRedberry.Core/TensorGenerators/GeneratedTensor.cs | ./core/src/main/java/cc/redberry/core/tensorgenerator/GeneratedTensor.java |
| ./NRedberry.Core/Parsers/TokenType.cs | ./core/src/main/java/cc/redberry/core/parser/TokenType.java |
| ./NRedberry.Core/Parsers/ParseTokenTransformer.cs | ./core/src/main/java/cc/redberry/core/parser/ParseTokenTransformer.java |
| ./NRedberry.Core/Parsers/ParseToken.cs | ./core/src/main/java/cc/redberry/core/parser/ParseToken.java |
| ./NRedberry.Core/Parsers/ParserException.cs | ./core/src/main/java/cc/redberry/core/parser/ParserException.java |
| ./NRedberry.Core/Parsers/Parser.cs | ./core/src/main/java/cc/redberry/core/parser/Parser.java |
| ./NRedberry.Core/Indices/EmptySimpleIndices.cs | ./core/src/main/java/cc/redberry/core/indices/EmptySimpleIndices.java |
| ./NRedberry.Core/Parsers/ITokenParser.cs | (no match) |
| ./NRedberry.Core/Numbers/Numeric.cs | ./core/src/main/java/cc/redberry/core/number/Numeric.java |
| ./NRedberry.Core/Numbers/NumberUtils.cs | ./core/src/main/java/cc/redberry/core/number/NumberUtils.java |
| ./NRedberry.Core/Numbers/INumber.cs | (no match) |
| ./NRedberry.Core/Numbers/Exponentiation.cs | ./core/src/main/java/cc/redberry/core/number/Exponentiation.java |
| ./NRedberry.Core/Numbers/ComplexUtils.cs | ./core/src/main/java/cc/redberry/core/number/ComplexUtils.java |
| ./NRedberry.Core/Numbers/ComplexField.cs | ./core/src/main/java/cc/redberry/core/number/ComplexField.java |
| ./NRedberry.Core/Parsers/ContextSettingsExtensions.cs | (no match) |
| ./NRedberry.Core.Utils/SingleIterator.cs | ./core/src/main/java/cc/redberry/core/utils/SingleIterator.java |
