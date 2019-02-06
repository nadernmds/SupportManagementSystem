using SupportManagemantSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SupportManagemantSystem
{
    public static class Common
    {
        public static string getStateColor(byte i)
        {
            switch (i)
            {
                case 1:
                    return "red";
                case 2:
                    return "primary";
                case 3:
                    return "yellow";
                case 4:
                    return "green";
                default:
                    return "default";
            }
        }
        public static IEnumerable<string> ToPersianDate(this IEnumerable<DateTime> dates)
        {
            foreach (var date in dates)
            {
                yield return pep.pep.ToPersianDateShortString(date);
            }
        }
        public static DateTime GetDate()
        {
            return new SupportManagemantSystemEntities()
                .Database.SqlQuery<DateTime>("Select getdate()")
                .AsEnumerable()
                .FirstOrDefault();
        }
    }
}