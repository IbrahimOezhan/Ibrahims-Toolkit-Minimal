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
        private List<UI_Selectable> selectables = new();

        protected List<GameObject> spawnedMenuItems = new();
        protected List<GameObject> spawnedListMenuItems = new();
        protected List<GameObject> spawnedCustomMenuItems = new();

        [FoldoutGroup("Title"), SerializeField] private UI_Localization title;

        [FoldoutGroup("Title"), Dropdown("Localization"), SerializeField] private string titleKey;

        [FoldoutGroup("MenuItems"), SerializeField] private Transform list;

        [FoldoutGroup("MenuItems"), SerializeField] private List<Menu_Item> listMenuItems = new();

        [FoldoutGroup("MenuItems"), SerializeField] private Menu_Item_Custom[] customMenuItems;

        [FoldoutGroup("MenuItems"), SerializeField] private UI_Menu_Config customConfig;

        [SerializeField] private bool reloadOnOpen;

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

        protected override void OnMenuDisable()
        {
            base.OnMenuDisable();

            foreach (UI_Selectable selectable in selectables)
            {
                UI_Navigation_Manager.Instance.RemoveSelectable(selectable);
            }

            UI_Navigation_Manager.Instance.UpdateSelectables();
        }

        protected override void OnMenuEnabled()
        {
            base.OnMenuEnabled();

            selectables = Transform_Utilities.GetChildren<UI_Selectable>(transform);

            foreach (UI_Selectable selectable in selectables)
            {
                UI_Navigation_Manager.Instance.AddSelectable(selectable);
            }

            UI_Navigation_Manager.Instance.UpdateSelectables();
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
            foreach (Menu_Item_Custom menuItem in customMenuItems)
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