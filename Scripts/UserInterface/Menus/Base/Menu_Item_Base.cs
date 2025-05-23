using UnityEngine;

namespace TemplateTools
{
    public abstract class Menu_Item_Base
    {
        [SerializeField] protected GameObject spawnedObject;

        public GameObject GetSpawnedObject()
        {
            return spawnedObject;
        }

        public abstract void Spawn(RectTransform parent, UI_Menu_Extended menu);
    }
}