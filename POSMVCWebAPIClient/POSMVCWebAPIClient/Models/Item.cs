using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace POSMVCWebAPIClient.Models
{
    public class Item
    {
        [XmlElement("ItemID")]
        public string ItemId { get; set; }

        [XmlElement("ItemName")]
        public string Name { get; set; }

        [XmlElement("UnitPrice")]
        public decimal Price { get; set; }

        //[XmlElement("ItemID")]
        //public int Index { get; set; }

        [XmlElement("Quantity")]
        public int Quantity { get; set; }

        [XmlElement("Amount")]
        public decimal PriceMultiplied { get; set; }

        public Item()
        {
            Quantity = 1;
           // PriceMultiplied = Price;
        }

    }
}