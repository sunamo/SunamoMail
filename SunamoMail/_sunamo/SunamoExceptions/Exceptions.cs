namespace SunamoMail._sunamo.SunamoExceptions;

/// <summary>
/// Exception handling utilities.
/// </summary>
internal sealed partial class Exceptions
{
    #region Other

    /// <summary>
    /// Gets the text representation of an exception and optionally its inner exceptions.
    /// </summary>
    /// <param name="exception">The exception to format.</param>
    /// <param name="isIncludingInnerExceptions">Whether to include inner exceptions in the output.</param>
    /// <returns>A formatted string containing the exception messages.</returns>
    internal static string TextOfExceptions(Exception exception, bool isIncludingInnerExceptions = true)
    {
        if (exception == null) return string.Empty;
        StringBuilder stringBuilder = new();
        stringBuilder.Append("Exception:");
        stringBuilder.AppendLine(exception.Message);
        if (isIncludingInnerExceptions)
            while (exception.InnerException != null)
            {
                exception = exception.InnerException;
                stringBuilder.AppendLine(exception.Message);
            }
        var result = stringBuilder.ToString();
        return result;
    }

    /// <summary>
    /// Gets the name of the calling method from the stack trace.
    /// </summary>
    /// <param name="stackFrameIndex">The stack frame index to retrieve (default is 1 for immediate caller).</param>
    /// <returns>The name of the calling method, or an error message if unavailable.</returns>
    internal static string CallingMethod(int stackFrameIndex = 1)
    {
        StackTrace stackTrace = new();
        var methodBase = stackTrace.GetFrame(stackFrameIndex)?.GetMethod();
        if (methodBase == null)
        {
            return "Method name cannot be get";
        }
        var methodName = methodBase.Name;
        return methodName;
    }
    #endregion

    #region IsNullOrWhitespace
    internal static readonly StringBuilder SbAdditionalInfoInner = new();
    internal static readonly StringBuilder SbAdditionalInfo = new();
    #endregion
}