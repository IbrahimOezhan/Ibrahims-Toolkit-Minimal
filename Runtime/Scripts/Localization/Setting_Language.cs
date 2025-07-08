namespace IbrahKit
{
    public class Setting_Language : Setting
    {
        public override void Init(string initialValue)
        {
            base.Init(initialValue);
            SetValueRange(new(GetValueRange().x, Localization_Manager.Instance.GetUsedLanguageAmount() - 1));
        }

        public override string GetDisplayValue()
        {
            return Localization_Manager.Instance.GetCurrentLanguageName();
        }

        public override void ApplyChanges()
        {
            base.ApplyChanges();

            SetValue(Localization_Manager.Instance.GetNextUsableLanguage((int)GetValue()));
            Localization_Manager.Instance.UpdateLanguage();
        }
    }
}