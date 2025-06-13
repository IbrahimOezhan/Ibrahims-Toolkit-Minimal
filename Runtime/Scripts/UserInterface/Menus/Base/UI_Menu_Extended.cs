using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace IbrahKit
{
    /// <summary>
    /// The base for all menus for containing UI
    /// </summary>
    public partial class UI_Menu_Extended : UI_Menu_Basic
    {
        [FoldoutGroup("Title")]
        [Tooltip("Localization component for the menu title.")]
        [SerializeField] private UI_Localization title;

        [ShowIf("@title != null")]
        [FoldoutGroup("Title")]
        [Dropdown("Localization"), Tooltip("Key used to localize the menu title.")]
        [SerializeField] private string titleKey;

        [FoldoutGroup("MenuItems")]
        [Tooltip("Parent transform for list menu items.")]
        [SerializeField] private Transform list;

        [ShowIf("@list != null")]
        [FoldoutGroup("MenuItems")]
        [Tooltip("List of predefined menu items.")]
        [SerializeField] private List<Menu_Item> listMenuItems = new();

        [FoldoutGroup("MenuItems")]
        [Tooltip("Array of custom menu items.")]
        [SerializeField] private Custom_Menu_Item[] customMenuItems;

        [FoldoutGroup("MenuItems")]
        [Tooltip("Custom menu configuration, optional.")]
        [SerializeField] private UI_Menu_Config customConfig;

        [FoldoutGroup("Settings")]
        [Tooltip("If true, reload menu items every time the menu is opened.")]
        [SerializeField] private bool reloadOnOpen;

        // Internal spawned items tracking
        [ShowInInspector, ReadOnly, FoldoutGroup("Spawned Items")]
        protected List<GameObject> spawnedMenuItems = new();

        [ShowInInspector, ReadOnly, FoldoutGroup("Spawned Items")]
        protected List<GameObject> spawnedListMenuItems = new();

        [ShowInInspector, ReadOnly, FoldoutGroup("Spawned Items")]
        protected List<GameObject> spawnedCustomMenuItems = new();

        private void OnDrawGizmos()
        {
            for (int i = 0; i < customMenuItems.Length; i++)
            {
                customMenuItems[i].OnDrawGizmos(GetComponentInChildren<Canvas>());
            }
        }

        protected override void Awake()
        {
            base.Awake();

            if (title) title.SetKey(titleKey);

            if (!reloadOnOpen) LoadMenuItems();
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            if (reloadOnOpen) ReloadMenu();
        }

        public void ReloadMenu()
        {
            ClearMenuItems();
            LoadMenuItems();
        }

        private void ClearMenuItems()
        {
            foreach (var item in spawnedMenuItems)
            {
                Destroy(item);
            }
            spawnedMenuItems.Clear();
            spawnedListMenuItems.Clear();
            spawnedCustomMenuItems.Clear();
        }

        private void LoadMenuItems()
        {
            StartCoroutine(LoadMenuItemsRoutine());
        }

        private IEnumerator LoadMenuItemsRoutine()
        {
            List<Setting> _settings = new();

            SpawnListItems(_settings);

            SpawnCustomMenuItems(_settings);

            InitMenuContent();

            MenuUpdate();

            SendMessage("OnMenuLoaded", null, SendMessageOptions.DontRequireReceiver);

            yield return null;

            UI_Navigation_Manager.Instance.UpdateSelectables();
        }

        private void SpawnListItems(List<Setting> _settings)
        {
            foreach (Menu_Item menuItem in listMenuItems)
            {
                if (SpawnMenuItem(menuItem, list as RectTransform, out GameObject _instance))
                {
                    spawnedListMenuItems.Add(_instance);
                    spawnedMenuItems.Add(_instance);
                }
            }
        }

        private void SpawnCustomMenuItems(List<Setting> _settings)
        {
            foreach (Custom_Menu_Item menuItem in customMenuItems)
            {
                if (SpawnMenuItem(menuItem, hiddenGroup.transform as RectTransform, out GameObject _instance))
                {
                    menuItem.SetRectTransform(_instance.transform as RectTransform);
                    spawnedCustomMenuItems.Add(_instance);
                    spawnedMenuItems.Add(_instance);
                }
            }
        }

        public UI_Menu_Config GetMenuConfig()
        {
            return customConfig != null ? customConfig : UI_Manager.Instance.GetDefaultMenuConfig();
        }

        public bool SpawnMenuItem(Menu_Item menuItem, RectTransform parent, out GameObject _goInstance)
        {
            _goInstance = null;

            _goInstance = menuItem.Spawn(parent, this);

            return _goInstance != null;
        }

        public virtual void Back()
        {
            if (overrideBackMenu != null)
            {
                MenuTransition(overrideBackMenu);
                overrideBackMenu = null;
            }
            else MenuTransitionToPrevious();
        }

        public void SetParams(CanvasGroup enabled, CanvasGroup hidden, Transform list)
        {
            enabledGroup = enabled;
            hiddenGroup = hidden;
            this.list = list;
        }
    }
}
