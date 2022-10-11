using System.Collections.Generic;

namespace TrinoClient.Model.SPI.Predicate
{
    /// <summary>
    /// Is a generic implementation for TupleDomain<T>
    /// </summary>
    public class TupleDomainPlaceHolder<T>
    {
        public IEnumerable<ColumnDomainPlaceHolder> ColumnDomains { get; set; }
    }
}
