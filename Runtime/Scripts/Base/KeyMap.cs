using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "NewKeyMap", menuName ="IbrahKit/Keymap")]
public class KeyMap : ScriptableObject
{
    public Key debugMenu;
    public Key hideUI;
    public Key screenshot;
    public Key screenshotNoUI;
}
