using System;
using System.Collections.Generic;
using System.Text;

namespace SalesTaxDomain
{
    public class ProductLineItem
    {
		public string Description { get; set; }
		public double Price { get; set; }
		public int Quantity { get; set; }
		public double Tax { get; set; }
		public double UnitPrice {
			get
			{
				return Price / (double)Quantity;
			}
		}

	    public string Output
	    {
		    get
		    {
			    var result = Description + ": " + (Price + Tax).ToString("0.00");
			    if (Quantity > 1)
			    {
				    result += " (" + Quantity + " @ " + ((Price + Tax) / Quantity).ToString("0.00") + ")";
			    }

			    return result;
		    }
	    }
    }
}
