namespace SunamoMail.Services;

public partial class MailSenderService(ILogger logger)
{
    // Centrum now requires SMS verification code for each email sent via SMTP,
    // making this method impractical for automated sending.
    // After one email succeeds, subsequent emails will be blocked.
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
