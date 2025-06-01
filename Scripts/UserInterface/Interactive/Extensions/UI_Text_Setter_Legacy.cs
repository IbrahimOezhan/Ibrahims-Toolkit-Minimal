using UnityEngine;
using UnityEngine.UI;

public class UI_Text_Setter_Legacy : UI_Text_Setter
{
    [SerializeField] private Text text;

    protected override void Init()
    {
        if (text == null) text = GetComponent<Text>();
        base.Init();
    }

    public override void SetText(string text)
    {
        if (!init) Init();

        this.text.text = text;

        UpdateUI();
    }
}
