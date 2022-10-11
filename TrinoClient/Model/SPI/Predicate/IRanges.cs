using System.Collections.Generic;

namespace TrinoClient.Model.SPI.Predicate
{
    /// <summary>
    /// From com.facebook.presto.spi.predicate.Ranges.java
    /// </summary>
    public interface IRanges
    {
        int GetRangeCount();

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Allowed non-overlapping predicate ranges sorted in increasing order</returns>
        IEnumerable<Range> GetOrderedRanges();

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Single range encompassing all of allowed the ranges</returns>
        Range GetSpan();
    }
}
