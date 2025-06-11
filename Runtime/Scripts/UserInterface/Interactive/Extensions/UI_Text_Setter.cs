namespace IbrahKit
{
    public abstract class UI_Text_Setter : UI_Extension
    {
        public abstract void SetText(string text);

        public override int GetOrder()
        {
            return 0;
        }
    }
}