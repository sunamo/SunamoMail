namespace SunamoMail.Data;

/// <summary>
/// SMTP server configuration together with login credentials for one configured mail account.
/// </summary>
public class SmtpAccountData : SmtpServerData
{
    /// <summary>
    /// Gets or sets the login (user name) used to authenticate against the SMTP server.
    /// </summary>
    public string Login { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the password used to authenticate against the SMTP server.
    /// </summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets whether this account is the default one used when no other account is selected.
    /// </summary>
    public bool IsDefault { get; set; }

    /// <summary>
    /// Fills this instance from a serialized row produced by <see cref="SmtpAccountDataCollection.ToRows"/>.
    /// </summary>
    /// <param name="row">Row with values in the order: server, port, login, password, isDefault.</param>
    public void FromRow(IReadOnlyList<string> row)
    {
        SmtpServer = row[0];
        Port = int.Parse(row[1]);
        Login = row[2];
        Password = row[3];
        IsDefault = row[4] == "1";
    }
}
