namespace SunamoMail.Data;

/// <summary>
/// Represents the smtp server data type.
/// </summary>
public class SmtpServerData
{
    /// <summary>
    /// Gets or sets the smtp server.
    /// </summary>
    public string SmtpServer { get; set; } = "smtp.gmail.com";

    /// <summary>
    /// Gets or sets the port.
    /// </summary>
    public int Port { get; set; } = 587;

    /// <summary>
    /// Gmail.
    /// </summary>
    /// <returns>The gmail result.</returns>
    public static SmtpServerData Gmail()
    {
        var serverData = new SmtpServerData();
        serverData.Port = 587;
        serverData.SmtpServer = "smtp.gmail.com";
        return serverData;
    }

    /// <summary>
    /// Seznam cz.
    /// </summary>
    /// <returns>The seznam cz result.</returns>
    public static SmtpServerData SeznamCz()
    {
        var serverData = new SmtpServerData();
        serverData.Port = 25;
        serverData.SmtpServer = "smtp.seznam.cz";
        return serverData;
    }
}
