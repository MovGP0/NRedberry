# Repository Guidelines

## Project Structure & Module Organization

- `NRedberry.Core/` is the main port of the Redberry engine. Subfolders group functionality (for example, `Tensors/`,
  `Transformations/`, `Contexts/`, `Utils/`) and generally mirror the original Java packages. Keep new code alongside
  the closest conceptual peer.
- Supporting libraries live under sibling projects such as `NRedberry.Apache.Commons.Math/`, `NRedberry.Core.Entities/`,
  and `NRedberry.Core.Utils/`; add shared primitives there rather than duplicating logic inside `NRedberry.Core/`.
- Automated tests reside in `NRedberry.Core.Tests/`, which references the core project directly. Place new test fixtures
  in module-matching subfolders to make cross-referencing straightforward.

## Build, Test, and Development Commands

- `dotnet restore NRedberry.slnx` — restore dependencies for the core library and its project references.
- `dotnet build NRedberry.slnx -c Release` — compile all linked projects (net9.0) with analyzers enabled.
- `dotnet test NRedberry.slnx ` — run the xUnit suite; use `--filter Category=...` to scope runs during iteration.
- IDE users can open `NRedberry.slnx`.

## Coding Style & Naming Conventions

- Use four-space indentation, Allman braces, and explicit visibility; follow the prevailing C# casing (`PascalCase`
  types/methods, `camelCase` locals).
- Nullable reference types are enabled—prefer non-nullable signatures and guard inputs explicitly.
- The solution relies on Roslynator analyzers; run `dotnet format` or address warnings before sending reviews. Mirror
  existing JavaDoc-style comments (`/** ... */`) when porting descriptive docs from the Java codebase.
- Keep blank lines consistent to satisfy Roslynator formatting rules: add a blank line between a closing brace and the
  next statement (RCS0008) and avoid stray blank lines before closing braces (RCS0063).
- use `ArgumentNullException.ThrowIfNull(param);` for null-guarding method parameters/arguments.
- When updating `IIndexSymbolConverter.GetSymbol`, update all converters plus `IndexConverterManager` to match the signature and prevent CS0535 build errors.

## Testing Guidelines

- Tests use xUnit (`[Fact]` and `[Theory]`) and live next to the feature they exercise.
- Name files `*Tests.cs` and methods in the form `ShouldResultWhenScenario`.
- Cover new tensor operations with both algebraic sanity checks and edge-case assertions (e.g., zero metrics, mixed
  index types). Maintain parity with the corresponding Java tests whenever possible.
- Keep the suite green locally (`dotnet test`) before pushing; enable code coverage locally via
  `dotnet test --collect:"XPlat Code Coverage"` when evaluating larger refactors.

## Commit & Pull Request Guidelines

- Follow the existing history’s concise, present-tense summaries (e.g., `migrating to slnx format`,
  `porting functions and utils`, `updating NuGet packages`). Limit to 120 characters, adding detail in the body if
  required.
- Each PR should describe the ported feature or fix, list any deviations from upstream behaviour, and link related
  issues. Include screenshots or sample expressions when the change affects symbolic output.
- Document validation steps (`dotnet test`, benchmarks, manual algebra checks) in the PR checklist so reviewers can
  reproduce results quickly.

# Java to C# Transformation Guide

This note captures the patterns we follow when porting Redberry Java sources in `./redberry` into their C# counterparts
under `./NRedberry`. Examples come from already mapped files such as
`core/src/main/java/cc/redberry/core/tensor/Tensor.java`, `core/src/main/java/cc/redberry/core/indices/IndexType.java`,
and their ports `NRedberry.Core/Tensors/Tensor.cs`, `NRedberry.Core.Entities/IndexType.cs`,
`NRedberry.Core/Indices/IndexType.cs`.

## Structure, Namespaces, and Casing

- Java packages (`package cc.redberry.core.tensor;`) become file-scoped namespaces (
  `namespace NRedberry.Core.Tensors;`), with PascalCase segments that mirror the target project layout.
- `import` and `import static` statements translate into `using` (and `using static`) directives grouped at the top of
  the file before the namespace, e.g. `using NRedberry.Contexts;` in `NRedberry.Core/Tensors/Tensor.cs`.
- Types, methods, and properties adopt C# casing rules (PascalCase for public members, camelCase locals) and Allman
  braces. Compare method declarations in `core/src/main/java/cc/redberry/core/tensor/Tensor.java` and
  `NRedberry.Core/Tensors/Tensor.cs`.
- Nested classes and helper types that live in separate `.java` files are often reorganized into partial classes or
  static helper classes when that yields clearer .NET idioms (see `NRedberry.Core/Indices/IndexType.cs` versus the
  larger Java enum that bundled behaviour).

## Enumerations

