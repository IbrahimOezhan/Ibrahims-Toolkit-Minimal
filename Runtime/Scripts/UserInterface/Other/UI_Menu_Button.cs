using UnityEngine;
using UnityEngine.Events;

namespace IbrahKit
{
    public class UI_Menu_Button : MonoBehaviour
    {
        [SerializeField] private UI_Selectable selec;
        [SerializeField] private UI_Localization localization;
        [SerializeField] private UI_Text_Setter textSetter;

        public UnityEvent Initialize(string value)
        {
            if (localization != null)
            {
                localization.SetKey(value);

            }
            else if (textSetter != null)
            {
                textSetter.SetText(value);
            }
            else
            {
                Debug.LogError("Neither localization or text setter component found");
            }

            return selec.OnClickEvent;
        }
    }
}