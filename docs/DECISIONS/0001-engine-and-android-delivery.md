# ADR 0001 — Engine and initial Android delivery

- **Status:** Accepted for initial production implementation; subject to device validation.
- **Date:** 2026-10-30

## Context

Charso is a Persian-first Android game with a 2D puzzle interface and a stylized/isometric presentation of real Iranian heritage. It needs a stable mobile toolchain, an offline-capable core, and a repeatable Android build path.

## Decision

- Use Unity 6.3 LTS and C#.
- Start with portrait Android delivery.
- Use a controlled mobile rendering setup; URP is the target after a measured visual/performance proof, not a reason to add expensive effects.
- Keep game rules in a pure C# domain assembly and keep Unity UI/platform integrations at the edges.
- Use a trusted Windows self-hosted runner for the first reproducible Android build while Unity Personal licensing is handled through Unity Hub on the developer machine.
- Do not assume cloud license injection for Unity Personal is supported.

## Consequences

- The exact Editor patch in ProjectSettings must match the runner installation.
- Android Build Support, SDK/NDK, and OpenJDK modules must be installed for the same Editor.
- The developer PC must be powered on and the runner online for self-hosted builds.
- Performance and compatibility must be measured on physical phones and tablets.
- Unity compilation, tests, and APK generation remain unverified until the runner executes them.

## Revisit when

- The installed Editor patch differs from the pinned project version.
- The device prototype shows URP or a chosen graphics feature is too expensive.
- Unity licensing or supported CI options materially change.
- Store SDKs impose a different minimum Android API or ABI requirement.
