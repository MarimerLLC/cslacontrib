using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CslaContrib.Mvc
{
    static class StringExtension
    {
        public static string[] FromCsvToArray(this string csvValues)
        {
            if (csvValues == null) return null;

            return (from a in csvValues.Split(',')
                    let atrim = a.Trim()
                    where !string.IsNullOrEmpty(atrim)
                    select atrim).ToArray();
        }
    }
}
