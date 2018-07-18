using System;
using System.Collections.Generic;
using System.Linq;
using SalesTaxDomain;
using SalesTaxInterfaces;

namespace SalesTaxRepository
{
    public class RepositoryManager : IRepositoryManager
    {
	    public List<Product> Products { get; set; } = new List<Product>();

	    public RepositoryManager()
	    {
		    if (Products.Count > 0)
		    {
				// just in case
			    return;
		    }

			// this is a fake repository.  I'm going to pretend that these records are read from a database using linq and EF.
		    Products.Add(new Product
			{
				Name = "book",
				Type = ProductType.Book,
				Exempt = true
			});
		    Products.Add(new Product
		    {
			    Name = "books", // I'm going to avoid a pluralizer in this sample code
			    Type = ProductType.Book,
				Exempt = true
			});
		    Products.Add(new Product
		    {
			    Name = "chocolate",
			    Type = ProductType.Food,
			    Exempt = true
			});
		    Products.Add(new Product
		    {
			    Name = "chocolates",
			    Type = ProductType.Food,
			    Exempt = true
			});
		    Products.Add(new Product
		    {
			    Name = "pills",
			    Type = ProductType.Medical,
			    Exempt = true
			});
		    Products.Add(new Product
		    {
			    Name = "perfume",
			    Type = ProductType.Other,
			    Exempt = false
		    });
		}


    }
}
