using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace TemplateTools
{
    public class UI_Setting_Range : UI_Setting
    {
        [FoldoutGroup("UI"), SerializeField] private UI_Selectable sub;
        [FoldoutGroup("UI"), SerializeField] private UI_Selectable add;

        protected override void Enable()
        {
            base.Enable();
            sub.OnClickEvent.RemoveAllListeners();
            add.OnClickEvent.RemoveAllListeners();
            sub.OnClickEvent.AddListener(() => ChangeValue(-setting.steps));
            add.OnClickEvent.AddListener(() => ChangeValue(setting.steps));
        }

        public override void UpdateUI()
        {
            base.UpdateUI();
            if (setting.loop) return;
            sub.interactable = setting.value >= setting.GetMinMax().x;
            add.interactable = setting.value <= setting.GetMinMax().y;
        }
    }

    [System.Serializable]
    public class RangeData
    {
        public UI_Localization title;
        public UI_Localization value;
        public Button sub;
        public Button add;
    }
}