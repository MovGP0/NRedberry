# Repository Guidelines

## Project Structure & Module Organization
- `NRedberry.Core/` is the main port of the Redberry engine. Subfolders group functionality (for example, `Tensors/`, `Transformations/`, `Contexts/`, `Utils/`) and generally mirror the original Java packages. Keep new code alongside the closest conceptual peer.
- Supporting libraries live under sibling projects such as `NRedberry.Apache.Commons.Math/`, `NRedberry.Core.Entities/`, and `NRedberry.Core.Utils/`; add shared primitives there rather than duplicating logic inside `NRedberry.Core/`.
- Automated tests reside in `NRedberry.Core.Tests/`, which references the core project directly. Place new test fixtures in module-matching subfolders to make cross-referencing straightforward.

## Build, Test, and Development Commands
- `dotnet restore NRedberry.slnx` — restore dependencies for the core library and its project references.
- `dotnet build NRedberry.slnx -c Release` — compile all linked projects (net9.0) with analyzers enabled.
- `dotnet test NRedberry.slnx ` — run the xUnit suite; use `--filter Category=...` to scope runs during iteration.
- IDE users can open `NRedberry.slnx`.

## Coding Style & Naming Conventions
- Use four-space indentation, Allman braces, and explicit visibility; follow the prevailing C# casing (`PascalCase` types/methods, `camelCase` locals).
- Nullable reference types are enabled—prefer non-nullable signatures and guard inputs explicitly.
- The solution relies on Roslynator analyzers; run `dotnet format` or address warnings before sending reviews. Mirror existing JavaDoc-style comments (`/** ... */`) when porting descriptive docs from the Java codebase.

## Testing Guidelines
- Tests use xUnit (`[Fact]` and `[Theory]`) and live next to the feature they exercise.
- Name files `*Tests.cs` and methods in the form `ShouldResultWhenScenario`.
- Cover new tensor operations with both algebraic sanity checks and edge-case assertions (e.g., zero metrics, mixed index types). Maintain parity with the corresponding Java tests whenever possible.
- Keep the suite green locally (`dotnet test`) before pushing; enable code coverage locally via `dotnet test --collect:"XPlat Code Coverage"` when evaluating larger refactors.

## Commit & Pull Request Guidelines
- Follow the existing history’s concise, present-tense summaries (e.g., `migrating to slnx format`, `porting functions and utils`, `updating NuGet packages`). Limit to 120 characters, adding detail in the body if required.
- Each PR should describe the ported feature or fix, list any deviations from upstream behaviour, and link related issues. Include screenshots or sample expressions when the change affects symbolic output.
- Document validation steps (`dotnet test`, benchmarks, manual algebra checks) in the PR checklist so reviewers can reproduce results quickly.

# Java to C# Transformation Guide

This note captures the patterns we follow when porting Redberry Java sources in `./redberry` into their C# counterparts under `./NRedberry`. Examples come from already mapped files such as `core/src/main/java/cc/redberry/core/tensor/Tensor.java`, `core/src/main/java/cc/redberry/core/indices/IndexType.java`, and their ports `NRedberry.Core/Tensors/Tensor.cs`, `NRedberry.Core.Entities/IndexType.cs`, `NRedberry.Core/Indices/IndexType.cs`.

## Structure, Namespaces, and Casing
- Java packages (`package cc.redberry.core.tensor;`) become file-scoped namespaces (`namespace NRedberry.Core.Tensors;`), with PascalCase segments that mirror the target project layout.
- `import` and `import static` statements translate into `using` (and `using static`) directives grouped at the top of the file before the namespace, e.g. `using NRedberry.Contexts;` in `NRedberry.Core/Tensors/Tensor.cs`.
- Types, methods, and properties adopt C# casing rules (PascalCase for public members, camelCase locals) and Allman braces. Compare method declarations in `core/src/main/java/cc/redberry/core/tensor/Tensor.java` and `NRedberry.Core/Tensors/Tensor.cs`.
- Nested classes and helper types that live in separate `.java` files are often reorganized into partial classes or static helper classes when that yields clearer .NET idioms (see `NRedberry.Core/Indices/IndexType.cs` versus the larger Java enum that bundled behaviour).

