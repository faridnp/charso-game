# Architecture Decision Log

A decision is not final until its status is marked Accepted and evidence is attached.

## ADR-000 — Project direction

- Status: Accepted as product direction
- Decision: build an original Persian word-puzzle and Iranian heritage-restoration mobile game.
- Constraints: Iranian market first; Persian RTL; modular parallel development; security, performance, and commercial readiness from the beginning.

## ADR-001 — Heritage authenticity

- Status: Accepted; non-negotiable
- Decision: every player-buildable landmark must be based on a real, documented Iranian historic place or structure.
- Clarification: hybrid visual direction means a range of authentic regional/historical styles, not modern or fantasy buildable buildings.
- Consequence: every landmark requires provenance and rights review before production.

## ADR-002 — Initial gameplay scope

- Status: Accepted for MVP planning
- Decision: prioritize Persian word puzzles and restoration progression. Defer Match-3 and unrelated secondary systems.
- Consequence: prototype the core loop before social or live-service complexity.

## ADR-003 — Engine and rendering baseline

- Status: Proposed, validation required
- Candidate: Unity 6 LTS, C#, URP; 2D RTL UI with a stylized/isometric 3D heritage presentation.
- Validation: editor/toolchain compatibility, Android build reproducibility, CPU/GPU/memory/thermal profiling, asset workflow, licensing, and minimum-device test.
- Do not lock an exact editor patch or package set until validation is complete.

## ADR-004 — Android storefronts

- Status: Direction accepted; implementation open
- Decision: target Myket and Cafe Bazaar with isolated store-specific adapters behind a provider-neutral billing contract.
- Validation: current store SDKs, purchase lifecycle, restore/refund behavior, server verification options, and policy requirements.

## ADR-005 — Backend and identity

- Status: Open
- Options: no account/backend for the first local prototype; managed backend; custom API.
- Decision criteria: cost, Iranian network reachability, authentication, secure purchase validation, privacy, portability, backup/restore, and operational burden.
- No provider is selected by this record.

## ADR-006 — Persian dictionary and word content

- Status: Open
- Required evidence: source license and commercial-use rights, editorial quality, coverage, inflection policy, normalization, and review workflow.
- No scraped or unlicensed dictionary corpus may be shipped.

## ADR-007 — Monetization and ads

- Status: Product direction accepted; provider details open
- Decision: optional rewarded ads and purchases may support the business model; essential progression must not depend on ad availability.
- Reward issuance must be tied to verified success and be idempotent.

## ADR-008 — Licensing and repository policy

- Status: Open
- Decision required: choose an explicit source-code license or keep the repository proprietary with a clear rights notice. Do not add a permissive license by default.
