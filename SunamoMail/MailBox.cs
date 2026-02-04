namespace SunamoMail;

/// <summary>
/// Static mailbox wrapper for sending emails via Google Apps mailbox.
/// </summary>
public class MailBox
{
    #region Shared mailbox instance

    /// <summary>
    /// Gets or sets the shared Google Apps mailbox instance.
    /// </summary>
    public static GoogleAppsMailbox? Mailbox = null;

    /// <summary>
    /// Sends an email using the first recipient address as reply-to.
    /// </summary>
    /// <param name="to">Recipient email address(es), semicolon-separated for multiple.</param>
    /// <param name="cc">Carbon copy email address(es), semicolon-separated for multiple.</param>
    /// <param name="bcc">Blind carbon copy email address(es), semicolon-separated for multiple.</param>
    /// <param name="isUsingFirstRecipientAsReplyTo">Whether to use the first recipient as the reply-to address.</param>
    /// <param name="subject">Email subject line.</param>
    /// <param name="htmlBody">HTML-formatted email body content.</param>
    /// <param name="attachments">Optional file paths to attach to the email.</param>
    /// <returns>Returns "success" on successful send, or "error: [message]" on failure.</returns>
    public static string SendEmail(string to, string cc, string bcc, bool isUsingFirstRecipientAsReplyTo, string subject, string htmlBody,
        params string[] attachments)
    {
        var replyTo = "";
        if (isUsingFirstRecipientAsReplyTo) replyTo = to;

        return Mailbox.SendEmail(to, cc, bcc, replyTo, subject, htmlBody, true, attachments);
    }

    /// <summary>
    /// Sends an HTML email with explicit reply-to address.
    /// </summary>
    /// <param name="to">Recipient email address(es), semicolon-separated for multiple.</param>
    /// <param name="cc">Carbon copy email address(es), semicolon-separated for multiple.</param>
    /// <param name="bcc">Blind carbon copy email address(es), semicolon-separated for multiple.</param>
    /// <param name="replyTo">Reply-to email address, or empty string to use the from address.</param>
    /// <param name="subject">Email subject line.</param>
    /// <param name="htmlBody">HTML-formatted email body content.</param>
    /// <param name="attachments">Optional file paths to attach to the email.</param>
    /// <returns>Returns "success" on successful send, or "error: [message]" on failure.</returns>
    public static string SendEmail(string to, string cc, string bcc, string replyTo, string subject, string htmlBody,
        params string[] attachments)
    {
        return Mailbox.SendEmail(to, cc, bcc, replyTo, subject, htmlBody, true, attachments);
    }

    #endregion
}
