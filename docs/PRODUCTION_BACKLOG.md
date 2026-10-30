# Production Backlog — Charso

This is the ordered implementation plan. Work should proceed through complete vertical slices, not by adding disconnected menus and placeholder features.

## P0 — stop treating the toy prototype as a release build

- [ ] Keep the current first-playable branch as a historical prototype; do not ship it.
- [ ] Establish a production branch with a clean, testable domain/application/presentation split.
- [ ] Pin Unity Editor and verify every package version against that Editor.
- [ ] Add a reproducible Unity Editor compile and Android build job on the trusted Windows runner.
- [ ] Make CI fail on compile errors, missing scenes, missing fonts, invalid content, and broken references.
- [ ] Add automated edit-mode tests for text normalization, puzzle validation, rewards, and save migrations.

## P1 — polished core game loop

- [x] Integrate RTLTMPro v4.0.0 and bundle the Vazirmatn Regular font with its license; Unity rendering and Android-device QA remain open.
- [ ] Navigation/state machine: boot, onboarding, home, district, puzzle, reward, restoration, settings, pause.
- [x] Add 200 original Persian word puzzles beyond the 20-level opening arc (220 total), with topic/difficulty metadata and automated structural/tile-inventory tests. Native editorial approval remains open.
- [ ] Data-driven puzzle and hint definitions.
- [ ] Deterministic puzzle engine with accepted answers and duplicate protection.
- [x] Transactional local save with v1/v2 migration, schema-v3 city ownership and corruption recovery.
- [x] Local economy ledger, one-time puzzle rewards, difficulty scaling and idempotent city-project purchases.
- [ ] Full first-session tutorial.
- [ ] Verify the new responsive portrait RTL HUD on phone and tablet aspect ratios, then tune safe areas and touch targets.
- [ ] Audio settings, feedback, accessibility, pause/resume.

## P2 — real heritage vertical slice

- [ ] Choose one landmark only after historical and rights review.
- [x] Create a sourced Hafezieh research dossier and separate proportional chapter blockout (historical and rights approval still pending).
- [ ] Obtain measured plans, rights-cleared references, and specialist review before final art.
- [ ] Build a performant, original 3D environment and restoration-stage variants.
- [x] Add isometric city camera movement, ambient city animation, live 3D chapter preview and visible restoration/progression feedback; final art and device profiling remain open.
- [ ] Add original characters and reviewed story beats.
- [x] Integrate puzzle rewards and the local city-project shop with persistent visual blockout unlocks.\n- [ ] Replace project blockout props with optimized authored art and profile device performance.

## P3 — production operations

- [ ] Device matrix, performance budgets, crash/ANR reporting, privacy review.
- [ ] Content validation pipeline and localization review.
- [ ] Secure backup/cloud sync only if product needs it; offline remains first-class.
- [ ] Myket and Bazaar consumable coin packs, receipt validation, refunds and restore tests.
- [ ] Rewarded ads with fail-closed reward handling and server-side verification before currency grant.
- [ ] Store listing, privacy policy, support workflow, release/rollback checklist.

## Definition of vertical-slice complete

A new player can install the build, launch without developer setup, understand the tutorial, solve the first sequence of puzzles, earn and spend rewards, see a genuine visible change in an authentic heritage environment, pause/exit/restart without data loss, and finish the slice on all declared target devices. All critical paths have automated tests and manual QA evidence.
