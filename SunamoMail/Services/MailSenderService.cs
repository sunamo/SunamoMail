// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
namespace SunamoMail.Services;

public partial class MailSenderService(ILogger logger)
{
    /// <summary>
    /// Přes centrum to už vůbec nejde - pro každý mail poslaný přes SMTP server mi to vrátí kód, který musím poslat v PremiumSMS
    /// Když jeden mail takto projde, další se zastaví
    /// </summary>
    /// <param name="to"></param>
    /// <param name="subject"></param>
    /// <param name="body"></param>
    /// <returns></returns>
    public bool SendCentrum(int attemps, From from, string to, MailMessage mailMessage)
    {
        // Required, otherwise https://github.com/jstedfast/MailKit/issues/488#issuecomment-292989711
        to = to.Trim();

        for (int i = 0; i < attemps; i++)
        {

            try
            {
                var smtpClient = new SmtpClient("smtp.centrum.cz")
                {
                    Port = 587, // Port pro SMTP s TLS
                    EnableSsl = true, // Povolení zabezpečení TLS
                    UseDefaultCredentials = false, // Nepoužívat výchozí přihlašovací údaje Windows
                    Credentials = new NetworkCredential(from.Mail, from.Password)
                };
                mailMessage.From = new MailAddress(from.Mail);
                // Vytvoření e-mailové zprávy
                //var mailMessage = new MailMessage
                //{
                //    From = new MailAddress(mailFrom),
                //    Subject = subject,
                //    Body = body,
                //    IsBodyHtml = false // Nastavení, zda je tělo zprávy ve formátu HTML (v tomto případě ne)
                //};
                mailMessage.To.Add(to); // Přidání příjemce
                                        // Odeslání e-mailu
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
    /// Zde se mi to zasekne na smtpClient.Send
    /// The operation has timed out.
    /// 
    /// Otázka je čím to je.
    /// MOhl bych si stáhnout mail klienta a zkusit v něm odeslat mail https://g.co/gemini/share/1af309732d3b
    /// 
    /// Možná ale kdybych používal seznam z email profi tak takové omezení tam nebude.
    /// </summary>
    /// <param name="to"></param>
    /// <param name="subject"></param>
    /// <param name="body"></param>
    /// <returns></returns>
    public bool SendSeznam(int attemps, From from, string to, MailMessage mailMessage)
    {
        // Required, otherwise https://github.com/jstedfast/MailKit/issues/488#issuecomment-292989711
        to = to.Trim();

        for (int i = 0; i < attemps; i++)
        {
            try
            {
                // Nastavení SMTP serveru Seznam.cz
                SmtpClient smtpClient = new SmtpClient("smtp.seznam.cz", 465);
                // Povolení SSL šifrování (vyžadováno Seznamem)
                smtpClient.EnableSsl = true;
                // Přihlašovací údaje k e-mailovému účtu
                smtpClient.Credentials = new NetworkCredential(from.Mail, from.Password);
                // Vytvoření zprávy
                mailMessage.To.Add(to);
                //MailMessage mailMessage = new MailMessage();
                //mailMessage.From = new MailAddress(mailFrom);
                //mailMessage.To.Add(to); // Můžete přidat více příjemců
                //mailMessage.Subject = subject;
                //mailMessage.Body = body;
                // Odeslání zprávy
                smtpClient.Send(mailMessage);
                Console.WriteLine("E-mail odeslán úspěšně.");
                return true;
            }
            catch (Exception ex)
            {
                // Zde se mi to zasekne na smtpClient.Send
                Console.WriteLine("Chyba při odesílání e-mailu: " + ex.Message);

            }
        }
        return false;
    }
}