# Roadmap

This roadmap is milestone-based. Dates are not promised before the Unity baseline, staffing, device matrix, and content rights are known.

## Phase 0 — repository and production foundation

**Status: in progress**

- [x] Establish repository and project charter.
- [ ] Agree product scope and provisional names.
- [ ] Create collaborator access and ownership rules.
- [ ] Record Unity/toolchain decision after a device prototype.
- [ ] Select the first landmark based on research and usable asset rights.
- [ ] Define Persian dictionary source and editorial policy.
- [ ] Create a risk register and initial threat model.
- [ ] Define target Android versions and representative device tiers.

**Exit gate:** architecture and toolchain decisions are recorded; no unresolved critical licensing/security blocker exists for the prototype.

## Phase 1 — technical prototype

- [ ] Persian text normalization and grapheme/input prototype.
- [ ] One complete, deterministic word puzzle.
- [ ] Save/resume and process-kill recovery test.
- [ ] One small heritage location with approved reference pack.
- [ ] Camera/UI prototype with RTL and screen scaling.
- [ ] Profile CPU, GPU, memory, load time, and thermal behavior on low/mid/high test devices.
- [ ] Confirm reproducible Unity Android build and device installation.

**Exit gate:** a repeatable prototype runs on the minimum device tier without critical input, save, or performance defects.

## Phase 2 — vertical slice

- [ ] Tutorial and first progression arc.
- [ ] A small level set with reviewed vocabulary.
- [ ] A multi-stage restoration sequence.
- [ ] Basic story and character presentation.
- [ ] Local save schema and migration tests.
- [ ] Audio, accessibility, settings, and localization review.
- [ ] Privacy-safe analytics event plan.

**Exit gate:** a new player can complete the first session and understand the puzzle-to-restoration loop without developer assistance.

## Phase 3 — closed alpha

- [ ] Expand puzzles and verified heritage content.
- [ ] Add economy balancing and hint costs.
- [ ] Add crash/ANR diagnostics with data minimization.
- [ ] Security review of save, reward, and entitlement flows.
- [ ] Automated regression and device matrix.
- [ ] External playtest and issue triage.

**Exit gate:** critical crash, data-loss, exploit, and progression blockers are resolved; retention and difficulty are measurable.

## Phase 4 — monetization and store readiness

- [ ] Integrate Myket adapter and test purchase lifecycle.
- [ ] Integrate Cafe Bazaar adapter and test purchase lifecycle.
- [ ] Add receipt verification and idempotent entitlement grants.
- [ ] Add optional rewarded ads with failure-safe reward handling.
- [ ] Test restore, refunds, interrupted payment, retries, duplicates, and offline behavior.
- [ ] Prepare store listing, privacy disclosures, support process, and release checklist.

**Exit gate:** store transactions and entitlements pass end-to-end tests; policy and legal reviews are complete.

## Phase 5 — release candidate and live operations

- [ ] Run compatibility, performance, security, and regression matrix.
- [ ] Complete localization, accessibility, content, and asset-rights review.
- [ ] Stage rollout with crash/ANR and economy monitoring.
- [ ] Document rollback and incident response.
- [ ] Establish content update and version compatibility policy.

**Exit gate:** release sign-off is based on measured quality gates, not assumptions of universal compatibility.

## Change control

Any scope expansion must state player value, cost, risks, module owner, test impact, and whether it delays an existing milestone.
