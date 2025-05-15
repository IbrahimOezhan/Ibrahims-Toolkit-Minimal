using UnityEngine;

namespace TemplateTools
{
    [CreateAssetMenu(fileName = "NewStyle", menuName = "ScriptableObjects/UI_Style")]
    public class UI_Style_SO : ScriptableObject
    {
        public UI_Style style = new();
    }
}