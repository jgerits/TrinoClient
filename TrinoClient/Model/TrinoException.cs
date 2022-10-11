using System;

namespace TrinoClient.Model
{
    /// <summary>
    /// A custom exception to wrap presto API errors
    /// </summary>
    public class TrinoException : Exception
    {
        #region Public Properties

        /// <summary>
        /// The raw representation of the data returned by presto
        /// </summary>
        public string RawResponseContent { get; }

        #endregion

        #region Constructors

        public TrinoException(string message) : base(message)
        {
            this.RawResponseContent = String.Empty;
        }

        public TrinoException(string message, Exception innerException) : base(message, innerException)
        {
            this.RawResponseContent = String.Empty;
        }

        public TrinoException(string message, string rawContent) : base(message)
        {
            this.RawResponseContent = rawContent;
        }

        public TrinoException(string message, string rawContent, Exception innerException) : base(message, innerException)
        {
            this.RawResponseContent = rawContent;
        }

        #endregion
    }
}
