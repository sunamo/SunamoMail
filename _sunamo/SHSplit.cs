using System.Linq;

namespace SunamoMail;
public class SHSplit
{
    public static List<string> Split(string p, params string[] newLine)
    {
        return p.Split(newLine, StringSplitOptions.RemoveEmptyEntries).ToList();
    }


}
