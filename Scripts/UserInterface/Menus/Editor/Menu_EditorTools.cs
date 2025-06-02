using IbrahKit;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace IbrahKit
{
    public class Menu_EditorTools
    {
        [MenuItem("GameObject/Template/Basic Menu", false, 10)]
        static void CreateBasicMenu(MenuCommand menuCommand)
        {
            GameObject go = new GameObject("Basic Menu");

            GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);

            UI_Menu_Basic basicMenu = go.AddComponent<UI_Menu_Basic>();

            GameObject canvas = new GameObject("Canvas");
            GameObjectUtility.SetParentAndAlign(canvas, go);

            Canvas _canvas = canvas.AddComponent<Canvas>();

            _canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            _canvas.vertexColorAlwaysGammaSpace = true;

            CanvasScaler _canvasScaler = canvas.AddComponent<CanvasScaler>();
            _canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;

            GraphicRaycaster raycaster = canvas.AddComponent<GraphicRaycaster>();

            GameObject enabledObject = new GameObject("Enabled");
            GameObjectUtility.SetParentAndAlign(enabledObject, canvas);

            RectTransform rectEn = enabledObject.AddComponent<RectTransform>();

            Transform_Utilities.SetRectStretchMode(rectEn);

            CanvasGroup enabledGroup = enabledObject.AddComponent<CanvasGroup>();

            GameObject hiddenObject = new GameObject("Hidden");
            GameObjectUtility.SetParentAndAlign(hiddenObject, enabledObject);

            RectTransform rectHid = hiddenObject.AddComponent<RectTransform>();

            Transform_Utilities.SetRectStretchMode(rectHid);

            CanvasGroup hiddenGroup = hiddenObject.AddComponent<CanvasGroup>();

            basicMenu.SetParams(enabledGroup, hiddenGroup);

            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            Selection.activeObject = go;
        }

        [MenuItem("GameObject/Template/Extended Menu", false, 10)]
        static void CreateExtendedMenu(MenuCommand menuCommand)
        {
            GameObject go = new GameObject("Extended Menu");

            GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);

            UI_Menu_Extended extendedMenu = go.AddComponent<UI_Menu_Extended>();

            GameObject canvas = new GameObject("Canvas");
            GameObjectUtility.SetParentAndAlign(canvas, go);

            Canvas _canvas = canvas.AddComponent<Canvas>();

            _canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            _canvas.vertexColorAlwaysGammaSpace = true;

            CanvasScaler _canvasScaler = canvas.AddComponent<CanvasScaler>();
            _canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;

            GraphicRaycaster raycaster = canvas.AddComponent<GraphicRaycaster>();

            GameObject enabledObject = new GameObject("Enabled");
            GameObjectUtility.SetParentAndAlign(enabledObject, canvas);

            RectTransform rect = enabledObject.AddComponent<RectTransform>();

            Transform_Utilities.SetRectStretchMode(rect);

            CanvasGroup enabledGroup = enabledObject.AddComponent<CanvasGroup>();

            GameObject hiddenObject = new GameObject("Hidden");
            GameObjectUtility.SetParentAndAlign(hiddenObject, enabledObject);

            RectTransform rect2 = hiddenObject.AddComponent<RectTransform>();

            Transform_Utilities.SetRectStretchMode(rect2);

            CanvasGroup hiddenGroup = hiddenObject.AddComponent<CanvasGroup>();

            GameObject menuItemList = new GameObject("MenuItem List");
            GameObjectUtility.SetParentAndAlign(menuItemList, hiddenObject);

            RectTransform rect3 = menuItemList.AddComponent<RectTransform>();

            Transform_Utilities.SetRectStretchMode(rect3);

            VerticalLayoutGroup layoutGroup = menuItemList.AddComponent<VerticalLayoutGroup>();

            extendedMenu.SetParams(enabledGroup,hiddenGroup, menuItemList.transform);

            layoutGroup.childForceExpandWidth = false;
            layoutGroup.childForceExpandHeight = false;

            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            Selection.activeObject = go;
        }
    }
}