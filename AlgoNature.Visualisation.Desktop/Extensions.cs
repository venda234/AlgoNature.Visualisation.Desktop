using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Drawing;
using System.Threading;
using System.Resources;
using System.Collections;
using System.IO;

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

        public static object CloneObject(this object objSource)
        {
            //Get the type of source object and create a new instance of that type
            Type typeSource = objSource.GetType();
            object objTarget = Activator.CreateInstance(typeSource);

            //Get all the properties of source object type
            PropertyInfo[] propertyInfo = typeSource.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            //Assign all source property to taget object 's properties
            foreach (PropertyInfo property in propertyInfo)
            {
                //Check whether property can be written to
                if (property.CanWrite)
                {
                    //check whether property type is value type, enum or string type
                    if (property.PropertyType.IsValueType || property.PropertyType.IsEnum || property.PropertyType.Equals(typeof(System.String)))
                    {
                        property.SetValue(objTarget, property.GetValue(objSource, null), null);
                    }
                    //else property type is object/complex types, so need to recursively call this method until the end of the tree is reached
                    else
                    {
                        object objPropertyValue = property.GetValue(objSource, null);
                        if (objPropertyValue == null)
                        {
                            property.SetValue(objTarget, null, null);
                        }
                        else
                        {
                            property.SetValue(objTarget, objPropertyValue.CloneObject(), null);
                        }
                    }
                }
            }
            return objTarget;
        }

        /// <summary>
        /// Tries to translate given key - if fails, returns the given key.
        /// </summary>
        /// <param name="RM">Resource manager</param>
        /// <param name="key">Translation key</param>
        /// <returns></returns>
        public static string TryTranslate(this System.Resources.ResourceManager RM, /*Dictionary<string, string> writingDictionary, string resourceFileNameWithoutExtension,*/ string key)
        {
            //var rs = RM.GetResourceSet(Thread.CurrentThread.CurrentCulture, true, true);

            //string value = RM.GetString(key, Thread.CurrentThread.CurrentCulture).ToString();

            //if (writingDictionary.Count == 0)
            //{
            //    /*string[] ress = Assembly.GetExecutingAssembly().GetManifestResourceNames();
            //    foreach (string r in ress)
            //    {
            //        Console.WriteLine(r);
            //    }*/

            //    //ResourceReader reader = new ResourceReader(Assembly.GetExecutingAssembly().GetManifestResourceStream(RM.BaseName + ".resources"));

            //    /*foreach (DictionaryEntry d in reader)
            //    {
            //        try
            //        {
            //            writingDictionary.Add((string)d.Key, (string)d.Value);
            //        }
            //        catch { }
            //    }*/


            //    //var assembly = Assembly.GetExecutingAssembly();

            //    try
            //    {
            //        //ResourceManager resmgr = new ResourceManager(thisType.Namespace + ".resources", Assembly.GetExecutingAssembly());
            //        //var strs = new ResourceReader()
            //        //var strs = assembly.GetManifestResourceNames();
            //        using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(RM.BaseName + ".resources"))
            //        using (StreamReader reader = new StreamReader(stream))
            //        {
            //            string line;
            //            string[] splitLine;
            //            while ((line = reader.ReadLine()) != null)
            //            {
            //                //if (line.Contains("System.Resources.ResourceReader")/* || line.Contains('\0')*/) continue;
            //                splitLine = line.Split(new char[8] { '=', '"', '\\', '\t', '\u0001', '\u0002', '\u0004', '�' }); //cleaning firstrow mess

            //                writingDictionary.AddKeysFromSplitLine(splitLine);
            //            }
            //        }
            //    }
            //    catch {  }
            //}

            
            string value = RM.GetString(key, Thread.CurrentThread.CurrentCulture);
            //try
            //{
            //    value = writingDictionary[key];
            //}
            //catch { value = ""; }

            if (value != null && value != "")
            {
                return value;
            }
            else
            {
                return key;
            }
        }

        //private static void AddKeysFromSplitLine(this Dictionary<string, string> dict, string[] splitLine)
        //{
        //    try // throws an exception if already exists
        //    {
        //        if (splitLine[splitLine.Length - 2] == "") // empty
        //            dict.Add(splitLine[splitLine.Length - 4], splitLine[splitLine.Length - 4]);
        //        else
        //            dict.Add(splitLine[splitLine.Length - 4], splitLine[splitLine.Length - 2]);
        //    }
        //    catch { }
        //    splitLine = splitLine.Where(new Func<string, int, bool>((str, i) => (i <= splitLine.Length - 7))).ToArray();

        //    if (splitLine.Length >= 4)
        //    {
        //        if (splitLine[splitLine.Length - 1] == "") dict.AddKeysFromSplitLine(splitLine);
        //    }
        //}
    }
}
