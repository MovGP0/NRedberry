# NRedberry to Redberry Class Mapping

This document lists each NRedberry class (.cs) alongside the Redberry source (.java) it was ported from.

| NRedberry class | Redberry source | Notes |
| --- | --- | --- |
| ./NRedberry.Apache.Commons.Math/BigFraction.cs | (external: Apache Commons Math org/apache/commons/math3/fraction/BigFraction.java) | Ported from Apache Commons Math; original file is not part of ./redberry checkout. |
| ./NRedberry.Apache.Commons.Math/IField.cs | (external: Apache Commons Math org/apache/commons/math3/Field.java) | Field abstraction comes from Apache Commons Math; not bundled in ./redberry. |
| ./NRedberry.Apache.Commons.Math/IFieldElement.cs | (external: Apache Commons Math org/apache/commons/math3/FieldElement.java) | FieldElement interface supplied by Apache Commons Math. |
| ./NRedberry.Core.Combinatorics/Combinatorics.cs | ./core/src/main/java/cc/redberry/core/combinatorics/Combinatorics.java |  |
| ./NRedberry.Core.Combinatorics/Extensions/BitArrayExtensions.cs | ./core/src/main/java/cc/redberry/core/utils/BitArray.java | C# extension wrappers around BitArray operations defined in Java BitArray. |
| ./NRedberry.Core.Combinatorics/Extensions/EnumeratorExtensions.cs | (no matching Java source) | No Java symbol named EnumeratorExtensions; helper introduced for IEnumerable support. |
| ./NRedberry.Core.Combinatorics/IIntCombinatorialGenerator.cs | ./core/src/main/java/cc/redberry/core/combinatorics/IntCombinatorialGenerator.java |  |
| ./NRedberry.Core.Combinatorics/IIntCombinatorialPort.cs | ./core/src/main/java/cc/redberry/core/combinatorics/IntCombinatorialPort.java |  |
| ./NRedberry.Core.Combinatorics/InconsistentGeneratorsException.cs | ./core/src/main/java/cc/redberry/core/combinatorics/InconsistentGeneratorsException.java, ./core/src/main/java/cc/redberry/core/groups/permutations/InconsistentGeneratorsException.java |  |
| ./NRedberry.Core.Combinatorics/IntCombinationPermutationGenerator.cs | ./core/src/main/java/cc/redberry/core/combinatorics/IntCombinationPermutationGenerator.java |  |
| ./NRedberry.Core.Combinatorics/IntCombinationsGenerator.cs | ./core/src/main/java/cc/redberry/core/combinatorics/IntCombinationsGenerator.java |  |
| ./NRedberry.Core.Combinatorics/IntCombinatorialGenerator.cs | ./core/src/main/java/cc/redberry/core/combinatorics/IntCombinatorialGenerator.java |  |
| ./NRedberry.Core.Combinatorics/IntDistinctTuplesPort.cs | ./core/src/main/java/cc/redberry/core/combinatorics/IntDistinctTuplesPort.java |  |
| ./NRedberry.Core.Combinatorics/IntPermutationsGenerator.cs | ./core/src/main/java/cc/redberry/core/combinatorics/IntPermutationsGenerator.java |  |
| ./NRedberry.Core.Combinatorics/IntPermutationsSpanGenerator.cs | (no matching Java source) | String search for IntPermutationsSpanGenerator yielded no hits; likely added during .NET port. |
| ./NRedberry.Core.Combinatorics/IntTuplesPort.cs | ./core/src/main/java/cc/redberry/core/combinatorics/IntTuplesPort.java |  |
| ./NRedberry.Core.Combinatorics/IOutputPortUnsafe.cs | ./core/src/main/java/cc/redberry/core/utils/OutputPort.java | Interfaces with OutputPort nested iterators; Java version uses OutputPort directly without Unsafe wrapper. |
| ./NRedberry.Core.Combinatorics/Permutation.cs | ./core/src/main/java/cc/redberry/core/groups/permutations/Permutation.java |  |
| ./NRedberry.Core.Combinatorics/PermutationPriorityTuple.cs | ./core/src/main/java/cc/redberry/core/combinatorics/IntPriorityPermutationsGenerator.java |  |
| ./NRedberry.Core.Combinatorics/PermutationsSpanIterator.cs | (no matching Java source) | No Java iterator named PermutationsSpanIterator; port-specific composition helper. |
| ./NRedberry.Core.Combinatorics/Symmetries/AbstractSymmetries.cs | ./core/src/main/java/cc/redberry/core/combinatorics/symmetries/AbstractSymmetries.java (missing) | Comment in C# file points to this path; file not present in current ./redberry snapshot. |
| ./NRedberry.Core.Combinatorics/Symmetries/DummySymmetries.cs | ./core/src/main/java/cc/redberry/core/combinatorics/symmetries/DummySymmetries.java (missing) | Referenced upstream path but source absent locally. |
| ./NRedberry.Core.Combinatorics/Symmetries/EmptySymmetries.cs | ./core/src/main/java/cc/redberry/core/combinatorics/symmetries/EmptySymmetries.java (missing) | Original Java file not included in ./redberry checkout. |
| ./NRedberry.Core.Combinatorics/Symmetries/FullSymmetries.cs | ./core/src/test/java/cc/redberry/core/tensorgenerator/TensorGeneratorTest.java |  |
| ./NRedberry.Core.Combinatorics/Symmetries/IntPriorityPermutationsGenerator.cs | ./core/src/main/java/cc/redberry/core/combinatorics/IntPriorityPermutationsGenerator.java |  |
| ./NRedberry.Core.Combinatorics/Symmetries/SymmetriesFactory.cs | ./core/src/main/java/cc/redberry/core/combinatorics/symmetries/SymmetriesFactory.java (missing) | Factory lives in symmetries package upstream; source not present here. |
| ./NRedberry.Core.Combinatorics/Symmetries/SymmetriesImpl.cs | ./core/src/main/java/cc/redberry/core/combinatorics/symmetries/SymmetriesImpl.java (missing) | Original Java implementation referenced in comments but absent. |
| ./NRedberry.Core.Combinatorics/Symmetry.cs | ./core/src/main/java/cc/redberry/core/context/NameDescriptorForTensorFieldDerivative.java, ./core/src/main/java/cc/redberry/core/groups/permutations/Permutation.java, ./core/src/main/java/cc/redberry/core/groups/permutations/PermutationOneLineByte.java, ./core/src/main/java/cc/redberry/core/groups/permutations/PermutationOneLineInt.java, ./core/src/main/java/cc/redberry/core/groups/permutations/PermutationOneLineShort.java, ./core/src/main/java/cc/redberry/core/indexmapping/IndexMappings.java, ./core/src/main/java/cc/redberry/core/indices/IndicesSymmetries.java, ./core/src/main/java/cc/redberry/core/tensor/random/RandomTensor.java, ./core/src/main/java/cc/redberry/core/tensor/Tensors.java, ./core/src/main/java/cc/redberry/core/transformations/EliminateDueSymmetriesTransformation.java, ./core/src/main/java/cc/redberry/core/transformations/symmetrization/SymmetrizeTransformation.java, ./core/src/main/java/cc/redberry/core/transformations/symmetrization/SymmetrizeUpperLowerIndicesTransformation.java, ./core/src/main/java/cc/redberry/core/utils/TensorUtils.java, ./core/src/test/java/cc/redberry/core/indexmapping/EqualsTest.java, ./core/src/test/java/cc/redberry/core/indexmapping/IndexMappingsTest.java, ./core/src/test/java/cc/redberry/core/indices/IndicesTest.java, ./core/src/test/java/cc/redberry/core/parser/ParserTest.java, ./core/src/test/java/cc/redberry/core/solver/InverseTensorTest.java, ./core/src/test/java/cc/redberry/core/tensor/ApplyIndexMappingTest.java, ./core/src/test/java/cc/redberry/core/tensor/HashingStrategyTest.java, ./core/src/test/java/cc/redberry/core/tensor/ProductTest.java, ./core/src/test/java/cc/redberry/core/tensor/ScalarsBackedProductBuilderTest.java, ./core/src/test/java/cc/redberry/core/tensor/StandardFormTest.java, ./core/src/test/java/cc/redberry/core/tensor/SumBuilderTest.java, ./core/src/test/java/cc/redberry/core/tensor/TensorFieldTest.java, ./core/src/test/java/cc/redberry/core/tensorgenerator/TensorGeneratorTest.java, ./core/src/test/java/cc/redberry/core/transformations/DifferentiateTransformationTest.java, ./core/src/test/java/cc/redberry/core/transformations/substitutions/ProductsBijectionsPortTest.java, ./core/src/test/java/cc/redberry/core/transformations/substitutions/SubstitutionsTest.java, ./core/src/test/java/cc/redberry/core/transformations/substitutions/SumBijectionPortTest.java, ./core/src/test/java/cc/redberry/core/transformations/symmetrization/SymmetrizeTransformationTest.java, ./core/src/test/java/cc/redberry/core/utils/TensorUtilsTest.java, ./physics/src/main/java/cc/redberry/physics/feyncalc/LeviCivitaSimplifyTransformation.java, ./physics/src/main/java/cc/redberry/physics/oneloopdiv/Benchmarks.java, ./physics/src/main/java/cc/redberry/physics/oneloopdiv/OneLoopCounterterms.java, ./physics/src/main/java/cc/redberry/physics/oneloopdiv/OneLoopInput.java, ./physics/src/main/java/cc/redberry/physics/oneloopdiv/OneLoopUtils.java, ./physics/src/test/java/cc/redberry/physics/feyncalc/LeviCivitaSimplifyTransformationTest.java, ./physics/src/test/java/cc/redberry/physics/feyncalc/UnitaryTraceTransformationTest.java, ./physics/src/test/java/cc/redberry/physics/oneloopdiv/AveragingTest.java, ./physics/src/test/java/cc/redberry/physics/oneloopdiv/OneLoopCountertermsTest.java |  |
| ./NRedberry.Core.Combinatorics/UnsafeCombinatorics.cs | (no matching Java source) | Utility for unchecked construction has no Java analogue; functionality inlined in Permutation constructors. |
| ./NRedberry.Core.Entities/BitArrayEqualityComparer.cs | ./core/src/main/java/cc/redberry/core/utils/BitArray.java | Equality logic derives from BitArray.equals implementation. |
| ./NRedberry.Core.Entities/BitArrayExtensions.cs | ./core/src/main/java/cc/redberry/core/utils/BitArray.java | Extension helpers mirroring BitArray methods. |
| ./NRedberry.Core.Entities/IIndexSymbolConverter.cs | ./core/src/main/java/cc/redberry/core/context/IndexSymbolConverter.java |  |
| ./NRedberry.Core.Entities/IndexType.cs | ./core/src/main/java/cc/redberry/core/indices/IndexType.java |  |
| ./NRedberry.Core.Entities/INumber.cs | ./core/src/main/java/cc/redberry/core/number/Number.java |  |
| ./NRedberry.Core.Entities/Numeric.cs | ./core/src/main/java/cc/redberry/core/number/Numeric.java |  |
| ./NRedberry.Core.Entities/OutputFormat.cs | ./core/src/main/java/cc/redberry/core/context/OutputFormat.java |  |
| ./NRedberry.Core.Entities/Rational.cs | ./core/src/main/java/cc/redberry/core/number/Rational.java |  |
| ./NRedberry.Core.Entities/RationalExtensions.cs | ./core/src/main/java/cc/redberry/core/number/Rational.java | Adds extension conveniences over Rational static helpers. |
| ./NRedberry.Core.Entities/Real.cs | ./core/src/main/java/cc/redberry/core/number/Real.java |  |
| ./NRedberry.Core.Entities/RealField.cs | ./core/src/main/java/cc/redberry/core/number/RealField.java |  |
| ./NRedberry.Core.Entities/TypeData.cs | ./core/src/main/java/cc/redberry/core/indices/StructureOfIndices.java |  |
| ./NRedberry.Core.Entities/UpperLowerIndices.cs | ./core/src/main/java/cc/redberry/core/indices/AbstractIndices.java |  |
| ./NRedberry.Core.Exceptions/InconsistentGeneratorsException.cs | ./core/src/main/java/cc/redberry/core/combinatorics/InconsistentGeneratorsException.java, ./core/src/main/java/cc/redberry/core/groups/permutations/InconsistentGeneratorsException.java |  |
| ./NRedberry.Core.Exceptions/IndexConverterException.cs | ./core/src/main/java/cc/redberry/core/context/IndexConverterException.java |  |
| ./NRedberry.Core.Utils/Arrays.cs | ./core/src/main/java/cc/redberry/core/combinatorics/Combinatorics.java, ./core/src/main/java/cc/redberry/core/combinatorics/IntDistinctTuplesPort.java, ./core/src/main/java/cc/redberry/core/combinatorics/IntPermutationsGenerator.java, ./core/src/main/java/cc/redberry/core/combinatorics/IntPriorityPermutationsGenerator.java, ./core/src/main/java/cc/redberry/core/combinatorics/IntTuplesPort.java, ./core/src/main/java/cc/redberry/core/context/defaults/GreekLettersConverter.java, ./core/src/main/java/cc/redberry/core/context/NameAndStructureOfIndices.java, ./core/src/main/java/cc/redberry/core/context/NameDescriptor.java, ./core/src/main/java/cc/redberry/core/context/NameDescriptorForMetricAndKronecker.java, ./core/src/main/java/cc/redberry/core/context/NameDescriptorForTensorField.java, ./core/src/main/java/cc/redberry/core/context/NameDescriptorForTensorFieldDerivative.java, ./core/src/main/java/cc/redberry/core/context/NameDescriptorForTensorFieldImpl.java, ./core/src/main/java/cc/redberry/core/context/NameManager.java, ./core/src/main/java/cc/redberry/core/graph/GraphUtils.java, ./core/src/main/java/cc/redberry/core/graph/PrimitiveSubgraph.java, ./core/src/main/java/cc/redberry/core/groups/permutations/AlgorithmsBacktrack.java, ./core/src/main/java/cc/redberry/core/groups/permutations/AlgorithmsBase.java, ./core/src/main/java/cc/redberry/core/groups/permutations/BacktrackSearch.java, ./core/src/main/java/cc/redberry/core/groups/permutations/InducedOrdering.java, ./core/src/main/java/cc/redberry/core/groups/permutations/PermutationGroup.java, ./core/src/main/java/cc/redberry/core/groups/permutations/PermutationOneLineAbstract.java, ./core/src/main/java/cc/redberry/core/groups/permutations/PermutationOneLineByte.java, ./core/src/main/java/cc/redberry/core/groups/permutations/PermutationOneLineInt.java, ./core/src/main/java/cc/redberry/core/groups/permutations/PermutationOneLineShort.java, ./core/src/main/java/cc/redberry/core/groups/permutations/Permutations.java, ./core/src/main/java/cc/redberry/core/groups/permutations/RandomPermutation.java, ./core/src/main/java/cc/redberry/core/groups/permutations/SchreierVector.java, ./core/src/main/java/cc/redberry/core/indexgenerator/IndexGeneratorFromData.java, ./core/src/main/java/cc/redberry/core/indexgenerator/IndexGeneratorImpl.java, ./core/src/main/java/cc/redberry/core/indexgenerator/IntGenerator.java, ./core/src/main/java/cc/redberry/core/indexmapping/FromToHolder.java, ./core/src/main/java/cc/redberry/core/indexmapping/IndexMappingBufferImpl.java, ./core/src/main/java/cc/redberry/core/indexmapping/IndexMappingBufferTester.java, ./core/src/main/java/cc/redberry/core/indexmapping/Mapping.java, ./core/src/main/java/cc/redberry/core/indexmapping/ProviderProduct.java, ./core/src/main/java/cc/redberry/core/indices/AbstractIndices.java, ./core/src/main/java/cc/redberry/core/indices/AbstractSimpleIndices.java, ./core/src/main/java/cc/redberry/core/indices/IndicesUtils.java, ./core/src/main/java/cc/redberry/core/indices/SimpleIndicesBuilder.java, ./core/src/main/java/cc/redberry/core/indices/SortedIndices.java, ./core/src/main/java/cc/redberry/core/indices/StructureOfIndices.java, ./core/src/main/java/cc/redberry/core/parser/Parser.java, ./core/src/main/java/cc/redberry/core/parser/ParserPowerAst.java, ./core/src/main/java/cc/redberry/core/parser/ParseToken.java, ./core/src/main/java/cc/redberry/core/parser/preprocessor/GeneralIndicesInsertion.java, ./core/src/main/java/cc/redberry/core/parser/preprocessor/IndicesInsertion.java, ./core/src/main/java/cc/redberry/core/parser/preprocessor/TypesAndNamesTransformer.java, ./core/src/main/java/cc/redberry/core/solver/ExternalSolver.java, ./core/src/main/java/cc/redberry/core/solver/frobenius/FrobeniusNumber.java, ./core/src/main/java/cc/redberry/core/solver/frobenius/FrobeniusSolver.java, ./core/src/main/java/cc/redberry/core/solver/InverseTensor.java, ./core/src/main/java/cc/redberry/core/solver/ReduceEngine.java, ./core/src/main/java/cc/redberry/core/tensor/AbstractSumBuilder.java, ./core/src/main/java/cc/redberry/core/tensor/ApplyIndexMapping.java, ./core/src/main/java/cc/redberry/core/tensor/HashingStrategy.java, ./core/src/main/java/cc/redberry/core/tensor/playground/Algorithm0.java, ./core/src/main/java/cc/redberry/core/tensor/playground/Algorithm1.java, ./core/src/main/java/cc/redberry/core/tensor/playground/Algorithm2.java, ./core/src/main/java/cc/redberry/core/tensor/playground/Algorithm3.java, ./core/src/main/java/cc/redberry/core/tensor/playground/GraphStructure.java, ./core/src/main/java/cc/redberry/core/tensor/playground/ProductData.java, ./core/src/main/java/cc/redberry/core/tensor/Product.java, ./core/src/main/java/cc/redberry/core/tensor/ProductBuilder.java, ./core/src/main/java/cc/redberry/core/tensor/ProductContent.java, ./core/src/main/java/cc/redberry/core/tensor/ProductFactory.java, ./core/src/main/java/cc/redberry/core/tensor/random/RandomTensor.java, ./core/src/main/java/cc/redberry/core/tensor/Split.java, ./core/src/main/java/cc/redberry/core/tensor/StructureOfContractions.java, ./core/src/main/java/cc/redberry/core/tensor/Sum.java, ./core/src/main/java/cc/redberry/core/tensor/TensorContraction.java, ./core/src/main/java/cc/redberry/core/tensor/TensorException.java, ./core/src/main/java/cc/redberry/core/tensor/TensorField.java, ./core/src/main/java/cc/redberry/core/tensorgenerator/TensorGenerator.java, ./core/src/main/java/cc/redberry/core/tensorgenerator/TensorGeneratorUtils.java, ./core/src/main/java/cc/redberry/core/transformations/collect/CollectTransformation.java, ./core/src/main/java/cc/redberry/core/transformations/DifferentiateTransformation.java, ./core/src/main/java/cc/redberry/core/transformations/EliminateMetricsTransformation.java, ./core/src/main/java/cc/redberry/core/transformations/expand/ExpandPort.java, ./core/src/main/java/cc/redberry/core/transformations/expand/ExpandUtils.java, ./core/src/main/java/cc/redberry/core/transformations/ExpandAndEliminateTransformation.java, ./core/src/main/java/cc/redberry/core/transformations/ExpandTensorsAndEliminateTransformation.java, ./core/src/main/java/cc/redberry/core/transformations/factor/JasFactor.java, ./core/src/main/java/cc/redberry/core/transformations/factor/jasfactor/edu/jas/poly/ExpVector.java, ./core/src/main/java/cc/redberry/core/transformations/factor/jasfactor/edu/jas/poly/GenPolynomialRing.java, ./core/src/main/java/cc/redberry/core/transformations/factor/jasfactor/edu/jas/poly/OrderedPolynomialList.java, ./core/src/main/java/cc/redberry/core/transformations/factor/jasfactor/edu/jas/poly/TermOrder.java, ./core/src/main/java/cc/redberry/core/transformations/factor/jasfactor/edu/jas/poly/TermOrderOptimization.java, ./core/src/main/java/cc/redberry/core/transformations/powerexpand/PowerExpandUtils.java, ./core/src/main/java/cc/redberry/core/transformations/reverse/SingleReverse.java, ./core/src/main/java/cc/redberry/core/transformations/substitutions/PrimitiveProductSubstitution.java, ./core/src/main/java/cc/redberry/core/transformations/substitutions/PrimitiveSumSubstitution.java, ./core/src/main/java/cc/redberry/core/transformations/substitutions/ProductsBijectionsPort.java, ./core/src/main/java/cc/redberry/core/transformations/substitutions/SubstitutionIterator.java, ./core/src/main/java/cc/redberry/core/transformations/substitutions/SubstitutionTransformation.java, ./core/src/main/java/cc/redberry/core/transformations/substitutions/SumBijectionPort.java, ./core/src/main/java/cc/redberry/core/transformations/symmetrization/SymmetrizeTransformation.java, ./core/src/main/java/cc/redberry/core/transformations/symmetrization/SymmetrizeUpperLowerIndicesTransformation.java, ./core/src/main/java/cc/redberry/core/transformations/TransformationCollection.java, ./core/src/main/java/cc/redberry/core/utils/ArraysUtils.java, ./core/src/main/java/cc/redberry/core/utils/BitArray.java, ./core/src/main/java/cc/redberry/core/utils/IntArray.java, ./core/src/main/java/cc/redberry/core/utils/IntArrayList.java, ./core/src/main/java/cc/redberry/core/utils/IntTimSort.java, ./core/src/main/java/cc/redberry/core/utils/MathUtils.java, ./core/src/main/java/cc/redberry/core/utils/TensorUtils.java, ./core/src/main/java/cc/redberry/core/utils/TimingStatistics.java, ./core/src/test/java/cc/redberry/core/combinatorics/IntCombinationPermutationGeneratorTest.java, ./core/src/test/java/cc/redberry/core/combinatorics/IntCombinationsGeneratorTest.java, ./core/src/test/java/cc/redberry/core/combinatorics/IntDistinctTuplesPortTest.java, ./core/src/test/java/cc/redberry/core/combinatorics/IntPriorityPermutationsGeneratorTest.java, ./core/src/test/java/cc/redberry/core/combinatorics/IntTuplesPortTest.java, ./core/src/test/java/cc/redberry/core/graph/PrimitiveSubgraphPartitionTest.java, ./core/src/test/java/cc/redberry/core/groups/permutations/AbstractPermutationTest.java, ./core/src/test/java/cc/redberry/core/groups/permutations/AlgorithmsBacktrackTest.java, ./core/src/test/java/cc/redberry/core/groups/permutations/AlgorithmsBaseTest.java, ./core/src/test/java/cc/redberry/core/groups/permutations/BacktrackSearchTest.java, ./core/src/test/java/cc/redberry/core/groups/permutations/InducedOrderingTest.java, ./core/src/test/java/cc/redberry/core/groups/permutations/PermutationGroupTest.java, ./core/src/test/java/cc/redberry/core/groups/permutations/PermutationOneLineIntTest.java, ./core/src/test/java/cc/redberry/core/groups/permutations/PermutationsTest.java, ./core/src/test/java/cc/redberry/core/groups/permutations/PermutationsTestUtils.java, ./core/src/test/java/cc/redberry/core/groups/permutations/RandomPermutationTest.java, ./core/src/test/java/cc/redberry/core/indexmapping/IndexMappingsTest.java, ./core/src/test/java/cc/redberry/core/indices/IndicesTest.java, ./core/src/test/java/cc/redberry/core/solver/frobenius/FrobeniusSolverTest.java, ./core/src/test/java/cc/redberry/core/solver/frobenius/FrobeniusUtils.java, ./core/src/test/java/cc/redberry/core/solver/InverseTensorTest.java, ./core/src/test/java/cc/redberry/core/solver/ReduceEngineTest.java, ./core/src/test/java/cc/redberry/core/tensor/ApplyIndexMappingTest.java, ./core/src/test/java/cc/redberry/core/tensor/HashingStrategyTest.java, ./core/src/test/java/cc/redberry/core/tensor/iterator/FromChildToParentIteratorTest.java, ./core/src/test/java/cc/redberry/core/tensor/iterator/FromParentToChildIteratorTest.java, ./core/src/test/java/cc/redberry/core/tensor/ProductTest.java, ./core/src/test/java/cc/redberry/core/tensor/StructureOfContractionsTest.java, ./core/src/test/java/cc/redberry/core/tensor/SumTest.java, ./core/src/test/java/cc/redberry/core/tensor/TensorFieldTest.java, ./core/src/test/java/cc/redberry/core/tensor/ToStringTest.java, ./core/src/test/java/cc/redberry/core/tensorgenerator/TensorGeneratorTest.java, ./core/src/test/java/cc/redberry/core/transformations/collect/CollectTransformationTest.java, ./core/src/test/java/cc/redberry/core/transformations/factor/JasFactorTest.java, ./core/src/test/java/cc/redberry/core/transformations/powerexpand/PowerExpandTransformationTest.java, ./core/src/test/java/cc/redberry/core/transformations/substitutions/ProductsBijectionsPortTest.java, ./core/src/test/java/cc/redberry/core/utils/ArraysUtilsTest.java, ./core/src/test/java/cc/redberry/core/utils/BitArrayTest.java, ./core/src/test/java/cc/redberry/core/utils/TensorUtilsTest.java, ./physics/src/main/java/cc/redberry/physics/feyncalc/AbstractTransformationWithGammas.java, ./physics/src/main/java/cc/redberry/physics/feyncalc/DiracOrderTransformation.java, ./physics/src/main/java/cc/redberry/physics/feyncalc/DiracSimplify0.java, ./physics/src/main/java/cc/redberry/physics/feyncalc/PassarinoVeltman.java, ./physics/src/main/java/cc/redberry/physics/feyncalc/SpinorsSimplifyTransformation.java, ./physics/src/main/java/cc/redberry/physics/feyncalc/UnitaryTraceTransformation.java, ./physics/src/main/java/cc/redberry/physics/oneloopdiv/OneLoopCounterterms.java, ./physics/src/main/java/cc/redberry/physics/oneloopdiv/OneLoopInput.java, ./physics/src/main/java/cc/redberry/physics/oneloopdiv/SqrSubs.java, ./physics/src/test/java/cc/redberry/physics/oneloopdiv/OneLoopCountertermsTest.java |  |
| ./NRedberry.Core.Utils/ArraysUtils.cs | ./core/src/main/java/cc/redberry/core/utils/ArraysUtils.java |  |
| ./NRedberry.Core.Utils/ArraysUtils.QuickSort.cs | ./core/src/main/java/cc/redberry/core/utils/ArraysUtils.java |  |
| ./NRedberry.Core.Utils/ArraysUtils.Swap.cs | ./core/src/main/java/cc/redberry/core/utils/ArraysUtils.java |  |
| ./NRedberry.Core.Utils/ArrayUtils.InsertionSort.cs | ./core/src/main/java/cc/redberry/core/utils/ArraysUtils.java |  |
| ./NRedberry.Core.Utils/BinaryToStringConverter.cs | ./core/src/main/java/cc/redberry/core/utils/ToStringConverter.java | Java ToStringConverter exposes BINARY formatter implemented inline. |
| ./NRedberry.Core.Utils/BitArrayExtensions.cs | ./core/src/main/java/cc/redberry/core/utils/BitArray.java | Extension helpers for BitArray. |
| ./NRedberry.Core.Utils/ByteBackedBitArray.cs | ./core/src/main/java/cc/redberry/core/utils/BitArray.java | Java BitArray is int-backed; this placeholder keeps API parity. |
| ./NRedberry.Core.Utils/DefaultToStringConverter.cs | ./core/src/main/java/cc/redberry/core/utils/ToStringConverter.java | Default formatter matches anonymous inner class in ToStringConverter. |
| ./NRedberry.Core.Utils/EnumerableEx.cs | ./core/src/main/java/cc/redberry/core/utils/HashFunctions.java | Hash aggregation mirrors HashFunctions.hash implementation. |
| ./NRedberry.Core.Utils/HashFunctions.cs | ./core/src/main/java/cc/redberry/core/utils/HashFunctions.java |  |
| ./NRedberry.Core.Utils/HexToStringConverter.cs | ./core/src/main/java/cc/redberry/core/utils/ToStringConverter.java | Hex converter corresponds to HEX formatter in ToStringConverter. |
| ./NRedberry.Core.Utils/IBitArray.cs | ./core/src/main/java/cc/redberry/core/utils/BitArray.java |  |
| ./NRedberry.Core.Utils/Indicator.cs | ./core/src/main/java/cc/redberry/core/utils/Indicator.java |  |
| ./NRedberry.Core.Utils/IntArray.cs | ./core/src/main/java/cc/redberry/core/utils/IntArray.java |  |
| ./NRedberry.Core.Utils/IntArrayList.cs | ./core/src/main/java/cc/redberry/core/utils/IntArrayList.java |  |
| ./NRedberry.Core.Utils/IntTimSort.cs | ./core/src/main/java/cc/redberry/core/utils/IntTimSort.java |  |
| ./NRedberry.Core.Utils/IToStringConverter.cs | ./core/src/main/java/cc/redberry/core/utils/ToStringConverter.java |  |
| ./NRedberry.Core.Utils/MathUtils.cs | ./core/src/main/java/cc/redberry/core/utils/MathUtils.java |  |
| ./NRedberry.Core.Utils/SingleIterator.cs | ./core/src/main/java/cc/redberry/core/utils/SingleIterator.java |  |
| ./NRedberry.Core/Concurrent/IOutputPort.cs | ./core/src/main/java/cc/redberry/core/utils/OutputPort.java |  |
| ./NRedberry.Core/Concurrent/IOutputPortUnsafe.cs | ./core/src/main/java/cc/redberry/core/utils/OutputPort.java | Unsafe port interface mimics OutputPort.PortIterator usage. |
| ./NRedberry.Core/Concurrent/PortEnumerator.cs | ./core/src/main/java/cc/redberry/core/utils/OutputPort.java | Wraps OutputPort.PortIterator from Java. |
| ./NRedberry.Core/Concurrent/Singleton.cs | ./core/src/main/java/cc/redberry/core/utils/OutputPort.java |  |
| ./NRedberry.Core/Contexts/ArrayExtensions.cs | (no matching Java source) | Java relied on java.util.Arrays.fill; helper added for .NET arrays. |
| ./NRedberry.Core/Contexts/CC.cs | ./core/src/main/java/cc/redberry/core/context/CC.java |  |
| ./NRedberry.Core/Contexts/Context.cs | ./core/src/main/java/cc/redberry/core/context/Context.java |  |
| ./NRedberry.Core/Contexts/ContextEvent.cs | ./core/src/main/java/cc/redberry/core/context/ContextEvent.java |  |
| ./NRedberry.Core/Contexts/ContextSettings.cs | ./core/src/main/java/cc/redberry/core/context/ContextSettings.java |  |
| ./NRedberry.Core/Contexts/Defaults/DefaultContextFactory.cs | ./core/src/main/java/cc/redberry/core/context/defaults/DefaultContextFactory.java |  |
| ./NRedberry.Core/Contexts/Defaults/DefaultContextSettings.cs | ./core/src/main/java/cc/redberry/core/context/defaults/DefaultContextSettings.java |  |
| ./NRedberry.Core/Contexts/Defaults/GreekLaTeXLowerCaseConverter.cs | ./core/src/main/java/cc/redberry/core/context/defaults/GreekLaTeXLowerCaseConverter.java |  |
| ./NRedberry.Core/Contexts/Defaults/GreekLaTeXUpperCaseConverter.cs | ./core/src/main/java/cc/redberry/core/context/defaults/GreekLaTeXUpperCaseConverter.java |  |
| ./NRedberry.Core/Contexts/Defaults/IContextFactory.cs | ./core/src/main/java/cc/redberry/core/context/ContextFactory.java |  |
| ./NRedberry.Core/Contexts/Defaults/IndexConverterExtender.cs | ./core/src/main/java/cc/redberry/core/context/defaults/IndexConverterExtender.java |  |
| ./NRedberry.Core/Contexts/Defaults/IndexWithStrokeConverter.cs | ./core/src/main/java/cc/redberry/core/context/defaults/IndexWithStrokeConverter.java |  |
| ./NRedberry.Core/Contexts/Defaults/LatinLowerCaseConverter.cs | ./core/src/main/java/cc/redberry/core/context/defaults/LatinLowerCaseConverter.java |  |
| ./NRedberry.Core/Contexts/Defaults/LatinUpperCaseConverter.cs | ./core/src/main/java/cc/redberry/core/context/defaults/LatinUpperCaseConverter.java |  |
| ./NRedberry.Core/Contexts/Defaults/SymbolArrayConverter.cs | ./core/src/main/java/cc/redberry/core/context/defaults/SymbolArrayConverter.java (missing) | Linked to upstream file; not in local snapshot. |
| ./NRedberry.Core/Contexts/IContextListener.cs | ./core/src/main/java/cc/redberry/core/context/ContextListener.java |  |
| ./NRedberry.Core/Contexts/IndexConverterManager.cs | ./core/src/main/java/cc/redberry/core/context/IndexConverterManager.java |  |
| ./NRedberry.Core/Contexts/IndexSymbolConverter.cs | ./core/src/main/java/cc/redberry/core/context/IndexSymbolConverter.java |  |
| ./NRedberry.Core/Contexts/LongBackedBitArray.cs | (no matching Java source) | No Java LongBackedBitArray class found; C# helper around IntArray. |
| ./NRedberry.Core/Contexts/LongExtensions.cs | (no matching Java source) | Convenience extension for ulong math; Java used primitive long operations inline. |
| ./NRedberry.Core/Contexts/NameAndStructureOfIndices.cs | ./core/src/main/java/cc/redberry/core/context/NameAndStructureOfIndices.java |  |
| ./NRedberry.Core/Contexts/NameDescriptor.cs | ./core/src/main/java/cc/redberry/core/context/NameDescriptor.java |  |
| ./NRedberry.Core/Contexts/NameDescriptorForMetricAndKronecker.cs | ./core/src/main/java/cc/redberry/core/context/NameDescriptorForMetricAndKronecker.java |  |
| ./NRedberry.Core/Contexts/NameDescriptorForSimpleTensor.cs | ./core/src/main/java/cc/redberry/core/context/NameDescriptorForSimpleTensor.java |  |
| ./NRedberry.Core/Contexts/NameDescriptorForTensorField.cs | ./core/src/main/java/cc/redberry/core/context/NameDescriptorForTensorField.java |  |
| ./NRedberry.Core/Contexts/NameDescriptorForTensorFieldDerivative.cs | ./core/src/main/java/cc/redberry/core/context/NameDescriptorForTensorFieldDerivative.java |  |
| ./NRedberry.Core/Contexts/NameDescriptorForTensorFieldImpl.cs | ./core/src/main/java/cc/redberry/core/context/NameDescriptorForTensorFieldImpl.java |  |
| ./NRedberry.Core/Contexts/NameManager.cs | ./core/src/main/java/cc/redberry/core/context/NameManager.java |  |
| ./NRedberry.Core/Contexts/OutputFormat.cs | ./core/src/main/java/cc/redberry/core/context/OutputFormat.java |  |
| ./NRedberry.Core/Contexts/ParseManager.cs | ./core/src/main/java/cc/redberry/core/parser/ParseManager.java |  |
| ./NRedberry.Core/Contexts/ULongExtensions.cs | (no matching Java source) | Unsigned helper added for .NET; no Java analogue. |
| ./NRedberry.Core/Globals.cs | (no matching Java source) | C# global using with no Java analogue. |
| ./NRedberry.Core/Graphs/GraphType.cs | ./core/src/main/java/cc/redberry/core/graph/GraphType.java |  |
| ./NRedberry.Core/Graphs/GraphUtils.cs | ./core/src/main/java/cc/redberry/core/graph/GraphUtils.java |  |
| ./NRedberry.Core/Graphs/PrimitiveSubgraph.cs | ./core/src/main/java/cc/redberry/core/graph/PrimitiveSubgraph.java |  |
| ./NRedberry.Core/Graphs/PrimitiveSubgraphPartition.cs | ./core/src/main/java/cc/redberry/core/graph/PrimitiveSubgraphPartition.java |  |
| ./NRedberry.Core/Groups/AlgorithmsBacktrack.cs | ./core/src/main/java/cc/redberry/core/groups/permutations/AlgorithmsBacktrack.java |  |
| ./NRedberry.Core/Groups/AlgorithmsBase.cs | ./core/src/main/java/cc/redberry/core/groups/permutations/AlgorithmsBase.java |  |
| ./NRedberry.Core/Groups/BacktrackSearch.cs | ./core/src/main/java/cc/redberry/core/groups/permutations/BacktrackSearch.java |  |
| ./NRedberry.Core/Groups/BacktrackSearchPayload.cs | ./core/src/main/java/cc/redberry/core/groups/permutations/BacktrackSearchPayload.java |  |
| ./NRedberry.Core/Groups/BacktrackSearchTestFunction.cs | ./core/src/main/java/cc/redberry/core/groups/permutations/BacktrackSearchTestFunction.java |  |
| ./NRedberry.Core/Groups/BruteForcePermutationIterator.cs | ./core/src/main/java/cc/redberry/core/groups/permutations/BruteForcePermutationIterator.java |  |
| ./NRedberry.Core/Groups/BSGSCandidateElement.cs | ./core/src/main/java/cc/redberry/core/groups/permutations/BSGSCandidateElement.java |  |
| ./NRedberry.Core/Groups/BSGSElement.cs | ./core/src/main/java/cc/redberry/core/groups/permutations/BSGSElement.java |  |
| ./NRedberry.Core/Groups/InducedOrdering.cs | ./core/src/main/java/cc/redberry/core/groups/permutations/InducedOrdering.java |  |
| ./NRedberry.Core/Groups/InducedOrderingOfPermutations.cs | ./core/src/main/java/cc/redberry/core/groups/permutations/InducedOrderingOfPermutations.java |  |
| ./NRedberry.Core/Groups/Permutation.cs | ./core/src/main/java/cc/redberry/core/groups/permutations/Permutation.java |  |
| ./NRedberry.Core/Groups/PermutationGroup.cs | ./core/src/main/java/cc/redberry/core/groups/permutations/PermutationGroup.java |  |
| ./NRedberry.Core/Groups/PermutationOneLineAbstract.cs | ./core/src/main/java/cc/redberry/core/groups/permutations/PermutationOneLineAbstract.java |  |
| ./NRedberry.Core/Groups/PermutationOneLineByte.cs | ./core/src/main/java/cc/redberry/core/groups/permutations/PermutationOneLineByte.java |  |
| ./NRedberry.Core/Groups/PermutationOneLineInt.cs | ./core/src/main/java/cc/redberry/core/groups/permutations/PermutationOneLineInt.java |  |
| ./NRedberry.Core/Groups/PermutationOneLineShort.cs | ./core/src/main/java/cc/redberry/core/groups/permutations/PermutationOneLineShort.java |  |
| ./NRedberry.Core/Groups/Permutations.cs | ./core/src/main/java/cc/redberry/core/groups/permutations/Permutations.java |  |
| ./NRedberry.Core/Groups/Permutations.LengthsOfCycles.cs | ./core/src/main/java/cc/redberry/core/groups/permutations/Permutations.java |  |
| ./NRedberry.Core/Groups/RandomPermutation.cs | ./core/src/main/java/cc/redberry/core/groups/permutations/RandomPermutation.java |  |
| ./NRedberry.Core/Groups/SchreierVector.cs | ./core/src/main/java/cc/redberry/core/groups/permutations/SchreierVector.java |  |
| ./NRedberry.Core/ICloneable`1.cs | ./core/src/main/java/cc/redberry/core/transformations/factor/jasfactor/edu/jas/poly/ExpVector.java, ./core/src/main/java/cc/redberry/core/transformations/factor/jasfactor/edu/jas/structure/Element.java, ./physics/src/main/java/cc/redberry/physics/feyncalc/DiracOptions.java |  |
| ./NRedberry.Core/IndexGeneration/IndexGenerator.cs | ./core/src/main/java/cc/redberry/core/indexgenerator/IndexGenerator.java |  |
| ./NRedberry.Core/IndexGeneration/IntGenerator.cs | ./core/src/main/java/cc/redberry/core/indexgenerator/IntGenerator.java |  |
| ./NRedberry.Core/Indices/AbstractIndices.cs | ./core/src/main/java/cc/redberry/core/indices/AbstractIndices.java |  |
| ./NRedberry.Core/Indices/EmptyIndices.cs | ./core/src/main/java/cc/redberry/core/indices/EmptyIndices.java |  |
| ./NRedberry.Core/Indices/EmptySimpleIndices.cs | ./core/src/main/java/cc/redberry/core/indices/EmptySimpleIndices.java |  |
| ./NRedberry.Core/Indices/IIndexMapping.cs | ./core/src/main/java/cc/redberry/core/indexmapping/IndexMapping.java |  |
| ./NRedberry.Core/Indices/InconsistentIndicesException.cs | ./core/src/main/java/cc/redberry/core/indices/InconsistentIndicesException.java |  |
| ./NRedberry.Core/Indices/IndexType.cs | ./core/src/main/java/cc/redberry/core/indices/IndexType.java |  |
| ./NRedberry.Core/Indices/Indices.cs | ./core/src/main/java/cc/redberry/core/indices/Indices.java |  |
| ./NRedberry.Core/Indices/IndicesArraysUtils.cs | ./core/src/main/java/cc/redberry/core/indices/IndicesUtils.java | Wraps array copy helpers hosted in IndicesUtils. |
| ./NRedberry.Core/Indices/IndicesBuilder.cs | ./core/src/main/java/cc/redberry/core/indices/IndicesBuilder.java |  |
| ./NRedberry.Core/Indices/IndicesFactory.cs | ./core/src/main/java/cc/redberry/core/indices/IndicesFactory.java |  |
| ./NRedberry.Core/Indices/IndicesSymmetries.cs | ./core/src/main/java/cc/redberry/core/indices/IndicesSymmetries.java |  |
| ./NRedberry.Core/Indices/IndicesUtils.cs | ./core/src/main/java/cc/redberry/core/indices/IndicesUtils.java |  |
| ./NRedberry.Core/Indices/SimpleIndices.cs | ./core/src/main/java/cc/redberry/core/indices/SimpleIndices.java |  |
| ./NRedberry.Core/Indices/SimpleIndicesAbstract.cs | ./core/src/main/java/cc/redberry/core/indices/AbstractSimpleIndices.java | Maps to AbstractSimpleIndices.java (name inverted). |
| ./NRedberry.Core/Indices/SimpleIndicesIsolated.cs | ./core/src/main/java/cc/redberry/core/indices/SimpleIndicesIsolated.java |  |
| ./NRedberry.Core/Indices/SimpleIndicesOfTensor.cs | ./core/src/main/java/cc/redberry/core/indices/SimpleIndicesOfTensor.java |  |
| ./NRedberry.Core/Indices/SortedIndices.cs | ./core/src/main/java/cc/redberry/core/indices/SortedIndices.java |  |
| ./NRedberry.Core/Indices/StructureOfIndices.cs | ./core/src/main/java/cc/redberry/core/indices/StructureOfIndices.java |  |
| ./NRedberry.Core/Maths/MathUtils.cs | ./core/src/main/java/cc/redberry/core/utils/MathUtils.java |  |
| ./NRedberry.Core/Numbers/Complex.cs | ./core/src/main/java/cc/redberry/core/number/Complex.java, ./core/src/main/java/cc/redberry/core/transformations/factor/jasfactor/edu/jas/poly/Complex.java |  |
| ./NRedberry.Core/Numbers/ComplexBuilder.cs | ./core/src/main/java/cc/redberry/core/number/Complex.java |  |
| ./NRedberry.Core/Numbers/ComplexField.cs | ./core/src/main/java/cc/redberry/core/number/ComplexField.java |  |
| ./NRedberry.Core/Numbers/ComplexUtils.cs | ./core/src/main/java/cc/redberry/core/number/ComplexUtils.java |  |
| ./NRedberry.Core/Numbers/Exponentiation.cs | ./core/src/main/java/cc/redberry/core/number/Exponentiation.java |  |
| ./NRedberry.Core/Numbers/INumber.cs | ./core/src/main/java/cc/redberry/core/number/Number.java |  |
| ./NRedberry.Core/Numbers/NumberUtils.cs | ./core/src/main/java/cc/redberry/core/number/NumberUtils.java |  |
| ./NRedberry.Core/Numbers/Numeric.cs | ./core/src/main/java/cc/redberry/core/number/Numeric.java |  |
| ./NRedberry.Core/Parsers/ContextSettingsExtensions.cs | (no matching Java source) | C# extension returning default parser; Java accessed static field directly. |
| ./NRedberry.Core/Parsers/ITokenParser.cs | ./core/src/main/java/cc/redberry/core/number/parser/TokenParser.java, ./core/src/main/java/cc/redberry/core/parser/TokenParser.java |  |
| ./NRedberry.Core/Parsers/Parser.cs | ./core/src/main/java/cc/redberry/core/parser/Parser.java |  |
| ./NRedberry.Core/Parsers/ParserException.cs | ./core/src/main/java/cc/redberry/core/parser/ParserException.java |  |
| ./NRedberry.Core/Parsers/ParseToken.cs | ./core/src/main/java/cc/redberry/core/parser/ParseToken.java |  |
| ./NRedberry.Core/Parsers/ParseTokenTransformer.cs | ./core/src/main/java/cc/redberry/core/parser/ParseTokenTransformer.java |  |
| ./NRedberry.Core/Parsers/TokenType.cs | ./core/src/main/java/cc/redberry/core/parser/TokenType.java |  |
| ./NRedberry.Core/TensorGenerators/GeneratedTensor.cs | ./core/src/main/java/cc/redberry/core/tensorgenerator/GeneratedTensor.java |  |
| ./NRedberry.Core/Tensors/AbstractSumBuilder.cs | ./core/src/main/java/cc/redberry/core/tensor/AbstractSumBuilder.java |  |
| ./NRedberry.Core/Tensors/ApplyIndexMapping.cs | ./core/src/main/java/cc/redberry/core/tensor/ApplyIndexMapping.java |  |
| ./NRedberry.Core/Tensors/BasicTensorIterator.cs | ./core/src/main/java/cc/redberry/core/tensor/BasicTensorIterator.java |  |
| ./NRedberry.Core/Tensors/CC.cs | ./core/src/main/java/cc/redberry/core/context/CC.java |  |
| ./NRedberry.Core/Tensors/ComplexSumBuilder.cs | ./core/src/main/java/cc/redberry/core/tensor/Split.java |  |
| ./NRedberry.Core/Tensors/FactorNode.cs | ./core/src/main/java/cc/redberry/core/tensor/FactorNode.java, ./core/src/main/java/cc/redberry/core/transformations/factor/FactorTransformation.java |  |
| ./NRedberry.Core/Tensors/Functions/ArcCos.cs | ./core/src/main/java/cc/redberry/core/tensor/functions/ArcCos.java |  |
| ./NRedberry.Core/Tensors/Functions/ArcCosFactory.cs | ./core/src/main/java/cc/redberry/core/tensor/functions/ArcCos.java |  |
| ./NRedberry.Core/Tensors/Functions/ArcCot.cs | ./core/src/main/java/cc/redberry/core/tensor/functions/ArcCot.java |  |
| ./NRedberry.Core/Tensors/Functions/ArcCotFactory.cs | ./core/src/main/java/cc/redberry/core/tensor/functions/ArcCot.java |  |
| ./NRedberry.Core/Tensors/Functions/ArcSin.cs | ./core/src/main/java/cc/redberry/core/tensor/functions/ArcSin.java |  |
| ./NRedberry.Core/Tensors/Functions/ArcSinFactory.cs | ./core/src/main/java/cc/redberry/core/tensor/functions/ArcSin.java |  |
| ./NRedberry.Core/Tensors/Functions/ArcTan.cs | ./core/src/main/java/cc/redberry/core/tensor/functions/ArcTan.java |  |
| ./NRedberry.Core/Tensors/Functions/ArcTanFactory.cs | ./core/src/main/java/cc/redberry/core/tensor/functions/ArcTan.java |  |
| ./NRedberry.Core/Tensors/Functions/ContextManager.cs | ./core/src/main/java/cc/redberry/core/context/ContextManager.java |  |
| ./NRedberry.Core/Tensors/Functions/Cos.cs | ./core/src/main/java/cc/redberry/core/tensor/functions/Cos.java |  |
| ./NRedberry.Core/Tensors/Functions/CosFactory.cs | ./core/src/main/java/cc/redberry/core/tensor/functions/Cos.java |  |
| ./NRedberry.Core/Tensors/Functions/Cot.cs | ./core/src/main/java/cc/redberry/core/tensor/functions/Cot.java |  |
| ./NRedberry.Core/Tensors/Functions/CotFactory.cs | ./core/src/main/java/cc/redberry/core/tensor/functions/Cot.java |  |
| ./NRedberry.Core/Tensors/Functions/Exp.cs | ./core/src/main/java/cc/redberry/core/tensor/functions/Exp.java |  |
| ./NRedberry.Core/Tensors/Functions/ExpFactory.cs | ./core/src/main/java/cc/redberry/core/tensor/functions/Exp.java |  |
| ./NRedberry.Core/Tensors/Functions/Log.cs | ./core/src/main/java/cc/redberry/core/tensor/functions/Log.java |  |
| ./NRedberry.Core/Tensors/Functions/LogFactory.cs | ./core/src/main/java/cc/redberry/core/tensor/functions/Log.java |  |
| ./NRedberry.Core/Tensors/Functions/ScalarFunction.cs | ./core/src/main/java/cc/redberry/core/tensor/functions/ScalarFunction.java |  |
| ./NRedberry.Core/Tensors/Functions/ScalarFunctionBuilder.cs | ./core/src/main/java/cc/redberry/core/tensor/functions/ScalarFunctionBuilder.java |  |
| ./NRedberry.Core/Tensors/Functions/ScalarFunctionFactory.cs | ./core/src/main/java/cc/redberry/core/tensor/functions/ScalarFunctionFactory.java |  |
| ./NRedberry.Core/Tensors/Functions/Sin.cs | ./core/src/main/java/cc/redberry/core/tensor/functions/Sin.java |  |
| ./NRedberry.Core/Tensors/Functions/SinFactory.cs | ./core/src/main/java/cc/redberry/core/tensor/functions/Sin.java |  |
| ./NRedberry.Core/Tensors/Functions/Tan.cs | ./core/src/main/java/cc/redberry/core/tensor/functions/Tan.java |  |
| ./NRedberry.Core/Tensors/Functions/TanFactory.cs | ./core/src/main/java/cc/redberry/core/tensor/functions/Tan.java |  |
| ./NRedberry.Core/Tensors/IndexMapper.cs | ./core/src/main/java/cc/redberry/core/parser/preprocessor/IndicesInsertion.java, ./core/src/main/java/cc/redberry/core/tensor/ApplyIndexMapping.java |  |
| ./NRedberry.Core/Tensors/Iterators/AllTraverseGuide.cs | ./core/src/main/java/cc/redberry/core/tensor/iterator/TraverseGuide.java | Wraps TraverseGuide.ALL anonymous implementation. |
| ./NRedberry.Core/Tensors/Iterators/ExceptFieldsTraverseGuide.cs | ./core/src/main/java/cc/redberry/core/tensor/iterator/TraverseGuide.java | Wraps TraverseGuide.EXCEPT_FIELDS anonymous implementation. |
| ./NRedberry.Core/Tensors/Iterators/ExceptFunctionsAndFieldsTraverseGuide.cs | ./core/src/main/java/cc/redberry/core/tensor/iterator/TraverseGuide.java | Wraps TraverseGuide.EXCEPT_FUNCTIONS_AND_FIELDS anonymous implementation. |
| ./NRedberry.Core/Tensors/Iterators/FromChildToParentIterator.cs | ./core/src/main/java/cc/redberry/core/tensor/iterator/FromChildToParentIterator.java |  |
| ./NRedberry.Core/Tensors/Iterators/ITreeIterator.cs | ./core/src/main/java/cc/redberry/core/tensor/iterator/TreeIterator.java |  |
| ./NRedberry.Core/Tensors/Iterators/TraverseGuide.cs | ./core/src/main/java/cc/redberry/core/tensor/iterator/TraverseGuide.java |  |
| ./NRedberry.Core/Tensors/Iterators/TraversePermission.cs | ./core/src/main/java/cc/redberry/core/tensor/iterator/TraversePermission.java |  |
| ./NRedberry.Core/Tensors/Iterators/TraverseState.cs | ./core/src/main/java/cc/redberry/core/tensor/iterator/TraverseState.java |  |
| ./NRedberry.Core/Tensors/Iterators/TreeIteratorAbstract.cs | ./core/src/main/java/cc/redberry/core/tensor/iterator/TreeIteratorAbstract.java |  |
| ./NRedberry.Core/Tensors/MultiTensor.cs | ./core/src/main/java/cc/redberry/core/tensor/MultiTensor.java |  |
| ./NRedberry.Core/Tensors/OutputFormat.cs | ./core/src/main/java/cc/redberry/core/context/OutputFormat.java |  |
| ./NRedberry.Core/Tensors/Power.cs | ./core/src/main/java/cc/redberry/core/tensor/Power.java, ./core/src/main/java/cc/redberry/core/transformations/factor/jasfactor/edu/jas/structure/Power.java |  |
| ./NRedberry.Core/Tensors/PowerBuilder.cs | ./core/src/main/java/cc/redberry/core/tensor/PowerBuilder.java |  |
| ./NRedberry.Core/Tensors/PowerFactory.cs | ./core/src/main/java/cc/redberry/core/tensor/PowerFactory.java |  |
| ./NRedberry.Core/Tensors/Product.cs | ./core/src/main/java/cc/redberry/core/tensor/Product.java |  |
| ./NRedberry.Core/Tensors/ProductContent.cs | ./core/src/main/java/cc/redberry/core/tensor/ProductContent.java |  |
| ./NRedberry.Core/Tensors/ProductFactory.cs | ./core/src/main/java/cc/redberry/core/tensor/ProductFactory.java |  |
| ./NRedberry.Core/Tensors/SimpleTensor.cs | ./core/src/main/java/cc/redberry/core/tensor/SimpleTensor.java |  |
| ./NRedberry.Core/Tensors/SimpleTensorBuilder.cs | ./core/src/main/java/cc/redberry/core/tensor/SimpleTensor.java | Maps to SimpleTensor.Builder inner class. |
| ./NRedberry.Core/Tensors/SimpleTensorExtensions.cs | ./core/src/main/java/cc/redberry/core/tensor/SimpleTensor.java | Provides extension helpers around SimpleTensor methods. |
| ./NRedberry.Core/Tensors/SimpleTensorFactory.cs | ./core/src/main/java/cc/redberry/core/tensor/SimpleTensor.java | Mirrors SimpleTensor.Factory inner class. |
| ./NRedberry.Core/Tensors/Split.cs | ./core/src/main/java/cc/redberry/core/tensor/Split.java, ./core/src/main/java/cc/redberry/core/transformations/collect/CollectTransformation.java |  |
| ./NRedberry.Core/Tensors/SplitNumbers.cs | ./core/src/main/java/cc/redberry/core/tensor/Split.java |  |
| ./NRedberry.Core/Tensors/StructureOfContractions.cs | ./core/src/main/java/cc/redberry/core/tensor/StructureOfContractions.java |  |
| ./NRedberry.Core/Tensors/StructureOfContractionsHashed.cs | ./core/src/test/java/cc/redberry/core/tensor/ProductTest.java, ./physics/src/main/java/cc/redberry/physics/oneloopdiv/SqrSubs.java |  |
| ./NRedberry.Core/Tensors/Sum.cs | ./core/src/main/java/cc/redberry/core/tensor/Sum.java, ./core/src/main/java/cc/redberry/core/transformations/factor/jasfactor/edu/jas/ps/UnivPowerSeries.java |  |
| ./NRedberry.Core/Tensors/SumBuilder.cs | ./core/src/main/java/cc/redberry/core/tensor/SumBuilder.java |  |
| ./NRedberry.Core/Tensors/SumFactory.cs | ./core/src/main/java/cc/redberry/core/tensor/SumFactory.java |  |
| ./NRedberry.Core/Tensors/Tensor.cs | ./core/src/main/java/cc/redberry/core/tensor/Tensor.java |  |
| ./NRedberry.Core/Tensors/TensorArrayUtils.cs | ./core/src/main/java/cc/redberry/core/utils/TensorUtils.java | Utility functions pulled from TensorUtils static helpers. |
| ./NRedberry.Core/Tensors/TensorBuilder.cs | ./core/src/main/java/cc/redberry/core/tensor/TensorBuilder.java |  |
| ./NRedberry.Core/Tensors/TensorContraction.cs | ./core/src/main/java/cc/redberry/core/tensor/TensorContraction.java |  |
| ./NRedberry.Core/Tensors/TensorException.cs | ./core/src/main/java/cc/redberry/core/tensor/TensorException.java |  |
| ./NRedberry.Core/Tensors/TensorExtensions.cs | ./core/src/main/java/cc/redberry/core/tensor/Tensors.java | Extension methods correspond to static helpers in Tensors. |
| ./NRedberry.Core/Tensors/TensorFactory.cs | ./core/src/main/java/cc/redberry/core/tensor/TensorFactory.java |  |
| ./NRedberry.Core/Tensors/TensorField.cs | ./core/src/main/java/cc/redberry/core/tensor/TensorField.java |  |
| ./NRedberry.Core/Tensors/Tensors.cs | ./core/src/main/java/cc/redberry/core/tensor/Tensors.java |  |
| ./NRedberry.Core/Tensors/TensorUtils.cs | ./core/src/main/java/cc/redberry/core/utils/TensorUtils.java |  |
| ./NRedberry.Core/Tensors/TensorWrapper.cs | ./core/src/main/java/cc/redberry/core/tensor/Sum.java, ./core/src/main/java/cc/redberry/core/tensor/TensorWrapper.java |  |
| ./NRedberry.Core/Transformations/Expand/AbstractExpandTransformation.cs | ./core/src/main/java/cc/redberry/core/transformations/expand/AbstractExpandTransformation.java |  |
| ./NRedberry.Core/Transformations/Expand/ExpandAllTransformation.cs | ./core/src/main/java/cc/redberry/core/transformations/expand/ExpandAllTransformation.java |  |
| ./NRedberry.Core/Transformations/Expand/ExpandDenominatorTransformation.cs | ./core/src/main/java/cc/redberry/core/transformations/expand/ExpandDenominatorTransformation.java |  |
| ./NRedberry.Core/Transformations/Expand/ExpandNumeratorTransformation.cs | ./core/src/main/java/cc/redberry/core/transformations/expand/ExpandNumeratorTransformation.java |  |
| ./NRedberry.Core/Transformations/Expand/ExpandPort.cs | ./core/src/main/java/cc/redberry/core/transformations/expand/ExpandPort.java |  |
| ./NRedberry.Core/Transformations/Expand/ExpandTransformation.cs | ./core/src/main/java/cc/redberry/core/transformations/expand/ExpandTransformation.java |  |
| ./NRedberry.Core/Transformations/Expand/ExpandUtils.cs | ./core/src/main/java/cc/redberry/core/transformations/expand/ExpandUtils.java |  |
| ./NRedberry.Core/Transformations/Factor/FactorTransformation.cs | ./core/src/main/java/cc/redberry/core/transformations/factor/FactorTransformation.java |  |
| ./NRedberry.Core/Transformations/Factor/JasFactor.cs | ./core/src/main/java/cc/redberry/core/transformations/factor/JasFactor.java |  |
| ./NRedberry.Core/Transformations/Fractions/GetDenominatorTransformation.cs | ./core/src/main/java/cc/redberry/core/transformations/fractions/GetDenominatorTransformation.java |  |
| ./NRedberry.Core/Transformations/Fractions/GetNumeratorTransformation.cs | ./core/src/main/java/cc/redberry/core/transformations/fractions/GetNumeratorTransformation.java |  |
| ./NRedberry.Core/Transformations/Fractions/NumeratorDenominator.cs | ./core/src/main/java/cc/redberry/core/transformations/fractions/NumeratorDenominator.java |  |
| ./NRedberry.Core/Transformations/Fractions/TogetherTransformation.cs | ./core/src/main/java/cc/redberry/core/transformations/fractions/TogetherTransformation.java |  |
| ./NRedberry.Core/Transformations/IdentityTransformation.cs | (no matching Java source) | No IdentityTransformation class in Java; method calls used identity lambda directly. |
| ./NRedberry.Core/Transformations/Substitutions/BijectionContainer.cs | ./core/src/main/java/cc/redberry/core/transformations/substitutions/SumBijectionPort.java |  |
| ./NRedberry.Core/Transformations/Substitutions/InconsistentSubstitutionException.cs | ./core/src/main/java/cc/redberry/core/transformations/substitutions/InconsistentSubstitutionException.java |  |
| ./NRedberry.Core/Transformations/Substitutions/IndexlessBijectionsPort.cs | ./core/src/main/java/cc/redberry/core/transformations/substitutions/IndexlessBijectionsPort.java |  |
| ./NRedberry.Core/Transformations/Substitutions/PrimitiveProductSubstitution.cs | ./core/src/main/java/cc/redberry/core/transformations/substitutions/PrimitiveProductSubstitution.java |  |
| ./NRedberry.Core/Transformations/Substitutions/PrimitiveSimpleTensorSubstitution.cs | ./core/src/main/java/cc/redberry/core/transformations/substitutions/PrimitiveSimpleTensorSubstitution.java |  |
| ./NRedberry.Core/Transformations/Substitutions/PrimitiveSubstitution.cs | ./core/src/main/java/cc/redberry/core/transformations/substitutions/PrimitiveSubstitution.java |  |
| ./NRedberry.Core/Transformations/Substitutions/PrimitiveSumSubstitution.cs | ./core/src/main/java/cc/redberry/core/transformations/substitutions/PrimitiveSumSubstitution.java |  |
| ./NRedberry.Core/Transformations/Substitutions/PrimitiveTensorFieldSubstitution.cs | ./core/src/main/java/cc/redberry/core/transformations/substitutions/PrimitiveTensorFieldSubstitution.java |  |
| ./NRedberry.Core/Transformations/Substitutions/ProductsBijectionsPort.cs | ./core/src/main/java/cc/redberry/core/transformations/substitutions/ProductsBijectionsPort.java |  |
| ./NRedberry.Core/Transformations/Substitutions/SubstitutionIterator.cs | ./core/src/main/java/cc/redberry/core/transformations/substitutions/SubstitutionIterator.java |  |
| ./NRedberry.Core/Transformations/Substitutions/SubstitutionTransformation.cs | ./core/src/main/java/cc/redberry/core/transformations/substitutions/SubstitutionTransformation.java |  |
| ./NRedberry.Core/Transformations/Substitutions/SumBijectionPort.cs | ./core/src/main/java/cc/redberry/core/transformations/substitutions/SumBijectionPort.java |  |
| ./NRedberry.Core/Transformations/Symmetrization/CollectNonScalarsTransformation.cs | ./core/src/main/java/cc/redberry/core/transformations/CollectNonScalarsTransformation.java |  |
| ./NRedberry.Core/Transformations/Symmetrization/CollectScalarFactorsTransformation.cs | ./core/src/main/java/cc/redberry/core/transformations/CollectScalarFactorsTransformation.java |  |
| ./NRedberry.Core/Transformations/Symmetrization/ComplexConjugateTransformation.cs | ./core/src/main/java/cc/redberry/core/transformations/ComplexConjugateTransformation.java |  |
| ./NRedberry.Core/Transformations/Symmetrization/DifferentiateTransformation.cs | ./core/src/main/java/cc/redberry/core/transformations/DifferentiateTransformation.java |  |
| ./NRedberry.Core/Transformations/Symmetrization/EliminateFromSymmetriesTransformation.cs | (no matching Java source) | Placeholder not present in Java sources (search returned no hits). |
| ./NRedberry.Core/Transformations/Symmetrization/EliminateMetricsTransformation.cs | ./core/src/main/java/cc/redberry/core/transformations/EliminateMetricsTransformation.java |  |
| ./NRedberry.Core/Transformations/Symmetrization/ExpandAndEliminateTransformation.cs | ./core/src/main/java/cc/redberry/core/transformations/ExpandAndEliminateTransformation.java |  |
| ./NRedberry.Core/Transformations/Symmetrization/ToNumericTransformation.cs | ./core/src/main/java/cc/redberry/core/transformations/ToNumericTransformation.java |  |
| ./NRedberry.Core/Transformations/Symmetrization/Transformation.cs | ./core/src/main/java/cc/redberry/core/transformations/Transformation.java |  |
| ./NRedberry.Core/Transformations/Symmetrization/TransformationCollection.cs | ./core/src/main/java/cc/redberry/core/transformations/TransformationCollection.java |  |
| ./NRedberry.Core/Transformations/Symmetrization/TransformationException.cs | ./core/src/main/java/cc/redberry/core/transformations/TransformationException.java |  |
| ./NRedberry.Core/Transformations/Symmetrization/Transformer.cs | ./core/src/main/java/cc/redberry/core/transformations/Transformer.java |  |
| ./NRedberry.Core/Transformations/Transformation.cs | ./core/src/main/java/cc/redberry/core/transformations/Transformation.java |  |
| ./NRedberry.Core/Utils/TensorHashCalculator.cs | ./core/src/main/java/cc/redberry/core/tensor/playground/GraphStructure.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/context/ToString.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/indexmapping/DummyIndexMappingProvider.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/indexmapping/IndexMappingBuffer.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/indexmapping/IndexMappingBufferRecord.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/indexmapping/IndexMappingProvider.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/indexmapping/IndexMappingProviderAbstract.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/indexmapping/IndexMappingProviderAbstractFT.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/indexmapping/IndexMappingProviderFactory.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/indexmapping/MappingsPort.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/indexmapping/MappingsPortRemovingContracted.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/indexmapping/MinusIndexMappingProvider.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/indexmapping/MinusIndexMappingProviderWrapper.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/indexmapping/PlusMinusIndexMappingProvider.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/indexmapping/ProviderComplex.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/indexmapping/ProviderFunctions.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/indexmapping/ProviderPower.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/indexmapping/SimpleProductMappingsPort.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/indices/ShortArrayFactory.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/number/parser/BracketToken.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/number/parser/BracketsError.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/number/parser/ComplexToken.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/number/parser/NumberParser.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/number/parser/OperatorToken.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/number/parser/RealToken.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/parser/BracketsError.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/parser/NodeParserComparator.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/parser/ParseTokenDerivative.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/parser/ParseTokenExpression.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/parser/ParseTokenNumber.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/parser/ParseTokenScalarFunction.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/parser/ParseTokenSimpleTensor.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/parser/ParseTokenTensorField.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/parser/ParseUtils.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/parser/ParserBrackets.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/parser/ParserDerivative.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/parser/ParserExpression.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/parser/ParserFunctions.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/parser/ParserIndices.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/parser/ParserNumber.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/parser/ParserOperator.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/parser/ParserPower.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/parser/ParserProduct.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/parser/ParserSimpleTensor.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/parser/ParserSum.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/parser/ParserTensorField.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/parser/preprocessor/ChangeIndicesTypesAndTensorNames.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/solver/ReducedSystem.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/solver/frobenius/DummySolutionProvider.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/solver/frobenius/FbUtils.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/solver/frobenius/FinalSolutionProvider.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/solver/frobenius/SingleSolutionProvider.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/solver/frobenius/SolutionProvider.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/solver/frobenius/SolutionProviderAbstract.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/solver/frobenius/TotalSolutionProvider.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/tensor/Expression.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/tensor/ExpressionBuilder.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/tensor/ExpressionFactory.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/tensor/FastTensors.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/tensor/PowersContainer.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/tensor/ScalarsBackedProductBuilder.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/tensor/SumBuilderSplitingScalars.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/tensor/iterator/DummyPayload.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/tensor/iterator/FromParentToChildIterator.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/tensor/iterator/Payload.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/tensor/iterator/PayloadFactory.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/tensor/iterator/StackPosition.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/tensor/iterator/TreeTraverseIterator.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/tensor/playground/ContentData.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/tensor/playground/GraphStructureHashed.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/tensor/playground/IAlgorithm.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/transformations/ApplyDiracDeltasTransformation.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/transformations/TransformationToStringAble.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/transformations/abbreviations/AbbreviationsBuilder.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/transformations/expand/AbstractExpandNumeratorDenominatorTransformation.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/transformations/expand/ExpandOptions.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/transformations/expand/ExpandTensorsOptions.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/transformations/expand/ExpandTensorsTransformation.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/transformations/factor/FactorOutNumber.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/transformations/factor/SingularFactorizationEngine.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/transformations/factor/jasfactor/edu/jas/arith/BigComplex.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/transformations/factor/jasfactor/edu/jas/arith/BigInteger.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/transformations/factor/jasfactor/edu/jas/arith/BigRational.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/transformations/factor/jasfactor/edu/jas/arith/Combinatoric.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/transformations/factor/jasfactor/edu/jas/arith/ModInteger.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/transformations/factor/jasfactor/edu/jas/arith/ModIntegerRing.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/transformations/factor/jasfactor/edu/jas/arith/ModLong.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/transformations/factor/jasfactor/edu/jas/arith/ModLongRing.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/transformations/factor/jasfactor/edu/jas/arith/Modular.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/transformations/factor/jasfactor/edu/jas/arith/ModularNotInvertibleException.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/transformations/factor/jasfactor/edu/jas/arith/ModularRingFactory.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/transformations/factor/jasfactor/edu/jas/arith/PrimeList.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/transformations/factor/jasfactor/edu/jas/gb/GroebnerBase.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/transformations/factor/jasfactor/edu/jas/gb/Reduction.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/transformations/factor/jasfactor/edu/jas/gb/ReductionAbstract.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/transformations/factor/jasfactor/edu/jas/gb/ReductionSeq.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/transformations/factor/jasfactor/edu/jas/poly/AlgebraicNotInvertibleException.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/transformations/factor/jasfactor/edu/jas/poly/AlgebraicNumber.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/transformations/factor/jasfactor/edu/jas/poly/AlgebraicNumberRing.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/transformations/factor/jasfactor/edu/jas/poly/ComplexRing.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/transformations/factor/jasfactor/edu/jas/poly/ExpVectorByte.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/transformations/factor/jasfactor/edu/jas/poly/ExpVectorInteger.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/transformations/factor/jasfactor/edu/jas/poly/ExpVectorLong.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/transformations/factor/jasfactor/edu/jas/poly/ExpVectorPair.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/transformations/factor/jasfactor/edu/jas/poly/ExpVectorShort.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/transformations/factor/jasfactor/edu/jas/poly/GenPolynomial.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/transformations/factor/jasfactor/edu/jas/poly/GenSolvablePolynomial.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/transformations/factor/jasfactor/edu/jas/poly/GenSolvablePolynomialRing.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/transformations/factor/jasfactor/edu/jas/poly/Monomial.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/transformations/factor/jasfactor/edu/jas/poly/OptimizedPolynomialList.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/transformations/factor/jasfactor/edu/jas/poly/PolyIterator.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/transformations/factor/jasfactor/edu/jas/poly/PolyUtil.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/transformations/factor/jasfactor/edu/jas/poly/PolynomialComparator.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/transformations/factor/jasfactor/edu/jas/poly/PolynomialList.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/transformations/factor/jasfactor/edu/jas/poly/RelationTable.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/transformations/factor/jasfactor/edu/jas/ps/Coefficients.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/transformations/factor/jasfactor/edu/jas/ps/PolynomialTaylorFunction.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/transformations/factor/jasfactor/edu/jas/ps/TaylorFunction.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/transformations/factor/jasfactor/edu/jas/ps/UnivPowerSeriesRing.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/transformations/factor/jasfactor/edu/jas/structure/AbelianGroupElem.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/transformations/factor/jasfactor/edu/jas/structure/AbelianGroupFactory.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/transformations/factor/jasfactor/edu/jas/structure/BinaryFunctor.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/transformations/factor/jasfactor/edu/jas/structure/ElemFactory.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/transformations/factor/jasfactor/edu/jas/structure/GcdRingElem.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/transformations/factor/jasfactor/edu/jas/structure/MonoidElem.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/transformations/factor/jasfactor/edu/jas/structure/MonoidFactory.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/transformations/factor/jasfactor/edu/jas/structure/NotInvertibleException.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/transformations/factor/jasfactor/edu/jas/structure/RingElem.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/transformations/factor/jasfactor/edu/jas/structure/RingFactory.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/transformations/factor/jasfactor/edu/jas/structure/UnaryFunctor.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/transformations/factor/jasfactor/edu/jas/ufd/FactorAbsolute.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/transformations/factor/jasfactor/edu/jas/ufd/FactorAbstract.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/transformations/factor/jasfactor/edu/jas/ufd/FactorAlgebraic.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/transformations/factor/jasfactor/edu/jas/ufd/FactorComplex.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/transformations/factor/jasfactor/edu/jas/ufd/FactorFactory.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/transformations/factor/jasfactor/edu/jas/ufd/FactorInteger.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/transformations/factor/jasfactor/edu/jas/ufd/FactorModular.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/transformations/factor/jasfactor/edu/jas/ufd/FactorQuotient.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/transformations/factor/jasfactor/edu/jas/ufd/FactorRational.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/transformations/factor/jasfactor/edu/jas/ufd/Factorization.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/transformations/factor/jasfactor/edu/jas/ufd/Factors.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/transformations/factor/jasfactor/edu/jas/ufd/GCDFactory.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/transformations/factor/jasfactor/edu/jas/ufd/GreatestCommonDivisor.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/transformations/factor/jasfactor/edu/jas/ufd/GreatestCommonDivisorAbstract.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/transformations/factor/jasfactor/edu/jas/ufd/GreatestCommonDivisorModEval.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/transformations/factor/jasfactor/edu/jas/ufd/GreatestCommonDivisorModular.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/transformations/factor/jasfactor/edu/jas/ufd/GreatestCommonDivisorPrimitive.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/transformations/factor/jasfactor/edu/jas/ufd/GreatestCommonDivisorSimple.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/transformations/factor/jasfactor/edu/jas/ufd/GreatestCommonDivisorSubres.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/transformations/factor/jasfactor/edu/jas/ufd/HenselApprox.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/transformations/factor/jasfactor/edu/jas/ufd/HenselMultUtil.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/transformations/factor/jasfactor/edu/jas/ufd/HenselUtil.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/transformations/factor/jasfactor/edu/jas/ufd/NoLiftingException.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/transformations/factor/jasfactor/edu/jas/ufd/PolyUfdUtil.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/transformations/factor/jasfactor/edu/jas/ufd/Quotient.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/transformations/factor/jasfactor/edu/jas/ufd/QuotientRing.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/transformations/factor/jasfactor/edu/jas/ufd/Squarefree.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/transformations/factor/jasfactor/edu/jas/ufd/SquarefreeAbstract.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/transformations/factor/jasfactor/edu/jas/ufd/SquarefreeFactory.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/transformations/factor/jasfactor/edu/jas/ufd/SquarefreeFieldChar0.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/transformations/factor/jasfactor/edu/jas/ufd/SquarefreeFieldCharP.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/transformations/factor/jasfactor/edu/jas/ufd/SquarefreeFiniteFieldCharP.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/transformations/factor/jasfactor/edu/jas/ufd/SquarefreeInfiniteAlgebraicFieldCharP.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/transformations/factor/jasfactor/edu/jas/ufd/SquarefreeInfiniteFieldCharP.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/transformations/factor/jasfactor/edu/jas/ufd/SquarefreeRingChar0.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/transformations/factor/jasfactor/edu/jas/util/CartesianProduct.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/transformations/factor/jasfactor/edu/jas/util/CartesianProductInfinite.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/transformations/factor/jasfactor/edu/jas/util/KsubSet.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/transformations/factor/jasfactor/edu/jas/util/ListUtil.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/transformations/factor/jasfactor/edu/jas/util/LongIterable.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/transformations/factor/jasfactor/edu/jas/vector/BasicLinAlg.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/transformations/options/Creator.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/transformations/options/IOptions.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/transformations/options/Option.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/transformations/options/Options.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/transformations/options/TransformationBuilder.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/transformations/powerexpand/PowerExpandTransformation.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/transformations/powerexpand/PowerUnfoldTransformation.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/transformations/reverse/ReverseTransformation.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/utils/ArrayIterator.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/utils/EmptyIterator.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/utils/InfiniteLoopIterable.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/utils/InfiniteLoopIterator.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/utils/IntComparator.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/utils/IteratorWithProgress.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/utils/LocalSymbolsProvider.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/utils/MatrixUtils.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/utils/ProgressReporter.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/utils/SoftReferenceWrapper.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/utils/THashMap.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/utils/THashSet.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/utils/TensorWrapperWithEquals.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/utils/Timing.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/utils/TransformationWithTimer.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/utils/stretces/IntObjectProvider.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/utils/stretces/PrecalculatedStretches.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/utils/stretces/Stretch.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/utils/stretces/StretchIterator.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/utils/stretces/StretchIteratorI.java |  |
| (no match) | ./core/src/main/java/cc/redberry/core/utils/stretces/StretchIteratorS.java |  |
| (no match) | ./core/src/test/java/cc/redberry/core/GlobalRunListener.java |  |
| (no match) | ./core/src/test/java/cc/redberry/core/context/IndexConverterManagerTest.java |  |
| (no match) | ./core/src/test/java/cc/redberry/core/context/NameDescriptorForTensorFieldDerivativeTest.java |  |
| (no match) | ./core/src/test/java/cc/redberry/core/context/defaults/GreekLettersConverterTest.java |  |
| (no match) | ./core/src/test/java/cc/redberry/core/groups/permutations/BruteForcePermutationIteratorTest.java |  |
| (no match) | ./core/src/test/java/cc/redberry/core/groups/permutations/GapGroupsInterface.java |  |
| (no match) | ./core/src/test/java/cc/redberry/core/groups/permutations/GapGroupsInterfaceTest.java |  |
| (no match) | ./core/src/test/java/cc/redberry/core/groups/permutations/TestWithGAP.java |  |
| (no match) | ./core/src/test/java/cc/redberry/core/indexmapping/IndexMappingTestUtils.java |  |
| (no match) | ./core/src/test/java/cc/redberry/core/indices/IndicesUtilsTest.java |  |
| (no match) | ./core/src/test/java/cc/redberry/core/indices/StructureOfIndicesTest.java |  |
| (no match) | ./core/src/test/java/cc/redberry/core/number/ComplexTest.java |  |
| (no match) | ./core/src/test/java/cc/redberry/core/number/ComplexUtilsTest.java |  |
| (no match) | ./core/src/test/java/cc/redberry/core/number/ExponentiationTest.java |  |
| (no match) | ./core/src/test/java/cc/redberry/core/number/NumberUtilsTest.java |  |
| (no match) | ./core/src/test/java/cc/redberry/core/number/NumericTest.java |  |
| (no match) | ./core/src/test/java/cc/redberry/core/number/RationalTest.java |  |
| (no match) | ./core/src/test/java/cc/redberry/core/number/parser/ComplexTest.java |  |
| (no match) | ./core/src/test/java/cc/redberry/core/number/parser/NumberParserTest.java |  |
| (no match) | ./core/src/test/java/cc/redberry/core/parser/ParserIndicesTest.java |  |
| (no match) | ./core/src/test/java/cc/redberry/core/parser/ParserPowerAstTest.java |  |
| (no match) | ./core/src/test/java/cc/redberry/core/parser/preprocessor/ChangeIndicesTypesAndTensorNamesTest.java |  |
| (no match) | ./core/src/test/java/cc/redberry/core/parser/preprocessor/GeneralIndicesInsertionTest.java |  |
| (no match) | ./core/src/test/java/cc/redberry/core/parser/preprocessor/IndicesInsertionTest.java |  |
| (no match) | ./core/src/test/java/cc/redberry/core/solver/frobenius/FrobeniusNumberTest.java |  |
| (no match) | ./core/src/test/java/cc/redberry/core/tensor/FastTensorsTest.java |  |
| (no match) | ./core/src/test/java/cc/redberry/core/tensor/PowerBuilderTest.java |  |
| (no match) | ./core/src/test/java/cc/redberry/core/tensor/PowerFactoryTest.java |  |
| (no match) | ./core/src/test/java/cc/redberry/core/tensor/ProductBuilderTest.java |  |
| (no match) | ./core/src/test/java/cc/redberry/core/tensor/SplitTest.java |  |
| (no match) | ./core/src/test/java/cc/redberry/core/tensor/TestParserGlobally.java |  |
| (no match) | ./core/src/test/java/cc/redberry/core/tensor/iterator/TreeTraverseIteratorTest.java |  |
| (no match) | ./core/src/test/java/cc/redberry/core/tensor/random/RandomTensorTest.java |  |
| (no match) | ./core/src/test/java/cc/redberry/core/tensorgenerator/TensorGeneratorUtilsTest.java |  |
| (no match) | ./core/src/test/java/cc/redberry/core/test/LongTest.java |  |
| (no match) | ./core/src/test/java/cc/redberry/core/test/PerformanceTest.java |  |
| (no match) | ./core/src/test/java/cc/redberry/core/test/RedberryTest.java |  |
| (no match) | ./core/src/test/java/cc/redberry/core/test/TestUtils.java |  |
| (no match) | ./core/src/test/java/cc/redberry/core/transformations/ApplyDiracDeltasTransformationTest.java |  |
| (no match) | ./core/src/test/java/cc/redberry/core/transformations/CollectNonScalarsTransformationTest.java |  |
| (no match) | ./core/src/test/java/cc/redberry/core/transformations/EliminateMetricsTransformationTest.java |  |
| (no match) | ./core/src/test/java/cc/redberry/core/transformations/abbreviations/AbbreviationsBuilderTest.java |  |
| (no match) | ./core/src/test/java/cc/redberry/core/transformations/expand/ExpandAllTransformationTest.java |  |
| (no match) | ./core/src/test/java/cc/redberry/core/transformations/expand/ExpandDenominatorTransformationTest.java |  |
| (no match) | ./core/src/test/java/cc/redberry/core/transformations/expand/ExpandNumeratorTransformationTest.java |  |
| (no match) | ./core/src/test/java/cc/redberry/core/transformations/expand/ExpandTensorsTransformationTest.java |  |
| (no match) | ./core/src/test/java/cc/redberry/core/transformations/expand/ExpandTest.java |  |
| (no match) | ./core/src/test/java/cc/redberry/core/transformations/factor/FactorOutNumberTest.java |  |
| (no match) | ./core/src/test/java/cc/redberry/core/transformations/factor/FactorTransformationTest.java |  |
| (no match) | ./core/src/test/java/cc/redberry/core/transformations/factor/SingularFactorizationEngineTest.java |  |
| (no match) | ./core/src/test/java/cc/redberry/core/transformations/fractions/NumeratorDenominatorTest.java |  |
| (no match) | ./core/src/test/java/cc/redberry/core/transformations/fractions/TogetherTransformationTest.java |  |
| (no match) | ./core/src/test/java/cc/redberry/core/transformations/options/TransformationBuilderTest.java |  |
| (no match) | ./core/src/test/java/cc/redberry/core/transformations/powerexpand/PowerUnfoldTransformationTest.java |  |
| (no match) | ./core/src/test/java/cc/redberry/core/transformations/reverse/ReverseTransformationTest.java |  |
| (no match) | ./core/src/test/java/cc/redberry/core/transformations/substitutions/SubstitutionIteratorTest.java |  |
| (no match) | ./core/src/test/java/cc/redberry/core/transformations/symmetrization/SymmetrizeUpperLowerIndicesTransformationTest.java |  |
| (no match) | ./core/src/test/java/cc/redberry/core/utils/IntTimSortTest.java |  |
| (no match) | ./core/src/test/java/cc/redberry/core/utils/ProgressReporterTest.java |  |
| (no match) | ./core/src/test/java/cc/redberry/core/utils/THashSetTest.java |  |
| (no match) | ./core/src/test/java/cc/redberry/core/utils/TimingStatisticsTest.java |  |
| (no match) | ./groovy/src/main/java/cc/redberry/groovy/DSLTransformation.java |  |
| (no match) | ./groovy/src/main/java/cc/redberry/groovy/DSLTransformationInst.java |  |
| (no match) | ./groovy/src/main/java/cc/redberry/groovy/MatrixDescriptor.java |  |
| (no match) | ./groovy/src/test/groovy/cc/redberry/groovy/GroovyGlobalRunListener.java |  |
| (no match) | ./physics/src/main/java/cc/redberry/physics/feyncalc/AbstractFeynCalcTransformation.java |  |
| (no match) | ./physics/src/main/java/cc/redberry/physics/feyncalc/DiracSimplify1.java |  |
| (no match) | ./physics/src/main/java/cc/redberry/physics/feyncalc/DiracSimplifyTransformation.java |  |
| (no match) | ./physics/src/main/java/cc/redberry/physics/feyncalc/DiracTraceTransformation.java |  |
| (no match) | ./physics/src/main/java/cc/redberry/physics/feyncalc/FeynCalcUtils.java |  |
| (no match) | ./physics/src/main/java/cc/redberry/physics/feyncalc/LeviCivitaSimplifyOptions.java |  |
| (no match) | ./physics/src/main/java/cc/redberry/physics/feyncalc/ProductOfGammas.java |  |
| (no match) | ./physics/src/main/java/cc/redberry/physics/feyncalc/SchoutenIdentities4.java |  |
| (no match) | ./physics/src/main/java/cc/redberry/physics/feyncalc/SimplifyGamma5Transformation.java |  |
| (no match) | ./physics/src/main/java/cc/redberry/physics/feyncalc/SpinorsSimplifyOptions.java |  |
| (no match) | ./physics/src/main/java/cc/redberry/physics/feyncalc/TraceUtils.java |  |
| (no match) | ./physics/src/main/java/cc/redberry/physics/feyncalc/UnitarySimplifyOptions.java |  |
| (no match) | ./physics/src/main/java/cc/redberry/physics/feyncalc/UnitarySimplifyTransformation.java |  |
| (no match) | ./physics/src/main/java/cc/redberry/physics/oneloopdiv/Averaging.java |  |
| (no match) | ./physics/src/main/java/cc/redberry/physics/oneloopdiv/NaiveSubstitution.java |  |
| (no match) | ./physics/src/test/java/GlobalRunListener.java |  |
| (no match) | ./physics/src/test/java/cc/redberry/physics/feyncalc/AbstractFeynCalcTest.java |  |
| (no match) | ./physics/src/test/java/cc/redberry/physics/feyncalc/DiracOptionsTest.java |  |
| (no match) | ./physics/src/test/java/cc/redberry/physics/feyncalc/DiracOrderTransformationTest.java |  |
| (no match) | ./physics/src/test/java/cc/redberry/physics/feyncalc/DiracSimplify0Test.java |  |
| (no match) | ./physics/src/test/java/cc/redberry/physics/feyncalc/DiracSimplify1Test.java |  |
| (no match) | ./physics/src/test/java/cc/redberry/physics/feyncalc/DiracSimplifyTransformationTest.java |  |
| (no match) | ./physics/src/test/java/cc/redberry/physics/feyncalc/DiracTraceTransformationTest.java |  |
| (no match) | ./physics/src/test/java/cc/redberry/physics/feyncalc/FeynCalcUtilsTest.java |  |
| (no match) | ./physics/src/test/java/cc/redberry/physics/feyncalc/PassarinoVeltmanTest.java |  |
| (no match) | ./physics/src/test/java/cc/redberry/physics/feyncalc/SchoutenIdentities4Test.java |  |
| (no match) | ./physics/src/test/java/cc/redberry/physics/feyncalc/SimplifyGamma5TransformationTest.java |  |
| (no match) | ./physics/src/test/java/cc/redberry/physics/feyncalc/SpinorsSimplifyTransformationTest.java |  |
| (no match) | ./physics/src/test/java/cc/redberry/physics/feyncalc/UnitarySimplifyTransformationTest.java |  |
| (no match) | ./physics/src/test/java/cc/redberry/physics/oneloopdiv/SqrSubsTest.java |  |
