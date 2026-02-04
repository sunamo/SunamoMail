namespace SunamoMail.Services;

/// <summary>
/// Email sending service supporting multiple providers.
/// </summary>
public partial class MailSenderService(ILogger logger)
{
    /// <summary>
    /// Sends email via Centrum.cz SMTP server.
    /// Note: Centrum now requires SMS verification code for each email sent via SMTP,
    /// making this method impractical for automated sending.
    /// After one email succeeds, subsequent emails will be blocked.
    /// </summary>
    /// <param name="attempts">Number of send attempts to make before giving up.</param>
    /// <param name="from">Sender credentials (name, email, password).</param>
    /// <param name="to">Recipient email address.</param>
    /// <param name="mailMessage">Pre-configured mail message to send.</param>
    /// <returns>True if email was sent successfully, false otherwise.</returns>
    public bool SendCentrum(int attempts, From from, string to, MailMessage mailMessage)
    {
        to = to.Trim();

        for (int attemptIndex = 0; attemptIndex < attempts; attemptIndex++)
        {

            try
            {
                var smtpClient = new SmtpClient("smtp.centrum.cz")
                {
                    Port = 587,
                    EnableSsl = true,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(from.Mail, from.Password)
                };
                mailMessage.From = new MailAddress(from.Mail);

                mailMessage.To.Add(to);

                smtpClient.Send(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }


        }
        return false;
    }

    /// <summary>
    /// Sends email via Seznam.cz SMTP server.
    /// Note: This method may timeout at smtpClient.Send with "The operation has timed out."
    /// Consider using Seznam Email Pro for better reliability.
    /// Alternatively, try using a desktop email client to verify SMTP settings.
    /// </summary>
    /// <param name="attempts">Number of send attempts to make before giving up.</param>
    /// <param name="from">Sender credentials (name, email, password).</param>
    /// <param name="to">Recipient email address.</param>
    /// <param name="mailMessage">Pre-configured mail message to send.</param>
    /// <returns>True if email was sent successfully, false otherwise.</returns>
    public bool SendSeznam(int attempts, From from, string to, MailMessage mailMessage)
    {
        to = to.Trim();

        for (int attemptIndex = 0; attemptIndex < attempts; attemptIndex++)
        {
            try
            {
                SmtpClient smtpClient = new SmtpClient("smtp.seznam.cz", 465);
                smtpClient.EnableSsl = true;
                smtpClient.Credentials = new NetworkCredential(from.Mail, from.Password);

                mailMessage.To.Add(to);

                smtpClient.Send(mailMessage);
                Console.WriteLine("Email sent successfully.");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error sending email: " + ex.Message);

            }
        }
        return false;
    }
}
