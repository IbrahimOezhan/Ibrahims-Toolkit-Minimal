public class OverrideFloat
{
    private float baseValue;
    private float? overrideValue = null;

    public OverrideFloat(float baseValue)
    {
        this.baseValue = baseValue;
    }

    public void SetOverride(float overrideBaseSpeed)
    {
        this.overrideValue = overrideBaseSpeed;
    }

    public void ClearOverride()
    {
        this.overrideValue = null;
    }

    public float GetValue()
    {
        return overrideValue != null ? overrideValue.Value : baseValue;
    }
}
