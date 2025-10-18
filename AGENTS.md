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
