using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace POSMVCWebAPIClient.Models
{
    [Serializable, XmlRoot("Transaction")]
    public class TransactionXml
    {
        [XmlAttribute]
        public int TransactionId { get; set; }

        [XmlAttribute]
        public decimal TotalAmount { get; set; }

        [XmlElement]
        public List<Item> Item { get; set; }

    }
}