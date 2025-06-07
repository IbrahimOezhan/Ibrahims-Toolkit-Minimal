using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;


namespace IbrahKit
{
    [ExecuteInEditMode]
    public class Toolkit_Manager : MonoBehaviour
    {
        [HorizontalGroup("Add"), HideLabel, Dropdown("TemplateManagers"), SerializeField]
        private string ManagerToAdd;

        [ReadOnly, SerializeField] private List<GameObject> managers = new();
        [ReadOnly, SerializeField] private List<string> names = new();

        [ListDrawerSettings(OnTitleBarGUI = "DrawRefreshButton"), ReadOnly, SerializeField]
        private List<GameObject> spawnedManagers = new();

        private void OnValidate()
        {
            if (gameObject.scene.IsValid())
            {
                if (ManagerToAdd != "None")
                {
                    GameObject ob = managers.Find(x => x.name == ManagerToAdd);
                    if (ob != null)
                    {
                        if (spawnedManagers.Find(x => x.name == ManagerToAdd) != null)
                        {
                            Debug.LogWarning("Object of the same type already exists");
                        }
                        else
                        {
#if UNITY_EDITOR
                            GameObject sOb = (GameObject)UnityEditor.PrefabUtility.InstantiatePrefab(ob, transform);
                            spawnedManagers.Add(sOb);
                            sOb.name = ManagerToAdd;
#endif
                        }
                    }
                }
            }

            ManagerToAdd = "None";
        }

        private void OnTransformChildrenChanged()
        {
            StartCoroutine(DelayedUpdate());
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
                Debug.LogWarning("Failed to load all assets!");
            }

            names.Sort((string one, string two) =>
            {
                return one.CompareTo(two);
            });

            if (names.Count > 0)
            {
                names.Insert(0, "None");
            }
            else names.Add("None");

            String_Utilities.CreateDropdown(names, "TemplateManagers");
        }

        private void DrawRefreshButton()
        {
            if (gameObject.activeInHierarchy)
            {
                StartCoroutine(DelayedUpdate());
            }
            else
            {
                Debug.Log("Object not active");
            }
        }

        [Button]
        public void Initialize()
        {
            managers.Clear();
            names.Clear();
            Addressables.LoadAssetsAsync<GameObject>("IbrahTemplate", OnAssetLoaded).Completed += OnAllAssetsLoaded;
        }

        [Button]
        public void SortManangers()
        {
            Transform_Utilities.SortGameobjects(spawnedManagers);
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
    }
}