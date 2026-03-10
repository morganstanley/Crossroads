# .NET 10 Upgrade Plan

Table of Contents

- Executive Summary
- Migration Strategy
- Detailed Dependency Analysis
- Project-by-Project Plans
- Package Update Reference
- Breaking Changes Catalog
- Testing & Validation Strategy
- Risk Management
- Complexity & Effort Assessment
- Source Control Strategy
- Success Criteria

---

## Executive Summary

### Selected Strategy
**All-At-Once Strategy** - All projects upgraded simultaneously in a single coordinated operation.

**Rationale**:
- 4 projects (small solution)
- Current targets: `net8.0` for all projects
- Assessment shows most packages compatible or have recommended upgrades
- All-at-once minimizes multi-targeting complexity and yields a single verification pass

### High-level Metrics (from assessment)

- Total Projects: 4
- Total Lines of Code: 6,099
- Projects with issues: all 4 (source or binary incompatibilities identified)
- Notable package upgrades: Microsoft.Extensions.* packages to `10.0.4`

### Critical Issues

- Source-incompatible API usages identified across projects (System.CommandLine, ConfigurationBinder, etc.)
- A small set of NuGet packages recommended for upgrade (`Microsoft.Extensions.*`)
- Test frameworks include deprecated `xunit` version flagged for replacement later

### Expected Iterations

- Phase 0: Preparation (SDK, branch)
- Phase 1: Atomic upgrade (update all projects and packages simultaneously)
- Phase 2: Test validation and fixes

## Migration Strategy

### Approach
Apply the All-At-Once Strategy: update all project target frameworks and package references in a single atomic operation, followed by a full solution restore, build and test run.

### Justification
- Solution size and dependency depth are small (4 projects, depth ≤ 2).
- Assessment indicates compatible package versions are available for .NET 10 or upgrades are straightforward.
- All-at-once minimizes multi-targeting complexity and simplifies dependency resolution.

### Phases (conceptual)
- Phase 0: Prerequisites - ensure .NET 10 SDK is available, update `global.json` if present, switch to `upgrade-to-NET10` branch.
- Phase 1: Atomic Upgrade - change TargetFramework to `net10.0` for all projects, update package versions as specified, restore and build the solution, fix compilation errors in a single pass.
- Phase 2: Testing & Validation - run all tests, address test failures, validate key runtime behaviors.

### Source control
- Create and work on branch `upgrade-to-NET10`. Commit the entire atomic upgrade as a single logical change with message: "Atomic: Upgrade solution to .NET 10 and update packages".

> Note: The plan prescribes WHAT to change and WHY. It does not execute changes.

## Detailed Dependency Analysis

Summary from assessment:
- Projects: `src\Crossroads\Crossroads.csproj`, `src\Crossroads.Launcher\Crossroads.Launcher.csproj`, `test\Crossroads.Test\Crossroads.Test.csproj`, `test\Crossroads.Launcher.Test\Crossroads.Launcher.Test.csproj`.
- Dependency graph depth ≤ 2. `Crossroads.Test` depends on `Crossroads`; `Crossroads.Launcher.Test` depends on `Crossroads.Launcher`.
- No circular dependencies detected.

Migration ordering (respect dependency constraints but apply All-At-Once): list all projects to be changed together:

- `src\Crossroads.Launcher\Crossroads.Launcher.csproj`
- `src\Crossroads\Crossroads.csproj`
- `test\Crossroads.Launcher.Test\Crossroads.Launcher.Test.csproj`
- `test\Crossroads.Test\Crossroads.Test.csproj`

Even though dependency order places libraries before consumers, the All-At-Once strategy updates all simultaneously and then builds the whole solution to reveal issues.

## Project-by-Project Plans

Below are per-project specifications. Each project section lists current state, target state and concrete migration steps.

### Project: `src\Crossroads\Crossroads.csproj`
**Current State**: TargetFramework `net8.0`; key packages include `Newtonsoft.Json (13.0.4)`, `Microsoft.Extensions.Hosting (9.0.10)`, `Microsoft.Extensions.Configuration (9.0.10)`, `Microsoft.Extensions.DependencyModel (9.0.10)`, `System.CommandLine.Hosting (0.3.0-alpha.*)`.

