using SunamoMail.Data;

namespace SunamoMail.Tests;

/// <summary>
/// Tests for the predefined SMTP servers.
/// </summary>
public class SmtpServerDataTests
{
    /// <summary>
    /// Tests that Seznam.cz uses the submission port 587.
    /// </summary>
    [Fact]
    public void SeznamCzUsesSubmissionPortTest()
    {
        var data = SmtpServerData.SeznamCz();
        Assert.Equal("smtp.seznam.cz", data.SmtpServer);
        Assert.Equal(587, data.Port);
    }

    /// <summary>
    /// Tests that Gmail uses the submission port 587.
    /// </summary>
    [Fact]
    public void GmailUsesSubmissionPortTest()
    {
        var data = SmtpServerData.Gmail();
        Assert.Equal("smtp.gmail.com", data.SmtpServer);
        Assert.Equal(587, data.Port);
    }
}
