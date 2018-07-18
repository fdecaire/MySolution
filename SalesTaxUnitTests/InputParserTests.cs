using System;
using SalesTaxBusinessLogic;
using SalesTaxRepository;
using Xunit;

namespace SalesTaxUnitTests
{
    public class InputParserTests
    {
        [Fact]
        public void LineItemDescription1()
        {
	        var parser = new InputParser(new RepositoryManager());

	        parser.Parse("1 book at 12.49");

	        Assert.Equal("Book", parser.Receipt[0].Description);
        }

	    [Fact]
	    public void LineItemDescription2()
	    {
		    var parser = new InputParser(new RepositoryManager());

		    parser.Parse("1 imported box of chocolates at 10.00");

		    Assert.Equal("Imported box of chocolates", parser.Receipt[0].Description);
	    }

	    [Fact]
	    public void LineItemDescription3()
	    {
		    var parser = new InputParser(new RepositoryManager());

		    parser.Parse("1 bottle of perfume at 18.99");

		    Assert.Equal("Bottle of perfume", parser.Receipt[0].Description);
	    }

	    [Fact]
	    public void LineItemDescription4()
	    {
		    var parser = new InputParser(new RepositoryManager());

		    parser.Parse("1 box of imported chocolates at 11.25");

		    Assert.Equal("Imported box of chocolates", parser.Receipt[0].Description);
	    }

		[Fact]
	    public void ImportedItem()
	    {
		    var parser = new InputParser(new RepositoryManager());

		    Assert.True(parser.IsItemImported("Imported bottle of perfume"));
	    }

	    [Fact]
	    public void NotImportedItem()
	    {
		    var parser = new InputParser(new RepositoryManager());

		    Assert.False(parser.IsItemImported("Packet of headache pills"));
	    }

	    [Fact]
	    public void ExemptItemBook()
	    {
		    var parser = new InputParser(new RepositoryManager());

		    Assert.True(parser.IsItemExempt("Book"));
	    }

	    [Fact]
	    public void ExemptItemChocolates()
	    {
		    var parser = new InputParser(new RepositoryManager());

		    Assert.True(parser.IsItemExempt("Box of imported chocolates"));
	    }

	    [Fact]
	    public void NotExemptItemPerfume()
	    {
		    var parser = new InputParser(new RepositoryManager());

		    Assert.False(parser.IsItemExempt("Bottle of perfume"));
	    }

	    [Fact]
	    public void RollUpDescription1()
	    {
		    var parser = new InputParser(new RepositoryManager());

		    parser.Parse("1 bottle of perfume at 18.99\n1 imported bottle of perfume at 27.99");

		    Assert.Equal(2, parser.Receipt.Count);
	    }

	    [Fact]
	    public void RollUpDescription2()
	    {
		    var parser = new InputParser(new RepositoryManager());

		    parser.Parse("1 box of imported chocolates at 11.25\n1 box of imported chocolates at 11.25");

		    Assert.Single(parser.Receipt);
		    Assert.Equal(22.50, parser.Receipt[0].Price);
		    Assert.Equal(2, parser.Receipt[0].Quantity);
	    }

	    [Fact]
	    public void RollUpDescription3()
	    {
		    var parser = new InputParser(new RepositoryManager());

		    parser.Parse("1 imported bottle of perfume at 27.99\n1 bottle of perfume at 18.99\n1 packet of headache pills at 9.75\n1 box of imported chocolates at 11.25\n1 box of imported chocolates at 11.25");

		    Assert.Equal(4, parser.Receipt.Count);
	    }

		[Fact]
	    public void SalesTax1()
	    {
		    var parser = new InputParser(new RepositoryManager());

		    parser.Parse("1 book at 12.49");

		    Assert.Equal(0, parser.Receipt[0].Tax);
	    }

	    [Fact]
	    public void SalesTax2()
	    {
		    var parser = new InputParser(new RepositoryManager());

		    parser.Parse("1 music CD at 14.99");

		    Assert.Equal(1.50, parser.Receipt[0].Tax);
	    }

