# Persian RTL Text Rendering — Integration & Validation

## Runtime integration

The first playable HUD now uses a real right-to-left text component instead of legacy `UnityEngine.UI.Text`.

- Text engine: TextMeshPro (`com.unity.textmeshpro` pinned to `2.0.1`).
- RTL shaping: RTLTMPro v4.0.0, source vendored under `Assets/_Project/Scripts/RTLTMPro` from upstream commit `69c9193fc27218f17679cc57382a61ac4a717b2b`.
- Runtime UI type: `RTLTMPro.RTLTextMeshPro`.
- Font: bundled `Assets/_Project/Resources/Fonts/Vazirmatn-Regular.ttf`, loaded from Resources and converted into a dynamic TMP SDF font asset at startup. OS-font fallback is only used if the bundled resource fails to load.
- Font license: SIL Open Font License 1.1; full notice is in `THIRD_PARTY_NOTICES.md`.
- RTLTMPro license: MIT; included next to the vendored source in `Assets/_Project/Scripts/RTLTMPro/LICENSE.txt`.

Vendoring RTLTMPro avoids a separate Git URL package clone during the Unity project import. TextMeshPro remains a known package dependency in `Packages/manifest.json`.

## What is implemented in the first playable HUD

- Persian right-to-left labels, puzzle clues, action buttons, city shop, progress and information panels use `RTLTextMeshPro`.
- The RTL component is configured for Farsi shaping, preserving numerals and rich-text tags. It does not force every English-only string through the RTL reordering path.
- The project bundles Vazirmatn rather than depending on arbitrary fonts installed on the developer's computer.
- The home chapter card renders a live camera view of the 3D Mehrgan city, not a static flat skyline placeholder.
- The UI includes a canvas entrance transition, floating ambient details, pressed-button scaling, panel pop/shake feedback and a small success confetti burst.

## Verification status

This is integrated source code, **not yet a verified release sign-off**. Unity Editor compilation, TMP font-atlas generation at runtime, glyph coverage, mixed-direction rendering, portrait phone/tablet layout, and Android device rendering still require actual Unity and device tests. A source-level CI check cannot replace those tests.

## Required visual and text test strings

- `چهارسو؛ بازسازی میراث ایران`
- `مرحله ۱۲ — Mehrgan District 3`
- `قیمت: ۱۲۵٬۰۰۰ تومان (Pro)`
- `می‌خواهم خانهٔ تاریخی را بازسازی کنم`
- `کابل‌کشی، عیب‌یابی و راه‌اندازی`
- `www.example.com — فارسی و English`

## Remaining release gates

1. Run these strings through the actual HUD and input controls in Unity 6000.3.27f1.
2. Verify ZWNJ, punctuation, brackets, Latin words, numbers, multiline wrapping and ellipsis at phone and tablet portrait ratios.
3. Verify dynamic TMP font-atlas population and fallback characters on the lowest supported Android tier.
4. Profile first-launch font creation. If it is too slow, generate a static TMP SDF font asset in the Editor and load that asset instead.
5. Capture screenshots from both declared Android reference devices before calling Persian UI complete.
