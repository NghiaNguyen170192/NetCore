# NetCore.Donation — Continuation Context

Last updated: 2026-08-15  
Purpose: archive of goals, progress, and conventions so work can resume later without re-deriving context from chat history.

Related chat: [Journal receipt storage](a49fabf7-2f24-461e-9b31-b677793ceab0)  
Plan (do not edit unless continuing that plan): `~/.cursor/plans/journal_receipt_storage_3c79af37.plan.md`

---

## Snapshot — where things stand

| Item | Status |
|---|---|
| Pass 1 (scaffold + donation CQRS) | Done; committed as `8cad2ea` |
| Pass 2 (Journal + preferences + receipt PDF/MinIO) | **Done in working tree; not committed** |
| Tests | **94 passed** (Domain 23, Infra DB 30, Application 37, Api 4) |
| Live Aspire smoke (POST journal/receipt → JSON/PDF GET) | Not completed — AppHost was left Waiting / later aborted; port + Postgres volume issues |
| Commit request | User has **not** asked to commit Pass 2 yet |

**Immediate resume actions:**

1. Commit Pass 2 if desired (large uncommitted set; keep `Dispatcher.cs` deleted).
2. Clean Aspire restart: stop leftover AppHost/UI, free ports `6000`/`6001`/`6010`/`6011`/`9000`/`9001`, delete Docker volume `netcore.donation.apphost-*-postgres-data` once if Postgres stays Waiting, then `dotnet run --project src/client/NetCore.Donation.AppHost`.
3. Smoke: `POST /api/v1/journals`, `POST /api/v1/receipts`, `GET` with JSON vs `Accept: application/pdf`.

---

## Project intent

Cloud-based donation management for non-profits (RMIT COSC29800 Assignment 3).

**Product flow (target):** donors submit donation requests (no real payments) → events processed asynchronously → transactions recorded → journals/receipts generated → notifications by preferred channel (email/SMS).

**Proposal AWS map:**

| Concern | Service | Local status |
|---|---|---|
| Data | RDS PostgreSQL | Aspire Postgres |
| API | API Gateway + Lambda | Local Kestrel API |
| Async processing | EventBridge / SNS + Lambda | Not started |
| Receipts storage | S3 | MinIO via Aspire + `AWSSDK.S3` |
| Notifications | SES + SNS | Preference flags only (`DoNotEmail` / `DoNotSms`) |
| Static/UI hosting | S3 / Elastic Beanstalk | Template UI only |
| Observability | CloudWatch | OpenTelemetry locally |

**Template base:** `personal/NetCore` (.NET 10 Clean Architecture + Aspire).  
**Code location:** `RMIT/NetCoreDonation/` with namespaces `NetCore.Donation.*`.

---

## Git

| Item | Value |
|---|---|
| Repo path | `C:\Nghia\source\RMIT\NetCoreDonation` |
| Remote | `https://github.com/NghiaNguyen170192/NetCore.git` |
| Branch | `donation-implementation` (tracks `origin/donation-implementation`) |
| Last commit on branch | `8cad2ea` — *Scaffold NetCore.Donation with donation CQRS APIs and Aspire local stack.* |
| Pass 2 | Uncommitted (modified + many untracked files; `CONTINUATION.md` itself untracked) |
| Keep deleted | `src/NetCore.Donation.Application/Messaging/Dispatcher.cs` — MediatR is the dispatcher |
| Sibling checkout | `personal/NetCore` is separate; do not mix worktrees casually |

---

## Goals already implemented

### Pass 1 — local scaffold (committed)

- [x] Copy/rename NetCore template → `NetCore.Donation.*`
- [x] Domain: Contact (+ `CountryId`), PaymentMethod, PaymentSchedule, Transaction, Receipt; keep Country
- [x] Country-style CQRS (one type per file), explicit controller bodies, per-entity repositories
- [x] Aspire Postgres/Redis, EF migrations, seeds, tests
- [x] User rejected “slice” / grouped files — stick to Country folder layout

### Pass 2 — journal, preferences, receipt PDF (code done, uncommitted)

**Contact preferences**

- [x] `DoNotEmail` / `DoNotSms` (default `false`) on entity, create/update, EF, DTOs, seeds/tests

**Journal**

- [x] Minimal `Journal : Entity, IAggregateRoot` (Id + audit only)
- [x] `IJournalRepository` + EF config/repo + `DbSet`
- [x] CQRS: Create, Get by id, OData list, Delete (no Update until mutable fields exist)
- [x] `JournalController` → `/api/v1/journals`

**Receipt documents**

- [x] Persisted metadata: object key, file name, content type, generated-at, size
- [x] `POST /api/v1/receipts` → allocate id, blank PDF, upload, persist metadata, `201`
- [x] `GET /api/v1/receipts/{id}` Accept negotiation:
  - JSON (`QueryReceiptDto`) for empty / `*/*` / `application/json`
  - PDF stream + `Content-Disposition: attachment` for `application/pdf`
  - `406` unsupported; `404` missing receipt/document
- [x] Update regenerates document when linkage changes; delete removes storage object
- [x] Compensating storage delete if upload succeeds but DB save fails

**Storage / Aspire**

- [x] Domain ports: `IReceiptDocumentGenerator`, `IReceiptDocumentStorage`
- [x] Project `NetCore.Donation.Infrastructure.Storage` (S3 client usable vs MinIO + AWS; blank PDF; in-memory double; `AddObjectStorage`)
- [x] Aspire MinIO (`9000`/`9001`, `minioadmin`/`minioadmin`), health `/minio/health/live`, ObjectStorage env for API + Migration
- [x] Stable Postgres password via `AppHost/appsettings.Development.json` → `Parameters:postgres-password`
- [x] Migration `20260815084604_AddJournalPreferencesAndReceiptDocuments`
- [x] `DonationSeed` creates Journal + full sample chain including receipt PDF in MinIO
- [x] DI prefers Aspire connection strings `ConnectionStrings:netcore-donation-db` / `redis` over appsettings