	    [Fact]
	    public void SalesTax3()
	    {
		    var parser = new InputParser(new RepositoryManager());

		    parser.Parse("1 imported bottle of perfume at 47.50");

			// 4.75 + 2.375
			Assert.Equal(7.15, parser.Receipt[0].Tax);
	    }

		[Fact]
	    public void TotalTax1()
	    {
		    var parser = new InputParser(new RepositoryManager());

		    parser.Parse("1 book at 12.49\n1 book at 12.49\n1 music CD at 14.99\n1 chocolate bar at 0.85");

		    Assert.Equal(1.50, parser.TotalSalesTax);
	    }

	    [Fact]
	    public void TotalTax2()
	    {
		    var parser = new InputParser(new RepositoryManager());

		    parser.Parse("1 imported box of chocolates at 10.00\n1 imported bottle of perfume at 47.50");

			// 0.5 + 4.75 + 2.375 = 7.62 => 7.65
		    Assert.Equal(7.65, parser.TotalSalesTax);
	    }

	    [Fact]
	    public void TotalTax3()
	    {
		    var parser = new InputParser(new RepositoryManager());

		    parser.Parse("1 imported bottle of perfume at 27.99\n1 bottle of perfume at 18.99\n1 packet of headache pills at 9.75\n1 box of imported chocolates at 11.25\n1 box of imported chocolates at 11.25");


			Assert.Equal(7.30, parser.TotalSalesTax);
	    }

	    [Fact]
	    public void Total1()
	    {
		    var parser = new InputParser(new RepositoryManager());

		    parser.Parse("1 book at 12.49\n\r1 book at 12.49\n\r1 music CD at 14.99\n\r1 chocolate bar at 0.85");

		    Assert.Equal(42.32, parser.TotalPrice);
	    }

	    [Fact]
	    public void Total2()
	    {
		    var parser = new InputParser(new RepositoryManager());

		    parser.Parse("1 imported box of chocolates at 10.00\n1 imported bottle of perfume at 47.50");

		    Assert.Equal(65.15, parser.TotalPrice);
	    }

	    [Fact]
	    public void Total3()
	    {
		    var parser = new InputParser(new RepositoryManager());

			parser.Parse("1 imported bottle of perfume at 27.99\n1 bottle of perfume at 18.99\n1 packet of headache pills at 9.75\n1 box of imported chocolates at 11.25\n1 box of imported chocolates at 11.25");

			Assert.Equal(86.53, parser.TotalPrice);
	    }

		[Fact]
	    public void ReceiptOutput1()
	    {
		    var parser = new InputParser(new RepositoryManager());

		    parser.Parse("1 book at 12.49\n\r1 book at 12.49\n\r1 music CD at 14.99\n\r1 chocolate bar at 0.85");

		    var result = parser.Output();

			Assert.Equal("Book: 24.98 (2 @ 12.49)\r\nMusic CD: 16.49\r\nChocolate bar: 0.85\r\nSales Taxes: 1.50\r\nTotal: 42.32\r\n", result);
	    }

	    [Fact]
	    public void ReceiptOutput2()
	    {
		    var parser = new InputParser(new RepositoryManager());

			parser.Parse("1 imported box of chocolates at 10.00\n1 imported bottle of perfume at 47.50");

			Assert.Equal("Imported box of chocolates: 10.50\r\nImported bottle of perfume: 54.65\r\nSales Taxes: 7.65\r\nTotal: 65.15\r\n", parser.Output());
	    }

	    [Fact]
	    public void ReceiptOutput3()
	    {
		    var parser = new InputParser(new RepositoryManager());

		    parser.Parse("1 imported bottle of perfume at 27.99\n1 bottle of perfume at 18.99\n1 packet of headache pills at 9.75\n1 box of imported chocolates at 11.25\n1 box of imported chocolates at 11.25");

			Assert.Equal("Imported bottle of perfume: 32.19\r\nBottle of perfume: 20.89\r\nPacket of headache pills: 9.75\r\nImported box of chocolates: 23.70 (2 @ 11.85)\r\nSales Taxes: 7.30\r\nTotal: 86.53\r\n", parser.Output());
	    }
	}
}
