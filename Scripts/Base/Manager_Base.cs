using System.Collections.Generic;
using UnityEngine;

namespace TemplateTools
{
    public class Manager_Base : MonoBehaviour
    {
        [Dropdown("TemplateManagers")]
        public List<string> dependencies = new();
    }
}