# Charso Android Build Pipeline

## Decision

Preferred path: GitHub-hosted GitHub Actions runner, provided a Unity-supported CI license can be used. Fallback: a Windows self-hosted runner on the developer PC using the Unity Editor activated through Unity Hub.

## Current repository state

The repository is still a pre-production foundation. It does not yet contain a runnable Unity project, including `Assets/`, `Packages/`, and `ProjectSettings/ProjectVersion.txt`. The cloud workflow checks for these files and skips its build job until the Unity project exists. The Windows workflow is manually triggered and validates the project, exact Editor version, Android module, and build entry point before attempting a build. A skipped or blocked build is not a successful APK build.

## Cloud build

The workflow `.github/workflows/android-apk.yml` uses GameCI's Unity Builder for Android and uploads APK output as the `Charso-Android-APK` artifact. It requires a complete Unity project, Android build support, and a CI-usable license payload in the GitHub Actions secret `UNITY_LICENSE`.

The repository is currently public. GitHub's standard hosted runners for public repositories are free. If the repository is made private, hosted runner use draws from the account's included compute allowance and may be billed after that allowance.

## Unity Personal licensing limitation

Unity's current official documentation says Unity Hub is the only supported activation and return method for Unity Personal; manual license activation is not supported for Personal plans. Therefore, do not assume a Personal seat can be activated on a fresh, ephemeral GitHub-hosted runner. Do not use browser/DOM workarounds or put passwords/license material in the repository. A cloud build should be enabled only if the account has a Unity-supported CI licensing path for the selected Editor version.

References:
- Unity license activation methods: https://docs.unity.com/en-us/engine/6000.3/manual/get-started/install-and-upgrade/licenses-and-activation/license-activation-methods
- GameCI activation notes: https://game.ci/docs/3/github/activation/

## Windows self-hosted fallback

The workflow `.github/workflows/android-apk-self-hosted.yml` is restricted to manual dispatch and requires a runner with labels `self-hosted`, `Windows`, `X64`, and `charso-unity-android`. It invokes the exact Unity Editor version declared in `ProjectSettings/ProjectVersion.txt` in batch mode. The project build entry point `Assets/Editor/BuildCommand.cs` will be added with the Unity runtime foundation.

Setup, once the Unity project version is fixed:
1. Install that exact Unity Editor version through Unity Hub.
2. Include Android Build Support, Android SDK & NDK Tools, and OpenJDK.
3. Activate Unity Personal through Unity Hub on the Windows account that will run the runner.
4. In GitHub, open **Settings → Actions → Runners → New self-hosted runner**, choose Windows x64, and configure it on this PC.
5. Add the custom runner label `charso-unity-android`.
6. Run the runner under the same Windows account that owns the Unity Hub activation. Keep it online only when you want to build.

Self-hosted jobs execute repository code on the PC. Do not enable pull-request triggers for this runner, do not run untrusted branches, and do not expose it to public contributors. Only manually dispatch from a trusted branch after reviewing the code.

GitHub currently lists self-hosted runner compute as free independently of included hosted compute usage. The developer PC supplies the hardware, power, and internet connection.

## Artifact delivery and signing

After a successful run, download the `Charso-Android-APK` artifact from the GitHub Actions run. It is a ZIP containing the APK. This initial pipeline is for test APKs; production signing, secure keystore storage, Myket/Cafe Bazaar release, and store submission will be separate protected steps. Never commit a keystore or signing password.

## Verification status

- Cloud workflow scaffold committed; no cloud Unity license configured or validated.
- Windows self-hosted workflow scaffold committed; it cannot run until the Unity project, matching Editor, runner, and build entry point exist.
- No Unity compilation or APK build has been run.
- Android device installation and gameplay QA remain pending.
