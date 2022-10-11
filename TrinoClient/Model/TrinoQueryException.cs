using TrinoClient.Model.Client;
using System;
using System.Net;

namespace TrinoClient.Model
{
    public class TrinoQueryException : TrinoWebException
    {
        #region Public Properties

        public string SqlState { get; }

        public int ErrorCode { get; }

        public string ErrorName { get; }

        public string ErrorType { get; }

        public ErrorLocation ErrorLocation { get; }

        public FailureInfo FailureInfo { get; }

        #endregion

        #region Constructors

        public TrinoQueryException(QueryError error) : base(error.Message, HttpStatusCode.OK)
        {
            if (error == null)
            {
                throw new ArgumentNullException("error");
            }

            this.ErrorCode = error.ErrorCode;
            this.ErrorLocation = error.ErrorLocation;
            this.ErrorName = error.ErrorName;
            this.ErrorType = error.ErrorType;
            this.FailureInfo = error.FailureInfo;
        }

        public TrinoQueryException(QueryError error, HttpStatusCode code) : base(error.Message, code)
        {
            if (error == null)
            {
                throw new ArgumentNullException("error");
            }

            this.ErrorCode = error.ErrorCode;
            this.ErrorLocation = error.ErrorLocation;
            this.ErrorName = error.ErrorName;
            this.ErrorType = error.ErrorType;
            this.FailureInfo = error.FailureInfo;
        }

        #endregion
    }
}
