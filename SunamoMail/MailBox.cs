namespace SunamoMail;

/// <summary>
/// Represents the mail box type.
/// </summary>
public class MailBox
{
    #region Shared mailbox instance

    /// <summary>
    /// Gets or sets the mailbox.
    /// </summary>
    public static GoogleAppsMailbox? Mailbox { get; set; }

    /// <summary>
    /// Send email.
    /// </summary>
    /// <param name="to">The to.</param>
    /// <param name="cc">The cc.</param>
    /// <param name="bcc">The bcc.</param>
    /// <param name="isUsingFirstRecipientAsReplyTo">The is using first recipient as reply to.</param>
    /// <param name="subject">The subject.</param>
    /// <param name="htmlBody">The html body.</param>
    /// <param name="attachments">The attachments.</param>
    /// <returns>The send email result.</returns>
    public static string SendEmail(string to, string cc, string bcc, bool isUsingFirstRecipientAsReplyTo, string subject, string htmlBody,
        params string[] attachments)
    {
        if (Mailbox is null)
            throw new InvalidOperationException("Mailbox must be initialized before sending emails.");

        var replyTo = "";
        if (isUsingFirstRecipientAsReplyTo) replyTo = to;

        return Mailbox.SendEmail(to, cc, bcc, replyTo, subject, htmlBody, true, attachments);
    }

    /// <summary>
    /// Send email.
    /// </summary>
    /// <param name="to">The to.</param>
    /// <param name="cc">The cc.</param>
    /// <param name="bcc">The bcc.</param>
    /// <param name="replyTo">The reply to.</param>
    /// <param name="subject">The subject.</param>
    /// <param name="htmlBody">The html body.</param>
    /// <param name="true">The true.</param>
    /// <param name="attachments">The attachments.</param>
    /// <returns>The send email result.</returns>
    public static string SendEmail(string to, string cc, string bcc, string replyTo, string subject, string htmlBody,
        params string[] attachments)
    {
        if (Mailbox is null)
            throw new InvalidOperationException("Mailbox must be initialized before sending emails.");

        return Mailbox.SendEmail(to, cc, bcc, replyTo, subject, htmlBody, true, attachments);
    }

    #endregion
}
