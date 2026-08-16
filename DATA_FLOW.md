# Data flow — website to C# API to database

Local architecture for the COSC29800 donation system. Auth is not enabled. Email/SMS notification is not wired yet.

Use the mermaid blocks below in the report (GitHub, VS Code, Word via a mermaid add-in, or export from [mermaid.live](https://mermaid.live)).

## 1. Website → C# API → database

Browser talks to Blazor Server. The **server** calls the API over HTTP (`DonationApiClient`). Donation rows go to PostgreSQL. Receipt PDF bytes go to MinIO; receipt metadata stays in `Receipts`. Redis is beside the write path (cache / idempotency), not the system of record.

```mermaid
flowchart LR
  Browser["Browser"]

  subgraph Websites["Websites — Blazor Server"]
    Donor["Hope & Help<br/>NetCore.Donation.UI<br/>:6010"]
    Admin["Donation admin<br/>NetCore.Donation.Admin<br/>:6020"]
  end

  subgraph ApiHost["C# API — NetCore.Donation.Api :6000"]
    Ctrl["Controllers /api/v1"]
    MediatR["MediatR CQRS"]
    Handlers["Command / query handlers"]
    Repos["EF repositories"]
    Pdf["Blank PDF generator"]
  end

  Postgres[("PostgreSQL<br/>Contacts, PaymentMethods,<br/>PaymentSchedules, Transactions,<br/>Journals, Receipts")]
  MinIO[("MinIO / S3<br/>receipt PDFs")]
  Redis[("Redis<br/>cache / idempotency")]

  Browser -->|"SignalR / HTTP"| Donor
  Browser -->|"SignalR / HTTP"| Admin
  Donor -->|"HTTP JSON<br/>DonationApiClient"| Ctrl
  Admin -->|"HTTP JSON<br/>DonationApiClient"| Ctrl
  Ctrl --> MediatR --> Handlers --> Repos
  Repos --> Postgres
  Handlers --> Pdf --> MinIO
  Repos -.-> Redis
```

## 2. Donate click — one gift, six writes

The public site does not have a single “donate” API. One form submit issues the spine in order: contact (create or reuse by email) → payment method → schedule → transaction → journal → receipt.

```mermaid
sequenceDiagram
  actor Donor
  participant UI as Donation.UI
  participant API as Donation.Api
  participant DB as PostgreSQL
  participant Store as MinIO

  Donor->>UI: Submit donate form
  UI->>API: GET /contacts
  API->>DB: read Contacts
  alt new email
    UI->>API: POST /contacts
    API->>DB: insert Contact
  else known email
    UI->>API: PATCH /contacts/{id}/preferences
    API->>DB: update DoNotEmail / DoNotSms
  end
  UI->>API: POST /payment-methods
  API->>DB: insert PaymentMethod
  UI->>API: POST /payment-schedules
  API->>DB: insert PaymentSchedule
  UI->>API: POST /transactions
  API->>DB: insert Transaction
  UI->>API: POST /journals  { transactionId }
  API->>DB: insert Journal
  UI->>API: POST /receipts  { contactId, transactionId }
  API->>Store: upload blank PDF
  API->>DB: insert Receipt + object key
  UI-->>Donor: Thank you + PDF download
```

Admin **reads** the same tables (`GET /contacts`, `/transactions`, `/journals`, `/receipts`) and downloads PDFs with `Accept: application/pdf`.

## 3. Inside one API request

Every create/update/delete follows Country-style CQRS: controller → MediatR command → handler → domain entity → EF repository → PostgreSQL. Query DTOs are kebab-case (`transaction-id`, `do-not-email`); write bodies are camelCase.

```mermaid
flowchart TB
  HTTP["HTTP POST /api/v1/transactions<br/>camelCase JSON"]
  Ctrl["TransactionController.Create"]
  Cmd["CreateTransactionCommand"]
  H["CreateTransactionCommandHandler"]
  V["Load Contact, Schedule, Method<br/>reject if missing / wrong owner"]
  E["Transaction.Create — domain rules"]
  R["ITransactionRepository.AddAsync"]
  EF["ApplicationDatabaseContext"]
  DB[("Transactions row")]

  HTTP --> Ctrl --> Cmd --> H --> V --> E --> R --> EF --> DB
```

## Report notes

| In these diagrams | Not in this path yet |
|---|---|
| Two websites → one API → Postgres | JWT / Identity |
| Receipt bytes in MinIO, metadata in `Receipts` | SES / SNS notify |
| Journal requires `TransactionId` | Payment processor |
| Redis only beside the write path | Browser talking to the API directly |

Later cloud swap (same CQRS and tables): API Gateway + Lambda instead of Kestrel, RDS instead of local Postgres, S3 instead of MinIO.
