# Persian RTL Text Rendering Decision & Test Plan

## Why this is a release blocker

Right alignment alone is not Persian text support. The game must render contextual Arabic-script glyph shaping, bidirectional mixed Persian/Latin strings, punctuation, numerals, multiline labels, and ZWNJ correctly. The current legacy UGUI `Text` bootstrap does not meet this bar.

## Research candidates

### RTLTMPro 4.0.0
- Upstream: https://github.com/pnarimani/RTLTMPro
- OpenUPM distribution: https://openupm.com/packages/com.nosuchstudio.rtltmpro/
- Upstream v4.0.0 release identifies Unity 6+ compatibility.
- Package supports Farsi, Arabic, Hebrew, TMP rich text, multiline, number modes, and ZWNJ according to its README.
- Runtime assembly name: `RTLTMPro`; UI component: `RTLTMPro.RTLTextMeshPro`.
- License reported as MIT by the package metadata.
- Risk: the upstream README says it is looking for maintainers; OpenUPM reports low download volume. Treat it as a candidate, not as a verified dependency.

### Font
- Vazirmatn upstream: https://github.com/rastikerdar/vazirmatn
- License: SIL Open Font License 1.1 (OFL), suitable for embedding subject to its terms.
- Prefer the UI variant where available; build a TMP SDF font asset with a reviewed Persian, Arabic, Latin, punctuation, and numeral glyph set.
- Include the OFL license and required notices in the shipped project.

## Integration policy

Do not switch production UI to a new text component until:
1. Unity 6.3.28f1 resolves the exact package graph without warnings/errors.
2. A Vazirmatn TMP font asset is generated and committed with its license.
3. The font atlas contains all characters used by the game and fallback behavior is intentional.
4. Mixed Persian/English labels, numbers, URLs, parentheses, punctuation, and ZWNJ render correctly.
5. Multiline wrapping, truncation, autosizing, buttons, puzzle tiles, and touch input are verified.
6. Android builds render identically on the minimum supported device tier and tablet tier.
7. Package source, exact version, license, and update policy are pinned.

## Required test strings

- `چهارسو؛ بازسازی میراث ایران`
- `مرحله ۱۲ — Mehrgan District 3`
- `قیمت: ۱۲۵٬۰۰۰ تومان (Pro)`
- `می‌خواهم خانهٔ تاریخی را بازسازی کنم`
- `کابل‌کشی، عیب‌یابی و راه‌اندازی`
- `www.example.com — فارسی و English`

## Current status

The current runtime HUD is still legacy UGUI Text and is not approved for release. The candidate research is recorded; package and font integration remain an explicit production task. Do not hide this gap or call the current UI RTL-complete.
