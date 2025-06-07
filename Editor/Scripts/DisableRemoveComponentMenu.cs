using UnityEditor;

public class DisableRemoveComponentMenu
{
    [MenuItem("CONTEXT/UI_Extension/Remove Component", true)]
    private static bool DisableRemoveComponent(MenuCommand command)
    {
        return false; // disables the "Remove Component" context menu item
    }
}
