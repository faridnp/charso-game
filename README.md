# Charso — Persian Word Puzzle & Iranian Heritage Restoration

> Working title: **Charso** (چهارسو)  
> Working city: **Mehrgan** (مهرگان)  
> Status: first Unity runtime prototype added; not yet editor-compiled or device-tested.

Charso is a Persian-first Android word puzzle game combining original vocabulary challenges with a respectful restoration-inspired journey through real, documented Iranian heritage.

## First playable prototype

A small Unity 6.3 LTS project is now present on the feature/first-playable-mvp branch. The prototype includes five hand-authored Persian word puzzles, letter selection and answer checking, hints, soft-coin rewards, a three-step conceptual Hafezieh restoration loop, and local JSON progress persistence.

See [First Playable](docs/FIRST_PLAYABLE.md) for scope, limitations, and run/build instructions. This is an early gameplay prototype, not a release candidate.

## Non-negotiable product pillars

1. **Heritage authenticity:** player-buildable landmarks must be real, documented historic buildings, bridges, gardens, or architectural complexes from Iran. No invented fantasy buildings or contemporary apartment/office blocks as buildable landmarks.
2. **Original game identity:** do not copy another game's levels, UI, art, story, text, progression, or assets.
3. **Persian-first quality:** correct Persian typography, RTL layout, normalization, culturally appropriate writing, and accessible mobile UI.
4. **Modular delivery:** independent systems, explicit contracts, small pull requests, automated checks, and clear ownership.
5. **Security and fair monetization:** purchases and premium entitlements require trusted verification; valuable balances must not be trusted to the client; rewarded ads grant rewards only after verified completion.
6. **Measured performance:** define device tiers and measurable frame-time, memory, startup, thermal, and crash targets. Never claim universal compatibility without device testing.

## Initial gameplay direction

Solve a Persian word puzzle → earn soft currency / restoration progress → restore a documented Iranian landmark → unlock the next area, story, or character.

Match-3 and other secondary mechanics remain outside the initial MVP until validated by product tests.

## Heritage rule

The city of Mehrgan is fictional as a game map, but its buildable landmarks must be real Iranian heritage. “Hybrid” means a curated variety of authentic regional and historical Iranian styles—not modern or fantasy buildable architecture.

Hafezieh is the first conceptual restoration candidate in the prototype. No historically accurate model, photograph, scan, or approved asset is included yet. A documented source pack, rights/provenance review, and visual accuracy review remain mandatory before production assets are made.

## Technology

- Unity 6.3 LTS, C#, and uGUI for the initial Android prototype.
- Portrait-first UI; future isometric city presentation remains a separate validated milestone.
- Myket and Cafe Bazaar adapters, backend, dictionary licensing, and production analytics are not implemented.
- The current local prototype economy is not suitable for paid currency or competitive use.

## Repository workflow

- main is the protected, releasable line.
- Work happens in focused feature, docs, or fix branches.
- Changes enter the release line through pull requests after review and required checks.
- Never commit secrets, signing keys, keystores, service-role keys, private purchase tokens, or production credentials.
- Do not commit generated Unity folders such as Library, Temp, Obj, Build, or local IDE state.

## Current verification

The Unity project scaffold and initial gameplay source are committed for review. Unity Editor compilation, Android APK generation, and physical-device QA have **not** yet been run. See [Android Build Pipeline](docs/BUILD_PIPELINE.md).
