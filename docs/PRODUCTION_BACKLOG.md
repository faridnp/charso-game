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

- [ ] Proper Persian text system and approved font/license (candidate evaluation documented in `RTL_TEXT_PIPELINE.md`; current UGUI text remains a blocker).
- [ ] Navigation/state machine: boot, onboarding, home, district, puzzle, reward, restoration, settings, pause.
- [ ] Data-driven puzzle and hint definitions.
- [ ] Deterministic puzzle engine with accepted answers and duplicate protection.
- [ ] Transactional local save with migration and corruption recovery.
- [ ] Economy ledger and idempotent reward transactions.
- [ ] Full first-session tutorial.
- [ ] Responsive portrait UI for phone/tablet.
- [ ] Audio settings, feedback, accessibility, pause/resume.

## P2 — real heritage vertical slice

- [ ] Choose one landmark only after historical and rights review.
- [ ] Create a sourced research dossier and approved art brief.
- [ ] Build a performant, original 3D environment and restoration-stage variants.
- [ ] Add camera movement, focal framing, environmental ambience, and clear progression feedback.
- [ ] Add original characters and reviewed story beats.
- [ ] Integrate the puzzle/reward/restoration loop with the 3D environment.

## P3 — production operations

- [ ] Device matrix, performance budgets, crash/ANR reporting, privacy review.
- [ ] Content validation pipeline and localization review.
- [ ] Secure backup/cloud sync only if product needs it; offline remains first-class.
- [ ] Myket and Bazaar store adapters, receipt validation, refunds and restore tests.
- [ ] Rewarded ads with fail-closed reward handling.
- [ ] Store listing, privacy policy, support workflow, release/rollback checklist.

## Definition of vertical-slice complete

A new player can install the build, launch without developer setup, understand the tutorial, solve the first sequence of puzzles, earn and spend rewards, see a genuine visible change in an authentic heritage environment, pause/exit/restart without data loss, and finish the slice on all declared target devices. All critical paths have automated tests and manual QA evidence.
