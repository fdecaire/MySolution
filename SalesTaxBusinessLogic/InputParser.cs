using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SalesTaxDomain;
using SalesTaxInterfaces;

namespace SalesTaxBusinessLogic
{
    public class InputParser : IInputParser
    {
	    private readonly IRepositoryManager _repositoryManager;
	    public List<ProductLineItem> Receipt { get; set; } = new List<ProductLineItem>();

	    public InputParser(IRepositoryManager repositoryManager)
	    {
		    _repositoryManager = repositoryManager;
	    }

	    public string Parse(string input)
	    {
		    int lineNumber = 0;

		    if (string.IsNullOrEmpty(input))
		    {
			    return "Error: No input!";
		    }

			// first, split into lines
		    var lines = input.Replace("\r", "").Split('\n');

			// process each line
		    foreach (var line in lines)
		    {
			    int quantity;
			    double itemPrice;

			    lineNumber++;

			    if (string.IsNullOrWhiteSpace(line))
			    {
				    continue;
			    }

			    // split into words
			    var words = line.Trim().Split(' ');

			    // first word should be quantity, if not, then set qty to 1
			    if (!int.TryParse(words[0], out quantity))
			    {
				    return "Error on Line " + lineNumber + ": Quantity is missing!";
				}

			    // last word should be price, if not, then there's an error
			    if (!double.TryParse(words[words.Length - 1], out itemPrice))
			    {
				    return "Error on Line " + lineNumber + ": Price is missing!";
			    }

				Receipt.Add(new ProductLineItem
				{
					Description = CreateDescription(words),
					Quantity = quantity,
					Price = itemPrice
				});
		    }

			// need to compute taxes before roll-up (because of the 0.5 round up)
		    ComputeTaxes();

		    GroupByProductType();

		    return "";
	    }

	    private string CreateDescription(string[] words)
	    {
		    var description = "";

			for (var i = 1; i < words.Length - 1; i++)
		    {
			    if (words[i].ToLower() != "at")
			    {
				    if (description != "")
				    {
					    description += " ";
				    }
				    description += words[i].Trim();
			    }
		    }

		    // make sure the word "imported" is a the beginning of the sentence
		    if (IsItemImported(description) && !description.StartsWith("imported"))
		    {
			    description = description.Replace("imported", "").Replace("  ", " ");
			    description = "Imported " + description;
		    }

		    // capitalize the first letter of the description
		    return description.CapitalizeFirstLetter();
		}

	    public void GroupByProductType()
	    {
			// group by description, totalling quantity and price
		    Receipt = Receipt.GroupBy(r => r.Description)
			    .Select(x => new ProductLineItem
			    {
					Description = x.First().Description,
				    Quantity = x.Sum(q => q.Quantity),
					Price = x.Sum(p => p.Price),
					Tax = x.Sum(p => p.Tax)
			    })
			    .ToList();
	    }

	    public double TotalSalesTax
	    {
		    get { return Receipt.Sum(x => x.Tax); }
	    }

	    public double TotalPrice
	    {
		    get { return Math.Round(Receipt.Sum(x => x.Price) * 100) / 100 + TotalSalesTax; }
	    }

	    public void ComputeTaxes()
	    {
		    foreach (var item in Receipt)
		    {
			    item.Tax = 0;

				if (!IsItemExempt(item.Description))
				{
					item.Tax = item.UnitPrice * 0.1;
				}

			    if (IsItemImported(item.Description))
			    {
				    item.Tax += item.UnitPrice * 0.05;
			    }

				// round up by 0.05
			    item.Tax = Math.Ceiling(item.Tax * 20) / 20 * item.Quantity;
		    }
	    }

	    public bool IsItemExempt(string description)
	    {
		    var words = description.Split(' ');

		    foreach (var word in words)
		    {
			    var result = (from p in _repositoryManager.Products
					    where p.Exempt &&
					          p.Name.Contains(word.ToLower())
					    select p)
				    .ToList();

			    if (result.Count > 0)
			    {
				    return result[0].Exempt;
			    }
		    }

		    return false;
	    }

	    public bool IsItemImported(string itemString)
	    {
		    return itemString.ToLower().Contains("imported");
	    }

	    public string Output()
	    {
		    var sb = new StringBuilder();

		    foreach (var item in Receipt)
		    {
			    sb.AppendLine(item.Output);
		    }

		    // tax and total
		    sb.AppendLine("Sales Taxes: " + TotalSalesTax.ToString("0.00"));
		    sb.AppendLine("Total: " + TotalPrice.ToString("0.00"));

		    return sb.ToString();
	    }
    }
}
