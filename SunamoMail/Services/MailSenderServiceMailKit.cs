namespace SunamoMail.Services;

/// <summary>
/// Email sending service using MailKit library.
/// </summary>
public partial class MailSenderService
{
    /// <summary>
    /// Sends email via Seznam.cz using MailKit library with retry logic.
    /// This is an async worker method that attempts to send the email multiple times.
    /// </summary>
    /// <param name="attempts">Number of send attempts to make before giving up.</param>
    /// <param name="from">Sender credentials (name, email, password).</param>
    /// <param name="to">Recipient email address.</param>
    /// <param name="subject">Email subject line.</param>
    /// <param name="plainTextBody">Plain text email body content.</param>
    /// <param name="attachments">Collection of file paths to attach to the email.</param>
    /// <returns>True if email was sent successfully, false otherwise.</returns>
    public async Task<bool> SendSeznamMailkitWorker(int attempts, From from, string to, string subject, string plainTextBody, IEnumerable<string> attachments)
    {
        to = to.Trim();

        var email = new MimeMessage();
        email.From.Add(new MailboxAddress(from.Name, from.Mail));
        email.To.Add(new MailboxAddress(to, to));
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

                }
            }

        }
        return false;
    }
}
