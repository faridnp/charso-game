# Architecture Overview

## Status

The Unity scaffold exists, but the first-playable bootstrap is prototype-only. The production branch introduces a separate pure C# domain assembly (Charso.Core) and EditMode tests. The Editor compile and tests have not yet been executed. Unity 6.3 LTS is the selected editor line; exact package versions and the Android toolchain remain subject to import/build verification.

## Proposed technology shape

- Unity 6 LTS line with C#.
- URP for a controlled mobile rendering pipeline.
- Persian-first 2D UI.
- A stylized/isometric 3D presentation for heritage districts, subject to prototype performance testing.
- Android packaging first; store-specific integrations isolated behind adapters.
- Offline-tolerant core gameplay, with authoritative reconciliation for purchases and valuable shared balances.

## Architectural principles

1. Keep game rules independent from Unity scene objects wherever practical.
2. Keep platform SDKs behind small interfaces.
3. Make persistent data versioned and migratable.
4. Treat remote input and all client state as untrusted.
5. Prefer data-driven content with schema validation.
6. Keep rendering, content, economy, and progression separable.
7. Keep the MVP intentionally small; do not build speculative infrastructure before a measured need exists.
8. Every module has an owner, public contract, tests, and documented dependency direction.

## Proposed module map

| Module | Responsibility | Must not own |
|---|---|---|
| GameCore | lifecycle, configuration, service composition | puzzle rules or provider SDK code |
| PersianText | normalization, grapheme/input utilities, display rules | dictionary editorial decisions |
| WordEngine | deterministic validation and puzzle rules | UI, monetization, network transport |
| DictionaryContent | curated lexicon and metadata | billing or save logic |
| LevelSystem | level loading and progression | direct UI rendering |
| StorySystem | dialogue, characters, narrative unlocks | authoritative economy |
| HeritageContent | landmark metadata, provenance, district grouping | unsourced landmark facts |
| CityPresentation | camera, scene presentation, visual transitions | historical facts |
| RestorationSystem | restoration stages and unlock requirements | receipt verification |
| Economy | reward/cost rules and balance invariants | trusting client-side paid balance |
| Inventory | item/material counts and migration | purchase-provider APIs |
| SaveSystem | schema versioning, local persistence, migration | unvalidated remote state |
| Shop | catalog presentation and purchase intent | provider secrets or receipt authority |
| BillingCore | provider-neutral purchase contract | store-specific UI |
| MyketAdapter / BazaarAdapter | platform-specific purchase calls and callbacks | game economy rules |
| AdsCore | ad placement contract and reward state machine | direct resource mutation |
| BackendGateway | authenticated API transport and response validation | provider credentials in client |
| UI | presentation, navigation, accessibility, localization | authoritative game rules |
| Audio | music, SFX, mixing, accessibility settings | gameplay progression |
| Telemetry | minimal typed events and privacy controls | sensitive personal data |
| QA | test fixtures, content validation, regression suites | shipping runtime code |

These are logical boundaries, not a requirement to create one assembly for every row on day one. Start with a small number of Unity assembly definitions and split only when compile-time isolation or ownership justifies it.

## Dependency direction

- UI calls application-facing interfaces/use cases.
- Application services coordinate domain rules and persistence.
- Domain modules do not depend on Unity UI or platform SDKs.
- Adapters implement interfaces owned by the application/domain boundary.
- External services are accessed through BackendGateway contracts.
- Heritage facts and word content enter through validated data pipelines.

Circular dependencies are not accepted. A module may not reach into another module's private implementation.

## Suggested future Unity folder layout

~~~text
Game/
  Assets/
    _Project/
      Art/
      Audio/
      Content/
        Dictionaries/
        Levels/
        Heritage/
        Localization/
      Prefabs/
      Scenes/
      Scripts/
        GameCore/
        PersianText/
        WordEngine/
        LevelSystem/
        StorySystem/
        Heritage/
        City/
        Economy/
        Inventory/
        Save/
        Billing/
        Ads/
        UI/
        Audio/
        Telemetry/
      Settings/
      Tests/
    Plugins/
    ThirdParty/
  Packages/
  ProjectSettings/
Documentation/
~~~

Do not create a large empty Unity hierarchy until editor version and package baseline are selected. Keep third-party assets in a documented area with license and version records.

## Contracts before parallel implementation

Every module task must specify public interfaces, data ownership, serialization/versioning, error and cancellation behavior, threading expectations, trust level, required tests, performance constraints, and allowed files.

Avoid allowing two contributors to edit the same Unity scene, prefab, ScriptableObject schema, assembly definition, package manifest, or shared interface simultaneously.

## Persistence and offline behavior

Local saves must be atomic or recoverable after process termination. Every save has a schema version and explicit migration path. Keep local progress usable during temporary network failure where rules allow it. Do not overwrite newer server state with stale client state. Resolve conflicts using documented rules; never silently duplicate rewards. Paid entitlements and high-value balances must be reconciled with a trusted authority.

## Content pipeline

Store levels and landmark metadata in version-controlled, schema-validated source files. Add stable IDs; do not use translated display names as database keys. Validate uniqueness, required fields, links, and asset references in CI. Every asset record needs creator/source, license, attribution requirements, and permitted distribution. Do not use scraped assets or assets extracted from competing games.

## Build and release pipeline

CI validates documentation, content schemas, source formatting, and automated tests. Android build jobs are added after a reproducible Unity editor and licensing setup is agreed. Signing is handled through protected CI secrets or a controlled release environment, never committed. Release artifacts must be versioned and traceable to a commit. Keep development, test, and store release configurations separate.

## Architecture Decision Records

Record decisions before locking exact Unity patch and packages, rendering/camera, UI and Persian shaping, dictionary/license, save format, backend/identity, store adapters, ads/reward verification, analytics/retention, and CI build/signing. Each ADR includes context, options, decision, consequences, proof-of-concept results, and date.
