using IbrahKit;
using Sirenix.OdinInspector;
using UnityEngine;
using Debug = IbrahKit.Debug;

namespace IbrahKit
{
    [CreateAssetMenu(fileName = "NewUnlockable", menuName = "ScriptableObjects/Unlockable")]
    public class Unlockable : ScriptableObject
    {
        [TabGroup("Localization")]
        [Dropdown(Localization_Manager.KEY)]
        [SerializeField] protected string key;

        [SerializeField] private Unlockable[] unlockOnUnlock;

        public virtual void Unlock()
        {
            if (IsUnlocked()) return;

            if (unlockOnUnlock != null)
            {
                for (int i = 0; i < unlockOnUnlock.Length; i++)
                {
                    if (unlockOnUnlock[i] == null)
                    {
                        Debug.LogWarning(nameof(unlockOnUnlock) + " contains null values");
                        continue;
                    }
                    unlockOnUnlock[i].Unlock();
                }
            }

            Unlockables_Manager.Instance.Unlock(this);
        }

        public bool IsUnlocked()
        {
            return Unlockables_Manager.Instance.IsUnlocked(key);
        }

        public string GetKey()
        {
            return key;
        }
    }
}