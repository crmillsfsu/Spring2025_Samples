using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.eCommerce.Util
{
    public class QueryRequest
    {
        public string Query { get; set; }
        public QueryRequest()
        {
            Query = string.Empty;
        }
    }
}
