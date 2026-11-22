# SunamoMail

## Short description

Odesílání e-mailů přes více poskytovatelů (Seznam.cz, Gmail a další). Obsahuje třídy pro SMTP servery a schránky a metody pro odeslání zprávy. Je součástí balíčků Sunamo pro .NET.

Sending mail with several providers (Seznam.cz, Gmail, etc.)

## Overview

SunamoMail is part of the Sunamo package ecosystem, providing modular, platform-independent utilities for .NET development.

## Main Components

### Key Classes

- **SmtpServerData**
- **GoogleAppsMailbox**
- **MailBox**
- **SeznamMailbox**

### Key Methods

- `Gmail()`
- `SeznamCz()`
- `SendEmail()`
- `SendCentrum()`
- `SendSeznam()`
- `SendSeznamMailkitWorker()`

## Installation

```bash
dotnet add package SunamoMail
```

## Dependencies

- **Microsoft.Extensions.Logging.Abstractions** (v9.0.3)
- **MailKit** (v4.11.0)
- **Microsoft.Extensions.Logging** (v9.0.3)
- **MimeKit** (v4.11.0)

## Package Information

- **Package Name**: SunamoMail
- **Version**: 25.6.9.1
- **Target Framework**: net9.0
- **Category**: Platform-Independent NuGet Package
- **Source Files**: 13

## Related Packages

This package is part of the Sunamo package ecosystem. For more information about related packages, visit the main repository.

## License

See the repository root for license information.

<!-- sunamo:test-coverage:start -->
## Test coverage

| Metric | Value |
|---|---|
| Method | static |
| Test projects | 1 |
| Public types referenced by tests | 1/8 (12.5 %) |
| Public methods referenced by tests | 2/11 (18.2 %) |
| Line coverage | n/a |
| Branch coverage | n/a |
| Generated (UTC) | 2026-11-22 21:24 |

<!-- sunamo:test-coverage:data {"package":"SunamoMail","container":"pinp","testProjects":1,"publicTypes":8,"typesReferencedByTests":1,"publicMethods":11,"methodsReferencedByTests":2,"lineRate":null,"branchRate":null,"method":"static","generatedUtc":"2026-11-22 21:24"} -->
<!-- sunamo:test-coverage:end -->
