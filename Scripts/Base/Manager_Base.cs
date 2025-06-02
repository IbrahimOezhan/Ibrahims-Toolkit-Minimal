using System.Collections.Generic;
using UnityEngine;

namespace IbrahKit
{
    public class Manager_Base : MonoBehaviour
    {
        [Dropdown("TemplateManagers")]
        public List<string> dependencies = new();
    }
}