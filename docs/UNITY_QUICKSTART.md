# Run Charso in Unity

## Exact Editor version

This repository is pinned to **Unity 6000.3.27f1** in `ProjectSettings/ProjectVersion.txt`. Use that exact editor patch for the first import. In Unity Hub, install the Unity 6.3 LTS editor and select the exact `6000.3.27f1` patch if it is not already installed. For Android APK builds, include:
- Android Build Support
- Android SDK & NDK Tools
- OpenJDK

The project currently uses Unity UGUI and Unity Test Framework packages from `Packages/manifest.json`; Unity downloads them during the first project import.

## Download and open

1. Download the development branch ZIP: https://github.com/faridnp/charso-game/archive/refs/heads/production/release-quality-foundation.zip
2. Extract it to a normal writable folder, for example `D:\\UnityProjects\\Charso`.
3. Open Unity Hub → Projects → Add → Add project from disk.
4. Select the extracted **repository root**. It must be the folder containing all three folders `Assets`, `Packages` and `ProjectSettings`. Do not select `Assets` itself.
5. When Hub asks for an Editor, choose Unity `6000.3.27f1`. Do not upgrade the project to Unity 7 Alpha for this first run.
6. Let Unity finish package resolution and script compilation. Open Window → General → Console and check for red errors before continuing.

## Open and play the included first scene

The repository already contains the first playable scene at `Assets/_Project/Scenes/Charso.unity`; you do not need to generate it manually or use a custom menu item.

1. In the Project panel, expand `Assets` → `_Project` → `Scenes`.
2. Double-click `Charso.unity`. The Hierarchy should show a `CharsoGame` object.
3. Confirm the Console has no red errors.
4. Press the **Play** triangle. The bootstrap creates the Mehrgan city blockout, Persian puzzle interface, coin balance, restoration progress and city shop at runtime.
5. Select the letter tiles in order, press `بررسی پاسخ`, and use `بازار شهر` to open the local restoration shop. Camera exploration supports mouse drag/scroll in the Editor and touch gestures on devices.

The separate Hafezieh research blockout can still be generated from `Charso → Generate Hafezieh Research Blockout`. It is a research scene, not the main Mehrgan gameplay scene.

## Run the automated tests

1. Wait until Unity has completed compilation.
2. Open **Window → General → Test Runner**.
3. Select **EditMode**.
4. Click **Run All** and inspect the results. The test assembly is under `Assets/_Project/Tests/Editor`.

The Python CI checks for the 220-puzzle content and coin economy are separate from Unity's C# EditMode tests; passing one does not imply the other has passed.

## Important current limitations

- The environment and restoration projects are procedural blockout geometry, not final production art.
- Persian text currently uses legacy UGUI Text; correct Persian glyph shaping and mixed-direction RTL layout are not yet integrated.
- Reward currency and city-project ownership are local offline save data. They are not tamper-proof.
- Rewarded ads, verified Myket/Bazaar coin-pack purchases, and a trusted server-side balance are not connected yet.
- Unity compilation, EditMode tests, Android APK creation and device QA have not been verified by the project team yet. Do not treat the project as release-ready until those checks pass.

## Android build entry point

The Editor script `Assets/Editor/BuildCommand.cs` exposes **Charso.Editor.BuildCommand.BuildAndroidApk**. The repository's trusted Windows self-hosted workflow runs EditMode tests before building an ARM64/ARMv7 APK. Do not enable that workflow on untrusted pull requests; the PC runner executes repository code.
