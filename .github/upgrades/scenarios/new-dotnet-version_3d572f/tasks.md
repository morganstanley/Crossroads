# Crossroads .NET 10 Upgrade Tasks

## Overview

This document lists executable tasks to perform an atomic upgrade of the Crossroads solution to .NET 10: prerequisites verification, a single atomic framework & package upgrade, test execution and fixes, and a final commit. Tasks follow the All-At-Once approach: update all projects and packages together, then restore, build, and test.

**Progress**: 0/4 tasks complete (0%) ![0%](https://progress-bar.xyz/0)

---

## Tasks

### [▶] TASK-001: Verify prerequisites
**References**: Plan §Phase 0, Plan §Migration Strategy

- [▶] (1) Verify the required .NET 10 SDK is installed on the execution environment per Plan §Phase 0
- [ ] (2) Runtime/SDK version meets minimum requirements (**Verify**)
- [ ] (3) If a `global.json` file exists, verify it is compatible with the required SDK per Plan §Phase 0
- [ ] (4) `global.json` is compatible or noted for update (**Verify**)

### [ ] TASK-002: Atomic framework and package upgrade (all projects)
**References**: Plan §Detailed Dependency Analysis, Plan §Project-by-Project Plans, Plan §Package Update Reference, Plan §Breaking Changes Catalog

- [ ] (1) Update `<TargetFramework>` to `net10.0` in all projects listed in Plan §Detailed Dependency Analysis (all projects updated in one atomic change)
- [ ] (2) All project files contain `net10.0` (**Verify**)
- [ ] (3) Update package references across all projects per Plan §Package Update Reference (focus: Microsoft.Extensions.* → 10.0.4 and other packages noted in the plan)
- [ ] (4) Restore solution dependencies (e.g., `dotnet restore`) per Plan §Project-by-Project Plans
- [ ] (5) Build the full solution and fix all compilation errors surfaced by the upgrade, using Plan §Breaking Changes Catalog for guidance
- [ ] (6) Solution builds with 0 errors (**Verify**)

### [ ] TASK-003: Run full test suite and validate upgrade
**References**: Plan §Testing & Validation Strategy, Plan §Project-by-Project Plans, Plan §Breaking Changes Catalog

- [ ] (1) Run tests for `test\Crossroads.Test\Crossroads.Test.csproj` and `test\Crossroads.Launcher.Test\Crossroads.Launcher.Test.csproj` (e.g., `dotnet test`) per Plan §Project-by-Project Plans
- [ ] (2) Fix any test failures (reference Plan §Breaking Changes Catalog for common issues)
- [ ] (3) Re-run tests after fixes
- [ ] (4) All tests pass with 0 failures (**Verify**)

### [ ] TASK-004: Final commit
**References**: Plan §Source Control Strategy

- [ ] (1) Commit all remaining changes with message: "Atomic: Upgrade solution to .NET 10 and update packages (per assessment)"
