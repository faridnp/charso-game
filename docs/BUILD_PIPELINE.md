# Charso Android Build Pipeline

## Decision

The preferred delivery path is a GitHub-hosted GitHub Actions runner that builds an Android APK and publishes it as a workflow artifact. A Windows self-hosted runner on the developer PC is the fallback if Unity Personal licensing or hosted-runner constraints prevent reliable cloud builds.

## Current repository state

The repository is still a pre-production foundation. It does not yet contain a runnable Unity project, including `Assets/`, `Packages/`, and `ProjectSettings/ProjectVersion.txt`. Therefore, the APK workflow intentionally checks for these files and skips the build until the Unity project is committed. A skipped build is not a successful APK build.

## Cloud build prerequisites

1. A complete Unity project committed to this repository.
2. A Unity Editor version pinned in `ProjectSettings/ProjectVersion.txt`.
3. Android Build Support configured for that Editor version.
4. A Unity license method that Unity currently permits for the selected CI environment.
5. A GitHub Actions secret named `UNITY_LICENSE`, only if the chosen license workflow legitimately provides a CI-usable license payload.

The workflow uses GameCI's Unity Builder action for the Android target and uploads generated APK files as the `Charso-Android-APK` artifact. It does not upload signing keys or publish to an app store.

## Unity Personal licensing caveat

Do not assume that a Unity Personal seat can be activated on an arbitrary ephemeral GitHub-hosted runner. Unity's current documentation identifies Unity Hub as the supported activation route for Personal, while GameCI documents that the older manual Personal-license activation flow is no longer supported in some current configurations. Before relying on cloud builds, validate a compliant licensing path for the exact Unity Editor release and account. Never put Unity passwords, license files, keystores, or signing credentials in the repository.

If a compliant cloud license cannot be configured, use the Windows self-hosted runner fallback. The developer PC must have the matching Unity Editor with Android Build Support installed and activated in Unity Hub. The runner must execute under the same Windows account/context that can access that activated Unity installation. The self-hosted workflow should invoke Unity in batch mode and must be restricted to trusted branches because repository workflows execute code on the host.

## Cost and privacy

- Standard GitHub-hosted runners are free for public repositories; private repositories use the account's included Actions quota and may incur charges after the allowance.
- Self-hosted runner compute is hosted on the developer's own PC. GitHub's Actions platform billing policy may still apply depending on the plan and current pricing; verify current billing before enabling it.
- Do not expose a self-hosted runner to untrusted pull requests. Run it only for trusted pushes or manual dispatch from a protected branch.
- Keep the repository private if source confidentiality is required. Public-repository free runner availability is not a reason to expose proprietary game code.

## Artifact delivery

After a successful workflow run, open the run under **Actions** and download the `Charso-Android-APK` artifact. The artifact is a ZIP containing the APK output. An artifact is not a Play Store/Myket release and is not a signed production release unless a separate secure signing and release pipeline is added.

## Current verification status

- Workflow and documentation are prepared as an initial pipeline scaffold.
- No Unity project exists yet, so no Unity compilation or APK build has been run.
- Cloud license activation has not been configured or validated.
- Android device installation and gameplay QA remain pending.
