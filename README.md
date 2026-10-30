# Charso — چهارسو

**A Persian-first Android word-puzzle adventure about Iranian heritage.**

Charso is being developed as a real consumer game, not a UI demo. The goal is a polished, culturally respectful experience that combines authored Persian word puzzles, meaningful progression, and visually rewarding restoration of documented Iranian heritage in the fictional city of Mehrgan.

## Production status

The original five-level bootstrap is a throwaway first-playable prototype and is **not release-ready**. The production branch is being rebuilt around testable gameplay rules, safe progression, Persian text correctness, authored content, and measurable device quality.

Current production work includes:
- A separate pure C# domain core for Persian comparison, puzzle validation, and idempotent reward transactions.
- EditMode tests for Persian normalization, answer validation, tile selection, and reward invariants.
- A release quality bar, content bible, QA device matrix, and ordered production backlog.
- A first procedural Mehrgan city blockout with a central square, covered bazaar, Chahar Bagh garden, residential quarters, civic courtyard, restoration workshop, city gate, and landmark restoration variants.
- Original runtime-generated UI icons, card depth, world camera touch/mouse exploration, and stage-to-environment integration.
- Visual direction and Persian RTL text evaluation documents.
- A trusted Windows self-hosted workflow that runs Unity EditMode tests before producing an Android artifact.

None of those items is a claim that Unity compilation, tests, APK generation, or device QA have already passed. The Unity Editor and runner must execute the project to verify them.

## Game identity

- **Title:** چهارسو (Charso)
- **World hub:** مهرگان (Mehrgan), a fictional connected game city.
- **Core loop:** solve a curated Persian word puzzle → earn resources → choose a restoration action → see a real, sourced Iranian heritage place change → unlock the next story beat.
- **Buildable heritage:** only real, documented Iranian historic sites. Hybrid means regional and historical variety, never fantasy or modern buildings presented as historic.
- **MVP focus:** one complete, polished vertical slice before broad content expansion.

## Non-negotiable standards

- Correct Persian shaping, RTL, punctuation, numerals, ZWNJ, and mixed-direction display.
- Original art, writing, puzzle design, and code; no assets extracted from other games.
- Provenance and distribution rights for every shipped font, image, model, texture, and audio file.
- Versioned, recoverable saves and idempotent rewards.
- Paid purchases and premium entitlements verified by a trusted authority; no secrets in the APK.
- Rewarded ads never grant rewards on no-fill, failure, early close, or missing reward callback.
- Measured performance on declared Android device tiers; no claim of universal compatibility.

## Production documents

- [Release quality bar](docs/RELEASE_QUALITY_BAR.md)
- [Content bible](docs/CONTENT_BIBLE.md)
- [Production backlog](docs/PRODUCTION_BACKLOG.md)
- [Android QA device matrix](docs/QA_DEVICE_MATRIX.md)
- [Game design](docs/GAME_DESIGN.md)
- [Architecture](docs/ARCHITECTURE.md)
- [Heritage asset pipeline](docs/HERITAGE_ASSET_PIPELINE.md)
- [Hafezieh research dossier](docs/HERITAGE_RESEARCH/HAFEZIEH.md)
- [Android build pipeline](docs/BUILD_PIPELINE.md)
- [Security threat model](docs/SECURITY_THREAT_MODEL.md)
- [Engine and Android delivery decision](docs/DECISIONS/0001-engine-and-android-delivery.md)

## Development

Unity 6.3 LTS is the selected editor line. The exact patch in ProjectSettings/ProjectVersion.txt is pinned for reproducibility and must match the installed Editor. Android builds use a trusted Windows self-hosted runner with Android Build Support.

## Verification status

**Not yet verified:** Unity Editor import/compile, EditMode test execution, Android APK build, Persian rendering on devices, performance, store SDK integrations, and release readiness. A green documentation workflow is not a Unity build or a release sign-off.