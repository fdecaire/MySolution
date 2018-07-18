using System;

namespace SalesTaxDomain
{
	public class Product
	{
		public string Name { get; set; }
		public ProductType Type { get; set; }
		public bool Exempt { get; set; }
	}
}
