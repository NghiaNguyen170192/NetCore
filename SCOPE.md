# Scope — Cloud-based donation management (COSC29800 Assignment 3)

Simulated donation workflow for a non-profit. **No real payments.** Donors submit a request; the system records a transaction, writes a journal line, stores a digital receipt, and later notifies by email or SMS according to `DoNotEmail` / `DoNotSms`.

Inspired by Microsoft NFP (`msnfp_*` / `kpmg_nfp_transactionjournal`) but **only** the donation spine. The Dynamics dump (`tables_mapping.txt`, gitignored) is not the schema we ship.

## In scope

| Aggregate | Role | Cardinality / FKs |
|---|---|---|
| Contact | Donor profile + communication prefs | Country; many payment methods and schedules |
| PaymentMethod | Saved way to give (`DisplayName` only) | **Contact 1 → many methods** |
| PaymentSchedule | Recurring or planned gift | Contact; **this schedule’s current method** |
| Transaction | Posted gift (simulated) | Contact; schedule; **this gift’s method** (kept even if the schedule later changes method) |
| Receipt | Digital PDF in S3/MinIO | Contact; transaction; optional payment schedule (copied from the transaction when linked) |
| Journal | Ledger line for a posted gift | **Required transaction** (`kpmg_nfp_transactionjournal.kpmg_transaction`) |

Flow: request → transaction → journal → receipt → notify (SES/SNS later).

```
Contact 1──* PaymentMethod
Contact 1──* PaymentSchedule ──> PaymentMethod
Transaction ──> PaymentSchedule, PaymentMethod, Contact
Receipt ──> Transaction?, PaymentSchedule?, Contact
Journal ──> Transaction
```

## Out of scope (Dynamics dump)

Appeals, events, packages, membership, gift batches, tributes, designations, donor commitments, grants/awards, bank runs, payment processors / merchant suite, receipt stacks, refunds, planned giving, and reverse FKs on PaymentMethod pointing at a single schedule or transaction.

## AWS map (unchanged intent)

RDS PostgreSQL, Lambda, EventBridge/SNS, SES + SNS, S3 (receipts + static UI), API Gateway, CloudWatch. Locally: Aspire Postgres, Redis, MinIO, Kestrel API.
