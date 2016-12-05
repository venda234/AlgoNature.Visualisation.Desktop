using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace AlgoNature.Visualisation.Desktop
{
    internal static class Extensions
    {
        public static bool ImplementsInterface(this Type type, Type ifaceType)
        {
            Type[] intf = type.GetInterfaces();
            for (int i = 0; i < intf.Length; i++)
            {
                if (intf[i] == ifaceType)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="properties">Properties to be filtered</param>
        /// <param name="filterTypes">Types used for filtering properties</param>
        /// <param name="includeOnlyTypesPropsOrExcludeThemFromGeneral">If <code>true</code>, will display only properties contained in <paramref name="filterTypes"/> types (if has them), 
        /// otherwise will display all properties except those contained in <paramref name="filterTypes"/> types</param>
        /// <returns></returns>
        public static PropertyInfo[] FilterPropertiesBasedOnOtherTypes(this PropertyInfo[] properties, Type[] filterTypes, bool includeOnlyTypesPropsOrExcludeThemFromGeneral)
        {

        }
    }
}
