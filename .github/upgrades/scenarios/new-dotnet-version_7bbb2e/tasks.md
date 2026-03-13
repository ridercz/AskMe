# AskMe .NET 10.0 Upgrade Tasks

## Overview

This document tracks the execution of the AskMe solution upgrade from .NET 9.0 to .NET 10.0. All three projects will be upgraded simultaneously in a single operation, followed by testing and validation.

**Progress**: 3/4 tasks complete (75%) ![0%](https://progress-bar.xyz/75)

---

## Tasks

### [✓] TASK-001: Verify prerequisites *(Completed: 2026-03-13 22:57)*
**References**: Plan §Phase 0

- [✓] (1) Verify .NET 10.0 SDK installed
- [✓] (2) SDK version 10.0 or higher available (**Verify**)

---

### [✓] TASK-002: Atomic framework and package upgrade *(Completed: 2026-03-13 23:01)*
**References**: Plan §Phase 1, Plan §Package Update Reference, Plan §Breaking Changes Catalog

- [✓] (1) Update TargetFramework to net10.0 in all 3 project files per Plan §Phase 1 (Altairis.AskMe.Data, Altairis.AskMe.Web.Mvc, Altairis.AskMe.Web.RazorPages)
- [✓] (2) All project files updated to net10.0 (**Verify**)
- [✓] (3) Update all package references per Plan §Package Update Reference (6 packages to version 10.0.5, remove Microsoft.VisualStudio.Web.BrowserLink)
- [✓] (4) All package references updated correctly (**Verify**)
- [✓] (5) Restore all dependencies
- [✓] (6) All dependencies restored successfully (**Verify**)
- [✓] (7) Build solution and fix all compilation errors per Plan §Breaking Changes Catalog (focus: Configure extension method binary incompatibility in both web projects' Program.cs)
- [✓] (8) Solution builds with 0 errors (**Verify**)

---

### [✓] TASK-003: Run test suite and validate *(Completed: 2026-03-14 00:02)*
**References**: Plan §Phase 2, Plan §Testing & Validation Strategy

- [✓] (1) Run tests via `dotnet test` to discover and execute all test projects
- [✓] (2) Fix any test failures (reference Plan §Breaking Changes Catalog for common issues)
- [✓] (3) Re-run tests after fixes
- [✓] (4) All tests pass with 0 failures (**Verify**)

---

### [▶] TASK-004: Final commit
**References**: Plan §Source Control Strategy

- [▶] (1) Commit all changes with message: "TASK-004: Complete upgrade to .NET 10.0"

---










