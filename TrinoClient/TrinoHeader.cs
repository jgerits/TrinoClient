namespace TrinoClient
{
    /// <summary>
    /// Available Presto headers
    /// https://github.com/prestodb/presto/blob/master/presto-client/src/main/java/com/facebook/presto/client/PrestoHeaders.java
    /// </summary>
    public class TrinoHeader
    {
        private TrinoHeader(string value) { Value = value; }

        public string Value { get; set; }

        public static readonly TrinoHeader TRINO_USER = new TrinoHeader("X-Trino-User");
        public static readonly TrinoHeader TRINO_SOURCE = new TrinoHeader("X-Trino-Source");
        public static readonly TrinoHeader TRINO_CATALOG = new TrinoHeader("X-Trino-Catalog");
        public static readonly TrinoHeader TRINO_SCHEMA = new TrinoHeader("X-Trino-Schema");
        public static readonly TrinoHeader TRINO_TIME_ZONE = new TrinoHeader("X-Trino-Time-Zone");
        public static readonly TrinoHeader TRINO_LANGUAGE = new TrinoHeader("X-Trino-Language");
        public static readonly TrinoHeader TRINO_SESSION = new TrinoHeader("X-Trino-Session");
        public static readonly TrinoHeader TRINO_SET_CATALOG = new TrinoHeader("X-Trino-Set-Catalog");
        public static readonly TrinoHeader TRINO_SET_SCHEMA = new TrinoHeader("X-Trino-Set-Schema");
        public static readonly TrinoHeader TRINO_SET_SESSION = new TrinoHeader("X-Trino-Set-Session");
        public static readonly TrinoHeader TRINO_CLEAR_SESSION = new TrinoHeader("X-Trino-Clear-Session");
        public static readonly TrinoHeader TRINO_PREPARED_STATEMENT = new TrinoHeader("X-Trino-Prepared-Statement");
        public static readonly TrinoHeader TRINO_ADDED_PREPARE = new TrinoHeader("X-Trino-Added-Prepare");
        public static readonly TrinoHeader TRINO_DEALLOCATED_PREPARE = new TrinoHeader("X-Trino-Deallocated-Prepare");
        public static readonly TrinoHeader TRINO_TRANSACTION_ID = new TrinoHeader("X-Trino-Transaction-Id");
        public static readonly TrinoHeader TRINO_STARTED_TRANSACTION_ID = new TrinoHeader("X-Trino-Started-Transaction-Id");
        public static readonly TrinoHeader TRINO_CLEAR_TRANSACTION_ID = new TrinoHeader("X-Trino-Clear-Transaction-Id");
        public static readonly TrinoHeader TRINO_CLIENT_INFO = new TrinoHeader("X-Trino-Client-Info");
        public static readonly TrinoHeader TRINO_CLIENT_TAGS = new TrinoHeader("X-Trino-Client-Tags");

        public static readonly TrinoHeader TRINO_CURRENT_STATE = new TrinoHeader("X-Trino-Current-State");
        public static readonly TrinoHeader TRINO_MAX_WAIT = new TrinoHeader("X-Trino-Max-Wait");
        public static readonly TrinoHeader TRINO_MAX_SIZE = new TrinoHeader("X-Trino-Max-Size");
        public static readonly TrinoHeader TRINO_TASK_INSTANCE_ID = new TrinoHeader("X-Trino-Task-Instance-Id");
        public static readonly TrinoHeader TRINO_PAGE_TOKEN = new TrinoHeader("X-Trino-Page-Sequence-Id");
        public static readonly TrinoHeader TRINO_PAGE_NEXT_TOKEN = new TrinoHeader("X-Trino-Page-End-Sequence-Id");
        public static readonly TrinoHeader TRINO_BUFFER_COMPLETE = new TrinoHeader("X-Trino-Buffer-Complete");

        public static readonly TrinoHeader TRINO_DATA_NEXT_URI = new TrinoHeader("X-Trino-Data-Next-Uri");

        public override string ToString()
        {
            return this.Value;
        }
    }
}
