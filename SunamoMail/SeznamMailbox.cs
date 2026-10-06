namespace SunamoMail;

public class SeznamMailbox
{
    private readonly string? password;

    private readonly SmtpServerData smtpServerData = new();

    public string? FromEmail { get; set; }

    public string? FromName { get; set; }

    public string? MailOfAdmin { get; set; }

    public SeznamMailbox()
    {
    }

    public SeznamMailbox(string fromName, string fromEmail, string mailOfAdmin, string password,
        SmtpServerData? smtpServer = null)
    {
        this.FromName = fromName;
        this.FromEmail = fromEmail;
        this.MailOfAdmin = mailOfAdmin;
        this.password = password;
        if (smtpServer is not null) smtpServerData = smtpServer;
    }

    public string SendEmail(int attempts, string to, string cc, string bcc, string replyTo, string subject, string body,
        bool isBodyHtml, params string[] attachments)
    {
        to = to.Trim();

        var emailStatus = string.Empty;

        if (string.IsNullOrEmpty(FromEmail))
            return "error: FromEmail is not configured.";

        var client = new SmtpClient();
        client.EnableSsl = true;
        client.UseDefaultCredentials = false;
        client.Credentials = new NetworkCredential(FromEmail, password);
        client.Port = smtpServerData.Port;
        client.Host = smtpServerData.SmtpServer;

        var mail = new MailMessage();

        var mailAddress = new MailAddress(FromEmail, FromName);
        mail.From = mailAddress;
        if (replyTo != "")
        {
            var replyToAddress = new MailAddress(replyTo, replyTo);
            mail.ReplyToList.Add(replyToAddress);
        }
        else
        {
            mail.ReplyToList.Add(mailAddress);
        }

        mail.Sender = mailAddress;

        #region Recipient

        if (to.Contains(";"))
        {
            var emailsTo = SHSplit.Split(to, ";");
            for (var i = 0; i < emailsTo.Count; i++)
                if (!string.IsNullOrWhiteSpace(emailsTo[i]))
                    mail.To.Add(new MailAddress(emailsTo[i]));
            if (mail.To.Count == 0)
            {
                emailStatus = "error: No primary recipient was specified.";
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
                emailStatus = "error: No primary recipient was specified.";
                return emailStatus;
            }
        }

        #endregion

        #region Carbon copy

        if (cc.Contains(";"))
        {
            var emailsCc = SHSplit.Split(cc, ";");
            for (var i = 0; i < emailsCc.Count; i++)
                if (!string.IsNullOrWhiteSpace(emailsCc[i]))
                    mail.CC.Add(new MailAddress(emailsCc[i]));
        }
        else
        {
            if (!string.IsNullOrWhiteSpace(cc)) mail.CC.Add(new MailAddress(cc));
        }

        #endregion

        #region Blind Carbon copy

        if (bcc.Contains(";"))
        {
            var emailsBcc = SHSplit.Split(bcc, ";");
            for (var i = 0; i < emailsBcc.Count; i++) mail.Bcc.Add(new MailAddress(emailsBcc[i]));
        }
        else
        {
            if (!string.IsNullOrWhiteSpace(bcc)) mail.Bcc.Add(new MailAddress(bcc));
        }

        #endregion

        mail.Subject = subject;
        mail.Body = body;
        mail.IsBodyHtml = isBodyHtml;

        foreach (var attachmentPath in attachments)
            if (File.Exists(attachmentPath))
                mail.Attachments.Add(new Attachment(attachmentPath));

        for (int attemptIndex = 0; attemptIndex < attempts; attemptIndex++)
        {
            try
            {
                client.Send(mail);
                mail.Dispose();
                emailStatus = "success";
                break;
            }
            catch (Exception ex)
            {
                emailStatus = "error: ";
                if (ex.Message is not null) emailStatus += ex.Message + ". ";
            }
        }

        return emailStatus;
    }
}
