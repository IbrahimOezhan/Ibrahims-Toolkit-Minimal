using UnityEngine;
using UnityEngine.UI;

namespace IbrahKit
{
    public class UI_Text_Setter_Legacy : UI_Text_Setter
    {
        [SerializeField] private Text text;

        protected override void Init()
        {
            if (text == null && !TryGetComponent(out text))
            {
                return;
            }

            base.Init();
        }

        public override void SetText(string text)
        {
            if (!init) Init();
            if (!init) return;

            this.text.text = text;

            UpdateUI();
        }
    }
}