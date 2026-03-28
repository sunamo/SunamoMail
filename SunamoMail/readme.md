# SunamoMail

A .NET library for sending emails via multiple SMTP providers including Gmail, Seznam.cz, and Centrum.cz.

## Features

- **Google Apps Mailbox** - Send emails via Gmail SMTP with support for TO, CC, BCC, reply-to, HTML body, and attachments
- **Seznam.cz Mailbox** - Send emails via Seznam.cz SMTP with retry logic
- **MailKit Integration** - Async email sending via Seznam.cz using the MailKit library with attachment support
- **Static MailBox Wrapper** - Simplified static API for common email sending scenarios

## Installation

```
dotnet add package SunamoMail
```

## Quick Start

```csharp
// Using GoogleAppsMailbox
var mailbox = new GoogleAppsMailbox("Sender Name", "sender@gmail.com", "admin@example.com", "password");
string result = mailbox.SendEmail("recipient@example.com", "", "", "", "Subject", "<p>Hello</p>", true);

// Using static MailBox wrapper
MailBox.Mailbox = mailbox;
string result = MailBox.SendEmail("recipient@example.com", "", "", "Subject", "<p>Hello</p>");

// Using MailSenderService with MailKit (async)
var service = new MailSenderService(logger);
var from = new From("Sender", "sender@seznam.cz", "password");
bool success = await service.SendSeznamMailkitWorker(3, from, "recipient@example.com", "Subject", "Body text", Array.Empty<string>());
```

## Target Frameworks

**Supported:** `net10.0`, `net9.0`, `net8.0`

## Links

- [NuGet](https://www.nuget.org/profiles/sunamo)
- [GitHub](https://github.com/sunamo/PlatformIndependentNuGetPackages)
- [Developer Site](https://sunamo.cz)

For feature requests or bug reports: [Email](mailto:radek.jancik@sunamo.cz) or open an issue on GitHub.
