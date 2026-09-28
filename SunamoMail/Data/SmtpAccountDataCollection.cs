namespace SunamoMail.Data;

/// <summary>
/// Collection of configured SMTP accounts (server, port and credentials), one of which may be the default.
/// </summary>
public class SmtpAccountDataCollection : Collection<SmtpAccountData>
{
    /// <summary>
    /// Serializes every account to a row (server, port, login, password, isDefault) suitable for persistence.
    /// </summary>
    /// <returns>A list of rows, one per account.</returns>
    public List<List<string>> ToRows()
    {
        var rows = new List<List<string>>(Count);
        foreach (var account in this)
        {
            rows.Add(
            [
                account.SmtpServer,
                account.Port.ToString(),
                account.Login,
                account.Password,
                account.IsDefault ? "1" : "0"
            ]);
        }

        return rows;
    }

    /// <summary>
    /// Gets the account marked as default, or null if none is marked.
    /// </summary>
    /// <returns>The default account, or null.</returns>
    public SmtpAccountData? GetDefault()
    {
        return this.FirstOrDefault(account => account.IsDefault);
    }
}
