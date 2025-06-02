namespace IbrahKit
{
    public class Setting_Language : Setting
    {
        protected override void Init()
        {
            base.Init();
            maxValue = Localization_Manager.Instance.GetUsedLanguageAmount() - 1;
        }

        public override string GetDisplayValue()
        {
            return Localization_Manager.Instance.GetCurrentLanguageName();
        }

        public override void ApplyChanges()
        {
            base.ApplyChanges();

            value = Localization_Manager.Instance.GetNextUsableLanguage((int)value);
            Localization_Manager.Instance.UpdateLanguage();
        }
    }
}