# Shared contract rules

- Contract namespace: `ParsehNo.Contracts`.
- DTOs carry display state and command results, not UI components.
- Presentation reads snapshots and requests commands; it does not calculate taxes or money.
- Domain owns validation, authorization, state transitions, pricing, tax, income and refund calculations.
- No class under Domain may depend on Presentation.
- Changes to a public interface require a coordinated PR and tests on both sides.
