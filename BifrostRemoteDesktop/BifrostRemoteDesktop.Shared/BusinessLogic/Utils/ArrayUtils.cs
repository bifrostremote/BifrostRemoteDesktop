using System;
using System.Collections.Generic;
using System.Text;

namespace BifrostRemoteDesktop.BusinessLogic.Utils
{
    public static class ArrayUtils
    {
        public static string[] ConvertToStringArray(object[] array)
        {
            return Array.ConvertAll(array, ConvertObjectToString);
        }

        private static string ConvertObjectToString(object obj)
        {
            return obj?.ToString() ?? string.Empty;
        }
    }
}
