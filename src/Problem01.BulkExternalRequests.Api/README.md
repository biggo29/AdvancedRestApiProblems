# Problem 01 — Bulk External Requests

> **Goal:** Fetch many external API responses concurrently without blocking the server thread.

---

## 🧠 The Problem

When a client sends a list of user IDs, a naive implementation fetches them **one by one**:

```csharp
// ❌ Sequential — blocks on every request
foreach (var id in ids)
{
    await client.GetUserAsync(id);
}
```

### Why this is bad

| Scenario | Time per request | Total time |
|---|---|---|
| 1 000 users (sequential) | 100 ms | **≈ 100 seconds** |
| 1 000 users (parallel) | 100 ms | **≈ 100 ms** |

---

## ✅ The Solution — `Task.WhenAll`

Fire **all** tasks at once and await them together:

```csharp
// ✅ Parallel — all requests in-flight simultaneously
var tasks = userIds.Select(id => _externalUserClient.GetUserAsync(id));
var results = await Task.WhenAll(tasks);
```

`Task.WhenAll` returns when **all** tasks complete. Failed tasks return `null` and are filtered out gracefully (partial failure handling).

---

## 🏗️ Architecture

```
Controllers
  └── BulkFetchController       POST /api/bulk/users

Services
  └── IBulkFetchService
  └── BulkFetchService          orchestrates Task.WhenAll

Clients
  └── IExternalUserClient
  └── ExternalUserClient        typed HttpClient → jsonplaceholder.typicode.com

Models
  ├── Requests/BulkFetchRequest
  ├── Responses/BulkFetchResponse
  └── Dtos/ExternalUserDto

Extensions
  └── ServiceCollectionExtensions   DI registration
```

---

## 🌐 API Endpoint

### `POST /api/bulk/users`

**Request body**

```json
{
  "userIds": [1, 2, 3, 4, 5]
}
```

**Response**

```json
{
  "users": [
    { "id": 1, "name": "Leanne Graham", "email": "Sincere@april.biz" }, 
    { "id": 2, "name": "Ervin Howell",  "email": "Shanna@melissa.tv" }
  ],
  "totalFetched": 2
}
```

> `totalFetched` reflects only successfully fetched users — IDs that return a non-2xx response are silently excluded.

---

## 🔑 Key Classes

| Class | Responsibility |
|---|---|
| `BulkFetchController` | Accepts the HTTP request, delegates to the service |
| `BulkFetchService` | Builds the task list and calls `Task.WhenAll` |
| `ExternalUserClient` | Typed `HttpClient` wrapper; returns `null` on failure |
| `ServiceCollectionExtensions` | Registers `IBulkFetchService` and the typed `HttpClient` |

---

## 🔧 External API

This project uses [JSONPlaceholder](https://jsonplaceholder.typicode.com/) as a free mock REST API.

Base URL: `https://jsonplaceholder.typicode.com/`  
Endpoint used: `GET /users/{id}`

---

## 📚 Topics Covered

- `async` / `await`
- `Task.WhenAll` for bounded parallel I/O
- `IHttpClientFactory` via typed `HttpClient`
- Partial failure handling (null filtering)
- Dependency injection with interface abstractions

---

## 🚩 Important Improvement: Bounded Concurrency (Next Lesson)

> **Current implementation has a real-world risk!**

If you send 1,000 requests simultaneously, you may:

- Exhaust available sockets (resource exhaustion)
- Overload the external API (causing failures for others)
- Hit rate limits (get blocked or throttled)

**Next improvement:**

- Limit the number of concurrent requests (bounded concurrency)
- Use `SemaphoreSlim` or `Parallel.ForEachAsync` to control parallelism

This is what senior engineers mention in interviews and is essential for production-grade systems.
