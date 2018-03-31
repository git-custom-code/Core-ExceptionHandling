namespace CustomCode.Core.ExceptionHandling
{
    using System.Text;

    /// <summary>
    /// This type encapsulates the following information for an exception:
    /// - The name of the method that has raised the associated exception.
    /// - The name of the type that has raised the associated exception.
    /// - The name of the sourcecode file where the associated exception was raised.
    /// - The line number inside of the sourcecode file where the associated exception was raised.
    /// </summary>
    public sealed class ExceptionContext
    {
        #region Dependencies

        /// <summary>
        /// Creates a new instance of the <see cref="ExceptionContext"/> type.
        /// </summary>
        /// <param name="methodName"> The name of the method that has raised the associated exception. </param>
        /// <param name="typeName"> The name of the type that has raised the associated exception. </param>
        /// <param name="fileName"> The name of the sourcecode file where the associated exception was raised. </param>
        /// <param name="lineNumber"> The line number inside of the sourcecode file where the associated exception was raised. </param>
        public ExceptionContext(string methodName, string typeName, string fileName = null, uint? lineNumber = null)
        {
            MethodName = methodName;
            TypeName = typeName;
            FileName = fileName;
            LineNumber = lineNumber;
        }

        #endregion

        #region Data

        /// <summary>
        /// Gets the name of the sourcecode file where the associated exception was raised.
        /// </summary>
        /// <remarks>
        /// This will only work if pdb's are deployed, otherwise null is returned.
        /// </remarks>
        public string FileName { get; }

        /// <summary>
        /// Gets the line number inside of the sourcecode file where the associated exception was raised.
        /// </summary>
        /// <remarks>
        /// This will only work if pdb's are deployed, otherwise null is returned.
        /// </remarks>
        public uint? LineNumber { get; }

        /// <summary>
        /// Gets the name of the method that has raised the associated exception.
        /// </summary>
        public string MethodName { get; }

        /// <summary>
        /// Gets the name of the type that has raised the associated exception.
        /// </summary>
        public string TypeName { get; }

        #endregion

        #region Logic

        /// <summary>
        /// Converts this instance to a human readable string representation.
        /// </summary>
        /// <returns> A human readable string representation of this instance. </returns>
        public override string ToString()
        {
            var result = new StringBuilder();
            if (!string.IsNullOrEmpty(TypeName))
            {
                var index = TypeName.LastIndexOf('.') + 1;
                if (index > 0)
                {
                    result.AppendFormat("{0}.", TypeName.Substring(index));
                }
                else
                {
                    result.AppendFormat("{0}.", TypeName);
                }
            }

            if (LineNumber.HasValue)
            {
                result.AppendFormat("{0} ({1})", MethodName, LineNumber.Value);
            }
            else
            {
                result.AppendFormat("{0}", MethodName);
            }

            return result.ToString();
        }

        #endregion
    }
}