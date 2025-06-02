using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace IbrahKit
{
    public class UI_Interactive : UI_Base, IMenuUpdate
    {
        [SerializeField, ReadOnly] private List<UI_Extension> extensions = new();

        [SerializeField, OnValueChanged("OnValueChanged"), ValueDropdown("GetAllSubtypes")] private string extension = "None";

        protected override void OnEnable()
        {
            base.OnEnable();
            UpdateUI();
        }

        private Type[] GetDerivedTypes()
        {
            var baseType = typeof(UI_Extension);

            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(t => t.IsClass
                            && !t.IsAbstract
                            && baseType.IsAssignableFrom(t)).ToArray();
        }

        private IEnumerable GetAllSubtypes()
        {
            List<string> subtypes = GetDerivedTypes().Select(x => x.Name).ToList();
            subtypes.Insert(0, "None");

            return subtypes;
        }

        public void OnValueChanged()
        {
            extensions.RemoveAll(x => x == null);

           List<Type> types = GetDerivedTypes().ToList();

            Type type = types.Find(x => x.Name == extension);

            if(type != null)
            {
                UI_Extension extensionToAdd = gameObject.AddComponent(type) as UI_Extension;
                extensions.Add(extensionToAdd);
                SortList();
            }

            extension = "None";
        }

        private void SortList()
        {
            extensions.Sort((a, b) =>
            {
                return a.GetOrder().CompareTo(b.GetOrder());
            });
        }

        [Button]
        public void UpdateExtensionList()
        {
            UI_Extension[] _extension = GetComponents<UI_Extension>();

            extensions = new(_extension.ToList());

            SortList();
        }

        [Button]
        public void UpdateUI()
        {
            List<UI_Interactive> children = Transform_Utilities.GetChildren<UI_Interactive>(transform);

            foreach (UI_Interactive child in children)
            {
                child.UpdateUI();
            }

            foreach (UI_Extension extension in extensions)
            {
                extension.Execute();
            }
        }

        public void MenuUpdate()
        {
            UpdateUI();
        }
    }
}