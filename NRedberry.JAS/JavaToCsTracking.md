# Java to C# Scaffolding Tracking

This document tracks the scaffolding status of all Java files in the NRedberry.JAS project.

## Status Legend
- ✅ **Scaffolded** - Class skeleton with public methods/properties created, throwing NotImplementedException
- ✔️ **Complete** - Fully implemented

## Files

### Arith (12 files)
| Java File | C# File | Status | Notes |
|-----------|---------|--------|-------|
| BigComplex.java | BigComplex.cs | ✔️ Complete | |
| BigInteger.java | BigInteger.cs | ✔️ Complete | |
| BigRational.java | BigRational.cs | ✔️ Complete | |
| Combinatoric.java | Combinatoric.cs | ✔️ Complete | |
| ModInteger.java | ModInteger.cs | ✔️ Complete | |
| ModIntegerRing.java | ModIntegerRing.cs | ✔️ Complete | |
| ModLong.java | ModLong.cs | ✔️ Complete | |
| ModLongRing.java | ModLongRing.cs | ✔️ Complete | |
| Modular.java | Modular.cs | ✔️ Complete | |
| ModularNotInvertibleException.java | ModularNotInvertibleException.cs | ✔️ Complete | |
| ModularRingFactory.java | ModularRingFactory.cs | ✔️ Complete | |
| PrimeList.java | PrimeList.cs | ✔️ Complete | |

### Gb (4 files)
| Java File | C# File | Status | Notes |
|-----------|---------|--------|-------|
| GroebnerBase.java | GroebnerBase.cs | ✔️ Complete | |
| Reduction.java | Reduction.cs | ✔️ Complete | |
| ReductionAbstract.java | ReductionAbstract.cs | ✔️ Complete | |
| ReductionSeq.java | ReductionSeq.cs | ✔️ Complete | |

### Poly (24 files)
| Java File | C# File | Status    | Notes |
|-----------|---------|-----------|-------|
| AlgebraicNotInvertibleException.java | AlgebraicNotInvertibleException.cs | ✔️ Complete | |
| AlgebraicNumber.java | AlgebraicNumber.cs | ✔️ Complete | Algebraic number with GenPolynomial value |
| AlgebraicNumberRing.java | AlgebraicNumberRing.cs | ✔️ Complete | Factory for AlgebraicNumber |
| Complex.java | Complex.cs | ✔️ Complete | |
| ComplexRing.java | ComplexRing.cs | ✔️ Complete | |
| ExpVector.java | ExpVector.cs | ✔️ Complete | Core compare/order helpers and random generation ported |
| ExpVectorByte.java | ExpVectorByte.cs | ✔️ Complete | |
| ExpVectorInteger.java | ExpVectorInteger.cs | ✔️ Complete | |
| ExpVectorLong.java | ExpVectorLong.cs | ✔️ Complete | |
| ExpVectorPair.java | ExpVectorPair.cs | ✔️ Complete | |
| ExpVectorShort.java | ExpVectorShort.cs | ✔️ Complete | |
| GenPolynomial.java | GenPolynomial.cs | ✔️ Complete | |
| GenPolynomialRing.java | GenPolynomialRing.cs | ✔️ Complete | |
| GenSolvablePolynomial.java | GenSolvablePolynomial.cs | ✔️ Complete | |
| GenSolvablePolynomialRing.java | GenSolvablePolynomialRing.cs | ✔️ Complete | |
| Monomial.java | Monomial.cs | ✔️ Complete | |
| OptimizedPolynomialList.java | OptimizedPolynomialList.cs | ✔️ Complete | Permutation metadata & string output |
| OrderedPolynomialList.java | OrderedPolynomialList.cs | ✔️ Complete | Implements sorting & comparer wrappers |
| PolyIterator.java | PolyIterator.cs | ✔️ Complete | Implements IEnumerator<Monomial<C>> adaptor
| PolynomialComparator.java | PolynomialComparator.cs | ✔️ Complete | Standard IComparer implementation
| PolynomialList.java | PolynomialList.cs | ✔️ Complete | Equality/compare/toString implemented |
| PolyUtil.java | PolyUtil.cs | ✅ Scaffolded | Initial conversion utilities implemented; further methods pending
| RelationTable.java | RelationTable.cs | ✅ Scaffolded | Minimal storage; relation ops pending |
| TermOrder.java | TermOrder.cs | ✔️ Complete | Single/split/weight orders with extend/contract/reverse |
| TermOrderOptimization.java | TermOrderOptimization.cs | ✅ Scaffolded | Variable permutation optimization |

