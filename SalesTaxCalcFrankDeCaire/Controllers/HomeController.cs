using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SalesTaxCalcFrankDeCaire.Models;
using SalesTaxInterfaces;

namespace SalesTaxCalcFrankDeCaire.Controllers
{
    public class HomeController : Controller
    {
	    private readonly IInputParser _inputParser;

	    public HomeController(IInputParser inputParser)
	    {
		    _inputParser = inputParser;
	    }

        public IActionResult Index()
        {
            return View();
        }

	    [HttpPost]
	    public IActionResult ParseInput(string txtInput)
	    {
		    ViewBag.Error = _inputParser.Parse(txtInput);

		    ViewBag.Input = txtInput;

		    if (ViewBag.Error == "")
		    {
			    ViewBag.Result = _inputParser.Output();
		    }
		    else
		    {
			    ViewBag.Result = "";
		    }

		    return View("Index");
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
