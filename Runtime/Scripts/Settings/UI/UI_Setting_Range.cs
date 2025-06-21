using Sirenix.OdinInspector;
using UnityEngine;

namespace IbrahKit
{
    public class UI_Setting_Range : UI_Setting
    {
        [BoxGroup("UI"), SerializeField] private UI_Selectable sub;
        [BoxGroup("UI"), SerializeField] private UI_Selectable add;

        protected override void OnEnable()
        {
            base.OnEnable();

            if (setting == null)
            {
                Debug.LogWarning("Setting is null");
                return;
            }

            if (sub != null)
            {
                sub.OnClickEvent.RemoveAllListeners();
                sub.OnClickEvent.AddListener(() => ChangeValue(-setting.GetStep()));
            }
            else
            {
                Debug.LogWarning("Sub Selectable is null");
            }

            if (add != null)
            {
                add.OnClickEvent.RemoveAllListeners();
                add.OnClickEvent.AddListener(() => ChangeValue(setting.GetStep()));
            }
            else
            {
                Debug.LogWarning("Add Selectable is null");
            }
        }

        public override void UpdateUI()
        {
            base.UpdateUI();

            if (setting != null)
            {
                if (sub != null)
                {
                    sub.SetInteractable((setting.GetValue() > setting.GetMinMax().x) || setting.GetLoop());
                }

                if (add != null)
                {
                    add.SetInteractable((setting.GetValue() < setting.GetMinMax().y) || setting.GetLoop());
                }
            }
        }
    }
}