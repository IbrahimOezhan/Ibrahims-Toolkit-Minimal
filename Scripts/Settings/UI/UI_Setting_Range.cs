using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace IbrahKit
{
    public class UI_Setting_Range : UI_Setting
    {
        [BoxGroup("UI"), SerializeField] private UI_Selectable sub;
        [BoxGroup("UI"), SerializeField] private UI_Selectable add;

        protected override void OnEnable()
        {
            base.OnEnable();
            sub.OnClickEvent.RemoveAllListeners();
            add.OnClickEvent.RemoveAllListeners();
            sub.OnClickEvent.AddListener(() => ChangeValue(-setting.GetStep()));
            add.OnClickEvent.AddListener(() => ChangeValue(setting.GetStep()));
        }

        public override void UpdateUI()
        {
            base.UpdateUI();
            if (setting.GetLoop()) return;
            sub.interactable = setting.GetValue() >= setting.GetMinMax().x;
            add.interactable = setting.GetValue() <= setting.GetMinMax().y;
        }
    }
}