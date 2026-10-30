# First Playable — Charso 0.1.0

## Prototype scope
- Five hand-authored Persian word puzzles.
- Letter selection, answer checking, clear, and a hint that costs 3 soft coins.
- 8 coins awarded for a correct answer.
- A three-step conceptual restoration progression for Hafezieh, Shiraz; each step costs 10 coins.
- Local JSON progress save with schema version 1.
- Editor menu command to generate the scene and a batch build entry point for Android ARMv7 and ARM64.

## Important limitations
- This prototype uses Unity legacy UI Text and a built-in Arial font. Persian glyph shaping, bidirectional order, font fallback, and mixed digit behavior are not production-verified. A proper Persian shaping/RTL text solution and licensed font are required before release.
- Hafezieh is a real Iranian heritage candidate, but no approved historical 3D model, photo, scan, or accurate geometry is included. The current card is a conceptual placeholder, not a reconstruction.
- The tiny vocabulary is prototype content, not a production dictionary.
- There is no account, backend, billing, ad SDK, paid currency, server-authoritative entitlement, or cloud sync.
- Local save files are for prototype testing and are not tamper-resistant.
- No Unity compile, APK build, or physical-device test is claimed until the Editor and runner execute.

## Run in Unity
Open with Unity 6000.3.28f1, wait for import/compilation, choose Charso > Generate First Playable Scene, then press Play.

## Android build
The trusted Windows self-hosted workflow invokes Charso.Editor.BuildCommand.BuildAndroidApk and expects build/Android/Charso.apk.
