namespace SunamoMail._sunamo.SunamoStringSplit;

internal class SHSplit
{
    internal static List<string> Split(string input, params string[] delimiters)
    {
        return input.Split(delimiters, StringSplitOptions.RemoveEmptyEntries).ToList();
    }
}
