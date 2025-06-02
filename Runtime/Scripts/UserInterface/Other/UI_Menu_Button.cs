using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace IbrahKit
{
    public class UI_Menu_Button : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private UI_Selectable selec;
        [SerializeField] private UI_Localization localization;

        public UnityEvent Initialize(string localizationKey)
        {
            localization.SetKey(localizationKey);

            return selec.OnClickEvent;
        }
    }
}