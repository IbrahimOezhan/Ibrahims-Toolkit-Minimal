using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace TemplateTools
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

        private void OnValidate()
        {
            foreach (var element in listMenuItems)
            {
                element.OnTypeChanged();
            }
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
                if (menuItem.skip) continue;

                if (SpawnMenuItem(menuItem, _settings, list, out GameObject _instance))
                {
                    spawnedListMenuItems.Add(_instance);
                }
            }
        }

        private void SpawnCustomMenuItems(List<Setting> _settings)
        {
            foreach (Menu_Item_Custom menuItem in customMenuItems)
            {
                if (menuItem.skip) continue;

                if (SpawnMenuItem(menuItem, _settings, hiddenGroup.transform, out GameObject _instance))
                {
                    spawnedCustomMenuItems.Add(_instance);
                    spawnedCustomMenuItems[^1].transform.position = menuItem.spawn.position;
                }
            }
        }

        public bool SpawnMenuItem(Menu_Item menuItem, List<Setting> _settings, Transform parent)
        {
            return SpawnMenuItem(menuItem, _settings, parent, out GameObject _instance);
        }

        public bool SpawnMenuItem(Menu_Item menuItem, List<Setting> _settings, Transform parent, out GameObject _goInstance)
        {
            _goInstance = null;

            if (menuItem.layoutSpecific && !UI_Manager.Instance.ShowLayout(menuItem.layout)) return false;

            UI_Menu_Config config = customConfig != null ? customConfig : UI_Manager.Instance.GetDefaultMenuConfig();

            switch(menuItem.menuType)
            {
                case Menu_Item_Type.SETTING:
                    Setting _foundSetting;

                    if (menuItem.setting.settingType == SettingsInterfaceType.REFERENCE)
                    {
                        _foundSetting = menuItem.setting.reference;
                        if (_foundSetting == null)
                        {
                            UnityEngine.Debug.LogWarning("Setting reference is null or not assigned");
                            return false;
                        }
                    }
                    else
                    {
                        if (!Settings_Manager.Instance.GetSetting(menuItem.setting.settingsKey, out _foundSetting))
                        {
                            return false;
                        }
                    }

                    if (!_settings.Contains(_foundSetting))
                    {
                        _settings.Add(_foundSetting);

                        UI_Setting _prefab = config.settingPrefabs.Find(x => x.settingType == _foundSetting.GetSettingsType());

                        if (_prefab != null)
                        {
                            UI_Setting instance = Instantiate(_prefab, parent);
                            instance.interfaceType = menuItem.setting.settingType;

                            if (instance.interfaceType == SettingsInterfaceType.REFERENCE) instance.setting = menuItem.setting.reference;
                            else instance.settingKey = _foundSetting.GetKey();

                            instance.UpdateUI();

                            _goInstance = instance.gameObject;

                            spawnedMenuItems.Add(_goInstance);

                            return true;
                        }
                        else
                        {
                            UnityEngine.Debug.LogWarning("No Prefab found");
                            return false;
                        }
                    }
                    break;
                default:
                    UI_Menu_Button b = Instantiate(config.menuButtonPrefab, parent);

                    switch (menuItem.menuType)
                    {
                        case Menu_Item_Type.MENUREF:
                            (UI_Menu_Basic menuReference, int transitionReference) = menuItem.menu.GetMenu();
                            if (transitionReference == -1) b.Initialize(menuItem.localizationKey).AddListener(() => MenuTransition(menuReference));
                            else b.Initialize(menuItem.localizationKey).AddListener(() => MenuTransition(transitionReference));
                            break;
                        case Menu_Item_Type.CUSTOM:
                            UnityEvent uevent = menuItem.customEvent;
                            b.Initialize(menuItem.localizationKey).AddListener(() => uevent.Invoke());
                            break;
                        case Menu_Item_Type.BACK:
                            b.Initialize(menuItem.localizationKey).AddListener(() => Back());
                            break;
                        case Menu_Item_Type.QUIT:
                            b.Initialize(menuItem.localizationKey).AddListener(() => Application.Quit());
                            break;
                    }

                    _goInstance = b.gameObject;

                    spawnedMenuItems.Add(_goInstance);

                    return true;

            }

            return false;
        }

        public Menu_Item GetMenuItemByLocalKey(string key)
        {
            return listMenuItems.Find(x => x.localizationKey == key);
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