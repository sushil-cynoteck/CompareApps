using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CompareApps.Areas.Admin.Logging
{
    public static class LINQExtensionMethods
    {
        public static string LogQueryableToApplicationLog(this IQueryable query, bool todisk = true, string Subject="")
        {
            string Result = "";
            System.Reflection.MethodInfo toTraceStringMethod = query.GetType().GetMethod("ToTraceString");

            if (toTraceStringMethod != null)
                Result = toTraceStringMethod.Invoke(query, null).ToString();
            else
                Result = query.ToString();

            if (todisk)
            {
                if (!(String.IsNullOrEmpty(Subject)))
                {
                    Logger.WriteLine(Subject);
                }
                Logger.WriteLine(Result);
            }
            return Result;
        }
    }
}