using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TemplateTools
{
    [ExecuteInEditMode]
    public class Manager_Manager : MonoBehaviour
    {
#if UNITY_EDITOR
        [ReadOnly, SerializeField] private List<GameObject> managers = new();
        [ReadOnly, SerializeField] private List<string> names = new();

        [ListDrawerSettings(OnTitleBarGUI = "DrawRefreshButton"), ReadOnly, SerializeField]
        private List<GameObject> spawnedManagers = new();

        [HorizontalGroup("Add"), HideLabel, Dropdown("TemplateManagers"), SerializeField]
        private string ManagerToAdd;

        private void OnTransformChildrenChanged()
        {
            StartCoroutine(DelayedUpdate());
        }

        [HorizontalGroup("Add"), Button]
        public void Add()
        {
            GameObject ob = managers.Find(x => x.name == ManagerToAdd);
            if (ob != null)
            {
                if (spawnedManagers.Find(x => x.name == ManagerToAdd) != null)
                {
                    UnityEngine.Debug.LogWarning("Object of the same type already exists");
                }
                else
                {
                    GameObject sOb = (GameObject)PrefabUtility.InstantiatePrefab(ob, transform);
                    spawnedManagers.Add(sOb);
                    sOb.name = ManagerToAdd;
                }
            }
        }

        [Button]
        public void Initialize()
        {
            managers.Clear();
            names.Clear();
            Addressables.LoadAssetsAsync<GameObject>("IbrahTemplate", OnAssetLoaded).Completed += OnAllAssetsLoaded;
        }

        private void OnAssetLoaded(GameObject loadedAsset)
        {
            managers.Add(loadedAsset);
            names.Add(loadedAsset.name);
            Debug.Log("Loaded Asset: " + loadedAsset.name);
        }

        private void OnAllAssetsLoaded(AsyncOperationHandle<IList<GameObject>> handle)
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                Debug.Log("All assets loaded successfully!");
            }
            else
            {
                Debug.LogError("Failed to load all assets!");
            }

            names.Sort((string one, string two) =>
            {
                return one.CompareTo(two);
            });

            String_Utilities.CreateDropdown(names, "TemplateManagers");
        }

        public void DrawRefreshButton()
        {
            if(gameObject.activeInHierarchy)
            {
                StartCoroutine(DelayedUpdate());
            }
        }

        private IEnumerator DelayedUpdate()
        {
            yield return new WaitForSeconds(.1f);

            spawnedManagers.Clear();

            foreach (Transform child in transform)
            {
                spawnedManagers.Add(child.gameObject);
            }
        }

        [Button]
        public void SortManangers()
        {
            spawnedManagers.Sort((GameObject one, GameObject two) =>
            {
                return one.name.CompareTo(two.name);
            });

            for (int i = 0; i < spawnedManagers.Count; i++)
            {
                spawnedManagers[i].transform.SetSiblingIndex(i);
            }
        }
#endif
    }
}