**Verification**

- [x] Unit/integration tests green (**94**)
- [ ] Live Aspire smoke still outstanding (see Snapshot)

---

## Key paths (Pass 2)

| Area | Path |
|---|---|
| Contact prefs | `Domain/Entities/Contact.cs` |
| Journal entity | `Domain/Entities/Journal.cs` |
| Receipt metadata | `Domain/Entities/Receipt.cs` |
| Storage ports | `Domain/Storage/` |
| Journal CQRS | `Application/Journal/` |
| Receipt doc flow | `Application/Receipt/` (+ `ReceiptDocumentService`, `GetReceiptDocument/`) |
| Storage impl | `Infrastructure.Storage/` |
| EF + migration | `Infrastructure.Database/` (Journal repo, `*AddJournalPreferencesAndReceiptDocuments*`) |
| API | `Controllers/JournalController.cs`, `ReceiptController.cs` |
| Aspire | `client/NetCore.Donation.AppHost/Program.cs` |
| Seed | `Migration/Seeds/Base/DonationSeed.cs` |
| API tests | `test/.../ReceiptAndJournalApiTests.cs` |

Object key format: `receipts/{receiptId:N}.pdf`.

---

## Local run

```bash
dotnet run --project src/client/NetCore.Donation.AppHost
```

| Endpoint | URL |
|---|---|
| Aspire Dashboard | URL printed by AppHost |
| API | `http://localhost:6000` / `https://localhost:6001` |
| UI | `http://localhost:6010` / `https://localhost:6011` |
| MinIO API / Console | `9000` / `9001` (`minioadmin` / `minioadmin`) |
| Swagger | `https://localhost:6001/swagger` |

Routes: `/api/v1/countries|contacts|payment-methods|payment-schedules|transactions|journals|receipts`

---

## Decisions locked (do not reopen casually)

| Decision | Choice |
|---|---|
| Naming | `NetCore.Donation.*` under `RMIT/NetCoreDonation/` |
| First local host | Aspire (Postgres + Redis + MinIO); Lambda later |
| CQRS style | Country-style one-type-per-file; MediatR (not custom Dispatcher) |
| Controllers | Explicit action bodies (no arrow-only stubs) |
| Query DTOs | OData/list/get only; kebab-case `[JsonPropertyName]`; CUD via Commands |
| Receipt retrieval | Same URI; Accept negotiation (not a separate `/pdf` route) |
| Journal now | Create / Get / List / Delete only; no Update until properties exist |
| Storage | One S3-compatible impl for MinIO local + AWS later |
| PDF for now | Minimal valid blank PDF; real merge template later |

---

## Goals still pending

### Near-term

- [ ] Commit Pass 2 (when asked)
- [ ] Complete live Aspire smoke after clean restart
- [ ] Expand PaymentMethod fields
- [ ] Replace blank PDF with real document-merge template
- [ ] Link Journal to Transaction / Contact once properties are decided
- [ ] Use `DoNotEmail` / `DoNotSms` in SES/SNS notification flow
- [ ] Domain events for donation lifecycle
- [ ] JSON string enums (`JsonStringEnumConverter`)
- [ ] Donation-focused UI
- [ ] Trim leftover Identity Provider / nested ServiceDefaults if not needed

### Cloud / assignment

- [ ] API Gateway + Lambda packaging
- [ ] EventBridge/SNS async donation processing
- [ ] Real AWS S3 (same storage code; drop custom ServiceUrl / ForcePathStyle; use IAM + region)
- [ ] SES email + SNS SMS
- [ ] Elastic Beanstalk / static UI hosting
- [ ] CloudWatch + architecture docs from assignment PDF (`ASSESSMENT_3_S2.pdf`)

---

## Architecture conventions (must keep)

- Mirror `personal/NetCore/src/NetCore.Application/Country/` layout.
- Handlers depend on Domain storage ports only — never AWS SDK types in Application.
- Prefer Aspire-injected connection strings when present.
- Do not recreate Pass 2 plan todos; mark/complete only if continuing that plan file.

---

## Known quirks

1. Enum JSON still numeric unless a converter is registered.
2. Query DTOs use kebab-case `[JsonPropertyName]`.
3. Receipt GET needs `Accept: application/pdf` for binary download; browsers sending HTML Accept may get `406`.
4. Local MinIO credentials are development-only (`minioadmin`).
5. Keep `Dispatcher.cs` deleted; MediatR dispatches.
6. Aspire Postgres password is stable via `Parameters:postgres-password` in AppHost Development settings. An older generated password + `WithDataVolume()` causes eternal Waiting — delete volume `netcore.donation.apphost-*-postgres-data` once.
7. Fixed ports require a clean prior AppHost shutdown; rebuild can fail if `NetCore.Donation.UI` locks Application DLLs.
8. Earlier Aspire journals probe saw HTTP **500** while API process was up but DB/storage not healthy — do not treat “port open” as success.

---

## Suggested next session order

1. Commit Pass 2 (if requested) — include Journal, Storage project, migration, docs; do not revive `Dispatcher.cs`.
2. Clean Aspire restart + smoke journal/receipt JSON+PDF.
3. Decide Journal business properties / FKs.
4. Upgrade blank PDF to a real merge template.
5. Begin AWS Lambda packaging on the existing MediatR API surface.
