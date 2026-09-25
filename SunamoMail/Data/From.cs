namespace SunamoMail.Data;

/// <summary>
/// Email sender credentials record.
/// </summary>
/// <param name="Name">Display name of the sender.</param>
/// <param name="Mail">Email address of the sender.</param>
/// <param name="Password">Password for the email account.</param>
public record struct From(string Name, string Mail, string Password)
{
}