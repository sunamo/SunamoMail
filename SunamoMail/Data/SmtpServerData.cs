namespace SunamoMail.Data;

/// <summary>
/// SMTP server configuration data.
/// </summary>
public class SmtpServerData
{
    /// <summary>
    /// Gets or sets the SMTP server hostname.
    /// </summary>
    public string SmtpServer { get; set; } = "smtp.gmail.com";

    /// <summary>
    /// Gets or sets the SMTP server port.
    /// </summary>
    public int Port { get; set; } = 587;

    /// <summary>
    /// Creates SMTP server configuration for Gmail.
    /// </summary>
    /// <returns>SMTP configuration for Gmail (smtp.gmail.com:587).</returns>
    public static SmtpServerData Gmail()
    {
        var serverData = new SmtpServerData();
        serverData.Port = 587;
        serverData.SmtpServer = "smtp.gmail.com";
        return serverData;
    }

    /// <summary>
    /// Creates SMTP server configuration for Seznam.cz.
    /// </summary>
    /// <returns>SMTP configuration for Seznam.cz (smtp.seznam.cz:25).</returns>
    public static SmtpServerData SeznamCz()
    {
        var serverData = new SmtpServerData();
        serverData.Port = 25;
        serverData.SmtpServer = "smtp.seznam.cz";
        return serverData;
    }
}