using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using TrinoClient.Model.Client;

namespace TrinoClient.Model.Statement
{
    [method: JsonConstructor]
    public class QueryResultsV2(
        string id,
        Uri infoUri,
        Uri finalUri,
        Uri nextUri,
        bool nextUriDone,
        IEnumerable<Column> columns,
        IEnumerable<Uri> dataUris,
        Actions actions,
        QueryError error
            ) : QueryResults(id, infoUri, nextUri, columns, error), IQueryData
    {
        #region Public Properties

        public bool NextUriDone { get; } = nextUriDone;

        public Uri FinalUri { get; } = finalUri;

        public IEnumerable<Uri> DataUris { get; } = dataUris;

        public Actions Actions { get; } = actions;

        public IEnumerable<List<dynamic>> Data { get; set; }

        #endregion
        #region Constructors

        #endregion

        #region Public Methods

        public IEnumerable<List<dynamic>> GetData()
        {
            return Data;
        }

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

        #endregion
    }
}
