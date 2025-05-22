using UnityEngine;
using UnityEngine.UI;

namespace TemplateTools
{
    [RequireComponent(typeof(CanvasRenderer))]
    public class UI_CursorHandler : Graphic, ICursorHandler
    {
        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear(); // Prevents any actual rendering
        }

        public override void SetMaterialDirty() { }
        public override void SetVerticesDirty() { }
    }
}