**Target State**: TargetFramework `net10.0`; update `Microsoft.Extensions.Hosting`, `Microsoft.Extensions.Configuration`, `Microsoft.Extensions.DependencyModel` to `10.0.4` per assessment. Retain `Newtonsoft.Json` unless opting to migrate to `System.Text.Json` (separate modernization scenario).

**Migration Steps**:
1. Ensure Phase 0 prerequisites satisfied: .NET 10 SDK installed, `global.json` compatible or updated. Switch to `upgrade-to-NET10` branch.
2. Update `<TargetFramework>` to `net10.0` (or append if multi-targeted).
3. Update PackageReference versions for `Microsoft.Extensions.Hosting`, `Microsoft.Extensions.Configuration`, `Microsoft.Extensions.DependencyModel` to `10.0.4`.
4. Run `dotnet restore` for the solution.
5. Build the full solution to surface compilation errors.
6. Address compilation errors caused by API changes (see Breaking Changes Catalog). Make focused code fixes and document each change.
7. Run unit tests and address failing tests.

**Validation Checklist**:
- [ ] Project file contains `net10.0`.
- [ ] Updated package versions are present.
- [ ] Project builds without errors as part of full solution build.
- [ ] Unit tests pass for project and dependants.

### Project: `src\Crossroads.Launcher\Crossroads.Launcher.csproj`
**Current State**: TargetFramework `net8.0`.

**Target State**: `net10.0`.

**Migration Steps**:
1. Update `<TargetFramework>` to `net10.0`.
2. Update referenced Microsoft.Extensions packages to `10.0.4` where applicable.
3. Restore and build; resolve binary/source incompatibilities reported by analysis.
4. Validate that custom MSBuild targets (`CopyLauncher`, `CleanLauncherDirectory`) still behave as intended under new SDK. If behavior changed, update target declarations accordingly.

**Validation Checklist**:
- [ ] `net10.0` set in project.
- [ ] Solution builds cleanly.
- [ ] Launcher files are copied to output as expected.

### Project: `test\Crossroads.Launcher.Test\Crossroads.Launcher.Test.csproj` & `test\Crossroads.Test\Crossroads.Test.csproj`
**Current State**: TargetFramework `net8.0`. Test packages include `Microsoft.NET.Test.Sdk (18.0.1)`, `xunit (2.9.3)` (flagged as deprecated in assessment), `Moq`.

**Target State**: `net10.0` and compatible test package versions.

**Migration Steps**:
1. Update `<TargetFramework>` to `net10.0`.
2. Update `Microsoft.NET.Test.Sdk` to the latest compatible release for .NET 10 (verify exact version during execution).
3. Replace deprecated `xunit` package versions with supported versions (investigate latest `xunit` stable compatible with .NET 10).
4. Restore, build and run tests; fix API incompatibilities in test code (e.g., mocking / test runner APIs).

**Validation Checklist**:
- [ ] Test project files contain `net10.0`.
- [ ] Tests run and pass in CI.

## Package Update Reference

### Common Package Updates (affecting multiple projects)

| Package | Current | Target | Projects Affected | Reason |
|---|---:|---:|---|---|
| Microsoft.Extensions.Hosting | 9.0.10 | 10.0.4 | `Crossroads`, `Crossroads.Launcher` | Framework compatibility |
| Microsoft.Extensions.Configuration | 9.0.10 | 10.0.4 | `Crossroads`, `Crossroads.Launcher` | Framework compatibility |
| Microsoft.Extensions.DependencyModel | 9.0.10 | 10.0.4 | `Crossroads` | Framework compatibility |

### Project-specific notes
- `Newtonsoft.Json` (13.0.4): assessment marks this as compatible with .NET 10. Migration to `System.Text.Json` is optional and documented as a separate modernization scenario.
- `System.CommandLine.Hosting` is an alpha package in the project; verify compatible stable release or adjust code to match newer package API.
- `xunit` versions flagged as deprecated: plan to update to a supported stable release before or after the atomic upgrade, preferably as part of Phase 2 fixes if tests fail.

## Breaking Changes Catalog

