using System;
using System.Collections.Generic;
using SalesTaxDomain;

namespace SalesTaxInterfaces
{
    public interface IRepositoryManager
    {
		List<Product> Products { get; set; }
	}
}
