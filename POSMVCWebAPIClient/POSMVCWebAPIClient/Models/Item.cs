using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POSMVCWebAPIClient.Models
{
    public class Item
    {
        public string ItemId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Index { get; set; }
        public int Quantity { get; set; }
        public decimal PriceMultiplied { get; set; }

        public Item()
        {
            Quantity = 1;
           // PriceMultiplied = Price;
        }

    }
}