namespace SunamoMail;

/// <summary>
/// Represents the google apps mailbox type.
/// </summary>
public class GoogleAppsMailbox
{
    /// <summary>
    /// Gets or sets the from email.
    /// </summary>
    public string? FromEmail { get; set; }

    /// <summary>
    /// Gets or sets the from name.
    /// </summary>
    public string? FromName { get; set; }

    /// <summary>
    /// Gets or sets the mail of admin.
    /// </summary>
    public string? MailOfAdmin { get; set; }

    /// <summary>
    /// Gets or sets the password.
    /// </summary>
    public string? Password { get; set; }

    /// <summary>
    /// Gets or sets the smtp server data.
    /// </summary>
    public SmtpServerData SmtpServerData { get; set; } = new();

    /// <summary>
    /// Initializes a new instance of the google apps mailbox class.
    /// </summary>
    public GoogleAppsMailbox()
    {

    }

    /// <summary>
    /// Google apps mailbox.
    /// </summary>
    /// <returns>The google apps mailbox result.</returns>
    public GoogleAppsMailbox(string fromEmail, string mailOfAdmin, string password, SmtpServerData? smtpServer = null) :
        this(string.Empty, fromEmail, mailOfAdmin, password, smtpServer)
    {
    }

    /// <summary>
    /// Google apps mailbox.
    /// </summary>
    /// <returns>The google apps mailbox result.</returns>
    public GoogleAppsMailbox(string fromName, string fromEmail, string mailOfAdmin, string password,
        SmtpServerData? smtpServer = null)
    {
        this.FromName = fromName;
        this.FromEmail = fromEmail;
        this.MailOfAdmin = mailOfAdmin;
        this.Password = password;

        if (smtpServer is not null) SmtpServerData = smtpServer;
    }

    /// <summary>
    /// Initializes a new instance of the send email class.
    /// </summary>
    /// <param name="string.Empty">The string empty.</param>
    /// <param name="fromEmail">The from email.</param>
    /// <param name="mailOfAdmin">The mail of admin.</param>
    /// <param name="password">The password.</param>
    /// <param name="smtpServer">The smtp server.</param>
    public string SendEmail(string to, string cc, string bcc, string replyTo, string subject, string body,
        bool isBodyHtml, params string[] attachments)
    {
        to = to.Trim();

        var emailStatus = string.Empty;

        if (string.IsNullOrEmpty(FromEmail))
            return "error: FromEmail is not configured.";

        var client = new SmtpClient();
        client.EnableSsl = true;

        client.UseDefaultCredentials = false;
        client.Credentials = new NetworkCredential(FromEmail, Password);
        client.Port = SmtpServerData.Port;
        client.Host = SmtpServerData.SmtpServer;

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

        try
        {
            client.Send(mail);
            mail.Dispose();
            emailStatus = "success";
        }
        catch (Exception ex)
        {
            emailStatus = "error: " + Exceptions.TextOfExceptions(ex);
            throw new Exception(Exceptions.CallingMethod());
        }

        return emailStatus;
    }
}
