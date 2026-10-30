# Charso — Persian Word Puzzle & Iranian Heritage Restoration

> Working title: **Charso** (چهارسو)  
> Working city: **Mehrgan** (مهرگان)  
> Status: pre-production foundation; names are provisional.

Charso is a mobile game concept combining original Persian word puzzles with the restoration and discovery of documented Iranian historic landmarks. It is designed for the Iranian Android market, with Persian-first RTL presentation and a modular codebase suitable for parallel development.

## Non-negotiable product pillars

1. **Heritage authenticity:** player-buildable landmarks must be real, documented historic buildings, bridges, gardens, or architectural complexes from Iran. No invented fantasy buildings or contemporary apartment/office blocks as buildable landmarks.
2. **Original game identity:** take genre-level inspiration only. Do not copy another game's levels, UI, art, story, text, progression, or assets.
3. **Persian-first quality:** correct Persian typography, RTL layout, Persian text normalization, culturally appropriate writing, and accessible mobile UI.
4. **Modular delivery:** independent systems, explicit contracts, small pull requests, automated checks, and clear ownership.
5. **Security and fair monetization:** purchases and premium entitlements must be verified securely; valuable balances must not be trusted to the client; rewarded ads are optional and must grant rewards only after a verified completion callback.
6. **Measured performance:** define device tiers and measurable frame-time, memory, startup, thermal, and crash targets. Never claim universal zero-lag compatibility without device testing.

## Initial gameplay direction

Solve a Persian word puzzle → earn soft currency / restoration materials / progress → restore a documented Iranian landmark → unlock the next area, story, or character.

The first playable milestone prioritizes the word-game loop and a small, high-quality heritage restoration sequence. Match-3 and other secondary mechanics are out of the initial MVP until validated by product tests.

## Heritage rule and visual interpretation

The city of Mehrgan is fictional as a game map, but its buildable landmarks are based on real Iranian heritage. “Hybrid” visual direction means a curated variety of authentic regional and historical Iranian styles—not modern or fantasy buildings. Contemporary-looking infrastructure may appear only where necessary as non-buildable gameplay/UI dressing and must not be presented as a historic landmark.

Every landmark needs a source pack before production: verified location and period, reference photographs/drawings, silhouette and proportions, materials, colors, ornament details, rights/provenance, and a review record. Districts should preserve regional/period context rather than mixing unrelated monuments arbitrarily.

## Technology direction (proposed, not yet locked)

- Unity 6 LTS line, C#, and URP for Android.
- 2D Persian-first UI; evaluate an optimized stylized/isometric presentation for the city.
- Native or provider-specific Android billing adapters for Myket and Cafe Bazaar, isolated behind a platform-neutral purchase interface.
- Backend selection is open. Core puzzle play should remain usable offline where practical; purchases, entitlements, and valuable cross-device balances require trusted verification and reconciliation.

No Unity editor version, SDK, billing SDK, backend, or third-party asset is considered production-approved until it is recorded in an Architecture Decision Record (ADR), tested, and reviewed.

## Repository workflow

- main is the protected, releasable line.
- Work happens in focused feature, docs, or fix branches.
- Changes enter main through pull requests after review and required checks.
- Do not commit secrets, signing keys, keystores, service-role keys, private purchase tokens, or production credentials.
- Do not commit generated Unity folders such as Library, Temp, Obj, Build, or local IDE state.

Start here: [Architecture](docs/ARCHITECTURE.md) · [Game Design](docs/GAME_DESIGN.md) · [Roadmap](docs/ROADMAP.md) · [Heritage Asset Pipeline](docs/HERITAGE_ASSET_PIPELINE.md) · [Module Ownership](docs/MODULE_OWNERSHIP.md) · [Decision Log](docs/DECISIONS.md) · [AI Workflow](AI_WORKFLOW.md) · [Contributing](CONTRIBUTING.md) · [Security](SECURITY.md) · [Android Build Pipeline](docs/BUILD_PIPELINE.md).

## Current phase

**Phase 0 — foundation and technical validation.** This repository currently contains planning and engineering standards only. A runnable Unity game, Android build, store integration, backend, or performance claim does not exist yet.
