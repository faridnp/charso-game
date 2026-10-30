# Android QA and Device Matrix

## Purpose

A build is not considered compatible because it installs on one developer phone. Charso must be tested on a declared set of physical Android devices and OS versions.

## Known reference devices

| Device | Role | Status |
|---|---|---|
| Xiaomi 24117RN76G | Reference phone | Available for future validation; OS/build must be recorded at test time |
| Samsung SM-T225N | Reference tablet | Available for future validation; OS/build must be recorded at test time |

These are known devices, not proof of compatibility. Capture exact Android version, security patch, RAM, GPU, display resolution/density, and available storage with every test run.

## Device tiers

| Tier | Target profile | Required behavior |
|---|---|---|
| Minimum | Android 7.1/API 25 baseline only if store policy and SDK dependencies permit; low-memory ARM device | Reduced quality, stable input/save, no critical crash |
| Mainstream | Current mid-range Android phone, 4–6 GB RAM | Target visual quality and smooth gameplay |
| High | Recent flagship Android phone | Highest supported quality without thermal instability |
| Tablet | 8–11 inch Android tablet | Responsive layout; no stretched phone UI |

The actual minimum OS and device tier must be revisited against Myket/Bazaar requirements, target API policies, analytics, and SDK support before release.

## Required test passes

- Fresh install, first launch, tutorial, level completion, restoration, app restart.
- Background/foreground during every gameplay state.
- Force-stop and process kill during save/reward/transition.
- Low storage, corrupted save, interrupted write, and recovery.
- Offline launch, network loss/recovery, and repeated sync if backend is later added.
- Rotation policy, display cutouts, gesture navigation, system font scaling.
- Small/large screens, different densities, tablet layout.
- Persian text, Arabic variants, ZWNJ, mixed numbers/Latin strings, long clues.
- Touch accuracy, double taps, rapid input, accessibility focus.
- Audio interruption, silent mode, Bluetooth/headphone changes.
- Thermal soak and extended session.
- Install/update/upgrade path and app data preservation.
- Myket/Bazaar purchase, restore, refund, pending, cancel, timeout, duplicate callback.
- Rewarded-ad no-fill, network error, early close, completed but not rewarded, rewarded callback.

## Evidence required per test

Record build artifact URL and commit SHA, device model, OS/build, app version, test case ID, expected result, actual result, pass/fail, logs/screenshots, and tester/date.

No universal Android compatibility claim is allowed. The supported range is the set actually tested and approved.
