using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FluffNStuff.Models
{
    public class cart
    {
        public int subcat_id { get; set; }
        public string subcat_name { get; set; }
        public Nullable<int> subcat_price { get; set; }
        public Nullable<int> o_qty { get; set; }
        public Nullable<double> o_bill { get; set; }
    }
}