- Java enums can store fields, constructors, and methods. C# enums are value-only, so behavioural logic is moved to
  companion types.
    - Example: `core/src/main/java/cc/redberry/core/indices/IndexType.java` embedded converter logic directly inside the
      enum. The C# port splits responsibilities between the plain enum (`NRedberry.Core.Entities/IndexType.cs`) and a
      static helper (`NRedberry.Core/Indices/IndexTypeMethods.cs`) containing extension methods, lookup tables, and
      cached converters.
- When the Java enum exposed instance members (`getSymbolConverter()`, `getShortString()`), we surface equivalent
  functionality as extension methods or static helpers (`GetSymbolConverter`, `GetShortString`) so call sites remain
  fluent.
- Stored state such as the Java `shortString` field is represented as dictionary maps (`CommonNames` in
  `IndexTypeMethods`) to preserve lookups without extending the enum definition.

## Properties vs. Getter/Setter Methods

- Java `getX()`/`setX(T value)` methods are mapped to C# properties with `get;`/`set;` accessors. The property name
  drops the `get`/`set` prefix and uses PascalCase:
    - `Tensor.getIndices()` → `public override Indices.Indices Indices { get; }` (`NRedberry.Core/Tensors/Tensor.cs`).
    - `SimpleTensor.getName()` → `public int Name { get; }` (`NRedberry.Core/Tensors/SimpleTensor.cs`).
- When the Java class supplied only a setter or getter, we create a read-only (`{ get; }`) or write-only (rare) property
  as appropriate. Backing validation logic from `setX(...)` becomes guard clauses inside the property setter or
  constructor.
- Fluent `setProperty(T value)` patterns often change to immutable constructors or builder methods because C# favours
  immutable types for thread safety—note how `SimpleTensor` checks for `null` in its constructor instead of deferring to
  `setIndices`.

## Documentation Comments

- JavaDoc `/** ... */` blocks convert into XML documentation comments with triple slashes. Tags map as:
    - `@return` → `<returns>`
    - `@see` → `<seealso>`
    - `{@link Type}` or `{@code}` → `<see cref="Type"/>` or `<c>literal</c>`, respectively.
- Example: the XML `<summary>` and `<seealso>` in `NRedberry.Core/Tensors/SimpleTensor.cs` replace the JavaDoc above
  `SimpleTensor.getName()`. (Some legacy block comments remain and should be converted during future clean up.)
- Multi-line license headers are omitted when the top-level solution already tracks licensing, unless a direct copy is
  legally required. Internal comments remain `//` and are tightened to explain non-obvious logic only.

## Iteration and Enumerables

- Java’s `implements Iterable<T>` and `Iterator<T> iterator()` map to `IEnumerable<T>` plus `GetEnumerator()` in C#.
  Non-generic iteration uses the explicit interface implementation required by .NET (see
  `NRedberry.Core/Tensors/Tensor.cs`).
- `for` loops remain familiar, but enhanced-for loops are often rewritten with LINQ helpers or standard `foreach`. When
  Java relied on `Iterator.remove()`, we refactor to collection copies or builder patterns because .NET enumerators are
  read-only.

## Exception and Null Handling

- Java’s `NullPointerException` and `IndexOutOfBoundsException` become `ArgumentNullException`/
  `ArgumentOutOfRangeException`/`IndexOutOfRangeException` as appropriate (`Tensor.set` → `Tensor.Set` in
  `NRedberry.Core/Tensors/Tensor.cs`).
- Assertions enforced with `Objects.requireNonNull` or `Preconditions.checkArgument` convert into guard clauses using
  standard .NET exceptions.
- Checked exceptions are removed; callers rely on documentation and tests because C# lacks checked exception signatures.

## Collections and Generics

- Java collections (`List`, `EnumSet`, `Iterator`) translate to `List<T>`, `HashSet<T>`, or specific structures from
  `System.Collections.Generic`. When Java used specialized Redberry iterators, C# ports may introduce helper classes or
  yield iterators (`yield return`) to match idiomatic enumeration (see `SimpleTensor.GetEnumerator()`).
- Java array cloning (`array.clone()`) becomes `Array.Copy`, `Span`, or new array allocations with loops, depending on
  performance needs.

## Formatting and Bracing Conventions

- Every type and method uses Allman-style braces even inside record-like constructs. File-scoped namespaces reduce
  indentation relative to Java’s outer braces.
- Expression-bodied members (`=>`) appear for simple getters to keep code succinct while honoring the 4-space
  indentation guideline.
- `var` is reserved for obvious anonymous types; otherwise we keep explicit types to mirror Java’s clarity (see
  `Tensor.Set` for explicit `var size` only when the inferred type is evident).

## Additional Observed Conventions

