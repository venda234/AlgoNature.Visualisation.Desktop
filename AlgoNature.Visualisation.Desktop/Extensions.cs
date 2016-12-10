using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Drawing;

namespace AlgoNature.Visualisation.Desktop
{
    internal static partial class Extensions
    {
        public static Point ToPoint(this string value)
        {
            Point res = new Point();
            string[] vals = value.Split(new char[3] { '{', '}', ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string str in vals)
            {
                if (str.ToLower().Contains('x'))
                {
                    res.X = Int32.Parse(str.Remove(0, 2));
                }
                if (str.ToLower().Contains('y'))
                {
                    res.Y = Int32.Parse(str.Remove(0, 2));
                }
            }
            return res;
        }
        public static PointF ToPointF(this string value)
        {
            PointF res = new PointF();
            string[] vals = value.Split(new char[3] { '{', '}', ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string str in vals)
            {
                if (str.ToLower().Contains('x'))
                {
                    res.X = Single.Parse(str.Remove(0, 2));
                }
                if (str.ToLower().Contains('y'))
                {
                    res.Y = Single.Parse(str.Remove(0, 2));
                }
            }
            return res;
        }

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
            
            List<PropertyInfo> propertiesToDisplay = new List<PropertyInfo>();
            if (includeOnlyTypesPropsOrExcludeThemFromGeneral)
            {
                string[] allProperties = properties.ToPropertiesNamesArray();
                // pokud nebude fungovat, užít
                //properties.MapFunct<PropertyInfo, string>(new Func<PropertyInfo, string>((property) => (property.ToString())));

                foreach (Type type in filterTypes)
                {
                    propertiesToDisplay.AddRange(type.GetProperties().Where(new Func<PropertyInfo, bool>((property) => (allProperties.Contains(property.Name)))));
                }
            }
            else
            {
                propertiesToDisplay = properties.ToList();
                foreach (Type type in filterTypes)
                {
                    string[] props = type.GetProperties().ToPropertiesNamesArray();
                    propertiesToDisplay = propertiesToDisplay.Where(new Func<PropertyInfo, bool>((property) => (!props.Contains(property.Name)))).ToList();
                }
            }
            return propertiesToDisplay.ToArray();
        }

        public static List<string> ToPropertiesNamesList(this IEnumerable<PropertyInfo> properties)
            => properties.MapFunct<PropertyInfo, string>(new Func<PropertyInfo, string>((property) => property.Name)).ToList();
        public static string[] ToPropertiesNamesArray(this IEnumerable<PropertyInfo> properties)
            => properties.MapFunct<PropertyInfo, string>(new Func<PropertyInfo, string>((property) => property.Name)).ToArray();

        public static IEnumerable<TResult> MapFunct<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> mapFunction)
        {
            int length = source.Count();
            List<TResult> result = new List<TResult>();
            foreach (TSource item in source)
            {
                result.Add(mapFunction(item));
            }
            return result.ToArray().AsEnumerable();
        }
        // identical with .Where()
        /*public static IEnumerable<T> Filter<T>(this IEnumerable<T> source, Func<T, bool> filterFunction)
        {
            int length = source.Count();
            List<T> result = new List<T>();
            foreach (T item in source)
            {
                if (filterFunction(item)) result.Add(item);
            }
            return result.ToArray().AsEnumerable();
        }*/
    }
}