## Enumerations
- Java enums can store fields, constructors, and methods. C# enums are value-only, so behavioural logic is moved to companion types.
  - Example: `core/src/main/java/cc/redberry/core/indices/IndexType.java` embedded converter logic directly inside the enum. The C# port splits responsibilities between the plain enum (`NRedberry.Core.Entities/IndexType.cs`) and a static helper (`NRedberry.Core/Indices/IndexTypeMethods.cs`) containing extension methods, lookup tables, and cached converters.
- When the Java enum exposed instance members (`getSymbolConverter()`, `getShortString()`), we surface equivalent functionality as extension methods or static helpers (`GetSymbolConverter`, `GetShortString`) so call sites remain fluent.
- Stored state such as the Java `shortString` field is represented as dictionary maps (`CommonNames` in `IndexTypeMethods`) to preserve lookups without extending the enum definition.

## Properties vs. Getter/Setter Methods
- Java `getX()`/`setX(T value)` methods are mapped to C# properties with `get;`/`set;` accessors. The property name drops the `get`/`set` prefix and uses PascalCase:
  - `Tensor.getIndices()` → `public override Indices.Indices Indices { get; }` (`NRedberry.Core/Tensors/Tensor.cs`).
  - `SimpleTensor.getName()` → `public int Name { get; }` (`NRedberry.Core/Tensors/SimpleTensor.cs`).
- When the Java class supplied only a setter or getter, we create a read-only (`{ get; }`) or write-only (rare) property as appropriate. Backing validation logic from `setX(...)` becomes guard clauses inside the property setter or constructor.
- Fluent `setProperty(T value)` patterns often change to immutable constructors or builder methods because C# favours immutable types for thread safety—note how `SimpleTensor` checks for `null` in its constructor instead of deferring to `setIndices`.

## Documentation Comments
- JavaDoc `/** ... */` blocks convert into XML documentation comments with triple slashes. Tags map as:
  - `@return` → `<returns>`
  - `@see` → `<seealso>`
  - `{@link Type}` or `{@code}` → `<see cref="Type"/>` or `<c>literal</c>`, respectively.
- Example: the XML `<summary>` and `<seealso>` in `NRedberry.Core/Tensors/SimpleTensor.cs` replace the JavaDoc above `SimpleTensor.getName()`. (Some legacy block comments remain and should be converted during future clean up.)
- Multi-line license headers are omitted when the top-level solution already tracks licensing, unless a direct copy is legally required. Internal comments remain `//` and are tightened to explain non-obvious logic only.

## Iteration and Enumerables
- Java’s `implements Iterable<T>` and `Iterator<T> iterator()` map to `IEnumerable<T>` plus `GetEnumerator()` in C#. Non-generic iteration uses the explicit interface implementation required by .NET (see `NRedberry.Core/Tensors/Tensor.cs`).
- `for` loops remain familiar, but enhanced-for loops are often rewritten with LINQ helpers or standard `foreach`. When Java relied on `Iterator.remove()`, we refactor to collection copies or builder patterns because .NET enumerators are read-only.

## Exception and Null Handling
- Java’s `NullPointerException` and `IndexOutOfBoundsException` become `ArgumentNullException`/`ArgumentOutOfRangeException`/`IndexOutOfRangeException` as appropriate (`Tensor.set` → `Tensor.Set` in `NRedberry.Core/Tensors/Tensor.cs`).
- Assertions enforced with `Objects.requireNonNull` or `Preconditions.checkArgument` convert into guard clauses using standard .NET exceptions.
- Checked exceptions are removed; callers rely on documentation and tests because C# lacks checked exception signatures.

## Collections and Generics
- Java collections (`List`, `EnumSet`, `Iterator`) translate to `List<T>`, `HashSet<T>`, or specific structures from `System.Collections.Generic`. When Java used specialized Redberry iterators, C# ports may introduce helper classes or yield iterators (`yield return`) to match idiomatic enumeration (see `SimpleTensor.GetEnumerator()`).
- Java array cloning (`array.clone()`) becomes `Array.Copy`, `Span`, or new array allocations with loops, depending on performance needs.

