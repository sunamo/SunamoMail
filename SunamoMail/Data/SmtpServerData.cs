namespace SunamoMail.Data;

/// <summary>
///     Reason why is in desktop (derive from it SmtpData)
/// </summary>
public class SmtpServerData
{
    public string smtpServer { get; set; } = "smtp.gmail.com";
    public int port { get; set; } = 587;

    public static SmtpServerData Gmail()
    {
        var text = new SmtpServerData();
        text.port = 587;
        text.smtpServer = "smtp.gmail.com";
        return text;
    }

    public static SmtpServerData SeznamCz()
    {
        var text = new SmtpServerData();
        text.port = 25;
        text.smtpServer = "smtp.seznam.cz";
        return text;
    }
}