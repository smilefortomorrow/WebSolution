using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkFlowSystem.ViewModels
{
    public class InvoiceView
    {
        public int ID { get; set; }
        public string InvoiceNo { get; set; }
        public int UserID { get; set; }
        public string City { get; set; } = "";
        public string Country { get; set; } = "";
        public string PostalCode { get; set; } = "";
        public string Business { get; set; } = "";
        public string Username { get; set; } = "";
        public string Email {  get; set; } = "";
        public int PackageCount { get; set; }
        public double Total { get; set; }
        public string Status { get; set; }
        public DateTime Regist { get; set; } // 'date' SQL type is mapped to 'DateTime' in .NET
        public double GST { get; set; } // Nullable int, since 'Allow Nulls' is checked
    }

}
