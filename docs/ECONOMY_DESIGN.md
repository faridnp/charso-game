# Charso Economy Design — Draft 1

## Player promise

The player earns the ordinary soft currency by solving puzzles. Currency is spent on visible, permanent improvements to Mehrgan. Rewarded ads and paid coin packs are optional accelerators; neither is required to finish the core story or unlock the normal puzzle sequence.

## Currency and rewards

- Currency display name: سکه مهرگان (soft currency; not cash and not transferable).
- First 20 tutorial puzzles: 8 coins for each first-time correct solve.
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

The eight draft project costs total 1,770 coins, leaving a small reserve from the 1,890-coin full-clear path. Costs are balancing hypotheses, not final prices. Every project must have a real, persistent visual state change before it can be sold. Never charge for a capped or already-owned visual stage. The current blockout only has three generic landmark restoration states; the catalog above is not yet integrated into the runtime city and must not be presented as implemented.

## Economy sanity checks

- The current 200-puzzle draft bank awards 1,730 coins at its authored difficulty mix (143 easy × 8, 49 medium × 10, 8 hard × 12), before hints. The 20 opening puzzles add 160 coins, for 1,890 total across all 220.
- At the proposed ad cap, ads add at most 60 coins per server day.
- The coin sources and sinks must be simulated across a full chapter before locking prices. Measure time-to-first-project, time-to-each-project, unused balance, hint usage and ad opt-in rate.
- Do not tune the economy to pressure players into watching ads or paying. Ensure a free-play path exists for every core progression gate.

## Research basis

- Unity LevelPlay documents client reward events and server-to-server callbacks; its S2S callback includes user ID, reward amount and unique event ID. See https://docs.unity.com/grow/levelplay/platform/settings/server-to-server-callback and https://docs.unity.com/en-us/grow/levelplay/sdk/unity/rewarded-ad-integration-package.
- Google documents rewarded-ad server-side verification as additional protection against spoofed client callbacks, including signed callback parameters and unique transaction IDs: https://developers.google.com/admob/unity/ssv.
- Google Play's payments policy explicitly includes virtual currency among digital in-app purchases and requires Play Billing for Play-distributed apps unless an applicable exception/program applies: https://support.google.com/googleplay/android-developer/answer/9858738. Myket/Bazaar integrations require their own current provider terms and verification paths; do not assume Google Play rules or SDKs substitute for them.

## Implementation status

This is a design proposal. The existing runtime currently awards 8 local coins per first-time puzzle solve and spends 3 on a hint or 10 at the first three tutorial restoration transitions. There is no rewarded-ad coin grant, paid coin pack, city project shop, server-side account balance, or persistent per-project unlock yet. Unity Editor tests and device verification are pending.
