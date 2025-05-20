namespace SunamoMail;

/// <summary>
///     Google: working, save sent do outbox
///     Seznam: working, DONT save sent to outbox
/// </summary>
public class SeznamMailbox
{
    /// <summary>
    ///     Povinný. Heslo k mailu userName, které se taktéž nastavuje na https://ks.aspone.cz/
    /// </summary>
    private readonly string password;

    private readonly SmtpServerData smtpServerData = new();

    /// <summary>
    ///     Povinný. Celá adresa emailu který jste si nastavili na https://ks.aspone.cz/
    /// </summary>
    public string fromEmail;

    /// <summary>
    ///     Řetězec, který se objeví u příjemce jako odesílatel. Nemusí to být mailová adresa.
    /// </summary>
    public string fromName;

    public string mailOfAdmin;

    /// <summary>
    ///     For sending from noreply@sunamo.cz
    /// </summary>
    public SeznamMailbox()
    {
    }

    /// <summary>
    ///     Do A3 se ve výchozí stavu předává GeneralCells.EmailOfUser(1). Can be null, its used in scz to send mails to
    ///     webmaster
    ///     Dont forget set password for A2 or use without-parametric ctor
    /// </summary>
    /// <param name="fromName"></param>
    /// <param name="fromEmail"></param>
    /// <param name="mailOfAdmin"></param>
    public SeznamMailbox(string fromName, string fromEmail, string mailOfAdmin, string password,
        SmtpServerData smtpServer = null)
    {
        this.fromName = fromName;
        this.fromEmail = fromEmail;
        this.mailOfAdmin = mailOfAdmin;
        this.password = password;
        if (smtpServer != null) smtpServerData = smtpServer;
    }

    /// <summary>
    ///     Do A1, A2, A3 se může zadat více adres, stačí je oddělit středníkem
    ///     A4 nastav na "", pokud chceš použít jako reply-to adresu A1
    /// </summary>
    /// <param name="to"></param>
    /// <param name="cc"></param>
    /// <param name="bcc"></param>
    /// <param name="subject"></param>
    /// <param name="htmlBody"></param>
    /// <param name="attachments"></param>
    public string SendEmail(int attempts, string to, string cc, string bcc, string replyTo, string subject, string body,
        bool htmlBody, params string[] attachments)
    {
        // Required, otherwise https://github.com/jstedfast/MailKit/issues/488#issuecomment-292989711
        to = to.Trim();

        var emailStatus = string.Empty;

        var client = new SmtpClient();
        client.EnableSsl =
            true; //Mail aspone nefunguje na SSL zatím, pokud byste zde dali true, tak vám vznikne výjimka se zprávou Server does not support secure connections.
        client.UseDefaultCredentials = false; // must be before set up Credentials
        client.Credentials = new NetworkCredential(fromEmail, password);
        client.Port = smtpServerData.port; //Fungovalo mi to když jsem žádný port nezadal a jelo mi to na výchozím
        client.Host =
            smtpServerData
                .smtpServer; //Adresa smtp serveru. Může končit buď na název vašeho webu nebo na aspone.cz. Zadává se bez protokolu, jak je zvykem

        var mail = new MailMessage();

        var ma = new MailAddress(fromEmail, fromName);
        mail.From = ma;
        if (replyTo != "")
        {
            var ma2 = new MailAddress(replyTo, replyTo);
            mail.ReplyToList.Add(ma2);
        }
        else
        {
            mail.ReplyToList.Add(ma);
        }

        mail.Sender = ma;

        #region Recipient

        if (to.Contains(";"))
        {
            var _EmailsTO = SHSplit.Split(to, ";");
            for (var i = 0; i < _EmailsTO.Count; i++)
                if (!string.IsNullOrWhiteSpace(_EmailsTO[i]))
                    mail.To.Add(new MailAddress(_EmailsTO[i]));
            if (mail.To.Count == 0)
            {
                emailStatus = "error: Nebyl zadán primární příjemce zprávy. ";
                return emailStatus;
            }
        }
        else
        {
            if (!string.IsNullOrWhiteSpace(to))
            {
                mail.To.Add(new MailAddress(to));
            }
            else
            {
                emailStatus = "error: Nebyl zadán primární příjemce zprávy. ";
                return emailStatus;
            }
        }

        #endregion

        #region Carbon copy

        if (cc.Contains(";"))
        {
            var _EmailsCC = SHSplit.Split(cc, ";");
            for (var i = 0; i < _EmailsCC.Count; i++)
                if (!string.IsNullOrWhiteSpace(_EmailsCC[i]))
                    mail.CC.Add(new MailAddress(_EmailsCC[i]));
        }
        else
        {
            if (!string.IsNullOrWhiteSpace(cc)) mail.CC.Add(new MailAddress(cc));
            // Neděje se nic, prostě uživatel nic nezadal
        }

        #endregion

        #region Blind Carbon copy

        //BCC
        if (bcc.Contains(";"))
        {
            var _EmailsBCC = SHSplit.Split(bcc, ";");
            for (var i = 0; i < _EmailsBCC.Count; i++) mail.Bcc.Add(new MailAddress(_EmailsBCC[i]));
        }
        else
        {
            if (!string.IsNullOrWhiteSpace(bcc)) mail.Bcc.Add(new MailAddress(bcc));
        }

        #endregion

        mail.Subject = subject;
        mail.Body = body;
        mail.IsBodyHtml = htmlBody;

        foreach (var item in attachments)
            if (File.Exists(item))
                mail.Attachments.Add(new Attachment(item));

        for (int i = 0; i < attempts; i++)
        {
            try
            {
                client.Send(mail);
                mail.Dispose();
                mail = null;
                emailStatus = "success";
                break;
            }
            catch (Exception ex)
            {
                emailStatus = "error: ";
                if (ex.Message != null) emailStatus += ex.Message + ". ";
            }
        }

        return emailStatus;
    }
}