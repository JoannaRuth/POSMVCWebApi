using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POSMVCWebAPIClient.Models
{
    public class Transaction
    {
        public int TransactionId { get; set; }
        public string TransactionList { get; set; }
        public System.DateTime TransactionDate { get; set; }

    }
}