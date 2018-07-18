using System.Globalization;

namespace SalesTaxBusinessLogic
{
    public static class StringUtility
    {
	    public static string CapitalizeFirstLetter(this string s)
	    {
		    if (s.Length < 1)
		    {
			    return s;
		    }

			return char.ToUpper(s[0]) + s.Substring(1);
		}
    }
}