- Static utility access moves through dedicated context types: Java’s `Context.get()` is mirrored by `Context.Get()` in
  C# with PascalCase static methods (`NRedberry.Core/Tensors/Tensor.cs`).
- Where Java code relied on method overloading with raw types, we surface clearer signatures or nullable annotations (
  `TensorFactory? GetFactory()`).
- Java inner classes that act as builders or factories become separate public or internal C# types in the same
  namespace (`SimpleTensorBuilder`, `SimpleTensorFactory`) and frequently live in distinct files to satisfy .NET
  file-per-type expectations.
- Analyzers expect explicit nullable annotations: return types that may be absent are marked with `?`, and we introduce
  `ArgumentNullException` checks early to keep flow analyzers satisfied.

## Checklist for Future Ports

1. Update namespaces, using directives, and casing immediately after copying Java logic.
2. Replace getters/setters with properties and align constructor validation to guard inputs.
3. Convert JavaDoc to XML doc comments; remove lingering `/** */` blocks.
4. Refactor enums with behavior into enums plus companion static classes or extension methods.
5. Revisit iteration and collections to leverage `IEnumerable<T>` and C# iterators.
6. Audit exception types to match .NET expectations and nullability annotations.

## Translate different idioms between Java and C#

- Java uses an `bool MoveNext()` method, a `T? Current` property, and a `void Reset()` method for enumerations
    - When converting Java to C#, re-implement those methods and properties for compatibility reasons, but also
      implement `IEnumerable<T>` or an `IEnumerator<T>`
- Java uses `Copy()` while C# uses `Clone()`
    - When converting Java to C#, implement the `ICloneable` interface in those cases.
    - You may provide the `public T Copy() => Clone();` method as fallback.

```csharp
    // instead of T Copy()
    public T Clone() => new(...);
    object ICloneable.Clone() => Clone();

    // instead of T Copy(T value) when required
    public static T Clone(T c) => new(...);
```

- When a class implements the `Equals` method, also implement the `IEquatable<T>` interface
    - Also override the `==` and `!=` operators for equatable classes.
    - Also override the `ToHashCode()` method.
    - When the class is a reference type, make sure that the equality methods also work with nullable arguments:
      `public bool Equals(T? left, T? right)`.

## Class mappings

- Note all type mappings from the original java file to the cs file in the `ClassMapping.md` file, such that code ports
  are traceable to their original source file.
- When porting java types to C#, first create the empty *.cs file with a skeleton implementation, throwing
  `NotImplementedException` and using inline comment block to reflect the original source code that is to be ported.
- The port of the business logic should only be started, when the solution is building with the skeleton code.
- Avoid public fields. Prefer public Properties instead.
- Use `is null` instead of `== null` and `is not null` instead of `!= null`
- Do not note the variable type twice. Instead of `SomeType varible = new SomeType(5);` use either
  `var varible = new SomeType(5);` or `SomeType varible = new(5);`
- Use C# naming conventions.
    - Instead of method names like `isZero()` or `IsZERO()` use `IsZero()` instead.
    - Instead of a property named `ZERO` use `Zero` instead.
- When there are method names abbreviated like `GCD`, implement the method with a proper name like
  `GreatestCommonDivisor` and provide the `GCD` method as a redirection:
  ```csharp
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public int GCD(int a, int b) => GreatestCommonDivisor(a, b);
  ```  
- Avoid empty constructors when the empty constructor is the only constructor.
- Feel fee to use Unicode symbols in comments and summaries. I.e. write "Gröbner" instead of "Groebner" and "Poincaré"
  instead of "Poincare".
- Note that some elements like `Zero`, `One`, and `Imag` (complex i) elements are static properties in the C#
  interface (see `MonoidFactory.cs` and `AbelianGroupFactory.cs`).
    - They are implemented as non-static `GetONE()`, `GetZERO()` and `GetIMAG()` methods in Java. Do not replicate that
      pattern.
