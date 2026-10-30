# Module Ownership and Parallel Work

## Ownership rules

- Every active module has one accountable owner for the current task.
- A contributor may implement only the agreed issue scope and allowed paths.
- Shared contracts are owned by the architecture lead until ownership is transferred.
- One person owns a Unity scene or prefab during a task.
- Shared schemas, package manifests, and project settings require explicit coordination.
- Cross-module changes must be split or coordinated through a contract-first issue.

## Suggested work packages

| Work package | Main paths in the future Unity layout | Deliverable |
|---|---|---|
| Persian text | Scripts/PersianText, Tests/PersianText | normalization rules, grapheme tests, RTL input test scene |
| Word engine | Scripts/WordEngine, Tests/WordEngine | deterministic validation API and test fixtures |
| Dictionary/content | Content/Dictionaries, Content/Levels | curated data, source/license manifest, schema validation |
| Level progression | Scripts/LevelSystem, Content/Levels | versioned level model and progression tests |
| Heritage research | Content/Heritage, Documentation/Heritage | sourced landmark records and rights pack |
| City presentation | Scripts/City, Scenes/City | camera/scene prototype and device profiling report |
| Economy | Scripts/Economy, Tests/Economy | reward/cost rules and invariants |
| Save system | Scripts/Save, Tests/Save | versioned atomic save and migration tests |
| Billing adapters | Scripts/Billing, Plugins/Android | provider adapter behind agreed purchase contract |
| Ads | Scripts/Ads, Plugins/Android | ad state machine and idempotent reward contract |
| UI/localization | Scripts/UI, Content/Localization | Persian RTL screens and device-size evidence |
| QA/performance | Tests, Documentation/QA | automated suite, device matrix, performance report |

Paths are proposed; they become active after the Unity project structure is approved.

## Shared contract checklist

Before a work package starts, agree on type names and namespaces, file paths and assembly ownership, public method signatures and error states, serialization schema and stable IDs, threading and cancellation requirements, trust boundary/server authority, tests and mocks, and performance/memory budget.

## Integration protocol

1. Contract owner creates or approves the interface first.
2. Contributors work in separate branches and isolated paths.
3. CI checks each pull request.
4. Integration owner tests the combined result in Unity.
5. Contract changes are versioned and migration impact is reviewed.
6. Merge only after review and evidence.

Do not ask two contributors to change the same Unity scene, prefab, ScriptableObject definition, package manifest, project setting, or public interface in parallel.
