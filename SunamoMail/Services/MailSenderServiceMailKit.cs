namespace SunamoMail.Services;

public partial class MailSenderService
{
    public Task<bool> SendSeznamMailkitWorker(int attempts, From from, string to, string subject, string plainTextBody, IEnumerable<string> attachments)
    {
        return SendSeznamMailkitWorker(attempts, from, to, subject, plainTextBody, attachments, null);
    }

    public async Task<bool> SendSeznamMailkitWorker(int attempts, From from, string to, string subject, string plainTextBody, IEnumerable<string> attachments, IEnumerable<string>? cc)
    {
        to = to.Trim();

        var email = new MimeMessage();
        email.From.Add(new MailboxAddress(from.Name, from.Mail));
        email.To.Add(new MailboxAddress(to, to));
        if (cc is not null)
        {
            foreach (var ccAddress in cc)
            {
                if (string.IsNullOrWhiteSpace(ccAddress)) continue;
                var trimmed = ccAddress.Trim();
                email.Cc.Add(new MailboxAddress(trimmed, trimmed));
            }
        }
        email.Subject = subject;
        dynamic emailInfo = new ExpandoObject();
        emailInfo.To = to;
        emailInfo.Subject = subject;
        if (attachments.Any())
        {
            var bodyBuilder = new BodyBuilder();
            bodyBuilder.TextBody = plainTextBody;
            foreach (var attachmentPath in attachments)
            {
                await bodyBuilder.Attachments.AddAsync(attachmentPath);
            }
            email.Body = bodyBuilder.ToMessageBody();
        }
        else
        {
            email.Body = new TextPart(MimeKit.Text.TextFormat.Plain)
            {
                Text = plainTextBody
            };
        }

        for (int attemptIndex = 0; attemptIndex < attempts; attemptIndex++)
        {
            using (var smtp = new MailKit.Net.Smtp.SmtpClient())
            {
                try
                {
                    smtp.Connect("smtp.seznam.cz", 465, true);
                    smtp.Authenticate(from.Mail, from.Password);
                    smtp.Send(email);
                    smtp.Disconnect(true);
                    string information = JsonSerializer.Serialize(emailInfo);
                    logger.LogInformation(information);
                    return true;
                }
                catch (Exception ex)
                {
                    emailInfo.Exc = ex.Message;
                    string error = JsonSerializer.Serialize(emailInfo);
                    logger.LogError(error);
                    Console.Error.WriteLine($"[SunamoMail SMTP attempt {attemptIndex + 1}/{attempts}] to={to} subject=\"{subject}\" ex={ex.GetType().Name}: {ex.Message}");
                    if (attemptIndex == attempts - 1)
                    {
                        throw new InvalidOperationException($"SMTP send to {to} failed after {attempts} attempt(s): {ex.Message}", ex);
                    }
                }
            }

        }
        return false;
    }
}
