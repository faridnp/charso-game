# ParsehNo 2100 — Unity migration workspace

Source baseline: `ParsehNo_2100_v0.9.2_RealChanges.zip` (2026-11-03).
Unity Editor target: `6000.3.27f1`.

This repository is being separated by work ownership. The Unity port uses **one shared Unity source project** and produces two distinct deliverables:
- Native Android: APK for QA and AAB for store submission.
- Web Desktop: browser build for desktop browsers only.

There is **no Web Mobile target** in this plan. Android and Web Desktop have separate input, quality and build profiles, but they share the same assets, scenes, contracts and gameplay source to avoid duplicate forks.

## Work ownership
- Graphics / product owner (user + ChatGPT): `UnityProject/Assets/ParsehNo/Art/` and `UnityProject/Assets/ParsehNo/Presentation/`.
- Gameplay friend: `UnityProject/Assets/ParsehNo/Gameplay/Domain/`.
- Shared boundary: `UnityProject/Assets/ParsehNo/Contracts/`.

The full first Unity workspace is distributed separately as `ParsehNo_2100_Unity_0.1.0_FOUNDATION.zip`. This Git branch initially holds the collaboration boundary and handoff documents; it is not yet the complete binary-asset upload. GLB files and textures must be committed from the downloaded workspace using Git / GitHub Desktop.

## Current state
The current Unity work is a **presentation foundation**, not a full gameplay port. The temporary visual sandbox only tests rendering and placement. It must not invent prices, taxes, income or treasury effects.
