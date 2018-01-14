using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MVCTest.Models
{
    public class QueryOptions
    {
        public QueryOptions()
        {
            SortField = "Id";
            SortOrder = SortOrder.ASC;
            CurrentPage = 1;
            PageSize = 1;
        }

        public string SortField { get; set; }

        public SortOrder SortOrder { get; set; }

        public string Sort => $"{SortField} {SortOrder.ToString()}";

        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }

        public int PageSize { get; set; }
    }
}