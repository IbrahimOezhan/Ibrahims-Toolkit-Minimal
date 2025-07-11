using IbrahKit;
using UnityEngine;
using UnityEngine.UI;
using Debug = IbrahKit.Debug;

public class UI_Fitter_Legacy : UI_Fitter
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

    public override void Execute()
    {
        if (!init) Init();
        if (!init) return;

        (Text text, RectTransform rect, UI_Fitter_Config config) = (GetText(), GetRect(), GetConfig());

        if (text == null)
        {
            Debug.LogWarning("Text reference is null");
            return;
        }

        if (rect == null)
        {
            Debug.LogWarning("Rect reference is null");
            return;
        }

        if (config == null)
        {
            Debug.LogWarning("Config reference is null");
            return;
        }

        if (scaleWidth) SetSize(text.preferredWidth, maxWidth, 0, config, RectTransform.Axis.Horizontal);
        if (scaleHeight) SetSize(text.preferredHeight, maxHeight, heightOffset, config, RectTransform.Axis.Vertical);
    }

    private Text GetText()
    {
        return text;
    }
}
