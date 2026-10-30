# Security Policy

## Scope

Security is a product requirement, not a late release task. This policy applies to the Android client, backend services, purchase flows, ad rewards, build pipeline, repository, and player data.

## Threat model baseline

Assume an attacker can inspect, decompile, modify, and repackage the client; change local files and clock values; replay, reorder, or fabricate client requests; automate gameplay; interrupt app, network, or payment flows; obtain public configuration embedded in the application; and abuse duplicate requests, race conditions, and stale sessions.

The client is not a trusted authority for paid entitlements, high-value balances, account ownership, or reward issuance.

## Required controls

### Identity and sessions

- Use established, maintained authentication protocols/providers; do not invent password or token cryptography.
- Store session credentials using platform-appropriate secure storage.
- Use short-lived access tokens and refresh-token rotation where supported.
- Revoke sessions after logout, compromise, or suspicious activity.
- Avoid collecting phone numbers or identity details without a documented need.

### Purchases and entitlements

- Verify store purchase evidence through a trusted backend wherever the provider supports it.
- Never embed private store verification credentials or service-role keys in the APK, Unity assets, or repository.
- Make purchase processing idempotent using stable transaction identifiers.
- Handle pending, cancelled, failed, refunded, revoked, duplicated, and interrupted purchases.
- Keep a server-side entitlement ledger for paid unlocks where an online authority is used.
- Reconcile ownership on restore and account changes.
- Do not grant permanent premium access based only on a client-controlled flag.

### Currency, rewards, and ads

- Keep high-value or purchased balances server-authoritative.
- Validate reward amount, source, player, request identity, and eligibility.
- Prevent duplicate reward grants using idempotency keys and transaction constraints.
- Grant rewarded-ad benefits only after successful provider completion and rewarded signal. Use server-side verification when available.
- No-fill, timeout, playback failure, SDK error, or network error must not grant a reward.
- Bound daily rewards, hints, and event claims; validate cooldowns with trusted time when online.

### API and data

- Use TLS for network transport.
- Enforce authorization on every protected operation.
- Validate type, length, range, ownership, and business rules on the server.
- Rate-limit authentication, reward claims, purchase verification, and abuse-prone endpoints.
- Avoid logging credentials, full receipts, personal data, or sensitive request payloads.
- Minimize analytics collection and define retention/deletion policies.
- Encrypt sensitive server-side data at rest where appropriate.

### Client and build pipeline

- Treat obfuscation and anti-tamper as defense-in-depth, not as the security boundary.
- Keep signing keys, keystores, API secrets, and private tokens outside Git.
- Use protected CI secrets with least privilege.
- Pin third-party build actions and dependencies to reviewed versions/commits where practical.
- Review package licenses, maintenance health, and known vulnerabilities.
- Separate development, test, and production environments.
- Restrict release signing and store upload permissions.

## Vulnerability reporting

Do not publish exploit details or player data in public issues. Report suspected vulnerabilities privately to the project owner through an agreed private channel. Include impact, affected version/commit, reproduction steps, and a safe proof of concept. Do not test against real players or production payment accounts without explicit authorization.

## Security review gates

Before a public release, complete authentication/session review (if accounts exist), purchase/entitlement threat review, save tampering and reward abuse tests, API authorization/rate-limit tests (if a backend exists), dependency and secret scanning, privacy disclosure and retention review, incident response, and rollback planning.