### Ps (5 files)
| Java File | C# File | Status | Notes |
|-----------|---------|--------|-------|
| Coefficients.java | Coefficients.cs | ✔️ Complete | |
| PolynomialTaylorFunction.java | PolynomialTaylorFunction.cs | ✔️ Complete | |
| TaylorFunction.java | TaylorFunction.cs | ✔️ Complete | |
| UnivPowerSeries.java | UnivPowerSeries.cs | ✔️ Complete | |
| UnivPowerSeriesRing.java | UnivPowerSeriesRing.cs | ✔️ Complete | |

### Structure (13 files)
| Java File | C# File | Status | Notes |
|-----------|---------|--------|-------|
| AbelianGroupElem.java | AbelianGroupElem.cs | ✔️ Complete | |
| AbelianGroupFactory.java | AbelianGroupFactory.cs | ✔️ Complete | |
| BinaryFunctor.java | BinaryFunctor.cs | ✔️ Complete | |
| Element.java | Element.cs | ✔️ Complete | |
| ElemFactory.java | ElemFactory.cs | ✔️ Complete | |
| GcdRingElem.java | GcdRingElem.cs | ✔️ Complete | |
| MonoidElem.java | MonoidElem.cs | ✔️ Complete | |
| MonoidFactory.java | MonoidFactory.cs | ✔️ Complete | |
| NotInvertibleException.java | NotInvertibleException.cs | ✔️ Complete | |
| Power.java | Power.cs | ✔️ Complete | |
| RingElem.java | RingElem.cs | ✔️ Complete | |
| RingFactory.java | RingFactory.cs | ✔️ Complete | |
| UnaryFunctor.java | UnaryFunctor.cs | ✔️ Complete | |

