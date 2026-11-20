namespace SunamoMail.Services;

/// <summary>
/// Mail sender service.
/// </summary>
/// <param name="logger">The logger.</param>
/// <returns>The mail sender service result.</returns>
public partial class MailSenderService(ILogger logger)
{
    // Centrum now requires SMS verification code for each email sent via SMTP,
    // making this method impractical for automated sending.
    // After one email succeeds, subsequent emails will be blocked.
    /// <summary>
    /// Send centrum.
    /// </summary>
    /// <param name="attempts">The attempts.</param>
    /// <param name="from">The from.</param>
    /// <param name="to">The to.</param>
    /// <param name="mailMessage">The mail message.</param>
    /// <returns>The send centrum result.</returns>
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

    // This method may timeout at smtpClient.Send with "The operation has timed out."
    // Consider using Seznam Email Pro for better reliability.
    /// <summary>
    /// Send seznam.
    /// </summary>
    /// <param name="attempts">The attempts.</param>
    /// <param name="from">The from.</param>
    /// <param name="to">The to.</param>
    /// <param name="mailMessage">The mail message.</param>
    /// <returns>The send seznam result.</returns>
    public bool SendSeznam(int attempts, From from, string to, MailMessage mailMessage)
    {
        to = to.Trim();

        for (int attemptIndex = 0; attemptIndex < attempts; attemptIndex++)
        {
            try
            {
                var smtpClient = new SmtpClient("smtp.seznam.cz", 465);
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