## Formatting and Bracing Conventions
- Every type and method uses Allman-style braces even inside record-like constructs. File-scoped namespaces reduce indentation relative to Java’s outer braces.
- Expression-bodied members (`=>`) appear for simple getters to keep code succinct while honoring the 4-space indentation guideline.
- `var` is reserved for obvious anonymous types; otherwise we keep explicit types to mirror Java’s clarity (see `Tensor.Set` for explicit `var size` only when the inferred type is evident).

## Additional Observed Conventions
- Static utility access moves through dedicated context types: Java’s `Context.get()` is mirrored by `Context.Get()` in C# with PascalCase static methods (`NRedberry.Core/Tensors/Tensor.cs`).
- Where Java code relied on method overloading with raw types, we surface clearer signatures or nullable annotations (`TensorFactory? GetFactory()`).
- Java inner classes that act as builders or factories become separate public or internal C# types in the same namespace (`SimpleTensorBuilder`, `SimpleTensorFactory`) and frequently live in distinct files to satisfy .NET file-per-type expectations.
- Analyzers expect explicit nullable annotations: return types that may be absent are marked with `?`, and we introduce `ArgumentNullException` checks early to keep flow analyzers satisfied.

## Checklist for Future Ports
1. Update namespaces, using directives, and casing immediately after copying Java logic.
2. Replace getters/setters with properties and align constructor validation to guard inputs.
3. Convert JavaDoc to XML doc comments; remove lingering `/** */` blocks.
4. Refactor enums with behavior into enums plus companion static classes or extension methods.
5. Revisit iteration and collections to leverage `IEnumerable<T>` and C# iterators.
6. Audit exception types to match .NET expectations and nullability annotations.

## Translate different idioms between Java and C#
- Java uses an `bool MoveNext()` method, a `T? Current` property, and a `void Reset()` method for enumerations
  - When converting Java to C#, re-implement those methods and properties for compatibility reasons, but also implement `IEnumerable<T>` or an `IEnumerator<T>`
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
  - When the class is a reference type, make sure that the equality methods also work with nullable arguments: `public bool Equals(T? left, T? right)`.

## Class mappings

- Note all type mappings from the original java file to the cs file in the `ClassMapping.md` file, such that code ports are traceable to their original source file.
- When porting java types to C#, first create the empty *.cs file with a skeleton implementation, throwing `NotImplementedException` and using inline comment block to reflect the original source code that is to be ported.
- The port of the business logic should only be started, when the solution is building with the skeleton code.
- Avoid public fields. Prefer public Properties instead.
- Use `is null` instead of `== null` and `is not null` instead of `!= null`
- Do not note the variable type twice. Instead of `SomeType varible = new SomeType(5);` use either `var varible = new SomeType(5);` or `SomeType varible = new(5);`
- Use C# naming conventions. 
  - Instead of method names like `isZero()` or `IsZERO()` use `IsZero()` instead.
  - Instead of a property named `ZERO` use `Zero` instead.
- When there are method names abbreviated like `GCD`, implement the method with a proper name like `GreatestCommonDivisor` and provide the `GCD` method as a redirection:
  ```csharp
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public int GCD(int a, int b) => GreatestCommonDivisor(a, b);
  ```  
- Avoid empty constructors when the empty constructor is the only constructor.
- Feel fee to use Unicode symbols in comments and summaries. I.e. write "Gröbner" instead of "Groebner" and "Poincaré" instead of "Poincare".
- Note that some elements like `Zero`, `One`, and `Imag` (complex i) elements are static properties in the C# interface (see `MonoidFactory.cs` and `AbelianGroupFactory.cs`).
  - They are implemented as non-static `GetONE()`, `GetZERO()` and `GetIMAG()` methods in Java. Do not replicate that pattern.
- When implementing `GetHashCode`, consider using the ` HashCode  class:
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
- implement the skeleton of all the methods and properties of the type; the methods should throw NotImplementedException();
- Update the ClassMapping.md file with 'Skeleton pending method port' in the Notes column.
- Continue with the next class until finished

## Important Notes
- Do not try to port multiple files at once; implement one file at a time
- Always try to execute a build after every file; fix the build errors
- Do not use PowerShell to execute Python and do not use Python to execute PowerShell. Use either.
- Python scripts can mess up line endings in C# files (issues with the difference between \n and \r). Prefer a PowerShell script with a regex for file encoding and line ending fixes.

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
