# ParsehNo Unity Integration — Android + Web Desktop

## Decision
One Unity source project, two separately built platform outputs:
1. Android Native APK/AAB.
2. Web Desktop browser package.

Web Mobile is explicitly out of scope. No third mobile-web target is to be added.

## Unity
- Editor: `6000.3.27f1`
- Render pipeline: URP
- Android: IL2CPP, ARM64, landscape.
- Web: WebGL/WebAssembly target for desktop browsers.

## Merge protocol
- Graphics branch: `work/graphics-presentation`
- Domain branch: `work/gameplay-domain`
- Integration branch: `integration/unity-android-web`

No direct merges between workstreams. Integration PRs review contracts, scenes, platform-specific input, build settings, save compatibility and performance checks.

## Build output folders
- `Builds/Android/` — APK/AAB.
- `Builds/WebDesktop/` — desktop browser build.

The build folders are generated output and must stay out of Git. Commit source, assets, configuration and automated tests only.
