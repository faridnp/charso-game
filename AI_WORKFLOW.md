# AI Contributor Workflow

This file is the operating contract for contributors using ChatGPT, Claude, local coding agents, or other AI tools.

## Mission

Deliver small, reviewable, testable changes without damaging existing behavior or violating product rules. The human project owner defines priorities; AI contributors implement only the agreed scope and report evidence.

## Read before coding

1. README.md
2. docs/GAME_DESIGN.md
3. docs/ARCHITECTURE.md
4. docs/ROADMAP.md
5. CONTRIBUTING.md
6. SECURITY.md
7. The issue and relevant module contract.

## Non-negotiable instructions

- Do not delete, replace, rename, or redesign existing functionality unless the issue explicitly authorizes it.
- Do not broaden the task silently. Propose out-of-scope improvements separately.
- Do not invent APIs, package versions, SDK behavior, device compatibility, or test results.
- Do not claim code compiles or tests pass unless commands were run and output inspected.
- Never put secrets, signing keys, private purchase tokens, passwords, personal data, or production credentials in source, logs, prompts, issues, or commits.
- Do not copy source code, levels, visual assets, text, or progression from another commercial game.
- Buildable city landmarks must be real Iranian heritage assets with documented provenance; no fantasy/modern buildable buildings.
- Do not add a dependency without recording its need, version, license, maintenance status, and security implications.
- Do not modify the protected branch directly. Work on a focused branch and open a pull request.
- Avoid concurrent edits to shared Unity scenes, prefabs, ScriptableObject schemas, asmdefs, package manifests, and shared interfaces.
- Preserve project conventions and use the smallest safe change.

## Required task packet

Before implementation, write down goal/player behavior, scope and non-goals, allowed files/modules, interfaces and data ownership, security/privacy implications, performance/memory implications, acceptance criteria, tests, risks, and rollback strategy.

If a required contract is missing, do not invent an incompatible API. Propose the contract in the PR and mark implementation blocked where appropriate.

## Branch and pull request rules

Use feature/<short-name>, fix/<short-name>, docs/<short-name>, or test/<short-name>. Keep one coherent purpose per PR. Include summary, rationale, changed files, tests actually run, manual test steps, risks, and known limitations. Link the issue. Do not merge your own PR unless the project owner explicitly asks and checks/review are satisfied.

## Unity collaboration rules

- One named owner per scene or prefab during a task.
- Prefer prefab variants and additive scenes over simultaneous edits to the same scene.
- Do not commit Library, Temp, Obj, Logs, UserSettings, local builds, or IDE caches.
- Do not modify ProjectSettings or Packages/manifest.json without explicit scope and a documented reason.
- Keep ScriptableObject schemas backward-compatible or provide migrations.
- Never check in device-specific paths or local absolute paths.

## Test and evidence report

For every PR, report exact commands, Unity/editor/compiler version, test counts, device model/Android version/conditions, what was not tested, warnings, known defects, and unverified assumptions.

A static inspection is not a compile test. An editor compile is not an Android build test. An Android build is not store certification.

## Prompt template for an AI task

~~~text
You are contributing to Charso. First read README.md, AI_WORKFLOW.md, CONTRIBUTING.md,
SECURITY.md, docs/GAME_DESIGN.md, docs/ARCHITECTURE.md, and the linked issue.

Implement only the accepted scope. Preserve all existing behavior. Do not alter unrelated
files or introduce dependencies without approval. Respect Persian RTL and the strict
Iranian heritage asset rule. Treat client-side state and external input as untrusted.

Before coding, summarize the task, allowed files, interfaces, risks, and acceptance tests.
Create a focused branch. Implement the smallest maintainable change. Add or update tests.
Run available checks and report exact commands and results. Do not claim tests passed
unless they were executed. Open a pull request with changed files, test evidence,
limitations, and rollback notes. Do not merge.
~~~