Based on analysis, focus on the following categories when fixing compilation issues after the atomic upgrade. Each error encountered during the build should be mapped to one of these categories and annotated with the specific code location and proposed fix.

1. System.CommandLine API changes
   - Symptom: Types such as `CommandLineBuilder`, `Option`, `Command`, and `ICommandHandler` show source-incompatible changes.
   - Mitigation: Replace deprecated builder patterns with current `System.CommandLine` usage; migrate handler implementations to the new `SetHandler`/`DelegateBinding` patterns or equivalent. Review `ParseResult` extension usage and update method signatures.

2. Microsoft.Extensions.Configuration.Binding changes
   - Symptom: Calls to `ConfigurationBinder.Get<T>` or `Bind` may surface binary/source incompatibilities.
   - Mitigation: Replace problematic binder calls with explicit mapping or validate runtime behavior after upgrade. If needed, use `GetSection(...).Get<T>()` variants and add null checks.

3. MSBuild/SDK behavior
   - Symptom: Custom targets that reference `$(OutDir)` or rely on implicit copy behavior may behave differently under new SDK.
   - Mitigation: Verify `CopyLauncher` and `CleanLauncherDirectory` targets and adjust usage of `$(OutputPath)` / `$(TargetDir)` if necessary.

4. Test framework deprecations
   - Symptom: Deprecated `xunit` package version may not run under newer test SDK or runners.
   - Mitigation: Update `xunit` and `xunit.runner.visualstudio` to supported versions and adjust test attributes/fixtures if required.

5. Other common code fixes
   - Replace usages of APIs removed or made internal in .NET 10 with supported alternatives.

> ⚠️ For each compilation error discovered post-upgrade, record the diagnostic ID, file and line, and a concise code change recommendation here.

## Testing & Validation Strategy

1. Prerequisite validation: ensure .NET 10 SDK is installed and `global.json` (if present) references a compatible SDK.
2. After atomic upgrade, run `dotnet restore` and `dotnet build` for the solution.
3. Run all test projects via `dotnet test` or CI pipeline.
4. Triage and fix test failures; re-run tests.
5. Validate packaging scenarios (since this repo produces a packable tool). Verify `PackAsTool` scenarios still work.

Automated checks to include in CI after upgrade:
- Restore
- Build (no errors)
- Run unit tests

## Risk Management

High-level risks:
- Compilation failures across multiple projects due to API changes (source-incompatible APIs). Mitigation: All-at-once upgrade with dense code-fix pass and focused use of assessment diagnostics.
- Test failures due to behavioral changes: Mitigation: Run full test suite and add targeted integration tests for critical flows.
- Package deprecations (xunit): Mitigation: Upgrade test frameworks and validate test behavior.

Rollback strategy:
- If upgrade causes blocking issues, revert `upgrade-to-NET10` branch and investigate specific API or package problems. Preserve a patch branch with attempted fixes for later reapplication.

## Complexity & Effort Assessment

Per-project complexity (relative):
- `Crossroads.csproj` — Medium (79+ LOC to inspect for API changes)
- `Crossroads.Launcher.csproj` — Medium (44+ LOC)
- Test projects — Low/Medium (25-26+ LOC each)

Overall complexity: Low-to-Medium. All-At-Once is appropriate given repository size and compatibility assessments.

## Source Control Strategy

- Branch: `upgrade-to-NET10` (created). All changes committed to this branch in a single atomic commit if possible.
- Commit message template: "Atomic: Upgrade solution to .NET 10 and update packages (per assessment)".
- Open a PR from `upgrade-to-NET10` into `main` and request review. Include assessment and plan artifacts as PR attachments.

## Success Criteria

The upgrade is considered complete when:
1. All projects target `net10.0` as recorded in project files.
2. All package updates from the Package Update Reference are applied.
3. The solution builds with zero compilation errors.
4. All unit tests pass.
5. No high-severity security vulnerabilities remain in project dependencies.

---

Plan draft prepared from assessment results. Next steps in Execution stage: perform Phase 0 prerequisites and then apply the Atomic Upgrade as a single bounded operation, capture build diagnostics and map them to the Breaking Changes Catalog for focused fixes.
