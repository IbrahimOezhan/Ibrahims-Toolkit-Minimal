using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace TemplateTools
{
    public class UI_Interactive : UI_Base, IMenuUpdate
    {
        [SerializeField, ReadOnly] private List<UI_Extension> extensions = new();

        [SerializeField, OnValueChanged("OnValueChanged")] private Extension extension;

        protected override void OnEnable()
        {
            base.OnEnable();
            UpdateUI();
        }

        public void OnValueChanged()
        {
            extensions.RemoveAll(x => x == null);

            Extension newExtension = extension;

            if (extension != Extension.SELECT)
            {
                extension = Extension.SELECT;

                UI_Extension extensionToAdd = null;

                switch (newExtension)
                {
                    case Extension.Localization:
                        extensionToAdd = gameObject.AddComponent<UI_Localization_Legacy>();
                        break;
                    case Extension.Fitter:
                        extensionToAdd = gameObject.AddComponent<UI_Fitter>();
                        break;
                    case Extension.Audio:
                        extensionToAdd = gameObject.AddComponent<UI_Audio>();
                        break;
                    case Extension.Styling:
                        extensionToAdd = gameObject.AddComponent<UI_Styling>();
                        break;
                }

                extensions.Add(extensionToAdd);

                SortList();
            }
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

        private enum Extension
        {
            SELECT,
            Localization,
            Styling,
            Fitter,
            Audio,
        }
    }
}