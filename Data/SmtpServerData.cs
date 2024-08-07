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
        var s = new SmtpServerData();
        s.port = 587;
        s.smtpServer = "smtp.gmail.com";
        return s;
    }

    public static SmtpServerData SeznamCz()
    {
        var s = new SmtpServerData();
        s.port = 25;
        s.smtpServer = "smtp.seznam.cz";
        return s;
    }
}