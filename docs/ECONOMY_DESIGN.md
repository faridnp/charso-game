# Charso Economy Design — Draft 1

## Player promise

The player earns the ordinary soft currency by solving puzzles. Currency is spent on visible, permanent improvements to Mehrgan. Rewarded ads and paid coin packs are optional accelerators; neither is required to finish the core story or unlock the normal puzzle sequence.

## Currency and rewards

- Currency display name: سکه مهرگان (soft currency; not cash and not transferable).
- First 20 tutorial puzzles: difficulty-scaled first-time rewards (currently 16 easy × 8 coins and 4 medium × 10 coins; 168 coins total before restoration costs).
- Extended puzzles: 8 / 10 / 12 coins for difficulty 1 / 2 / 3. Difficulty must be calibrated through playtests, not inferred from answer length alone.
- No repeat-solve farming: one stable reward transaction per puzzle ID and content revision policy.
- Hints are a spend, not a source of currency. Hint costs and exact hint text must be visible before purchase.
- A solved puzzle must show a clear reward breakdown and update the persistent balance.

## Optional rewarded ads

- Proposed reward: 20 coins per successfully completed rewarded placement.
- Proposed cap: 3 rewarded completions per account per server day.
- The ad button must say exactly what it grants before the player opts in.
- Grant only after the provider's explicit rewarded/completed event; load, show, close, timeout, no-fill or SDK errors grant zero.
- Use a unique provider event/transaction ID and idempotent grant. If the game economy is online, verify signed server-to-server callbacks before treating the grant as trusted.
- If ads are unavailable, the puzzle and city progression remain playable; never fail-open or force an ad.
- Daily caps must be enforced by trusted server time once accounts/backend exist. A client clock is not a security boundary.

## Paid currency packs

- Sell fixed, clearly priced coin bundles through independent Myket and Cafe Bazaar billing adapters.
- The client never grants a paid balance based only on a local purchase-success boolean.
- Verify receipts with the relevant provider/backend, bind each receipt to one account, store provider transaction IDs, make grants idempotent, and support pending purchases, restore, refunds and revocations.
- Keep provider secrets out of the APK and Git repository.
- Purchases accelerate optional city decoration/restoration; they do not sell puzzle answers, block ordinary levels, or create a pay-to-win advantage.

## City project sink (proposed first catalog)

| Project | Target visible change | Draft cost |
|---|---|---:|
| Restore central fountain | Repaired basin, water and lighting state | 130 |
| Renew bazaar canopy | Repaired roof bays and shop fronts | 180 |
| Replant Chahar Bagh | Garden beds, trees and water channels | 160 |
| Repair residential facades | Doors, plaster and window details | 220 |
| Equip restoration workshop | Workbench and material storage | 180 |
| Renew city gate | Gate surface, doors and lanterns | 250 |
| Restore civic courtyard | Paving, arcade and water feature | 300 |
| Complete landmark chapter | Final approved chapter stages | 350 |

The eight draft project costs total 1,770 coins. The 220-puzzle bank awards 1,898 coins at the current authored difficulty mix; after the three 10-coin tutorial restoration payments, 1,868 remain, leaving a 98-coin reserve before hints. The current runtime now includes a local city shop, one-time project ownership, coin deduction, schema-v3 persistence and eight distinct blockout visual groups that activate when a project is purchased. These are prototype balancing costs and blockout visuals, not final prices or final art. Never charge for an already-owned project. Project unlocks and balances remain client-local and are not tamper-proof.

## Economy sanity checks

- The current 200-puzzle draft bank awards 1,730 coins at its authored difficulty mix (143 easy × 8, 49 medium × 10, 8 hard × 12), before hints. The 20 opening puzzles add 168 coins (16 easy and 4 medium), for 1,898 total across all 220.
- At the proposed ad cap, ads add at most 60 coins per server day.
- The coin sources and sinks must be simulated across a full chapter before locking prices. Measure time-to-first-project, time-to-each-project, unused balance, hint usage and ad opt-in rate.
- Do not tune the economy to pressure players into watching ads or paying. Ensure a free-play path exists for every core progression gate.

## Research basis

- Unity LevelPlay documents client reward events and server-to-server callbacks; its S2S callback includes user ID, reward amount and unique event ID. See https://docs.unity.com/grow/levelplay/platform/settings/server-to-server-callback and https://docs.unity.com/en-us/grow/levelplay/sdk/unity/rewarded-ad-integration-package.
- Google documents rewarded-ad server-side verification as additional protection against spoofed client callbacks, including signed callback parameters and unique transaction IDs: https://developers.google.com/admob/unity/ssv.
- Google Play's payments policy explicitly includes virtual currency among digital in-app purchases and requires Play Billing for Play-distributed apps unless an applicable exception/program applies: https://support.google.com/googleplay/android-developer/answer/9858738. Myket/Bazaar integrations require their own current provider terms and verification paths; do not assume Google Play rules or SDKs substitute for them.

- Tapsell's official Unity sample repository is archived and explicitly points developers to the newer `TapsellPlusSDK-UnityPlugin` repository. The archived README contains older API/dependency examples, so do not pin its old SDK coordinates or Gradle setup without checking the current package release and its API against Unity 6.3. See https://github.com/tapsellorg/TapsellPlusSDK-UnitySample and https://github.com/tapsellorg/TapsellPlusSDK-UnityPlugin.
- The archived Tapsell sample shows separate request, open, close, reward and error callbacks. The integration must bind reward to the documented reward event for the exact SDK version; close/open/impression alone must never grant coins. The exact callback contract and any server verification capability remain unverified for the current Unity package.


## Implementation status

Implemented locally: first-time puzzle solve rewards are difficulty-based (8/10/12); hints cost 3; the first three tutorial restoration transitions cost 10 each; the city shop spends the configured project cost once and persists ownership in save schema v3; purchased project groups change the Mehrgan blockout. Save v1 raw files and v2 envelopes migrate to v3. Not implemented: rewarded-ad coin grants, Myket/Bazaar coin packs, server-side balance/receipt verification, cloud sync, final art, Unity Editor execution, or device verification. Do not expose paid or ad-granted currency until provider verification and anti-replay backend are implemented.
