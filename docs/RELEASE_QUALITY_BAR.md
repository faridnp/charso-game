# Charso — Release Quality Bar

**This document is a release gate, not a wish list.** A feature is not complete because it compiles once or looks acceptable in the Editor. It is complete only when its acceptance criteria, failure cases, accessibility, performance, content rights, and device tests are satisfied.

## Product promise

Charso is a Persian-first word-puzzle adventure in which solving language challenges funds a respectful, visually rewarding journey through documented Iranian heritage. The player should feel they are exploring and restoring a living place—not operating a spreadsheet of buttons.

## Target experience

- Portrait-first Android game with a carefully art-directed, warm Iranian visual identity.
- A beautiful, readable puzzle screen and a distinct, explorable Mehrgan district presentation.
- The puzzle and heritage restoration are one connected loop; neither is a placeholder for the other.
- Short, satisfying play sessions with clear goals, meaningful progression, and no confusing economy.
- Native-feeling Persian text: correct shaping, joining, RTL order, numerals, punctuation, and mixed-direction strings.
- Respectful, sourced cultural storytelling. No invented historical claims.
- Smooth performance on the declared minimum device tier, with graceful visual scaling.

## Definition of done — every player-facing feature

1. Product behavior and edge cases are documented.
2. UX flow and all visible states exist: default, pressed, disabled, loading, success, error, empty, and recovery where applicable.
3. Persian copy is reviewed; RTL and mixed-direction content are tested on device.
4. Data has stable IDs, validation, versioning, and migration rules.
5. Gameplay logic is testable without UI objects.
6. Saves survive app termination at every transaction boundary.
7. No reward can be duplicated by tapping, retrying, restoring, or replaying a callback.
8. Low-memory, offline, process-restart, and interrupted-operation behavior is defined.
9. Analytics are minimal, documented, and privacy reviewed.
10. Accessibility, touch targets, font scaling, and contrast are checked.
11. CPU/GPU/frame-time/memory impact is measured on representative devices.
12. Automated tests and a human QA checklist pass.
13. All art, fonts, audio, text, and code have recorded provenance and distribution rights.
14. Release notes and known limitations are updated.

## Game-wide quality gates

### Gameplay
- No soft-locks in tutorial, puzzle, reward, restoration, or navigation.
- Every level has an explicit answer set and deterministic rules.
- Wrong answers, duplicate answers, hints, retries, app pause/resume, and level transitions behave predictably.
- Progression can be completed without payment or ad availability.
- Difficulty is playtested; it is not inferred from word length alone.

### Persian and accessibility
- A dedicated, licensed Persian font is selected and embedded.
- Arabic shaping, ligatures, ZWNJ, Persian/Arabic Yeh and Kaf, diacritics, numbers, and punctuation are tested.
- Screen-reader labels and logical focus order are provided where supported.
- Touch targets and contrast meet documented accessibility targets.
- No text is baked into artwork when it needs localization or dynamic scaling.

### Art and heritage
- No unlicensed or extracted game assets.
- Each historic landmark has a reviewed source pack, rights record, and approved art brief.
- Stylization preserves the identifying silhouette, proportions, material logic, ornament, and regional character.
- Fictional restoration actions are never presented as factual conservation history.
- The first release contains a small number of finished, optimized locations rather than many low-quality placeholders.

### Reliability and security
- Local save format is versioned, checksummed or otherwise corruption-detectable, recoverable, and migration-tested.
- Client-side currency is not trusted for paid or competitive value.
- Store receipts and entitlements are verified through an approved trusted path.
- Rewarded ads grant rewards only after the SDK reports completed and rewarded; failure never grants.
- Secrets, signing keys, private tokens, and service credentials never ship in the APK or source.
- Network failures do not destroy offline progress.

### Performance
- Profile on physical devices, not Editor FPS alone.
- Define target frame-time, cold start, scene load, memory, APK size, and thermal budgets before content scale-up.
- Use quality tiers, texture compression, atlases, LOD, baked lighting, pooling, and async loading where measured useful.
- No claim of support for every Android phone. Publish a tested compatibility range.

## Release gates

| Gate | Required evidence |
|---|---|
| G0 — Foundation | Unity version, packages, architecture, threat model, device tiers, content rules approved |
| G1 — Core loop | Puzzle, rewards, restoration, save/resume, navigation automated tests pass |
| G2 — Vertical slice | Tutorial through a complete first restoration, production-quality art/audio/UI, user test |
| G3 — Alpha | Content pipeline, device matrix, crash/ANR telemetry, accessibility and performance reports |
| G4 — Monetization | Store purchase, refund, restore, retry, duplicate callback, rewarded-ad failure tests |
| G5 — Release candidate | Security, legal/rights, localization, store policy, regression, performance, rollback sign-off |

A gate cannot be marked complete with a documentation-only CI pass. Attach build IDs, test logs, device models/OS versions, and reviewer sign-off.
