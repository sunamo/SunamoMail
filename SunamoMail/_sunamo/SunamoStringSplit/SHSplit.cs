namespace SunamoMail._sunamo.SunamoStringSplit;

/// <summary>
/// String helper for splitting operations.
/// </summary>
internal class SHSplit
{
    /// <summary>
    /// Splits a string by the specified delimiters and removes empty entries.
    /// </summary>
    /// <param name="input">The string to split.</param>
    /// <param name="delimiters">The delimiters to split by.</param>
    /// <returns>A list of non-empty substrings.</returns>
    internal static List<string> Split(string input, params string[] delimiters)
    {
        return input.Split(delimiters, StringSplitOptions.RemoveEmptyEntries).ToList();
    }
}