# Game Design — Charso

## Product statement

Charso is a Persian-first mobile word puzzle game wrapped in a long-term Iranian heritage restoration journey. Players solve original word puzzles, earn resources, and gradually restore real, documented Iranian landmarks in a fictional connected city map.

The experience should be welcoming and understandable to a broad Iranian audience. Complexity is introduced gradually. The heritage collection, regional context, Persian word content, and restoration stories are central to its identity.

## Core player fantasy

“I solve clever Persian word challenges, learn about Iran's cultural heritage, and bring remarkable historic places back to life.”

## Core loop

1. Enter a word puzzle.
2. Form valid Persian words using the provided letters and level rules.
3. Receive a transparent, predictable reward.
4. Spend earned resources on a clearly explained restoration step.
5. Reveal a new visual stage, a short story beat, or a connected heritage location.
6. Unlock the next puzzle set or district.

Puzzle difficulty, resource costs, and restoration pacing must be tuned together. The economy must not make normal progression feel paywalled.

## MVP scope

### Must have

- Persian letter selection and word submission.
- A curated, licensed, quality-reviewed Persian word list.
- Robust Persian normalization rules, including Arabic/Persian character variants, ZWNJ, diacritics, and input display behavior.
- A reliable level data format and deterministic validation.
- Basic hints with transparent costs and limits.
- A first restoration sequence containing a small number of accurately referenced heritage assets.
- Save/resume for progress, with safe handling of interrupted sessions.
- Persian RTL UI, readable typography, accessibility basics, audio controls, and device scaling.
- Basic privacy-conscious analytics events.
- Tests for puzzle rules, save migration, economy invariants, and content validation.

### Explicitly deferred

- Match-3 or unrelated secondary puzzle mechanics.
- Real-time multiplayer, guilds, chat, and user-generated content.
- Complex social systems.
- Seasonal passes before retention and economy are validated.
- User-to-user trading, transferable currency, or cash-out mechanics.
- Large open-world city simulation.

Deferred means these features require a separate product decision and measurable justification.

## Puzzle design requirements

- Every level has a reproducible answer set and explicit acceptance rules.
- Dictionary inclusion is based on editorial policy, not merely a raw corpus match.
- Proper nouns, offensive terms, obscure inflections, and regional words need explicit policy.
- Diacritics are not required unless a level deliberately teaches them.
- Persian Yeh/Kaf normalization must be consistent. ZWNJ handling must not accidentally join or split words.
- Duplicate submissions must not award duplicate progress.
- A solved level remains solved after restart, migration, or offline/online reconciliation.
- Difficulty progression must not rely only on longer words.
- Puzzle data is versioned so later edits do not corrupt player saves.

## Heritage restoration design

### Strict content rule

Every player-buildable landmark must correspond to a real Iranian historic building, bridge, garden, architectural complex, or other documented heritage structure. There are no invented fantasy monuments and no contemporary apartment or office blocks as buildable landmarks.

The fictional city of Mehrgan is a presentation and progression framework, not a claim that unrelated monuments historically existed together in one real city. District groupings must respect region, period, and historical context.

### Candidate landmark pool — research pending

These are initial candidates, not approved production assets:

- Shiraz: Hafezieh, Saadieh, Arg of Karim Khan.
- Isfahan: Si-o-se-pol, Khaju Bridge, Naqsh-e Jahan ensemble, Ali Qapu, Shah Mosque, Sheikh Lotfollah Mosque.
- Kashan: Fin Garden, Tabatabaei House.
- Yazd: Jameh Mosque and documented historic urban/house architecture.
- Tabriz: Historic Bazaar Complex, Arg-e Alishah.
- Kerman: Ganjali Khan Complex.
- Tehran: Golestan Palace.

Before approval, verify each candidate's name, location, period, boundaries, condition, visual references, and rights for photographs or scans. Remove candidates if evidence or rights are insufficient.

### Restoration should not rewrite history

- Restoration is a respectful game metaphor, not a claim that the player performs real conservation.
- Do not depict sacred, funerary, or culturally sensitive places carelessly.
- Avoid fictional damage narratives presented as historical fact.
- Separate documented facts from fictional character dialogue and game events.
- Add reviewed educational notes where appropriate.
- Stylization may simplify detail, but must preserve identifying silhouette, proportions, materials, ornament logic, and regional character.

### Hybrid visual direction — clarified

“Hybrid” means a variety of authentic Iranian regional and historical styles across districts, eras, and landmark types. It does not authorize modern or fantasy buildable architecture. Temporary scaffolding, paths, benches, lighting, signs, menus, and other non-landmark dressing may be simplified game elements, but must not be presented as historic buildings.

## Narrative and characters

Use an original narrative connecting the fictional city and its restoration journey. Do not invent historical statements and attribute them to real people or monuments. Character dialogue should be concise, culturally natural, and easy to localize. Every factual heritage statement needs a source record and editorial review.

## Economy and monetization principles

- Use a small number of currencies in the MVP.
- Show costs and rewards before a player commits.
- Ensure core progression is achievable without payment.
- Paid currency, entitlements, and server-backed rewards require authoritative validation and idempotent transactions.
- Support Myket and Cafe Bazaar through separate adapters behind a provider-neutral interface.
- Rewarded advertising is optional and clearly described before playback.
- No-fill, offline, SDK, timeout, or playback failures must not grant a reward.
- Essential game progress must not depend on ad availability.
- Never put private keys, signing credentials, or server secrets in the client.

## UX and localization acceptance

Persian is the primary language and RTL is the default layout direction. Define a consistent numeral policy. Test mixed-direction strings containing digits, Latin product IDs, and punctuation. Support common Android font scaling and screen sizes. Touch targets, contrast, and feedback must be suitable for long play sessions.

## Success measures

Review tutorial completion, first-session puzzle completion, D1/D7/D30 retention, puzzle abandonment, hint use, restoration progression, ad opt-in/completion/failure, purchase conversion/refunds, crash-free sessions, ANR rate, frame-time, memory, and thermal behavior. Define thresholds and privacy-safe event schemas before release. Do not collect unnecessary personal data for ordinary gameplay analytics.
