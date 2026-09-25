namespace SunamoMail._sunamo.SunamoExceptions;

internal sealed partial class Exceptions
{
    #region Other

    internal static string TextOfExceptions(Exception exception, bool isIncludingInnerExceptions = true)
    {
        if (exception is null) return string.Empty;
        StringBuilder stringBuilder = new();
        stringBuilder.Append("Exception:");
        stringBuilder.AppendLine(exception.Message);
        if (isIncludingInnerExceptions)
            while (exception.InnerException is not null)
            {
                exception = exception.InnerException;
                stringBuilder.AppendLine(exception.Message);
            }
        var result = stringBuilder.ToString();
        return result;
    }

    internal static string CallingMethod(int stackFrameIndex = 1)
    {
        StackTrace stackTrace = new();
        var methodBase = stackTrace.GetFrame(stackFrameIndex)?.GetMethod();
        if (methodBase is null)
        {
            return "Method name cannot be get";
        }
        var methodName = methodBase.Name;
        return methodName;
    }
    #endregion

}
