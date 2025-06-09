using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace IbrahKit
{
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public class DropdownAttribute : PropertyAttribute
    {
        public List<string> dropdownInput = new();
        public (bool,string) error;

        public DropdownAttribute(string filePath)
        {
            filePath = "Assets/Resources/DropdownFiles/" + filePath + ".txt";

            if (!File.Exists(filePath))
            {
                error = (true, "File doesnt exist");
            }

            dropdownInput = String_Utilities.GetDropdown(filePath);

            if(dropdownInput.Count == 0)
            {
                error = (true, "No options");
            }
        }

        public DropdownAttribute(List<string> input)
        {
            dropdownInput = new(input);
        }
    }
}