using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Uniza.Namedays;

public enum SlovakMonth
{
    [Display(Name = "Január", ShortName = "Jan", Order = 1)]
    January,
    [Display(Name = "Február", ShortName = "Feb", Order = 2)]
    February,
    [Display(Name = "Marec", ShortName = "Mar", Order = 3)]
    March,
    [Display(Name = "Apríl", ShortName = "Apr", Order = 4)]
    April,
    [Display(Name = "Máj", ShortName = "Máj", Order = 5)]
    May,
    [Display(Name = "Jún", ShortName = "Jún", Order = 6)]
    June,
    [Display(Name = "Júl", ShortName = "Júl", Order = 7)]
    July,
    [Display(Name = "August", ShortName = "Aug", Order = 8)]
    August,
    [Display(Name = "September", ShortName = "Sep", Order = 9)]
    September,
    [Display(Name = "Október", ShortName = "Okt", Order = 10)]
    October,
    [Display(Name = "November", ShortName = "Nov", Order = 11)]
    November,
    [Display(Name = "December", ShortName = "Dec", Order = 12)]
    December
}

public static class SlovakMonthUtility
{
    private static Dictionary<string, SlovakMonth> _monthDict;
    private static Dictionary<SlovakMonth, string> _monthDictReverse;

    static SlovakMonthUtility()
    {
        _monthDict = new Dictionary<string, SlovakMonth>();
        _monthDictReverse = new Dictionary<SlovakMonth, string>();

        foreach (SlovakMonth month in Enum.GetValues(typeof(SlovakMonth)))
        {
            var info = typeof(SlovakMonth).GetMember(month.ToString());
            var attributes = info[0].GetCustomAttribute<DisplayAttribute>();
            string slovakName = attributes.Name;

            _monthDict[slovakName] = month;
            _monthDictReverse[month] = slovakName;
        }
    }

    public static SlovakMonth GetMonthEnum(string slovakName)
    {
        return _monthDict[slovakName];
    }

    public static string GetSlovakName(SlovakMonth month)
    {
        return _monthDictReverse[month];
    }

    public static int? GetMonthOrder(string slovakName)
    {
        SlovakMonth selectedMonth = _monthDict[slovakName];

        var info = typeof(SlovakMonth).GetMember(selectedMonth.ToString());
        var attributes = info[0].GetCustomAttribute<DisplayAttribute>();

        return attributes?.GetOrder();
    }
}
