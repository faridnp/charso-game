# Gameplay Domain Handoff

## Owned path
`UnityProject/Assets/ParsehNo/Gameplay/Domain/`

## Your responsibility
Port and test the game rules in C#:
- City state, grid occupancy and build validation.
- Construction cost accounting, upgrades, demolition and the 75% refund rule.
- Treasury ledger, income, upkeep and cashflow.
- Residential / commercial / industrial taxation and fiscal budgets.
- Population, housing capacity, jobs, wages, commute and happiness.
- Production, inventory, market, imports and supply chains.
- Electricity, water, healthcare, education and other service simulations.
- Land value, rents, districts and service coverage.
- Military readiness, garrison costs, defense and strategic PvE resolution.
- Save repository, migrations, offline recovery and online authoritative commands.

## Non-negotiable boundaries
1. Do not create or edit presentation GameObjects, HUD, models, scenes or shaders.
2. Do not hardcode UI presentation in the domain.
3. Use shared request/response DTOs and interfaces from `UNITY_PORT/Contracts/`.
4. The domain returns immutable/read-only snapshots and domain events; Presentation renders them.
5. Do not trust client-side currency, purchase state or online entitlements.
6. Keep simulation deterministic where practical and write unit tests for every rule.
7. Preserve source behavior: empty founding city, 30,000 initial coins, 36x36 grid, tile size 2.2, no road adjacency requirement for basic placement, 75% refund of cumulative construction+upgrade investment, six continuous road tiles before ambient traffic eligibility, bounded/persisted tax and budget settings.
8. Keep offline treasury distinct from online wallet; never merge their trust boundaries.

## Reference material
The downloadable Unity foundation ZIP contains:
- `Reference/LogicReference/BabylonTS/game/` — original TypeScript domain logic.
- `Reference/LogicReference/BabylonTS/tests/` — 113 original unit tests.
- `Reference/LogicReference/DesignDocs/` — economy, progression, military and online architecture.
- `Reference/LogicReference/Online/supabase/` — draft online migrations and edge function.

Implement the Unity C# domain in small PRs. Avoid changing shared interfaces without agreement with the graphics owner.
