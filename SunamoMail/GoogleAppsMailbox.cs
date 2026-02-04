namespace SunamoMail;

/// <summary>
/// Google Apps mailbox for sending emails.
/// Working: saves sent messages to outbox.
/// </summary>
public class GoogleAppsMailbox
{
    private static Type type = typeof(GoogleAppsMailbox);

    /// <summary>
    /// Gets or sets the complete email address configured for sending.
    /// </summary>
    public string? FromEmail { get; set; }

    /// <summary>
    /// Gets or sets the sender name that appears to recipients (does not need to be an email address).
    /// </summary>
    public string? FromName { get; set; }

    /// <summary>
    /// Gets or sets the administrator's email address for notifications.
    /// </summary>
    public string? MailOfAdmin { get; set; }

    /// <summary>
    /// Gets or sets the password for the email account.
    /// </summary>
    public string? Password { get; set; }

    /// <summary>
    /// Gets or sets the SMTP server configuration.
    /// </summary>
    public SmtpServerData SmtpServerData { get; set; } = new();

    /// <summary>
    /// Initializes a new instance for sending from noreply@sunamo.cz.
    /// </summary>
    public GoogleAppsMailbox()
    {

    }

    /// <summary>
    /// Initializes a new instance with the specified email configuration.
    /// </summary>
    /// <param name="fromEmail">The email address to send from.</param>
    /// <param name="mailOfAdmin">The administrator's email address.</param>
    /// <param name="password">The email account password.</param>
    /// <param name="smtpServer">Optional SMTP server configuration.</param>
    public GoogleAppsMailbox(string fromEmail, string mailOfAdmin, string password, SmtpServerData? smtpServer = null) :
        this(string.Empty, fromEmail, mailOfAdmin, password, smtpServer)
    {
    }

    /// <summary>
    /// Initializes a new instance with full email configuration.
    /// Can be null, used to send mails to webmaster.
    /// </summary>
    /// <param name="fromName">The sender's display name.</param>
    /// <param name="fromEmail">The email address to send from.</param>
    /// <param name="mailOfAdmin">The administrator's email address.</param>
    /// <param name="password">The email account password.</param>
    /// <param name="smtpServer">Optional SMTP server configuration.</param>
    public GoogleAppsMailbox(string fromName, string fromEmail, string mailOfAdmin, string password,
        SmtpServerData? smtpServer = null)
    {
        this.FromName = fromName;
        this.FromEmail = fromEmail;
        this.MailOfAdmin = mailOfAdmin;
        this.Password = password;

        if (smtpServer != null) SmtpServerData = smtpServer;
    }

    /// <summary>
    /// Sends an email message.
    /// Returns either "success" or a message starting with "error:".
    /// Multiple addresses can be specified in to, cc, bcc separated by semicolons.
    /// Set replyTo to empty string to use the from address as reply-to.
    /// Use empty string for unused parameters, not null.
    /// </summary>
    /// <param name="to">Recipient email address(es), semicolon-separated for multiple.</param>
    /// <param name="cc">Carbon copy email address(es), semicolon-separated for multiple.</param>
    /// <param name="bcc">Blind carbon copy email address(es), semicolon-separated for multiple.</param>
    /// <param name="replyTo">Reply-to email address, or empty string to use the from address.</param>
    /// <param name="subject">Email subject line.</param>
    /// <param name="body">Email body content.</param>
    /// <param name="isBodyHtml">Whether the body content is HTML formatted.</param>
    /// <param name="attachments">Optional file paths to attach to the email.</param>
    /// <returns>Returns "success" on successful send, or "error: [message]" on failure.</returns>
    public string SendEmail(string to, string cc, string bcc, string replyTo, string subject, string body,
        bool isBodyHtml, params string[] attachments)
    {
        to = to.Trim();

        var emailStatus = string.Empty;

        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

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
            mail = null;
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
