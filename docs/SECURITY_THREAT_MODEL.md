# Charso Security and Abuse Threat Model

## Scope

This model covers the first offline-first Android game and the later store/ads/backend integrations. It is a design baseline, not a completed penetration test or security certification.

## Assets to protect

- Player progress and local save integrity.
- Soft-currency balance and restoration progression.
- Paid entitlements, purchase receipts, and refunds.
- Rewarded-ad completion state.
- Account/session tokens if accounts are introduced.
- Source code, signing keystore, release credentials, private store tokens, and backend secrets.
- Player privacy and diagnostic data.

## Trust boundaries

- The Android client is untrusted and can be modified, instrumented, rooted, restored from backups, or run with altered local files.
- Store SDK callbacks are inputs to the client, not proof of ownership by themselves.
- Ad SDK callbacks can be duplicated, reordered, interrupted, or absent.
- Backend and store provider responses must be authenticated, validated, bounded, and handled idempotently.
- CI runners execute repository code. Self-hosted runners must only execute trusted branches and reviewed code.

## Threats and required controls

| Threat | Impact | Required control | Release evidence |
|---|---|---|---|
| Local save editing | Free soft currency or skipped progression | Treat offline soft currency as non-premium convenience; validate state, clamp values, version and recover saves; never trust local state for paid value | Save tamper tests and economy review |
| Duplicate reward callback | Currency duplication | Stable transaction IDs and idempotent reward ledger | Duplicate/reordered callback tests |
| Forged purchase result | Unpaid Pro access | Verify receipt with a trusted provider/backend path; no client-only entitlement | Sandbox purchase, refund, replay, offline tests |
| Rewarded ad failure treated as success | Free rewards | Fail closed; require completed AND rewarded callback; timeout/no-fill/SDK failure grants nothing | Full ad-state-machine tests |
| Replayed network request | Duplicate purchase/reward | Server idempotency key, authenticated user binding, transaction uniqueness | Replay and concurrency tests |
| Token/keystore committed | Account takeover or app signing compromise | Secret scanning, protected CI secrets, short-lived credentials, separate debug/release signing | Repository scan and release access review |
| Malicious pull request on self-hosted runner | Developer PC compromise | No untrusted PR jobs on self-hosted runner; manual dispatch from reviewed trusted branch only | Workflow permissions and branch review |
| Dependency compromise | Supply-chain compromise | Pin versions/commits, record license and source, review package changes, scan dependencies | Dependency inventory and release diff review |
| PII overcollection | Privacy harm and compliance exposure | Data minimization, explicit purpose/retention, no sensitive personal data in analytics | Privacy review and event schema |
| Denial of service / API abuse | Backend cost or downtime | Auth, rate limits, quotas, payload limits, timeouts, abuse monitoring | Load and rate-limit tests |

## Monetization invariants

1. No purchase is granted solely because a client boolean says success.
2. Every provider transaction maps to at most one entitlement grant.
3. Refund/revocation is represented and reconciled.
4. A network timeout is an unknown/pending state, not a successful purchase.
5. Rewarded-ad reward is granted only when both completion and reward conditions are explicitly confirmed.
6. If the ad SDK is unavailable, the user can continue core gameplay without receiving the optional ad reward.
7. No private key, server secret, store private token, or signing credential exists in client code or APK.

## Offline policy

The core puzzle and ordinary progression should remain playable offline. Local progress is persisted safely and synchronized only when a trusted service is later introduced. Do not create a fake sense of server authority by storing a shared or paid balance only on-device. Conflict resolution and reconciliation rules must be specified before cloud sync.

## Verification status

The domain reward ledger and save codec are initial local safeguards. They are not tamper-proof, do not secure paid value, and have not yet been executed in Unity tests. No penetration test has been performed.
