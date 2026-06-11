# CLAUDE.md — CyberSourceServer

Minimal API server for CyberSource payment processing. See the parent project conventions at `C:\Users\ahw00\OneDrive\Documents\Development\Claude\TestHarness\CLAUDE.md` for hard constraints, DTO pattern, error handling, DB separation, and logging rules. This file records MLE-specific context and a critical documentation warning.

## CyberSourceSQLiteServer — Permanently Out of Scope

`CyberSourceSQLiteServer` is a derivative of this project managed entirely by a separate, independent process. Never read, modify, reference, or reason about any file under `CyberSourceSQLiteServer`. If an instruction points to it, stop and flag the conflict rather than proceeding.

## MLE — Two Separate Systems

**CyberSource uses the acronym "MLE" for two completely separate encryption systems. They use different keys, different request headers, and different response formats. Never mix them.**

### System 1 — Request/Response MLE

- **Reference**: `https://developer.cybersource.com/docs/cybs/en-us/tms/developer/all/rest/tms/tms-onboarding/tms-mle-setup.md`
- **What it does**: Encrypts the outbound request body inside a JWS bearer JWT. Optionally requests an encrypted response (`encryptedResponse` field in the JSON body).
- **Request headers added**: `v-c-merchant-mle-kid: <ResponseMleKid>` (serial number from `ahw_portfolio_sandbox_mle_key.p12`). `v-c-response-mle-kid` claim in the MLE JWT triggers a wrapped response.
- **Response format**: Normal JSON body, optionally `{ "encryptedResponse": "<JWE compact>" }`.
- **Decryption key**: `MleCredentials.ResponseMlePrivateKey` — private key from `ahw_portfolio_sandbox_mle_key.p12`.
- **Implementation**: `MleJwtHelper.GenerateMleJwt(isResponseMle: true/false)`.

### System 2 — BLOB / Legacy MLE (tokenized cards)

- **Reference**: `https://developer.cybersource.com/docs/cybs/en-us/tms/developer/all/rest/tms/tms-net-tkn-intro/tms-net-tkn-partner-card-intro.md`
- **What it does**: The tokenized-cards endpoint always returns its response as a JWE compact BLOB. The inbound request may optionally use System 1 request MLE, or be sent plain.
- **Request headers added**: `Accept: application/jose` only. **`v-c-merchant-mle-kid` is NOT sent** — that header belongs to System 1 exclusively.
- **Response format**: JWE compact serialization (5-part dot-separated token). The `kid` in the JWE response header is CyberSource's internal identifier — it does not match any kid we advertise and must not be used for key lookup.
- **Decryption key**: `MleCredentials.LegacyMlePrivateKey` — private key from `key/private.pem`. **Not** `ResponseMlePrivateKey`.
- **The BLOB response is automatic** for the tokenized-cards endpoint; it is not triggered by any request header.

### Request × Response Encryption Grid

| `RequestEncryption` | `ResponseEncryption` | System 1 request? | System 2 BLOB? | Extra headers | Decrypt key |
|---|---|---|---|---|---|
| `none` | `none` | — | — | — | — |
| `mle` | `none` | request only | — | — | — |
| `mle` | `wrapped` | request + response | — | — | `ResponseMlePrivateKey` |
| `none` | `blob` | — | response only | `Accept: application/jose` | `LegacyMlePrivateKey` |
| `mle` | `blob` | request | response | `Accept: application/jose` | `LegacyMlePrivateKey` |
| `none` | `wrapped` | — | — | — | **invalid combination** |

## MLE Implementation Files

| File | Purpose |
|------|---------|
| `CybsClass.Cybersource\Authentication\MleCredentials.cs` | Loads SJC public cert, `ResponseMlePrivateKey` (P12), and `LegacyMlePrivateKey` (PEM); exposes `SjcPublicKey`, `SjcKid`, `ResponseMlePrivateKey`, `ResponseMleKid`, `LegacyMlePrivateKey` |
| `CybsClass.Cybersource\Authentication\MleJwtHelper.cs` | `EncryptPayloadToJwe` — JWE-encrypts the request body; `GenerateMleJwt` — builds the full bearer token + encrypted body for a System 1 request |
| `CybsClass.Cybersource\Transactions\JoseJwtDecrypt.cs` | `DecryptJweWithKey` — decrypts a JWE compact token with a given RSA key |
| `CybsClass.Cybersource\Transactions\CallCyberSource.cs` | `CallCyberSourceApiJsonMleGrid` — single consolidated sender implementing the full 5-valid-cell grid above |

