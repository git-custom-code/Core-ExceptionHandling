namespace CustomCode.Core.ExceptionHandling
{
    /// <summary>
    /// Interface for exceptions that are caused by technical reasons.
    /// </summary>
    /// <remarks>
    /// Note that c# does not support multiple inheritance and does not allow using catch with interfaces.
    /// But starting from C# 6.0 you can use exception filtering to achive the same result.
    /// 
    /// try
    /// {
    ///     ...
    /// }
    /// catch(Exception e) when (e is ITechnicalException te)
    /// {
    ///     ...
    /// }
    /// </remarks>
    public interface ITechnicalException : ILocalizableException
    { }
}