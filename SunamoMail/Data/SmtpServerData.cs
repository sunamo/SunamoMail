namespace SunamoMail.Data;

public class SmtpServerData
{
    public string SmtpServer { get; set; } = "smtp.gmail.com";

    public int Port { get; set; } = 587;

    public static SmtpServerData Gmail()
    {
        var serverData = new SmtpServerData();
        serverData.Port = 587;
        serverData.SmtpServer = "smtp.gmail.com";
        return serverData;
    }

    public static SmtpServerData SeznamCz()
    {
        var serverData = new SmtpServerData();
        serverData.Port = 25;
        serverData.SmtpServer = "smtp.seznam.cz";
        return serverData;
    }
}
