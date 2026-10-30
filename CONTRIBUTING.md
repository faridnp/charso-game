# Contributing

## Collaboration model

The repository is intended for parallel contributions by the project owner and invited collaborators. Every contributor, including AI agents, follows the same issue, branch, review, and test standards.

## Before starting

Read README.md, AI_WORKFLOW.md, SECURITY.md, and relevant design/architecture documents. Select an issue or agree on a task with the project owner. Confirm ownership and allowed files. Confirm no other contributor is editing the same scene, prefab, content schema, or shared interface.

## Branch naming

Use feature/short-description, fix/short-description, docs/short-description, test/short-description, or chore/short-description. Do not push directly to main for normal feature work.

## Pull request checklist

- [ ] Scope matches the issue; no unrelated refactor.
- [ ] Existing behavior is preserved unless change is explicitly approved.
- [ ] Public interfaces and data ownership are documented.
- [ ] Tests added or updated.
- [ ] Exact test commands and results are included.
- [ ] Security, privacy, performance, and localization effects are described.
- [ ] New dependencies include version, license, and rationale.
- [ ] No secrets, generated files, or unlicensed assets are included.
- [ ] Screenshots or recordings are included for meaningful UI changes.
- [ ] Known limitations and rollback plan are stated.

## Code conventions

Prefer clear names and small cohesive classes. Keep domain rules independent of Unity presentation where practical. Avoid hidden global state and uncontrolled singletons. Validate external data at boundaries. Make asynchronous operations cancellable and handle app pause/termination. Use stable IDs for content and save data. Keep serialization changes backward-compatible or include explicit migrations.

## Content and localization

All player-facing content must be reviewed for natural Persian. Preserve correct RTL behavior in mixed Persian/Latin/numeric strings. Record lexicon sources and licenses. Record heritage fact sources and media rights. Never present fictional story text as historical fact.

## Reviews and merge

At least one human project-owner review is required for gameplay, economy, security, store integrations, architecture, and asset-rights changes. CI passing does not replace review. Do not merge until required checks and approvals are satisfied.
