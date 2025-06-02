using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace IbrahKit
{
    public class Type_Utilities
    {
        public static Type[] GetAllTypes(Type baseType)
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(t => t.IsClass
                            && !t.IsAbstract
                            && baseType.IsAssignableFrom(t)).ToArray();
        }

        public static IEnumerable GetAllTypesDropdownFormat(Type baseType)
        {
            List<string> subtypes = GetAllTypes(baseType).Select(x => x.Name).ToList();

            subtypes.Sort((a, b) =>
            {
                return a.CompareTo(b);
            });

            if (subtypes.Count > 0)
            {
                subtypes.Insert(0, "None");
            }
            else
            {
                subtypes.Add("None");
            }

            return subtypes;
        }
    }
}