### JWE Header (Required Fields — System 1)

| Field | Value | Notes |
|-------|-------|-------|
| `alg` | `RSA_OAEP_256` | SHA-256 based — do not use `RSA_OAEP` |
| `enc` | `A256GCM` | |
| `cty` | `JWT` | |
| `kid` | `MleCredentials.SjcKid` | SERIALNUMBER from SJC cert Subject DN |
| `iat` | `DateTimeOffset.UtcNow.ToUnixTimeSeconds()` | **Required custom header — see warning below** |

### JWS Payload (12 Required Fields)

`digest`, `digestAlgorithm`, `iat`, `exp` (iat+120), `iss`, `jti`, `request-host`, `request-method` (lowercase), `request-resource-path`, `v-c-jwt-version: "2"`, `v-c-merchant-id`, `v-c-response-mle-kid`

---

## WARNING — Official Documentation Is Wrong

The CyberSource MLE documentation at `developer.cybersource.com` contains two bugs in its code samples that together cause every request to return `HTTP 401 UNAUTHORIZED_USER`. **Do not use the documentation as the implementation reference.**

### Bug 1 — Wrong JWE algorithm

Documentation shows `RSA_OAEP` (SHA-1 based). CyberSource's server requires `RSA_OAEP_256`. Using the wrong algorithm returns 401.

### Bug 2 — Missing `iat` in JWE header (the decisive bug)

The documentation's JWE encryption sample builds the header with only `alg`, `enc`, `cty`, and `kid`. It does not include `iat`. CyberSource's server validates the JWE header for a freshness timestamp and treats a missing `iat` as an unauthorized request. This requirement appears nowhere in the public documentation.

**Correct C# implementation** (derived from decompiling `MLEUtility` in `AuthenticationSdk-0.0.43.jar`):
```csharp
var jweHeaders = new Dictionary<string, object>
{
    { "kid", MleCredentials.SjcKid ?? string.Empty },
    { "cty", "JWT" },
    { "iat", DateTimeOffset.UtcNow.ToUnixTimeSeconds() }  // required — not in docs
};

return Jose.JWT.Encode(
    jsonPayload,
    publicKey,
    Jose.JweAlgorithm.RSA_OAEP_256,   // not RSA_OAEP
    Jose.JweEncryption.A256GCM,
    null,
    jweHeaders,
    null);
```

### Documentation vs. SDK vs. Required

| Field | Documentation | SDK (`MLEUtility`) | Required |
|-------|--------------|-------------------|----------|
| `alg` | `RSA_OAEP` (SHA-1) | `RSA_OAEP_256` | `RSA_OAEP_256` |
| `iat` in JWE header | absent | present | **required** |

The root cause was only found by decompiling the Java `AuthenticationSdk-0.0.43.jar` and reading `MLEUtility.encryptAttributeWithAlgo`. The SDK diverges from the documentation in both respects. See the Java harness at `C:\Users\ahw00\OneDrive\Documents\Development\Claude\TestHarness\JavaMLE\401FixMain.md` for the full investigation record.

## Current MLE Status (2026-05-21)

401 resolved. Both bugs are fixed in `MleJwtHelper.cs`. The full MLE round-trip is confirmed working via the Java test harness (`JavaMLE`):
- JWE header includes `iat` and uses `RSA_OAEP_256`
- JWS bearer token carries all 12 required payload fields with `v-c-jwt-version: "2"`
- CyberSource server accepts auth (HTTP 400 body errors, not 401) and returns `encryptedResponse`
- Response JWE decrypts successfully with the MLE response private key
