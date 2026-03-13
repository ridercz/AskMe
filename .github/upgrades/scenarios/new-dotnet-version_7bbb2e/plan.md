# .NET 10 Upgrade Plan

## Table of Contents

- [Executive Summary](#executive-summary)
- [Migration Strategy](#migration-strategy)
- [Detailed Dependency Analysis](#detailed-dependency-analysis)
- [Project-by-Project Plans](#project-by-project-plans)
  - [Altairis.AskMe.Data](#altairisaskmedata)
  - [Altairis.AskMe.Web.Mvc](#altairisaskmewebmvc)
  - [Altairis.AskMe.Web.RazorPages](#altairisaskmewebrazorpages)
- [Package Update Reference](#package-update-reference)
- [Breaking Changes Catalog](#breaking-changes-catalog)
- [Risk Management](#risk-management)
- [Testing & Validation Strategy](#testing--validation-strategy)
- [Complexity & Effort Assessment](#complexity--effort-assessment)
- [Source Control Strategy](#source-control-strategy)
- [Success Criteria](#success-criteria)

---

## Executive Summary

### Scenario Description

Upgrade the AskMe solution from **.NET 9.0** to **.NET 10.0 (Long Term Support)**. This solution consists of 3 projects with a simple dependency structure:
- 1 class library (Altairis.AskMe.Data)
- 2 ASP.NET Core web applications (MVC and Razor Pages variants)

### Scope

**Projects Affected:** All 3 projects
- Altairis.AskMe.Data (ClassLibrary, net9.0 → net10.0)
- Altairis.AskMe.Web.Mvc (AspNetCore, net9.0 → net10.0)
- Altairis.AskMe.Web.RazorPages (AspNetCore, net9.0 → net10.0)

**Current State:**
- All projects currently target .NET 9.0
- 9 NuGet packages (6 require updates, 3 compatible)
- 4,099 total lines of code
- 93 code files (9 files with identified incidents)
- Estimated 14+ lines requiring modification (0.3% of codebase)

### Target State

- All projects target .NET 10.0
- All Entity Framework Core packages updated to 10.0.5
- All ASP.NET Core packages updated to 10.0.5
- Microsoft.VisualStudio.Web.BrowserLink removed (functionality now in framework)
- All API compatibility issues resolved

### Selected Strategy

**All-At-Once Strategy** - All projects upgraded simultaneously in single operation.

**Rationale:**
- **Small solution** (3 projects)
- **Simple dependency structure** (single tier: Data library → both web apps)
- **All projects on .NET 9.0** (uniform starting point)
- **Low complexity** (all projects marked Low difficulty)
- **Small codebase** (~4k LOC)
- **Clear package compatibility** (all required packages have net10.0 versions)
- **No security vulnerabilities** requiring urgent prioritization
- **No circular dependencies** or complex relationships

### Complexity Assessment

**Discovered Metrics:**
- Total Projects: 3
- Dependency Depth: 1 (single tier)
- Total LOC: 4,099
- Packages Requiring Update: 6 of 9
- API Compatibility Issues: 14 (2 binary, 8 source, 4 behavioral)
- Security Vulnerabilities: 0
- High-Risk Projects: 0

**Classification: Simple**

All projects can be upgraded atomically with coordinated package updates and targeted API compatibility fixes.

### Critical Issues

**API Compatibility:**
- **2 binary incompatible** APIs requiring code changes
- **8 source incompatible** APIs requiring recompilation/potential fixes
- **4 behavioral changes** requiring testing validation

**Package Deprecation:**
- Microsoft.VisualStudio.Web.BrowserLink (2.2.0) - functionality now included with framework reference

**Key Focus Areas:**
1. Update all project TargetFramework properties to net10.0
2. Update 6 NuGet packages to version 10.0.5
3. Remove deprecated Microsoft.VisualStudio.Web.BrowserLink package
4. Address binary incompatible APIs (OptionsConfigurationServiceCollectionExtensions.Configure)
5. Validate behavioral changes (System.Uri, TimeSpan.FromDays)
6. Verify source incompatible APIs compile successfully

### Recommended Approach

**All-At-Once Atomic Upgrade** with single comprehensive testing phase. Fast completion time with coordinated deployment.

**Expected Remaining Iterations:** 5-6 iterations total

---

## Migration Strategy

### Approach Selection

**Selected: All-At-Once Strategy**

All projects in the solution will be upgraded simultaneously in a single atomic operation.

### Justification

The solution characteristics strongly favor the All-At-Once approach:

**Size & Complexity:**
- ✅ Small solution (3 projects vs. threshold of 5)
- ✅ Low total LOC (4,099 lines)
- ✅ Simple dependency structure (single tier)
- ✅ All projects marked Low difficulty

**Technical Readiness:**
- ✅ All projects currently on .NET 9.0 (uniform starting point)
- ✅ All required packages have net10.0 versions
- ✅ No security vulnerabilities requiring staged mitigation
- ✅ Clear compatibility path for all dependencies

**Risk Profile:**
- ✅ Low API compatibility impact (14 issues across ~16k APIs = 0.09%)
- ✅ Small estimated code changes (14+ LOC = 0.3% of codebase)
- ✅ No high-risk projects identified
- ✅ Both web projects share nearly identical structure and dependencies

**Alternatives Considered:**

**Incremental Migration (Rejected)**
- Unnecessary complexity for 3 projects
- Would require multi-targeting Data library
- Extends timeline without meaningful risk reduction
- No clear phasing benefits given uniform current state

### All-At-Once Strategy Rationale

**Speed:** Fastest path to completion - single coordinated update eliminates intermediate states.

**Simplicity:** No multi-targeting complexity, no dependency version conflicts, no partial solution states.

**Unified Testing:** Single comprehensive test pass validates entire solution at once.

**Deployment Coordination:** Both web applications deploy together with guaranteed compatibility.

### Dependency-Based Ordering

While All-At-Once updates all projects simultaneously, the logical dependency order is:

1. **Altairis.AskMe.Data** (foundation library)
2. **Altairis.AskMe.Web.Mvc & Altairis.AskMe.Web.RazorPages** (consumers)

In practice, all three projects' TargetFramework properties and PackageReference elements are updated together before any build/restore operations, so the dependency order is enforced automatically by MSBuild during restoration and compilation.

### Execution Approach

**Sequential vs Parallel:**

**File Updates:** Parallel - all .csproj files updated simultaneously  
**Package Restoration:** Sequential - MSBuild resolves dependencies in order  
**Compilation:** MSBuild-managed - respects dependency graph automatically  
**Code Fixes:** Sequential by file - address compilation errors as discovered  
**Testing:** Sequential by project type - Data library tests first, then web applications

### Phase Definitions

**Phase 0: Prerequisites**
- Verify .NET 10.0 SDK installed
- Confirm current branch: upgrade-to-NET10

**Phase 1: Atomic Upgrade**
All projects updated in single coordinated operation:
- Update all TargetFramework properties (net9.0 → net10.0)
- Update all PackageReference versions (9.0.5 → 10.0.5)
- Remove deprecated package (Microsoft.VisualStudio.Web.BrowserLink)
- Restore dependencies
- Build solution
- Fix all compilation errors
- Rebuild and verify 0 errors

**Deliverables:** Solution builds successfully with 0 errors

**Phase 2: Validation**
- Run all tests (if test projects exist)
- Manual smoke testing of web applications
- Verify functionality

**Deliverables:** All tests pass, applications function correctly

---

## Detailed Dependency Analysis

### Dependency Graph Summary

The solution has a clean, simple two-tier dependency structure:

**Tier 1 (Leaf - No Dependencies):**
- Altairis.AskMe.Data

**Tier 2 (Applications - Depend on Data):**
- Altairis.AskMe.Web.Mvc → Altairis.AskMe.Data
- Altairis.AskMe.Web.RazorPages → Altairis.AskMe.Data

**Visual Representation:**
```
┌─────────────────────────────────┐
│   Altairis.AskMe.Web.Mvc        │ (Tier 2)
│   Altairis.AskMe.Web.RazorPages │ (Tier 2)
└────────────┬────────────────────┘
             │ depends on
             ▼
    ┌────────────────────┐
    │ Altairis.AskMe.Data│         (Tier 1 - Leaf)
    └────────────────────┘
```

### Project Groupings by Migration Phase

**All-At-Once Strategy** - Single atomic migration phase:

**Phase 1: Atomic Upgrade (All Projects Simultaneously)**
1. Altairis.AskMe.Data
2. Altairis.AskMe.Web.Mvc
3. Altairis.AskMe.Web.RazorPages

All projects are updated together in a coordinated operation. Since all projects currently target net9.0 and have compatible dependencies, there are no intermediate multi-targeting requirements.

### Critical Path Identification

**No critical path blocking issues:**
- ✅ All projects on same framework version (net9.0)
- ✅ All required NuGet packages have net10.0 versions available
- ✅ No circular dependencies
- ✅ Simple linear dependency flow (Data → Web Apps)

**Migration Order Justification:**

While dependencies technically require Data library to be upgraded first, the All-At-Once strategy updates all projects simultaneously because:
1. All projects share the same current framework (net9.0)
2. Package dependency resolution happens atomically during restore
3. Both web applications have identical dependencies and similar code patterns
4. Coordinated update reduces overall migration time and simplifies testing

### Circular Dependency Details

**None found.** The dependency graph is acyclic with clear tier separation.

---

## Project-by-Project Plans

### Altairis.AskMe.Data

#### Current State
- **Target Framework:** net9.0
- **Project Type:** ClassLibrary (SDK-style)
- **Dependencies:** 0 project references
- **Dependants:** 2 (Altairis.AskMe.Web.Mvc, Altairis.AskMe.Web.RazorPages)
- **LOC:** 2,015
- **Files:** 13 (1 file with incidents)
- **Risk Level:** 🟢 Low

**Current NuGet Packages:**
- Microsoft.AspNetCore.Identity.EntityFrameworkCore 9.0.5
- Microsoft.EntityFrameworkCore.Sqlite 9.0.5
- Microsoft.EntityFrameworkCore.SqlServer 9.0.5
- Microsoft.EntityFrameworkCore.Tools 9.0.5
- NLipsum 1.1.0 (compatible, no update needed)

#### Target State
- **Target Framework:** net10.0
- **Updated Packages:** 4 packages updated to 10.0.5

#### Migration Steps

**1. Prerequisites**
- Both web projects (dependants) will be upgraded simultaneously
- No intermediate multi-targeting required

**2. Framework Update**
Update `Altairis.AskMe.Data\Altairis.AskMe.Data.csproj`:
```xml
<TargetFramework>net10.0</TargetFramework>
```

**3. Package Updates**

| Package | Current Version | Target Version | Reason |
|:---|:---:|:---:|:---|
| Microsoft.AspNetCore.Identity.EntityFrameworkCore | 9.0.5 | 10.0.5 | Framework compatibility |
| Microsoft.EntityFrameworkCore.Sqlite | 9.0.5 | 10.0.5 | Framework compatibility |
| Microsoft.EntityFrameworkCore.SqlServer | 9.0.5 | 10.0.5 | Framework compatibility |
| Microsoft.EntityFrameworkCore.Tools | 9.0.5 | 10.0.5 | Framework compatibility |
| NLipsum | 1.1.0 | (no change) | ✅ Already compatible |

**4. Expected Breaking Changes**
✅ **None identified** for this project. No API compatibility issues found.

**5. Code Modifications**
✅ **No code changes required.** Assessment indicates no incidents in code files.

**6. Testing Strategy**
- **Build Validation:** Project builds without errors or warnings
- **Dependency Check:** Verify package restore succeeds
- **Data Access Testing:** 
  - Test database connection (both SQLite and SQL Server)
  - Verify Identity functionality (ApplicationUser, ApplicationRole)
  - Test any data seeding or sample data generation (NLipsum usage)
- **Integration Testing:** Verify both web projects can reference and use updated Data library

**7. Validation Checklist**
- [ ] Project file updated to net10.0
- [ ] All 4 EF Core packages updated to 10.0.5
- [ ] `dotnet restore` succeeds
- [ ] Project builds with 0 errors
- [ ] Project builds with 0 warnings
- [ ] Database migrations still work
- [ ] Identity schema remains compatible
- [ ] Both web projects build successfully against updated library

### Altairis.AskMe.Web.Mvc

#### Current State
- **Target Framework:** net9.0
- **Project Type:** AspNetCore (SDK-style)
- **Dependencies:** 1 (Altairis.AskMe.Data)
- **Dependants:** 0
- **LOC:** 1,043
- **Files:** 51 (4 files with incidents)
- **Risk Level:** 🟢 Low

**Current NuGet Packages:**
- Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore 9.0.5
- Microsoft.EntityFrameworkCore.Design 9.0.5
- Microsoft.SyndicationFeed.ReaderWriter 1.0.2 (compatible, no update needed)
- Microsoft.VisualStudio.Web.BrowserLink 2.2.0 (deprecated, remove)

#### Target State
- **Target Framework:** net10.0
- **Updated Packages:** 2 packages updated to 10.0.5
- **Removed Packages:** 1 package removed (BrowserLink)

#### Migration Steps

**1. Prerequisites**
- Altairis.AskMe.Data upgraded to net10.0 (done simultaneously in All-At-Once approach)

**2. Framework Update**
Update `Altairis.AskMe.Web.Mvc\Altairis.AskMe.Web.Mvc.csproj`:
```xml
<TargetFramework>net10.0</TargetFramework>
```

**3. Package Updates**

| Package | Current Version | Target Version | Reason |
|:---|:---:|:---:|:---|
| Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore | 9.0.5 | 10.0.5 | Framework compatibility |
| Microsoft.EntityFrameworkCore.Design | 9.0.5 | 10.0.5 | Framework compatibility |
| Microsoft.SyndicationFeed.ReaderWriter | 1.0.2 | (no change) | ✅ Already compatible |
| Microsoft.VisualStudio.Web.BrowserLink | 2.2.0 | **REMOVE** | ⚠️ Deprecated - functionality included in framework |

**4. Expected Breaking Changes**

**🔴 Binary Incompatible (Mandatory Fix):**

**Api.0001** - `OptionsConfigurationServiceCollectionExtensions.Configure`
- **File:** `Program.cs`, Line 44
- **Current Code:** `builder.Services.Configure<AppSettings>(builder.Configuration);`
- **Issue:** Method signature changed in .NET 10
- **Fix:** Add explicit type parameter or use `Configure<AppSettings>` overload with binding options
- **Reference:** [Breaking changes in .NET](https://go.microsoft.com/fwlink/?linkid=2262679)

**🟡 Source Incompatible (Potential Fix):**

**Api.0002** - `String.Split(ReadOnlySpan<Char>)`
- **File:** `TagHelpers\PlainTextTagHelper.cs`, Line 24
- **Current Code:** `var paragraphs = this.Text.Split('\r', '\n');`
- **Issue:** Ambiguous method overload with ReadOnlySpan
- **Fix:** Likely compiles without changes; if not, cast parameters or use different overload

**Api.0002** - `TimeSpan.FromDays(Int32)`
- **File:** `Program.cs`, Line 40
- **Current Code:** `options.ExpireTimeSpan = TimeSpan.FromDays(30);`
- **Issue:** Method now accepts double instead of int
- **Fix:** Change to `TimeSpan.FromDays(30.0)` or let compiler handle implicit conversion

**Api.0002** - `IdentityEntityFrameworkBuilderExtensions.AddEntityFrameworkStores`
- **File:** `Program.cs`, Line 26
- **Current Code:** `.AddEntityFrameworkStores<AskDbContext>()`
- **Issue:** Extension method signature or namespace changed
- **Fix:** May require additional using directive or null-check parameters

**🔵 Behavioral Changes (Testing Required):**

**Api.0003** - `System.Uri` constructor
- **File:** `Controllers\SyndicationController.cs`, Line 28
- **Current Code:** `new Uri(homepageUrl)`
- **Issue:** Uri parsing behavior changed in .NET 10
- **Fix:** Validate Uri construction still works correctly; may need Uri.TryCreate for validation
- **Testing:** Verify RSS feed generation with various homepage URLs

**5. Code Modifications**

**Files Requiring Review:**
1. **Program.cs** (3 API issues - lines 26, 40, 44)
   - Fix Configure extension method (binary incompatible)
   - Validate TimeSpan.FromDays compiles
   - Validate AddEntityFrameworkStores compiles

2. **Controllers\SyndicationController.cs** (2 behavioral changes - line 28)
   - Test Uri constructor behavior with actual homepage URLs

3. **TagHelpers\PlainTextTagHelper.cs** (1 source incompatible - line 24)
   - Validate String.Split compiles

**Specific Fix Required for Program.cs Line 44:**
```csharp
// Before (may not compile in .NET 10):
builder.Services.Configure<AppSettings>(builder.Configuration);

// After (explicit binding):
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
// OR use options pattern:
builder.Services.AddOptions<AppSettings>()
    .Bind(builder.Configuration)
    .ValidateDataAnnotations();
```

**6. Testing Strategy**
- **Build Validation:** Project builds without errors or warnings
- **Unit Tests:** Run all unit tests (if present)
- **Integration Tests:**
  - Test MVC routing and controllers
  - Test dependency injection (Identity, DbContext, AppSettings)
  - Test Entity Framework database operations
- **Functional Testing:**
  - Start application and verify homepage loads
  - Test RSS feed generation (SyndicationController)
  - Test plain text tag helper rendering
  - Test authentication/authorization (Identity)
- **Behavioral Validation:**
  - Verify Uri parsing with various URLs (homepageUrl)
  - Verify cookie expiration (30-day TimeSpan)

**7. Validation Checklist**
- [ ] Project file updated to net10.0
- [ ] 2 packages updated to 10.0.5
- [ ] Microsoft.VisualStudio.Web.BrowserLink removed
- [ ] `dotnet restore` succeeds
- [ ] Binary incompatible API fixed (Configure method)
- [ ] Project builds with 0 errors
- [ ] Project builds with 0 warnings
- [ ] All unit tests pass
- [ ] Application starts successfully
- [ ] MVC routes work correctly
- [ ] RSS feed generates correctly
- [ ] Identity authentication works
- [ ] No runtime exceptions

### Altairis.AskMe.Web.RazorPages

#### Current State
- **Target Framework:** net9.0
- **Project Type:** AspNetCore (SDK-style)
- **Dependencies:** 1 (Altairis.AskMe.Data)
- **Dependants:** 0
- **LOC:** 1,041
- **Files:** 51 (4 files with incidents)
- **Risk Level:** 🟢 Low

**Current NuGet Packages:**
- Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore 9.0.5
- Microsoft.EntityFrameworkCore.Design 9.0.5
- Microsoft.SyndicationFeed.ReaderWriter 1.0.2 (compatible, no update needed)
- Microsoft.VisualStudio.Web.BrowserLink 2.2.0 (deprecated, remove)

#### Target State
- **Target Framework:** net10.0
- **Updated Packages:** 2 packages updated to 10.0.5
- **Removed Packages:** 1 package removed (BrowserLink)

#### Migration Steps

**1. Prerequisites**
- Altairis.AskMe.Data upgraded to net10.0 (done simultaneously in All-At-Once approach)

**2. Framework Update**
Update `Altairis.AskMe.Web.RazorPages\Altairis.AskMe.Web.RazorPages.csproj`:
```xml
<TargetFramework>net10.0</TargetFramework>
```

**3. Package Updates**

| Package | Current Version | Target Version | Reason |
|:---|:---:|:---:|:---|
| Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore | 9.0.5 | 10.0.5 | Framework compatibility |
| Microsoft.EntityFrameworkCore.Design | 9.0.5 | 10.0.5 | Framework compatibility |
| Microsoft.SyndicationFeed.ReaderWriter | 1.0.2 | (no change) | ✅ Already compatible |
| Microsoft.VisualStudio.Web.BrowserLink | 2.2.0 | **REMOVE** | ⚠️ Deprecated - functionality included in framework |

**4. Expected Breaking Changes**

**🔴 Binary Incompatible (Mandatory Fix):**

**Api.0001** - `OptionsConfigurationServiceCollectionExtensions.Configure`
- **File:** `Program.cs`, Line 45
- **Current Code:** `builder.Services.Configure<AppSettings>(builder.Configuration);`
- **Issue:** Method signature changed in .NET 10
- **Fix:** Add explicit type parameter or use `Configure<AppSettings>` overload with binding options
- **Reference:** [Breaking changes in .NET](https://go.microsoft.com/fwlink/?linkid=2262679)

**🟡 Source Incompatible (Potential Fix):**

**Api.0002** - `String.Split(ReadOnlySpan<Char>)`
- **File:** `TagHelpers\PlainTextTagHelper.cs`, Line 24
- **Current Code:** `var paragraphs = this.Text.Split('\r', '\n');`
- **Issue:** Ambiguous method overload with ReadOnlySpan
- **Fix:** Likely compiles without changes; if not, cast parameters or use different overload

**Api.0002** - `TimeSpan.FromDays(Int32)`
- **File:** `Program.cs`, Line 41
- **Current Code:** `options.ExpireTimeSpan = TimeSpan.FromDays(30);`
- **Issue:** Method now accepts double instead of int
- **Fix:** Change to `TimeSpan.FromDays(30.0)` or let compiler handle implicit conversion

**Api.0002** - `IdentityEntityFrameworkBuilderExtensions.AddEntityFrameworkStores`
- **File:** `Program.cs`, Line 27
- **Current Code:** `.AddEntityFrameworkStores<AskDbContext>()`
- **Issue:** Extension method signature or namespace changed
- **Fix:** May require additional using directive or null-check parameters

**🔵 Behavioral Changes (Testing Required):**

**Api.0003** - `System.Uri` constructor
- **File:** `Controllers\SyndicationController.cs`, Line 28
- **Current Code:** `new Uri(homepageUrl)`
- **Issue:** Uri parsing behavior changed in .NET 10
- **Fix:** Validate Uri construction still works correctly; may need Uri.TryCreate for validation
- **Testing:** Verify RSS feed generation with various homepage URLs

**5. Code Modifications**

**Files Requiring Review:**
1. **Program.cs** (3 API issues - lines 27, 41, 45)
   - Fix Configure extension method (binary incompatible)
   - Validate TimeSpan.FromDays compiles
   - Validate AddEntityFrameworkStores compiles

2. **Controllers\SyndicationController.cs** (2 behavioral changes - line 28)
   - Test Uri constructor behavior with actual homepage URLs

3. **TagHelpers\PlainTextTagHelper.cs** (1 source incompatible - line 24)
   - Validate String.Split compiles

**Specific Fix Required for Program.cs Line 45:**
```csharp
// Before (may not compile in .NET 10):
builder.Services.Configure<AppSettings>(builder.Configuration);

// After (explicit binding):
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
// OR use options pattern:
builder.Services.AddOptions<AppSettings>()
    .Bind(builder.Configuration)
    .ValidateDataAnnotations();
```

**6. Testing Strategy**
- **Build Validation:** Project builds without errors or warnings
- **Unit Tests:** Run all unit tests (if present)
- **Integration Tests:**
  - Test Razor Pages routing and page models
  - Test dependency injection (Identity, DbContext, AppSettings)
  - Test Entity Framework database operations
- **Functional Testing:**
  - Start application and verify homepage loads
  - Test RSS feed generation (SyndicationController)
  - Test plain text tag helper rendering
  - Test authentication/authorization (Identity)
  - Test all Razor Pages functionality
- **Behavioral Validation:**
  - Verify Uri parsing with various URLs (homepageUrl)
  - Verify cookie expiration (30-day TimeSpan)

**7. Validation Checklist**
- [ ] Project file updated to net10.0
- [ ] 2 packages updated to 10.0.5
- [ ] Microsoft.VisualStudio.Web.BrowserLink removed
- [ ] `dotnet restore` succeeds
- [ ] Binary incompatible API fixed (Configure method)
- [ ] Project builds with 0 errors
- [ ] Project builds with 0 warnings
- [ ] All unit tests pass
- [ ] Application starts successfully
- [ ] Razor Pages render correctly
- [ ] RSS feed generates correctly
- [ ] Identity authentication works
- [ ] No runtime exceptions

---

## Package Update Reference

### Common Package Updates (Affecting Multiple Projects)

| Package | Current | Target | Projects Affected | Update Reason |
|:---|:---:|:---:|:---|:---|
| Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore | 9.0.5 | 10.0.5 | Altairis.AskMe.Web.Mvc<br/>Altairis.AskMe.Web.RazorPages | Framework compatibility |
| Microsoft.EntityFrameworkCore.Design | 9.0.5 | 10.0.5 | Altairis.AskMe.Web.Mvc<br/>Altairis.AskMe.Web.RazorPages | Framework compatibility |

### Data Project Updates

| Package | Current | Target | Projects Affected | Update Reason |
|:---|:---:|:---:|:---|:---|
| Microsoft.AspNetCore.Identity.EntityFrameworkCore | 9.0.5 | 10.0.5 | Altairis.AskMe.Data | Framework compatibility |
| Microsoft.EntityFrameworkCore.Sqlite | 9.0.5 | 10.0.5 | Altairis.AskMe.Data | Framework compatibility |
| Microsoft.EntityFrameworkCore.SqlServer | 9.0.5 | 10.0.5 | Altairis.AskMe.Data | Framework compatibility |
| Microsoft.EntityFrameworkCore.Tools | 9.0.5 | 10.0.5 | Altairis.AskMe.Data | Framework compatibility |

### Packages Requiring Removal

| Package | Current | Projects Affected | Reason for Removal |
|:---|:---:|:---|:---|
| Microsoft.VisualStudio.Web.BrowserLink | 2.2.0 | Altairis.AskMe.Web.Mvc<br/>Altairis.AskMe.Web.RazorPages | ⚠️ **Deprecated** - Functionality now included with framework reference. See [.NET Package Deprecation](https://github.com/dotnet/announcements/issues/217) |

### Compatible Packages (No Update Required)

| Package | Current | Projects Affected | Status |
|:---|:---:|:---|:---|
| Microsoft.SyndicationFeed.ReaderWriter | 1.0.2 | Altairis.AskMe.Web.Mvc<br/>Altairis.AskMe.Web.RazorPages | ✅ Compatible with net10.0 |
| NLipsum | 1.1.0 | Altairis.AskMe.Data | ✅ Compatible with net10.0 |

### Package Update Summary

- **Total Packages:** 9
- **Packages to Update:** 6 (66.7%)
- **Packages to Remove:** 1 (11.1%)
- **Compatible Packages:** 2 (22.2%)
- **Total Package Operations:** 7 (6 updates + 1 removal)

---

## Breaking Changes Catalog

### Summary

| Category | Count | Impact | Fix Complexity |
|:---|:---:|:---|:---|
| 🔴 Binary Incompatible | 2 | **High** - Requires code changes | 🟡 Medium |
| 🟡 Source Incompatible | 8 | **Medium** - Needs re-compilation | 🟢 Low |
| 🔵 Behavioral Changes | 4 | **Low** - Requires testing | 🟡 Medium |
| **Total** | **14** | 14 issues across 15,960 APIs (0.09%) | |

### Binary Incompatible APIs (Mandatory Fixes)

These APIs **will not compile** without code changes.

#### Api.0001: OptionsConfigurationServiceCollectionExtensions.Configure

**Impact:** 🔴 **HIGH** - Both web projects affected

**Affected Files:**
- `Altairis.AskMe.Web.Mvc\Program.cs` (Line 44)
- `Altairis.AskMe.Web.RazorPages\Program.cs` (Line 45)

**Current Code:**
```csharp
builder.Services.Configure<AppSettings>(builder.Configuration);
```

**Issue:**
The `Configure<TOptions>(IServiceCollection, IConfiguration)` extension method signature changed in .NET 10. Direct binding of entire configuration is no longer supported without explicit section specification.

**Required Fix:**
```csharp
// Option 1: Bind specific configuration section
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

// Option 2: Use options builder pattern
builder.Services.AddOptions<AppSettings>()
    .Bind(builder.Configuration)
    .ValidateDataAnnotations();

// Option 3: Bind to root (if AppSettings is at configuration root)
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection(string.Empty));
```

**Recommendation:** Option 1 or Option 2 depending on appsettings.json structure. Review configuration file to determine correct section name.

**Reference:** [Breaking changes in .NET](https://go.microsoft.com/fwlink/?linkid=2262679)

---

### Source Incompatible APIs (Potential Fixes)

These APIs **may not compile** due to ambiguous overloads or signature changes. Often resolved automatically by compiler.

#### Api.0002: String.Split(ReadOnlySpan\<Char\>)

**Impact:** 🟡 **MEDIUM** - Both web projects affected

**Affected Files:**
- `Altairis.AskMe.Web.Mvc\TagHelpers\PlainTextTagHelper.cs` (Line 24)
- `Altairis.AskMe.Web.RazorPages\TagHelpers\PlainTextTagHelper.cs` (Line 24)

**Current Code:**
```csharp
var paragraphs = this.Text.Split('\r', '\n');
```

**Issue:**
New overload `Split(ReadOnlySpan<char>)` may create ambiguity with existing `Split(params char[])`.

**Expected Outcome:**
✅ Likely compiles without changes (compiler resolves to `params char[]` overload).

**If Compilation Fails:**
```csharp
// Explicit array:
var paragraphs = this.Text.Split(new[] { '\r', '\n' });

// Or use StringSplitOptions:
var paragraphs = this.Text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
```

**Reference:** [Breaking changes in .NET](https://go.microsoft.com/fwlink/?linkid=2262679)

---

#### Api.0002: TimeSpan.FromDays(Int32)

**Impact:** 🟡 **MEDIUM** - Both web projects affected

**Affected Files:**
- `Altairis.AskMe.Web.Mvc\Program.cs` (Line 40)
- `Altairis.AskMe.Web.RazorPages\Program.cs` (Line 41)

**Current Code:**
```csharp
options.ExpireTimeSpan = TimeSpan.FromDays(30);
```

**Issue:**
`TimeSpan.FromDays` parameter type changed from `int` to `double` in .NET 10.

**Expected Outcome:**
✅ Likely compiles without changes (implicit int→double conversion).

**If Compilation Fails:**
```csharp
options.ExpireTimeSpan = TimeSpan.FromDays(30.0);
```

**Reference:** [Breaking changes in .NET](https://go.microsoft.com/fwlink/?linkid=2262679)

---

#### Api.0002: IdentityEntityFrameworkBuilderExtensions

**Impact:** 🟡 **MEDIUM** - Both web projects affected

**Affected Files:**
- `Altairis.AskMe.Web.Mvc\Program.cs` (Line 26)
- `Altairis.AskMe.Web.RazorPages\Program.cs` (Line 27)

**Current Code:**
```csharp
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options => { ... })
    .AddEntityFrameworkStores<AskDbContext>()
    .AddDefaultTokenProviders();
```

**Issue:**
`AddEntityFrameworkStores<TContext>` extension method signature or namespace may have changed.

**Expected Outcome:**
✅ Likely compiles without changes if correct namespace imported.

**If Compilation Fails:**
```csharp
// Ensure using directive:
using Microsoft.AspNetCore.Identity;

// Explicit context specification:
.AddEntityFrameworkStores<AskDbContext>()
```

**Reference:** [Breaking changes in .NET](https://go.microsoft.com/fwlink/?linkid=2262679)

---

### Behavioral Changes (Testing Required)

These APIs **compile successfully** but behavior changed. Requires runtime testing and validation.

#### Api.0003: System.Uri Constructor

**Impact:** 🔵 **LOW** - Both web projects affected, testing required

**Affected Files:**
- `Altairis.AskMe.Web.Mvc\Controllers\SyndicationController.cs` (Line 28)
- `Altairis.AskMe.Web.RazorPages\Controllers\SyndicationController.cs` (Line 28)

**Current Code:**
```csharp
await writer.Write(new SyndicationLink(new Uri(homepageUrl)));
```

**Issue:**
Uri parsing and validation behavior changed in .NET 10. More strict validation or different handling of malformed URIs.

**Validation Required:**
- Test with various homepage URL formats
- Test with relative URLs
- Test with URLs containing special characters
- Test with international domain names

**Potential Fix (if issues found):**
```csharp
// Add validation:
if (Uri.TryCreate(homepageUrl, UriKind.Absolute, out var uri))
{
    await writer.Write(new SyndicationLink(uri));
}
else
{
    // Handle invalid URL
    throw new ArgumentException($"Invalid homepage URL: {homepageUrl}");
}
```

**Testing Strategy:**
- Generate RSS feed with various homepage URLs
- Verify feed validates correctly
- Check for exceptions during feed generation

---

### Breaking Changes by Project

**Altairis.AskMe.Data:**
- ✅ **No breaking changes**

**Altairis.AskMe.Web.Mvc:**
- 1 Binary Incompatible (Configure method)
- 4 Source Incompatible (Split, TimeSpan, IdentityEF type/method)
- 2 Behavioral Changes (Uri constructor)

**Altairis.AskMe.Web.RazorPages:**
- 1 Binary Incompatible (Configure method)
- 4 Source Incompatible (Split, TimeSpan, IdentityEF type/method)
- 2 Behavioral Changes (Uri constructor)

### Resolution Priority

**Phase 1 - Mandatory (Before Build):**
1. Fix Binary Incompatible APIs (Configure method) in both web projects

**Phase 2 - Compilation (During Build):**
2. Address Source Incompatible APIs if compilation fails

**Phase 3 - Validation (After Build):**
3. Test Behavioral Changes with runtime testing

---

## Risk Management

### High-Level Risk Assessment

**Overall Risk Level: 🟢 Low**

All three projects assessed as Low difficulty with:
- Small codebase (4,099 total LOC)
- Modern SDK-style projects
- Clear migration path
- No security vulnerabilities
- Minimal API compatibility impact (0.09% of APIs affected)

### Risk Breakdown by Category

| Risk Category | Level | Description | Mitigation |
|:---|:---:|:---|:---|
| **Framework Compatibility** | 🟢 Low | .NET 9→10 is sequential upgrade | Standard migration path, well-documented |
| **Package Compatibility** | 🟢 Low | All packages have net10.0 versions | Direct version updates, no alternatives needed |
| **API Breaking Changes** | 🟡 Medium | 2 binary incompatible APIs | See Breaking Changes Catalog for fixes |
| **Data Migration** | 🟢 Low | Entity Framework 9→10 | EF Core migrations handle schema |
| **Configuration Changes** | 🟢 Low | ASP.NET Core 9→10 | Minimal configuration changes expected |
| **Testing Coverage** | 🟡 Medium | Unknown test coverage | Thorough manual testing required |
| **Deployment Impact** | 🟢 Low | Both web apps updated together | Coordinated deployment, no mixed versions |

### Project-Specific Risks

**Altairis.AskMe.Data:**
- **Risk Level:** 🟢 Low
- **Risk Factors:** 
  - No API compatibility issues identified
  - Entity Framework package updates (9.0.5 → 10.0.5)
  - No code files with incidents
- **Mitigation:** 
  - Update packages first to validate compatibility
  - Verify database migrations still work
  - Test data access layer thoroughly

**Altairis.AskMe.Web.Mvc:**
- **Risk Level:** 🟢 Low
- **Risk Factors:**
  - 1 binary incompatible API
  - 4 source incompatible APIs
  - 2 behavioral changes
  - 4 files with incidents
- **Mitigation:**
  - Address binary incompatible API first (Configure extension method)
  - Validate behavioral changes with runtime testing
  - Review all 4 incident files for required changes
  - Test all MVC routes and functionality

**Altairis.AskMe.Web.RazorPages:**
- **Risk Level:** 🟢 Low
- **Risk Factors:**
  - 1 binary incompatible API
  - 4 source incompatible APIs
  - 2 behavioral changes
  - 4 files with incidents
- **Mitigation:**
  - Address binary incompatible API first (Configure extension method)
  - Validate behavioral changes with runtime testing
  - Review all 4 incident files for required changes
  - Test all Razor Pages and functionality

### Security Vulnerabilities

**None identified.** No packages with security vulnerabilities requiring urgent updates.

### Contingency Plans

**If Build Fails:**
1. Review compilation errors against Breaking Changes Catalog
2. Address binary incompatible APIs first (highest priority)
3. Address source incompatible APIs next
4. Consult .NET 10 breaking changes documentation
5. Consider temporary workarounds if fixes are complex

**If Tests Fail:**
1. Identify failing test categories
2. Review behavioral changes that may affect test assertions
3. Update test expectations if behavioral changes are intentional
4. Fix application code if behavioral changes expose bugs

**If Performance Degrades:**
1. Profile application to identify bottlenecks
2. Review .NET 10 performance changes documentation
3. Consider rollback if degradation is significant
4. Investigate optimization opportunities in new framework

**If Package Compatibility Issues:**
1. All packages have confirmed net10.0 versions available
2. If conflicts arise, use `dotnet list package --vulnerable` to diagnose
3. Check for transitive dependency conflicts
4. Update package versions incrementally if needed

### Rollback Strategy

**Clean Rollback Available:**
- Branch: upgrade-to-NET10 (isolated from master)
- Can discard branch if upgrade fails
- Return to master branch to restore .NET 9.0 state

**Rollback Steps:**
1. Switch to master branch: `git checkout master`
2. Delete upgrade branch (optional): `git branch -D upgrade-to-NET10`
3. All changes isolated, no impact on production code

---

## Testing & Validation Strategy

### Multi-Level Testing Approach

Testing follows the All-At-Once strategy with atomic validation after all projects are upgraded.

### Phase 1: Per-Project Build Validation

After all project files and packages are updated, validate each project builds successfully.

**For Each Project:**

✅ **Build Criteria:**
- Project restores dependencies without errors
- Project compiles without errors
- Project compiles without warnings
- No package dependency conflicts

**Altairis.AskMe.Data:**
- [ ] `dotnet restore` succeeds
- [ ] `dotnet build` succeeds with 0 errors
- [ ] `dotnet build` succeeds with 0 warnings
- [ ] No package version conflicts

**Altairis.AskMe.Web.Mvc:**
- [ ] `dotnet restore` succeeds
- [ ] `dotnet build` succeeds with 0 errors
- [ ] `dotnet build` succeeds with 0 warnings
- [ ] No package version conflicts
- [ ] Binary incompatible API fixed (Configure method)

**Altairis.AskMe.Web.RazorPages:**
- [ ] `dotnet restore` succeeds
- [ ] `dotnet build` succeeds with 0 errors
- [ ] `dotnet build` succeeds with 0 warnings
- [ ] No package version conflicts
- [ ] Binary incompatible API fixed (Configure method)

### Phase 2: Solution-Level Build Validation

After individual projects build, validate entire solution builds together.

**Full Solution Build:**
- [ ] `dotnet build AskMe.slnx` succeeds with 0 errors
- [ ] `dotnet build AskMe.slnx` succeeds with 0 warnings
- [ ] All projects build in correct dependency order
- [ ] No assembly version conflicts
- [ ] No transitive dependency issues

### Phase 3: Automated Testing (If Tests Exist)

**Test Project Discovery:**
- Run `dotnet test` to discover test projects
- If no tests exist, proceed to manual testing

**If Tests Exist:**
- [ ] All unit tests pass
- [ ] All integration tests pass
- [ ] No test warnings or skipped tests
- [ ] Test coverage maintained or improved

**Expected Test Categories:**
- Data access tests (Altairis.AskMe.Data)
- Controller tests (both web projects)
- Page model tests (RazorPages)
- Identity/authentication tests
- Integration tests

### Phase 4: Functional Smoke Testing

Manual validation of core functionality for both web applications.

**Altairis.AskMe.Web.Mvc - Smoke Tests:**
- [ ] Application starts without errors (`dotnet run`)
- [ ] Homepage loads successfully
- [ ] Static resources load (CSS, JS, images)
- [ ] Database connection works
- [ ] User registration works (if applicable)
- [ ] User login/logout works
- [ ] MVC routes respond correctly
- [ ] RSS feed generates (`/Syndication` endpoint)
- [ ] Entity Framework queries execute
- [ ] No console errors or warnings
- [ ] Application stops gracefully

**Altairis.AskMe.Web.RazorPages - Smoke Tests:**
- [ ] Application starts without errors (`dotnet run`)
- [ ] Homepage loads successfully
- [ ] Static resources load (CSS, JS, images)
- [ ] Database connection works
- [ ] User registration works (if applicable)
- [ ] User login/logout works
- [ ] Razor Pages render correctly
- [ ] RSS feed generates (`/Syndication` endpoint)
- [ ] Entity Framework queries execute
- [ ] No console errors or warnings
- [ ] Application stops gracefully

### Phase 5: Behavioral Change Validation

Specific testing for identified behavioral changes.

**System.Uri Constructor Testing:**
- [ ] Test RSS feed with standard URL (e.g., `https://example.com`)
- [ ] Test RSS feed with URL with path (e.g., `https://example.com/blog`)
- [ ] Test RSS feed with URL with query string (e.g., `https://example.com?page=1`)
- [ ] Test RSS feed with international domain names
- [ ] Verify no exceptions during Uri construction
- [ ] Verify RSS feed validates against RSS spec

**TimeSpan.FromDays Testing:**
- [ ] Verify cookie expiration set correctly (30 days)
- [ ] Test user authentication persistence
- [ ] Verify "Remember Me" functionality (if applicable)

**Identity EntityFramework Testing:**
- [ ] Create new user account
- [ ] Login with existing user
- [ ] Verify role assignment (if applicable)
- [ ] Test password validation rules
- [ ] Verify identity schema in database

### Phase 6: Regression Testing

Verify no functionality broken by upgrade.

**Data Layer (Altairis.AskMe.Data):**
- [ ] All database migrations still work
- [ ] Sample data generation works (NLipsum)
- [ ] Identity tables accessible
- [ ] SQLite database operations work
- [ ] SQL Server database operations work

**Web Applications (Both):**
- [ ] All pages/routes accessible
- [ ] Form submissions work
- [ ] Validation works correctly
- [ ] Error handling works
- [ ] Logging works correctly
- [ ] Configuration loaded correctly (AppSettings)

### Testing Completion Criteria

**All testing phases pass:**
1. ✅ All projects build with 0 errors and 0 warnings
2. ✅ Full solution builds successfully
3. ✅ All automated tests pass (if present)
4. ✅ All smoke tests pass for both web applications
5. ✅ Behavioral changes validated
6. ✅ No regression issues identified

**Ready for Deployment:**
- All validation checklists complete
- No known issues or exceptions
- Performance acceptable
- Functionality verified

---

## Complexity & Effort Assessment

### Per-Project Complexity

| Project | Complexity | Dependencies | Packages to Update | API Issues | Risk | Justification |
|:---|:---:|:---:|:---:|:---:|:---:|:---|
| **Altairis.AskMe.Data** | 🟢 Low | 0 | 4 | 0 | 🟢 Low | Foundation library, no API issues, straightforward package updates |
| **Altairis.AskMe.Web.Mvc** | 🟢 Low | 1 | 3 + 1 removal | 7 | 🟢 Low | Standard ASP.NET Core upgrade, known API fixes |
| **Altairis.AskMe.Web.RazorPages** | 🟢 Low | 1 | 3 + 1 removal | 7 | 🟢 Low | Standard ASP.NET Core upgrade, known API fixes |

### Phase Complexity Assessment

**Phase 1: Atomic Upgrade**
- **Complexity:** 🟢 Low
- **Dependency Ordering:** Simple linear (Data → Web Apps)
- **Coordination Required:** All projects updated simultaneously
- **Expected Challenges:** 
  - Binary incompatible API in both web projects (same fix applies to both)
  - Package removal coordination (BrowserLink in both web projects)
  - Behavioral change validation (System.Uri, TimeSpan.FromDays)

**Phase 2: Validation**
- **Complexity:** 🟢 Low
- **Testing Scope:** Full solution
- **Expected Challenges:**
  - Unknown test coverage (may require manual testing)
  - Behavioral changes require runtime validation
  - Both web applications need smoke testing

### Resource Requirements

**Skill Levels:**
- **.NET Developer:** Required (intermediate level)
  - Familiarity with .NET framework upgrades
  - Understanding of ASP.NET Core and Entity Framework Core
  - Ability to diagnose compilation errors

**Parallel Capacity:**
- **Recommended:** Single developer
- **Alternative:** Small team (1-2 developers)
  - Developer 1: Project file updates and package management
  - Developer 2: Code fixes and testing

**Tools Required:**
- .NET 10.0 SDK
- Visual Studio 2022 (17.12+) or Visual Studio Code
- Git for source control

### Effort Distribution

**Estimated Relative Effort by Activity:**

| Activity | Complexity | Notes |
|:---|:---:|:---|
| **Project File Updates** | 🟢 Low | Mechanical changes to TargetFramework and PackageReference |
| **Package Updates** | 🟢 Low | All packages have clear upgrade paths |
| **Package Removal** | 🟢 Low | Remove BrowserLink from both web projects |
| **Dependency Restoration** | 🟢 Low | Automated by dotnet restore |
| **Build & Compilation** | 🟢 Low | Standard build process |
| **Fix Binary Incompatible APIs** | 🟡 Medium | Requires code changes (Configure extension method) |
| **Fix Source Incompatible APIs** | 🟢 Low | Likely resolved by recompilation |
| **Validate Behavioral Changes** | 🟡 Medium | Requires testing and validation |
| **Testing** | 🟡 Medium | Unknown coverage, manual testing needed |

### Constraints

**No Time Estimates:**
This plan uses relative complexity ratings (Low/Medium/High) instead of time estimates. The agent cannot reliably predict how long each activity will take due to:
- Variability in development environments
- Unknown test coverage and testing requirements
- Potential for unforeseen issues during compilation or testing
- Developer experience and familiarity with codebase

**Complexity ratings guide execution priority and resource allocation, not duration.**

---

## Source Control Strategy

### Branching Strategy

**Branch Structure:**
- **Source Branch:** `master` (original .NET 9.0 code)
- **Upgrade Branch:** `upgrade-to-NET10` (all upgrade changes)
- **Strategy:** Feature branch workflow

**Rationale:**
- Isolated upgrade work from main branch
- Easy rollback if issues discovered
- Clean merge when upgrade complete
- No impact on production code during upgrade

### Commit Strategy

**All-At-Once Single Commit Approach (Recommended):**

Given the atomic nature of this upgrade, prefer a single comprehensive commit containing all changes:

**Commit Structure:**
```
Upgrade to .NET 10.0

- Update all projects from net9.0 to net10.0
- Update Entity Framework Core packages to 10.0.5
- Update ASP.NET Core packages to 10.0.5
- Remove deprecated Microsoft.VisualStudio.Web.BrowserLink
- Fix binary incompatible Configure extension method
- Validate behavioral changes (Uri, TimeSpan)

Projects updated:
- Altairis.AskMe.Data
- Altairis.AskMe.Web.Mvc
- Altairis.AskMe.Web.RazorPages

All projects build successfully with 0 errors/warnings.
All tests pass.
```

**Advantages:**
- Atomic change - all or nothing
- Easy to revert if needed
- Clear upgrade boundary in git history
- Simplified review process
- No intermediate broken states

**Alternative: Phased Commits (If Preferred):**

If granular history is preferred:

1. **Commit 1: Project file updates**
   ```
   Update project files to net10.0

   - Update TargetFramework in all .csproj files
   - Update package versions to 10.0.5
   - Remove Microsoft.VisualStudio.Web.BrowserLink
   ```

2. **Commit 2: Breaking change fixes**
   ```
   Fix .NET 10 API breaking changes

   - Fix Configure extension method in Program.cs files
   - Address source incompatibilities
   ```

3. **Commit 3: Validation and cleanup**
   ```
   Validate upgrade and final cleanup

   - Verify all builds pass
   - Confirm all tests pass
   - Update documentation if needed
   ```

### Review and Merge Process

**Pre-Merge Checklist:**
- [ ] All projects build successfully (0 errors, 0 warnings)
- [ ] All automated tests pass
- [ ] Smoke tests completed for both web applications
- [ ] Behavioral changes validated
- [ ] No security vulnerabilities remain
- [ ] Performance acceptable
- [ ] Documentation updated (if needed)

**Pull Request Requirements:**

**Title:** `Upgrade to .NET 10.0 (net10.0)`

**Description Template:**
```markdown
## Upgrade Summary

Upgrades all projects from .NET 9.0 to .NET 10.0 (LTS).

## Changes

### Projects Updated (3)
- Altairis.AskMe.Data: net9.0 → net10.0
- Altairis.AskMe.Web.Mvc: net9.0 → net10.0
- Altairis.AskMe.Web.RazorPages: net9.0 → net10.0

### Package Updates (6)
- Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore: 9.0.5 → 10.0.5
- Microsoft.AspNetCore.Identity.EntityFrameworkCore: 9.0.5 → 10.0.5
- Microsoft.EntityFrameworkCore.Design: 9.0.5 → 10.0.5
- Microsoft.EntityFrameworkCore.Sqlite: 9.0.5 → 10.0.5
- Microsoft.EntityFrameworkCore.SqlServer: 9.0.5 → 10.0.5
- Microsoft.EntityFrameworkCore.Tools: 9.0.5 → 10.0.5

### Package Removals (1)
- Microsoft.VisualStudio.Web.BrowserLink: 2.2.0 (deprecated, functionality in framework)

### Breaking Changes Fixed
- Fixed Configure extension method (binary incompatible)
- Validated Uri constructor behavioral changes
- Validated TimeSpan.FromDays signature change
- Validated Identity EntityFramework extensions

## Testing

- ✅ All projects build with 0 errors
- ✅ All projects build with 0 warnings
- ✅ All tests pass
- ✅ Smoke tests completed
- ✅ Behavioral changes validated

## Migration Notes

- No database migrations required
- No configuration changes required
- Both web applications tested and functional
```

**Review Criteria:**
1. ✅ All .csproj files correctly updated
2. ✅ All package versions correct
3. ✅ Deprecated packages removed
4. ✅ Binary incompatible APIs fixed
5. ✅ All builds pass
6. ✅ All tests pass
7. ✅ No regressions identified

**Merge Strategy:**
- **Recommended:** Squash merge (single commit to master)
- **Alternative:** Merge commit (preserves branch history if phased commits used)

**Post-Merge Actions:**
1. Delete `upgrade-to-NET10` branch (optional)
2. Tag release: `git tag v10.0-upgrade`
3. Update documentation to reflect .NET 10 requirement
4. Update CI/CD pipelines to use .NET 10 SDK

### Rollback Plan

**If Issues Discovered After Merge:**

**Option 1 - Revert Merge:**
```bash
git revert -m 1 <merge-commit-sha>
git push origin master
```

**Option 2 - Reset to Pre-Merge:**
```bash
git reset --hard <pre-merge-commit-sha>
git push --force origin master  # Use with caution
```

**Option 3 - New Branch from Master:**
```bash
# upgrade-to-NET10 branch preserved
git checkout master
# Continue using .NET 9.0 code
```

### Git Commands Summary

**Setup (Already Complete):**
```bash
git checkout master
git checkout -b upgrade-to-NET10
```

**During Upgrade:**
```bash
# Stage changes
git add .

# Commit (single commit approach)
git commit -m "Upgrade to .NET 10.0

- Update all projects from net9.0 to net10.0
- Update EF Core and ASP.NET Core packages to 10.0.5
- Remove deprecated BrowserLink package
- Fix Configure extension method (binary incompatible)

All projects build successfully. All tests pass."
```

**After Successful Upgrade:**
```bash
# Push upgrade branch
git push origin upgrade-to-NET10

# Create pull request (via GitHub/Azure DevOps/etc.)

# After approval, merge to master
git checkout master
git merge --squash upgrade-to-NET10
git commit
git push origin master

# Tag release
git tag -a v10.0-upgrade -m ".NET 10.0 upgrade complete"
git push origin v10.0-upgrade

# Optional: Delete upgrade branch
git branch -d upgrade-to-NET10
git push origin --delete upgrade-to-NET10
```

---

## Success Criteria

### Technical Criteria

**Framework Migration:**
- ✅ All 3 projects target net10.0
- ✅ No projects remain on net9.0
- ✅ All TargetFramework properties correctly updated

**Package Updates:**
- ✅ All 6 required packages updated to version 10.0.5
- ✅ Microsoft.VisualStudio.Web.BrowserLink removed from both web projects
- ✅ No deprecated packages remain (except compatible ones)
- ✅ No security vulnerabilities in dependency graph

**Build Success:**
- ✅ `dotnet restore` succeeds for all projects
- ✅ `dotnet build AskMe.slnx` succeeds with 0 errors
- ✅ `dotnet build AskMe.slnx` succeeds with 0 warnings
- ✅ All projects build in correct dependency order
- ✅ No package version conflicts
- ✅ No assembly binding conflicts

**Code Quality:**
- ✅ All binary incompatible APIs fixed (Configure extension method)
- ✅ All source incompatible APIs compile successfully
- ✅ No compilation errors remain
- ✅ No new compiler warnings introduced
- ✅ Code quality maintained (no shortcuts or hacks)

**Testing:**
- ✅ All automated tests pass (if tests exist)
- ✅ No test regressions
- ✅ All smoke tests pass for both web applications
- ✅ Behavioral changes validated through testing
- ✅ No runtime exceptions in core scenarios

**Functionality:**
- ✅ Both web applications start successfully
- ✅ Database connectivity works (SQLite and SQL Server)
- ✅ Identity/authentication works
- ✅ RSS feed generation works
- ✅ All pages/routes accessible
- ✅ Forms and validation work correctly

### Quality Criteria

**Code Quality Maintained:**
- ✅ No temporary workarounds or commented code
- ✅ All breaking changes fixed properly (not bypassed)
- ✅ Code readability maintained
- ✅ Coding standards followed
- ✅ No introduction of code smells

**Test Coverage Maintained:**
- ✅ Existing tests still pass
- ✅ Test coverage not reduced
- ✅ New tests added for behavioral changes (if needed)
- ✅ All test projects compatible with net10.0

**Documentation Updated:**
- ✅ README.md updated with .NET 10 requirement (if applicable)
- ✅ Development setup docs updated
- ✅ CI/CD pipeline documentation updated (if applicable)
- ✅ Breaking changes documented in commit/PR

**Performance Maintained:**
- ✅ Application startup time acceptable
- ✅ Page load times similar or better
- ✅ Database query performance maintained
- ✅ No memory leaks introduced
- ✅ No obvious performance regressions

### Process Criteria

**All-At-Once Strategy Followed:**
- ✅ All projects updated simultaneously
- ✅ No intermediate multi-targeting states
- ✅ Single atomic upgrade operation
- ✅ Coordinated package updates across all projects
- ✅ Strategy principles applied throughout

**Source Control:**
- ✅ All changes on `upgrade-to-NET10` branch
- ✅ Clean commit history
- ✅ Descriptive commit message(s)
- ✅ Pull request created with complete description
- ✅ All review criteria met

**Risk Mitigation:**
- ✅ All identified risks addressed or mitigated
- ✅ Rollback plan documented and available
- ✅ No critical issues remain unresolved
- ✅ Contingency plans executed if needed

### Deployment Readiness

**Pre-Deployment Validation:**
- ✅ .NET 10.0 SDK available on target environment
- ✅ Deployment scripts updated (if needed)
- ✅ Database migrations tested (if applicable)
- ✅ Configuration files compatible
- ✅ No deployment blockers identified

**Ready for Production:**
- ✅ All success criteria above met
- ✅ Stakeholder sign-off obtained
- ✅ Deployment window scheduled
- ✅ Rollback plan communicated
- ✅ Monitoring and alerting ready

### Definition of Done

**The .NET 10 upgrade is complete when:**

1. ✅ **All projects successfully upgraded** to net10.0
2. ✅ **All required packages updated** to version 10.0.5
3. ✅ **All deprecated packages removed**
4. ✅ **Solution builds** with 0 errors and 0 warnings
5. ✅ **All tests pass** (or comprehensive manual testing completed)
6. ✅ **Both web applications functional** and tested
7. ✅ **Binary incompatible APIs fixed** (Configure extension method)
8. ✅ **Behavioral changes validated** (Uri, TimeSpan)
9. ✅ **No security vulnerabilities** in dependencies
10. ✅ **Code merged to master** via approved pull request
11. ✅ **Documentation updated** as needed
12. ✅ **Release tagged** in git (v10.0-upgrade)

**Final Validation:**
```bash
# Verify all projects target net10.0
dotnet list package --framework net10.0

# Verify no outdated packages
dotnet list package --outdated

# Verify no vulnerabilities
dotnet list package --vulnerable

# Build entire solution
dotnet build AskMe.slnx --configuration Release

# Run all tests
dotnet test

# Confirm 0 errors, 0 warnings, all tests pass
```

**Upgrade considered successful and complete when all criteria above are met.** ✅
