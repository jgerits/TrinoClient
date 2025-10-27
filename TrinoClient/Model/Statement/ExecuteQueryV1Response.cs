﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrinoClient.Model.Client;
using TrinoClient.Serialization;

namespace TrinoClient.Model.Statement
{
    /// <summary>
    /// A response from executing a query
    /// </summary>
    public class ExecuteQueryV1Response
    {
        #region Private Properties

        private List<QueryResultsV1> _Responses = [];

        #endregion

        #region Public Properties

        /// <summary>
        /// The incremental response status info objects
        /// </summary>
        public IReadOnlyList<IQueryStatusInfo> Responses
        {
            get
            {
                return _Responses;
            }
        }

        /// <summary>
        /// The data from the responses
        /// </summary>
        public IEnumerable<List<dynamic>> Data
        {
            get
            {
                return _Responses.Where(x => x != null && x.Data != null).SelectMany(x => x.GetData()).Where(x => x != null);
            }
        }

        /// <summary>
        /// The columns requested in the query
        /// </summary>
        public IReadOnlyList<Column> Columns { get; }

        /// <summary>
        /// Indicates whether the query was successfully closed by the client.
        /// </summary>
        public bool QueryClosed { get; }

        /// <summary>
        /// If deserialization fails, the will contain the thrown exception. Otherwise, 
        /// this property is null.
        /// </summary>
        public Exception LastError { get; }

        #endregion

        #region Constructors

        internal ExecuteQueryV1Response(IEnumerable<QueryResultsV1> results, bool closed)
        {
            ArgumentNullException.ThrowIfNull(results);

            _Responses = results.ToList();
            QueryClosed = closed;
            LastError = null;

            if (_Responses.Count != 0)
            {
                // The first response may not have any column data, so find the first
                // response that does and pull the columns from that
                var columns = _Responses.First(x => x.Columns != null).Columns;
                Columns = columns as IReadOnlyList<Column> ?? columns.ToList();
            }
            else
            {
                Columns = [];
            }
        }

        internal ExecuteQueryV1Response(IEnumerable<QueryResultsV1> results, bool closed, Exception lastError) : this(results, closed)
        {
            LastError = lastError;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Converts the data rows to a CSV formatted array, each line
        /// in the array is CSV data and can be combined with a String.Join("\n", csvArray)
        /// function to get a single CSV string
        /// </summary>
        /// <returns>The data formatted as CSV with 1 line per index in the IEnumerable</returns>
        public IEnumerable<string> DataToCsv()
        {
            if (Data != null)
            {
                foreach (var Item in Data)
                {
                    if (Item == null || Item.Count == 0)
                    {
                        yield return string.Empty;
                        continue;
                    }

                    // Estimate capacity: average 20 chars per column + quotes and comma
                    StringBuilder SB = new(Item.Count * 25);

                    for (int i = 0; i < Item.Count; i++)
                    {
                        SB.Append('"');
                        SB.Append(Item[i]);
                        SB.Append('"');
                        
                        if (i < Item.Count - 1)
                        {
                            SB.Append(',');
                        }
                    }

                    yield return SB.ToString();
                }
            }
            else
            {
                throw new ArgumentNullException(nameof(Data), "The data in this query result is null.");
            }
        }

        /// <summary>
        /// Converts the data rows into a JSON string using a Dictionary type mapping
        /// </summary>
        /// <returns>The JSON formatted string</returns>
        public string DataToJson()
        {
            if (Data != null)
            {
                // Materialize data once to avoid multiple enumeration
                var dataList = Data as IList<List<dynamic>> ?? Data.ToList();
                
                Dictionary<string, Dictionary<string, object>[]> Wrapper = new()
                {
                    { "data", new Dictionary<string, object>[dataList.Count] }
                };

                int RowCounter = 0;

                foreach (List<dynamic> Row in dataList)
                {
                    // Keep track of the column number
                    int Counter = 0;

                    Wrapper["data"][RowCounter] = [];

                    foreach (dynamic Column in Row)
                    {
                        Column Col = Columns[Counter++];
                        object Value = null;

                        if (Column != null)
                        {
                            Value = TrinoTypeMapping.Convert(Column.ToString(), Col.TypeSignature.RawType);
                        }

                        Wrapper["data"][RowCounter].Add(Col.Name, Value);
                    }

                    RowCounter++;
                }

                return JsonConvert.SerializeObject(Wrapper);
            }
            else
            {
                return null;
            }
        }

        #endregion
    }
}
