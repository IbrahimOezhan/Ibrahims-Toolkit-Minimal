using IbrahKit;
using QFSW.QC;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using UnityEngine;
using Debug = IbrahKit.Debug;

[DefaultExecutionOrder(-1)]
public class Unlockables_Manager : MonoBehaviour
{
    private const string saveDataName = "Unlockables";

    [SerializeField] private SaveData saveData = new();
    [SerializeField] private List<Unlockable> unlockables = new();

    public static Unlockables_Manager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;

            DontDestroyOnLoad(gameObject);

            saveData = (SaveData)Save_Manager.currentFolder.LoadObject(saveDataName, new SaveData());
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Save_Manager.currentFolder.SaveObject(saveDataName, saveData);
        }
    }

    public void LoadUnlockables(IEnumerable<Unlockable> list)
    {
        List<Unlockable> newUnlockables = new(list);

        unlockables.AddRange(newUnlockables.Except(unlockables));
    }

    public void Unlock(List<Unlockable> unlockable)
    {
        unlockable.ForEach(x => Unlock(x));
    }

    public void Unlock(Unlockable unlockable)
    {
        Unlock(unlockable.GetKey());
    }

    public void Unlock(string key)
    {
        saveData.Unlock(key);
    }

    public void ListUnlockables()
    {
        StringBuilder sb = new();

        sb.AppendLine("\nAll Unlockables:");

        for (int i = 0; i < unlockables.Count; i++)
        {
            sb.AppendLine("Key: " + unlockables[i].GetKey() + " Unlocked? " + unlockables[i].IsUnlocked());
        }

        Debug.Log(sb.ToString());
    }

    public bool IsUnlocked(string key)
    {
        return saveData.IsUnlocked(key);
    }

    [System.Serializable]
    private class SaveData : Savable
    {
        [JsonInclude][SerializeField] private List<string> unlockedUnlockables = new();

        public bool IsUnlocked(string key)
        {
            return unlockedUnlockables.Contains(key);
        }

        public void Unlock(string key)
        {
            if (!unlockedUnlockables.Contains(key))
            {
                unlockedUnlockables.Add(key);
                Debug.Log("Unlocked " + key);
            }
        }
    }
}