- When implementing `GetHashCode`, consider using the ` HashCode class:

```csharp
public override int GetHashCode()
{
    HashCode hashCode = new();
    hashCode.Add(Foo);
    hashCode.Add(Bar);
    return hashCode.ToHashCode();
} 
```

- Do not implement the `[Serializable]` attribute. The mechanism is obsolete in modern C#.
- Implement the standard Exception constructors when implementing custom exceptions.

## Class mapping process

- Check the ClassMapping.md file.
- find a file that marked with 'Skeleton pending full port'
- check the original *.java file; paths are relative to the D:/GitHub/redberry/ folder
- check the ported *.cs file; paths are relative to the D:/GitHub/NRedberry/ folder
- implement the skeleton of all the methods and properties of the type; the methods should throw
  NotImplementedException();
- Update the ClassMapping.md file with 'Skeleton pending method port' in the Notes column.
- Continue with the next class until finished

## Important Notes

- Do not try to port multiple files at once; implement one file at a time
- Always try to execute a build after every file; fix the build errors
- When a build error is encountered, update `AGENTS.md` with a note describing the root cause and the preventative step(s) needed to avoid the error in the future.
- Do not use PowerShell to execute Python and do not use Python to execute PowerShell. Use either.
- Python scripts can mess up line endings in C# files (issues with the difference between \n and \r). Prefer a PowerShell script with a regex for file encoding and line ending fixes.
- Build error note: `IndicesBuilder.cs` referenced obsolete `IntArray` ([Obsolete(..., true)]), causing CS0619; prevent by avoiding `IntArray`/`IntArrayList` in new ports and using `ImmutableArray<int>`/`List<int>` or `IEnumerable<int>` overloads instead.
- Build error note: `IndicesUtils.cs` used high-bit hex constants like `0x80000000`/`0x80FFFFFF`/`0xFF000000` without casts, which the compiler treated as `uint` (CS0266); prevent by using `unchecked((int)...)` (or named `int` masks) for signed constants with the high bit set.
- Build error note: `NRedberry.Core/Indices/IndicesUtils.cs` calls `GetSortedDistinct` as an extension method on `int[]`; keep the `this int[]` signature in `NRedberry.Maths.MathUtils.GetSortedDistinct` (or update call sites) to avoid CS1061.
- Build error note: Roslynator RCS1058 flagged `result = result * currentBase;` in `NumberUtils.cs`; prefer compound assignments like `result *= currentBase;`.
- Build error note: CS0246 for missing `Fact` in a test file; add `using Xunit;` at the top of new test skeletons.
- Build error note: RCS1073 flags simple guard `if` patterns; replace with a direct boolean return expression where appropriate.
- Build error note: `INumberTokenParser<T>` should constrain to `NRedberry.INumber<T>` (not `NRedberry.Numbers.INumber<T>`); the latter broke Complex/Real implementations with CS0311/CS0314.
- Build error note: after constraining `INumberTokenParser<T>` to `NRedberry.INumber<T>`, also add the same constraint to `NumberParser<T>`, `BracketToken<T>`, and `OperatorToken<T>` to avoid CS0314.
- Build error note: Roslynator RCS1085 flagged ExpandUtils.ExpandIndexlessSubproduct backing field; use an auto-implemented get-only property with initializer.
- Build error note: RCS0063 flagged an extra blank line after an opening brace in ExpandUtils; remove the stray blank line.
- Build error note: RCS0063 flagged trailing blank lines in Parser.cs; remove extra blank lines at file end to satisfy Roslynator.
- Build error note: RCS0063 flagged an extra blank line before the Multiply comment in GenSolvablePolynomial; avoid double blank lines between members.
- Build error note: RCS0063 flagged an extra blank line before the closing brace in OptimizedPolynomialList; remove the stray blank line.
- Build error note: Roslynator RCS0061 requires blank lines between switch sections; add a blank line between case blocks.
- Build error note: ParseToken.GetIndices must return Indices.Indices (not Indices), because Indices is a namespace; this avoids CS0118 and override mismatch CS0508.
- Build error note: Roslynator RCS1111 flags switch cases with multiple statements; wrap the case body in braces.
- Build error note: In ParseToken.cs, Tensors.Sum resolved to the Sum type because Tensors matches a namespace; use NRedberry.Tensors.Tensors.Sum(...) (or a using alias) to call the static method.
- Build error note: AbstractSumBuilder.Build/Put must be marked virtual so SumBuilder overrides compile (CS0506).
- Build error note: CS0542 occurs if a class defines a member with the same name as the class (e.g., ApplyIndexMapping.ApplyIndexMapping); rename such methods (e.g., Apply/ApplyInternal) when porting.
- Build error note: CS1540 occurs when calling protected Tensor.ToString<T> on a base-typed instance; use Tensor.ToStringWith<T>(OutputFormat) to format other tensors with the caller context.
- Build error note: CS1540 occurs when accessing a protected member through a base-typed qualifier (e.g., `ring.vars` on `GenPolynomialRing`); use a public accessor like `GetVars()` or keep the qualifier as the derived type.
- Build error note: RCS0055 flags binary expression chains; format multi-line bitwise expressions with each operator on its own line and consistent indentation.
- Build error note: StructureOfContractions masks mixed ulong and long, causing CS0019; keep mask types as long (e.g., UpperInfoMask) to match long operands, and expose Contraction as auto-properties to satisfy RCS1085.
- Build error note: Roslynator RCS1085 flagged backing fields for simple storage; use auto-implemented get-only properties (e.g., `private Tensor[] Args { get; }`).
- Build error note: Roslynator RCS1089 flagged `sb.Length -= 1;`; use the decrement operator (`sb.Length--;`) instead.
- Build error note: CS0246 in TensorUtils for `Permutation`; use `NRedberry.Core.Combinatorics.Permutation` (add the correct using or fully qualify).
- Build error note: CS0039 when casting `IIndexMapping` from `MappingsPort` to `Mapping`; use `IndexMappings.CreatePortOfBuffers` and build `Mapping` from `IIndexMappingBuffer` instead.
- Build error note: RCS0054 flagged chained `StringBuilder` calls in `TensorUtils.Info`; start the chain on a new line and align subsequent calls.
- Build error note: RCS1134 flagged a redundant `continue` in `TensorUtils.Count`; remove the dead branch and rely on the inner-loop `break` only.
- Build error note: Roslynator RCS1085 flagged non-auto singleton properties (e.g., TensorWrapperFactory.Instance); use auto-implemented get-only properties with initializers (e.g., `Instance { get; } = new()`).
- Build error note: Roslynator RCS0003 requires a blank line between using directives and the namespace declaration
- Build error note: JasFactor.Engine was missing, causing CS0117; ensure JasFactor exposes the Engine singleton and implements ITransformation before referencing it.
- Build error note: CS0104 ambiguous BigInteger/Complex when using System.Numerics alongside JAS and tensor types; use aliases like SystemBigInteger or TensorComplex and avoid unqualified type names.
- Build error note: CS0029 occurred by reusing a FromChildToParentIterator variable for a FromParentToChildIterator; type the variable as ITreeIterator or use separate variables.
- Build error note: RCS0013 flags missing blank lines between different declaration kinds; add a blank line between fields and properties.
- Build error note: RCS1162 flags chained assignments; split variable initialization into separate statements.
- Build error note: RCS1134 flags redundant continue statements at the end of a loop branch; remove the redundant continue.
- Build error note: RCS1134 flagged a redundant `continue` in `ParserIndices.ParseSimpleIgnoringVariance`; remove the no-op continue and keep the inner-loop `break`.
- Build error note: CS0506 occurs if derived operator parsers override `ParseToken` while `ParserOperator.ParseToken` is not virtual; mark the base method as `virtual` or remove the override.
- Build error note: RCS0015 flags blank lines between using directives; keep using directives contiguous and only leave a blank line before the namespace.
- Build error note: CS0104 can occur when CC is ambiguous between NRedberry.Contexts and NRedberry.Tensors; use an alias or fully qualify the intended CC.
- Build error note: CS0104 can occur when MathUtils is ambiguous between NRedberry.Maths and NRedberry.Core.Utils; use an alias (e.g., MathsUtils) or fully qualify.
- Build error note: CS0246 occurs when referencing ProductBuilder in ports; use ScalarsBackedProductBuilder (or TensorBuilder) instead.
- Build error note: CS0246 for Averaging in Physics.Tests; add using NRedberry.Physics.Oneloopdiv (or fully qualify Averaging).
- Build error note: CS0019 occurs when using SimpleIndices.Size as a property; call Size() to get the count.
- Build error note: RCS1085 flagged the IIndexMappingProvider EmptyProvider property backing field; use an auto-implemented get-only property with initializer instead.
- Build error note: RCS1146 flagged a null check before RemoveContracted; prefer conditional access like `buffer?.RemoveContracted()` to satisfy the analyzer.
- Build error note: CS0117 occurs if referencing `IIndexMappingProvider.EmptyProvider`; use `IndexMappingProviderUtil.EmptyProvider` instead.
- Build error note: CS0246 can occur when using `IOutputPort<>` without `using NRedberry.Concurrent;` (e.g., ProviderFunctions); add the using to resolve the interface.
- Build error note: In files under NRedberry.Physics, `Tensors` can be resolved as a namespace; use an alias like `using TensorFactory = NRedberry.Tensors.Tensors;` when calling `Multiply`/`Expression` to avoid CS0234/CS1955.
- Build error note: RCS1146 also flags null checks before calling `GetSign()` on nullable mapping buffers; use conditional access like `buffer?.GetSign()` to satisfy the analyzer.
- Build error note: RCS1146 flagged null checks before calling `EqualsRegardlessOrder`; use conditional access like `_indices?.EqualsRegardlessOrder(...) != true` to satisfy the analyzer.
- Build error note: CS0403 occurs when returning null for generic number token parsers; return `default!` (or `default(T)`) to represent "no match" for `T`.
- Build error note: Xunit SkipException has no public constructors; use `SkipException.ForSkip("message")` when skipping tests.
- Build error note: RCS1132 flags overrides that only forward to the base implementation; remove the redundant override instead of calling `base`.
- Build error note: RCS1132 flagged a redundant `GetHashCode` override in GenSolvablePolynomial; avoid adding overrides that only return `base.GetHashCode()`.
- Build error note: RCS0054 flags multi-line call chains; split construction and method calls into separate statements.
- Build error note: ParserSimpleTensor.cs used SimpleIndices without `using NRedberry.Indices;`, causing CS0246; add the correct using when referencing index types.
- Build error note: GapGroupsInterface.cs referenced `NRedberry.Core.Groups`; use `NRedberry.Groups` for `PermutationGroup` to avoid CS0234.
- Build error note: GapGroupsInterface.cs needs `Permutation` from `NRedberry.Core.Combinatorics`; add the correct using to avoid CS0246.
- Build error note: RCS1074 flags redundant empty constructors; remove the no-op constructor unless it contains required initialization logic.
- Build error note: RCS1155 flags string comparisons without StringComparison; use string.Equals(..., StringComparison.OrdinalIgnoreCase) instead of ToLower/ToUpper comparisons.
- Build error note: CS0619 can occur when an API is marked `[Obsolete(..., true)]` and still used internally; change to non-error obsolete or avoid the deprecated type in code paths.
- Build error note: CS8859 occurs if a record defines a member named `Clone`; rename the method (e.g., `Copy`) or use a non-record type to avoid the restriction.
- Build error note: RCS1006 flags nested `if` inside `else`; merge into `else if` to satisfy Roslynator.
- Build error note: ExternalSolver.cs used `Tensors.Expression`/`Tensors.ParseExpression` and `Tensors` resolved to the namespace, causing CS1955/CS0234; use `NRedberry.Tensors.Tensors` (or a using alias) when calling static tensor helpers.
- Build error note: CS0246 when using `TransformationCollection` without the correct namespace; add `using NRedberry.Transformations.Symmetrization;` (the class lives there).
- Build error note: RCS0055/RCS0027 can flag long string concatenation chains; prefer `StringBuilder` (or keep each `+` on its own line with consistent indentation).
- Build error note: RCS0055 can also flag long string concatenations in expressions; prefer `string.Concat(...)` to avoid binary chain formatting errors.
- Build error note: FrobeniusNumber.cs used Java's `UnsupportedOperationException` (CS0246) and missed a blank line between const and static fields (RCS0013); use `NotSupportedException` and keep a blank line between declaration kinds.
- Porting note: Combinatoric.cs replaces Java `UnsupportedOperationException` with `NotSupportedException` (no direct C# equivalent).
- Porting note: ModInteger.cs implements modular inverse via a custom extended-GCD helper because `System.Numerics.BigInteger` lacks `modInverse`.
- Porting note: ModLongRing.cs adds `BigIntegerExtensions.IsProbablePrime` and `GetBitLength` to replace Java `BigInteger.isProbablePrime`/`bitLength` (no direct .NET equivalents).
- Porting note: ModularNotInvertibleException.cs stores factor payloads as `object?` because the Java `GcdRingElem` type parameters cannot be expressed on an exception type in C#.
- Porting note: AlgebraicNotInvertibleException.cs uses `object?` for polynomial factors because the original `GenPolynomial` generic payload cannot be carried by a non-generic exception.
- Build error note: InverseTensor.cs referenced missing `TensorGenerator` (CS0103); ensure `NRedberry.TensorGenerators.TensorGenerator` is ported (or stubbed) before solver classes call `GenerateStructure`.
- Build error note: `TensorGenerator.GenerateStructure` requires `SimpleIndices`; pass `SimpleTensor.SimpleIndices` (not `Indices`) to avoid CS1503.
- Build error note: RCS0053 flagged base type formatting in `AlgebraicNumberRing.cs`; keep base types on a single line (or align per Roslynator) to avoid formatting errors.
- Porting note: `ComplexRing.algebraicRing` in Java uses `pfac.getONE()`; in C# use `GenPolynomialRing.GetOneCoefficient()` with `Sum(...)` to build `x^2 + 1` since there is no instance `getONE()` for polynomials.
- Porting note: `AlgebraicNumberRing.Depth/TotalExtensionDegree` use reflection + `dynamic` to detect nested `AlgebraicNumberRing<>` because Java relied on raw `instanceof` checks that don't map cleanly to generic C# types.
- Build error note: FactorOutNumber.cs failed with CS0246 for `TransformationToStringAble`; add `using NRedberry.Transformations.Symmetrization;` (the interface lives in that namespace).
- Build error note: BigComplex.cs hit RCS1146 for null checks; use conditional access like `S?.IsZero() != false` instead of `S is null || S.IsZero()`.
- Build error note: Renaming `ModLongRing.MAX_LONG` to `MaxLong` requires updating all references (e.g., Ufd/HenselUtil* and GreatestCommonDivisorModular) to avoid CS0117.
- Porting note: FactorOutNumber replaces Java `Product.getFactor()` with the `Product.Factor` property, Java `Complex.IMAGINARY_UNIT` with `Complex.ImaginaryOne`, and Java `BigInteger.gcd(...)` with `BigInteger.GreatestCommonDivisor(...)`.
- Porting note: SingularFactorizationEngine replaces Java `ProcessBuilder`/`BufferedReader`/`PrintStream` with `ProcessStartInfo` + `Process.StandardOutput`/`StandardInput` and maps `AutoCloseable`/`Closeable` to `IDisposable` plus a `Close()` wrapper.
- Porting note: SingularFactorizationEngine mirrors Java `toArray(new SimpleTensor[1])` by ensuring a minimum array length and emitting `"null"` for empty variable lists when building the Singular ring.
- Porting note: BigComplex replaces Java public fields `re/im` with `Re`/`Im` get-only properties, renames `ZERO/ONE/I` to `ZeroValue`/`OneValue`/`ImaginaryUnit` to follow PascalCase and avoid clashes with instance properties, and adds `IEquatable<BigComplex>` plus `==/!=` operators for C# equality semantics.
- Porting note: BigInteger replaces Java public field `val` with a get-only `Val` property, renames the static random generator to `s_random`, prefixes iterator state with `_` for C# field naming, and adds `IEquatable<BigInteger>` plus `==/!=` operators.
- Porting note: BigRational replaces Java public fields `num/den` with get-only `Num`/`Den` properties, renames the static random generator to `s_random`, and adds `IEquatable<BigRational>` plus `==/!=` operators.
- Porting note: Java `Tensors.setIndices(SimpleTensor, SimpleIndices)` has no direct C# equivalent; use `Tensor.SimpleTensor(name, IndicesFactory.CreateSimple(descriptor.GetSymmetries(), indices))` to rebuild with indices.
- Porting note: Java `TIntHashSet` replaced with `HashSet<int>` in C# when a specialized int set is unavailable.
- Porting note: Java `Product.getDataSubProduct()` is not exposed in C#; use `product.Data.Length == 1 && product.Data[0] is SimpleTensor` when only the simple-tensor data subproduct check is needed.
- Porting note: Java `TIntObjectHashMap` is replaced with `Dictionary<int, List<T>>` when porting collect/aggregation logic.
- Porting note: Java `IntArrayList` uses `List<int>` in C# when only simple append/clear/to-array behavior is needed.
- Build error note: CollectTransformation had a method named `Split` conflicting with the nested `Split` class (CS0102); rename the method (e.g., `SplitTerm`) to avoid name collisions.
- Porting note: Java `Product.getIndexlessSubProduct()` is not available in C#; re-create it from `Product.Factor` and `Product.IndexlessData` when needed (e.g., ExpandTensorsTransformation).
- Build error note: ExpandTensorsTransformation missed `using NRedberry.Numbers` for `Complex` (CS0103); add the correct using when referencing numeric types.
- Build error note: OptionAttribute exposes `Name`/`Index` properties (no `name`/`index` ctor args); use `[Option(Name = "...", Index = ...)]` to avoid CS1739.
- Build error note: OptionAttribute is valid only on fields/parameters (CS0592); apply it to backing fields instead of properties.
- Build error note: RCS0053 flags long argument lists; format method calls with each argument on its own line.
- Build error note: `Tensor`/`Tensors` can resolve as namespaces (CS0118/CS0234); use aliases like `TensorType = NRedberry.Tensors.Tensor` and `TensorFactory = NRedberry.Tensors.Tensors`.
- Build error note: RCS1118 flags locals that can be const; use `const` for computed locals like `const int pivot = K / 6;`.
- Build error note: `IndexType` is in the `NRedberry` namespace (not `NRedberry.Core.Entities`); use `using NRedberry;` to avoid CS0234.
- Build error note: `IdentityTransformation` has no singleton; instantiate with `new IdentityTransformation()` to avoid CS0117.
- Build error note: `NRedberry.Physics.Tests` has no xUnit reference; avoid `[Fact]`/`using Xunit` there to prevent CS0246.

## Roslynator Diagnostics Reference

- RCS0013: Add blank line between declarations (docs: https://josefpihrt.github.io/docs/roslynator/analyzers/RCS0013/).
- RCS0015: Remove blank line between using directives (docs: https://josefpihrt.github.io/docs/roslynator/analyzers/RCS0015/).
- RCS1085: Use auto-implemented property (docs: https://josefpihrt.github.io/docs/roslynator/analyzers/RCS1085/).
- RCS1089: Use --/++ operator instead of assignment (docs: https://josefpihrt.github.io/docs/roslynator/analyzers/RCS1089/).
- RCS1058: Use compound assignment (docs: https://josefpihrt.github.io/docs/roslynator/analyzers/RCS1058/).
- RCS1111: Add braces to switch section with multiple statements (docs: https://josefpihrt.github.io/docs/roslynator/analyzers/RCS1111/).
- RCS0054: Fix formatting of a call chain (docs: https://josefpihrt.github.io/docs/roslynator/analyzers/RCS0054/).
- RCS0055: Fix formatting of a binary expression chain (docs: https://josefpihrt.github.io/docs/roslynator/analyzers/RCS0055/).
- RCS0061: Add blank lines between switch sections (docs: https://josefpihrt.github.io/docs/roslynator/analyzers/RCS0061/).
- RCS0063: Remove unnecessary blank line (docs: https://josefpihrt.github.io/docs/roslynator/analyzers/RCS0063/).
- RCS0003: Add blank line after using directive list (docs: https://josefpihrt.github.io/docs/roslynator/analyzers/RCS0003/).
- RCS0008: Add blank line between closing brace and next statement (docs: https://josefpihrt.github.io/docs/roslynator/analyzers/RCS0008/).
- RCS1134: Remove redundant statement (docs: https://josefpihrt.github.io/docs/roslynator/analyzers/RCS1134/).
- RCS1132: Remove redundant override (docs: https://josefpihrt.github.io/docs/roslynator/analyzers/RCS1132/).
- RCS1146: Use conditional access (docs: https://josefpihrt.github.io/docs/roslynator/analyzers/RCS1146/).
- RCS1155: Use StringComparison (docs: https://josefpihrt.github.io/docs/roslynator/analyzers/RCS1155/).
- RCS1162: Avoid chained assignments (docs: https://josefpihrt.github.io/docs/roslynator/analyzers/RCS1162/).

## Global usings

- Global usings are enables. Do not import the following namespaces:

```csharp
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
```

## Conversion notes

Some types and methods need to be replaced with .NET types:

| Java Type                                                    | .NET Type           | Notes                                    |
|--------------------------------------------------------------|---------------------|------------------------------------------|
| IntArray                                                     | ImmutableArray<int> | Different type                           |
| IntArrayList                                                 | List<int>           | Different type                           |
| Degree()                                                     | Degree              | Property with getter instead of method   |
| Length()                                                     | Length              | Property with getter instead of method   |
| Parity()                                                     | Parity              | Property with getter instead of method   |
| GetIdentity()                                                | Identity            | Property with getter instead of method   |
| IsIdentity()                                                 | IsIdentity          | Property with getter instead of method   |
| Order()                                                      | Order               | Property with getter instead of method   |
| OrderIsOdd()                                                 | OrderIsOdd          | Property with getter instead of method   |
| GetONE()                                                     | One                 | Property with getter instead of method   |
| GetZERO()                                                    | Zero                | Property with getter instead of method   |
| LengthsOfCycles()                                            | LengthsOfCycles     | Property with getter instead of method   |
| GetNumerator()                                               | Numerator           | Property with getter instead of method   |
| GetDenominator()                                             | Denominator         | Property with getter instead of method   |
| CC.GetRandomGenerator()                                      | System.Random.Shared | System property instead of custom method |
| CC.GetNameManager()                                          | CC.NameManager     | Property with getter instead of method   |
| CC.GetIndexConverterManager()                                | CC.IndexConverterManager | Property with getter instead of method   |
| CC.GetDefaultOutputFormat() / CC.SetDefaultOutputFormat(OutputFormat) | CC.DefaultOutputFormat | Property with getter instead of method   |

# Issue tracking

- Before starting any work, run 'bd onboard' to understand the current project state and available issues.
- Do not make any file changes without a bd ticket

## Landing the Plane (Session Completion)

**When ending a work session**, you MUST complete ALL steps below. Work is NOT complete until `git push` succeeds.

**MANDATORY WORKFLOW:**

1. **File issues for remaining work** - Create issues for anything that needs follow-up
2. **Run quality gates** (if code changed) - Tests, linters, builds
3. **Update issue status** - Close finished work, update in-progress items
4. **PUSH TO REMOTE** - This is MANDATORY:
   ```bash
   git pull --rebase
   bd sync
   git push
   git status  # MUST show "up to date with origin"
   ```
5. **Clean up** - Clear stashes, prune remote branches
6. **Verify** - All changes committed AND pushed
7. **Hand off** - Provide context for next session

**CRITICAL RULES:**
- Work is NOT complete until `git push` succeeds
- NEVER stop before pushing - that leaves work stranded locally
- NEVER say "ready to push when you are" - YOU must push
- If push fails, resolve and retry until it succeeds