### Ufd (37 files)
| Java File | C# File | Status       | Notes |
|-----------|---------|--------------|-------|
| FactorAbsolute.java | FactorAbsolute.cs | ✔️ Complete | Abstract class for absolute factorization |
| FactorAbstract.java | FactorAbstract.cs | ✔️ Complete | Base abstract factorization class |
| FactorAlgebraic.java | FactorAlgebraic.cs | ✔️ Complete | Algebraic number coefficient factorization |
| FactorComplex.java | FactorComplex.cs | ✔️ Complete | Complex coefficient factorization using algebraic extensions |
| FactorFactory.java | FactorFactory.cs | ✔️ Complete | Factory for selecting factorization algorithms |
| FactorInteger.java | FactorInteger.cs | ✔️ Complete | Integer coefficient factorization (Hensel) |
| Factorization.java | Factorization.cs | ✔️ Complete | Factorization interface |
| FactorModular.java | FactorModular.cs | ✔️ Complete | Modular coefficient factorization |
| FactorQuotient.java | FactorQuotient.cs | ✔️ Complete | Quotient coefficient factorization |
| FactorRational.java | FactorRational.cs | ✔️ Complete | Rational coefficient factorization |
| Factors.java | Factors.cs | ✔️ Complete | Container for absolute factorization results |
| GCDFactory.java | GCDFactory.cs | ✔️ Complete | Factory for GCD algorithm selection |
| GreatestCommonDivisor.java | GreatestCommonDivisor.cs | ✔️ Complete | GCD interface with generic type parameter |
| GreatestCommonDivisorAbstract.java | GreatestCommonDivisorAbstract.cs | ✔️ Complete | Abstract GCD base with interface methods |
| GreatestCommonDivisorModEval.java | GreatestCommonDivisorModEval.cs | ✔️ Complete | GCD with modular evaluation algorithm |
| GreatestCommonDivisorModular.java | GreatestCommonDivisorModular.cs | ✔️ Complete | GCD with Chinese remainder algorithm |
| GreatestCommonDivisorPrimitive.java | GreatestCommonDivisorPrimitive.cs | ✔️ Complete | GCD with primitive polynomial remainder |
| GreatestCommonDivisorSimple.java | GreatestCommonDivisorSimple.cs | ✔️ Complete | GCD with monic polynomial remainder |
| GreatestCommonDivisorSubres.java | GreatestCommonDivisorSubres.cs | ✔️ Complete | GCD with subresultant remainder |
| HenselApprox.java | HenselApprox.cs | ✔️ Complete | Container for Hensel algorithm results |
| HenselMultUtil.java | HenselMultUtil.cs | ✅ Scaffolded | Hensel multivariate lifting utilities |
| HenselUtil.java | HenselUtil.cs | ✅ Scaffolded | Hensel univariate lifting utilities |
| NoLiftingException.java | NoLiftingException.cs | ✔️ Complete | Exception thrown when Hensel lifting fails |
| PolyUfdUtil.java | PolyUfdUtil.cs | ✅ Scaffolded | Polynomial UFD utilities and conversions |
| Quotient.java | Quotient.cs | ✅ Scaffolded | Quotient ring element (rational function) |
| QuotientRing.java | QuotientRing.cs | ✅ Scaffolded | Quotient ring factory with generic type |
| Squarefree.java | Squarefree.cs | ✔️ Complete | Squarefree decomposition interface |
| SquarefreeAbstract.java | SquarefreeAbstract.cs | ✔️ Complete | Abstract squarefree base with methods |
| SquarefreeFactory.java | SquarefreeFactory.cs | ✔️ Complete | Factory for squarefree decomposition |
| SquarefreeFieldChar0.java | SquarefreeFieldChar0.cs | ✔️ Complete | Squarefree for fields char 0 |
| SquarefreeFieldCharP.java | SquarefreeFieldCharP.cs | ✔️ Complete | Abstract squarefree for fields char p – needs full port before dependents |
| SquarefreeFiniteFieldCharP.java | SquarefreeFiniteFieldCharP.cs | ✔️ Complete | Blocked: relies on SquarefreeFieldCharP implementation |
| SquarefreeInfiniteAlgebraicFieldCharP.java | SquarefreeInfiniteAlgebraicFieldCharP.cs | ✅ Scaffolded | Squarefree for infinite algebraic fields |
| SquarefreeInfiniteFieldCharP.java | SquarefreeInfiniteFieldCharP.cs | ✅ Scaffolded | Squarefree for infinite fields char p |
| SquarefreeRingChar0.java | SquarefreeRingChar0.cs | ✔️ Complete | Squarefree for rings char 0 |

### Util (5 files)
| Java File | C# File | Status | Notes |
|-----------|---------|--------|-------|
| CartesianProduct.java | CartesianProduct.cs | ✔️ Complete | |
| CartesianProductInfinite.java | CartesianProductInfinite.cs | ✔️ Complete | |
| KsubSet.java | KsubSet.cs | ✔️ Complete | |
| ListUtil.java | ListUtil.cs | ✔️ Complete | |
| LongIterable.java | LongIterable.cs | ✔️ Complete | |

### Vector (1 file)
| Java File | C# File | Status | Notes |
|-----------|---------|--------|-------|
| BasicLinAlg.java | BasicLinAlg.cs | ✔️ Complete | |
