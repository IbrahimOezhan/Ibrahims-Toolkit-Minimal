using System;
using UnityEngine;

namespace TemplateTools
{
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public class DropdownAttribute : PropertyAttribute
    {
        public string filePath;

        public DropdownAttribute(string filePath)
        {
            this.filePath = "Assets/Resources/DropdownFiles/" + filePath + ".txt";
        }
    }
}