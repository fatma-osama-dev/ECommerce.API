# 🛒 ECommerce Core Engine (Onion Architecture RESTful APIs)

A production-ready, high-performance E-Commerce Backend Engine built with **.NET 8** following **Onion Architecture** (Domain-Centric Design) and SOLID principles. The system delivers asynchronous e-commerce business lifecycles, integrating modern memory caching and secure payment gateway loops.

---

## 🚀 Architectural Layers

The solution is strictly decoupled into distinct operational boundaries to achieve optimal maintainability and prevent database logic leaks:

*   **`Ecommerce.Domains` (Core)** — Holds domain entities, aggregate boundaries, static enums, and repository abstractions. Independent of any external packages.
*   **`Ecommerce.Applications`** — Enforces core business workflows, services implementations, high-speed AutoMapper DTO mappings, and custom execution responses.
*   **`Ecommerce.Infrastructures`** — Manages database persistence via EF Core, automatic SQL migrations, automatic JSON seed data injections, token generation services, and core database repositories.
*   **`Ecommerce.APIs` (Presentation)** — Contains API endpoints, pipeline configuration filters, custom attribute extensions, static asset serving, and request/response orchestration.

---

## ⚡ Key Technical Features

### 📦 1. Product Catalog & Smart Pipeline
*   Advanced query filtering, text-based search indexing, dynamic data sorting, and robust server-side data pagination.
*   Automated data seeding on application startup using structured JSON files (`brands.json`, `types.json`, `products.json`, `delivery.json`).

### 🛍️ 2. Customer Basket & Distributed Cache
*   Highly responsive basket recovery powered by **Redis Cache** memory.
*   Transient basket objects supporting runtime delivery method updates and automated token storage.

### 🔐 3. Authentication & Anti-IDOR Security
*   Secure user registry and authorization powered by **ASP.NET Core Identity**.
*   **Anti-IDOR Protection:** Endpoint resources are guarded by dynamically parsing cryptographic JWT token claims and cross-auditing verified emails before reading or mutating relational records.

### 💳 4. Hybrid Payment Architecture (Card & Cash)
*   **Stripe Gateway Integration:** Secure server-to-server tokenization processing, generating individual `PaymentIntentId` and `ClientSecret` values per session.
*   **Anti-Price Tampering Guard:** Relational product price verification against live SQL Server records during intent compilation to prevent request modifications.
*   **Asynchronous Cryptographic Webhooks:** Custom webhook endpoint validating signatures against the environment secret keys to intercept bank actions and mutate order statuses.
*   **Fallback COD Mode:** Integrated logic supporting hybrid checkouts with explicit "Cash on Delivery" processing fields inside the order tables.

### 🚀 5. Intelligent Caching Infrastructure
*   Custom **`[Cached]` Action Filter** leveraging Redis memory caches to store catalog endpoints and reduce database stress.
*   **Secure Multi-User Splitting:** Automated user context claim injection inside generated Redis string keys to strictly isolate single user query views.

---

## 🛠️ Key Technologies & SDKs

*   **Runtime:** .NET 8 (ASP.NET Core Web API)
*   **Database ORM:** Entity Framework Core (SQL Server)
*   **In-Memory Database:** Redis Cluster (StackExchange.Redis)
*   **Financial Gateway:** Stripe.net Package
*   **Object Mapper:** AutoMapper Extension
*   **Identity System:** JWT Bearer Authentication & ASP.NET Core Identity

---

## 📋 Prerequisites

To run this project locally, ensure you have the following environments configured:
1.  **.NET 8 SDK** installed.
2.  **SQL Server / LocalDB** instance running.
3.  **Redis Server** (Local container or remote cloud cluster) active.
4.  **Stripe Developer Account** (For secret keys and testing webhooks).

---

## 🏁 Getting Started & Local Setup

### 1. Clone the Solution
```bash
git clone <repository-url>
```

### 2. Configure Environment Keys
Open `Ecommerce.APIs/appsettings.json` and inject your active connection secrets inside the designated fields:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SQL_SERVER;Database=EcommerceDb;Trusted_Connection=True;TrustServerCertificate=True;",
    "Redis": "YOUR_REDIS_ENDPOINT,abortConnect=false"
  },
  "Token": {
    "Key": "YOUR_SUPER_SECRET_JWT_SIGNING_KEY_MIN_512_BITS_LONG"
  },
  "StripeSettings": {
    "PublishableKey": "pk_test_your_stripe_public_key",
    "SecretKey": "sk_test_your_stripe_secret_key",
    "WhSecret": "whsec_your_stripe_webhook_secret_key"
  }
}
```

### 3. Build and Run the Core Engine
Navigate into the root path using a CLI tool or terminal and trigger the following standard execution command:

```bash
cd Ecommerce
dotnet build
dotnet run --project Ecommerce.APIs/Ecommerce.APIs.csproj
```
*(Note: At startup, the engine will automatically run a Database.Migrate() loop, construct the SQL tables, and populate seed datasets).*

### 4. Explore the System
Once compilation succeeds, navigate your web browser to the Swagger UI page to interact with the API endpoints:
👉 `https://localhost:https-port/swagger`

---

## 📥 Stripe Webhook Routing

*   **API Webhook Target URL:** `POST /api/payment/webhook`
*   **Testing Tool Recommendation:** Use the Stripe CLI to proxy real events to your local server instance:
    ```bash
    stripe listen --forward-to https://localhost:<your-port>/api/payment/webhook
    ```
*   Copy the output webhook secret string and append it directly into your `StripeSettings:WhSecret` section inside `appsettings.json`.

---

## 🛢️ Manual Database Migrations Control

If you prefer executing explicit migration triggers from the CLI instead of automated startup hooks, execute:

```bash
dotnet ef database update --project Ecommerce.Infrastructures --startup-project Ecommerce.APIs
```

---

## 🤝 Contributing & License
All pull requests and feature extensions are highly appreciated. Ensure any additional controller configurations utilize the established `BaseResponse<T>` pattern and conform to loose-coupling guidelines.
