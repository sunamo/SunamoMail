namespace SunamoMail.Services;

public partial class MailSender
{
    public async Task<bool> SendSeznamMailkitWorker(From from, string to, string subject, string plainTextBody, IEnumerable<string> attachments)
    {
        var email = new MimeMessage();
        email.From.Add(new MailboxAddress(from.Name, from.Mail));
        email.To.Add(new MailboxAddress(to, to));
        email.Subject = subject;
        dynamic d = new ExpandoObject();
        d.To = to;
        d.Subject = subject;
        if (attachments.Any())
        {
            var bodyBuilder = new BodyBuilder();
            bodyBuilder.TextBody = plainTextBody;
            foreach (var item in attachments)
            {
                await bodyBuilder.Attachments.AddAsync(item);
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
        using (var smtp = new MailKit.Net.Smtp.SmtpClient())
        {
            try
            {
                smtp.Connect("smtp.seznam.cz", 465, true);
                // Note: only needed if the SMTP server requires authentication
                smtp.Authenticate(from.Mail, from.Password);
                smtp.Send(email);
                smtp.Disconnect(true);
                string information = JsonSerializer.Serialize(d);
                logger.LogInformation(information);
                return true;
            }
            catch (Exception ex)
            {
                // Username and Password not accepted.\ For more information, go to
                // 5.7.8  https://support.google.com/mail/?p=BadCredentials 5b1f17b1804b1-4212b8b39aesm70191195e9.46 - gsmtp
                d.Exc = ex.Message;
                string error = JsonSerializer.Serialize(d);
                logger.LogError(error);
                return false;
            }
        }
    }
}