﻿using System.ComponentModel.DataAnnotations;
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

    /// <summary>
    /// Inicializuje slovníky pre mapovanie slovenských názov mesiacov za pomoci atribútov.
    /// </summary>
    static SlovakMonthUtility()
    {
        _monthDict = new Dictionary<string, SlovakMonth>();
        _monthDictReverse = new Dictionary<SlovakMonth, string>();
    
        foreach (SlovakMonth month in Enum.GetValues(typeof(SlovakMonth)))
        {
            var info = typeof(SlovakMonth).GetMember(month.ToString());
            var attributes = info[0].GetCustomAttribute<DisplayAttribute>();
            if (attributes != null && attributes.Name != null)
            {
                string slovakName = attributes.Name;
    
                _monthDict[slovakName] = month;
                _monthDictReverse[month] = slovakName;
            }
        }
    }

    /// <summary>
    /// Vráti slovenský názov mesiaca pre danú hodnotu.
    /// </summary>
    public static string GetSlovakName(SlovakMonth month)
    {
        return _monthDictReverse[month];
    }
}
