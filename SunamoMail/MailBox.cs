namespace SunamoMail;

public class MailBox
{
    #region Shared mailbox instance

    public static GoogleAppsMailbox? Mailbox { get; set; }

    public static string SendEmail(string to, string cc, string bcc, bool isUsingFirstRecipientAsReplyTo, string subject, string htmlBody,
        params string[] attachments)
    {
        if (Mailbox is null)
            throw new InvalidOperationException("Mailbox must be initialized before sending emails.");

        var replyTo = "";
        if (isUsingFirstRecipientAsReplyTo) replyTo = to;

        return Mailbox.SendEmail(to, cc, bcc, replyTo, subject, htmlBody, true, attachments);
    }

    public static string SendEmail(string to, string cc, string bcc, string replyTo, string subject, string htmlBody,
        params string[] attachments)
    {
        if (Mailbox is null)
            throw new InvalidOperationException("Mailbox must be initialized before sending emails.");

        return Mailbox.SendEmail(to, cc, bcc, replyTo, subject, htmlBody, true, attachments);
    }

    #endregion
}
