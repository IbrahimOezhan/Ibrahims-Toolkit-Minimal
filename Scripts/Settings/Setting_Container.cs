using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using IbrahKit;
using UnityEngine;

namespace IbrahKit
{
    [Serializable]
    public class Setting_Container : MonoBehaviour
    {
        [SerializeField, OnValueChanged("OnValueChanged"), ValueDropdown("GetAllTypesDropdownFormat")]
        private string selectSetting = "None";

        [SerializeField, SerializeReference]
        private Setting setting;

        public Setting GetSetting()
        {
            return setting;
        }

        //Invoked by Odin
        private IEnumerable GetAllTypesDropdownFormat() { return Type_Utilities.GetAllTypesDropdownFormat(typeof(Setting)); }

        //Invoked by Odin
        private void OnValueChanged()
        {
            List<Type> types = Type_Utilities.GetAllTypes(typeof(Setting)).ToList();

            Type type = types.Find(x => x.Name == selectSetting);

            if (type != null)
            {
                setting = (Setting)Activator.CreateInstance(type);
            }

            selectSetting = "None";
        }
    }
}