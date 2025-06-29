using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace IbrahKit
{
    public class Debug_Manager : MonoBehaviour
    {
        private List<Debug_Item> items = new();
        private List<IDebug> debugs = new();

        [SerializeField] private Text debugContent;

        [SerializeField] private GameObject debugContainer;
        [SerializeField] private KeyMap keyMap;

        public bool disableLogs;

        public static Debug_Manager Instance;

        public static bool bufferLogs;
        public static bool s_disableLogs;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
        }

        private void Update()
        {
            if (Keyboard.current[keyMap.debugMenu].wasPressedThisFrame)
            {
                if (debugContainer.activeInHierarchy)
                {
                    debugContainer.SetActive(false);
                }
                else debugContainer.SetActive(true);
            }
            s_disableLogs = disableLogs;
        }

        private void FixedUpdate()
        {
            if (debugContainer.activeInHierarchy)
            {
                foreach (var debug in debugs)
                {
                    debug.Run();
                }

                StringBuilder sb = new();

                foreach (var item in items)
                {
                    sb.Append(item.content);
                    sb.AppendLine();
                }

                string s = sb.ToString();

                if (String_Utilities.IsEmpty(s))
                {
                    debugContent.text = "No Information";
                }
                else debugContent.text = s;
            }
        }

        public void Add(Debug_Item item, IDebug debug)
        {
            items.Add(item);
            debugs.Add(debug);
            items.Sort((a, b) =>
            {
                return a.order.CompareTo(b.order);
            });
        }

        public void Remove(Debug_Item item, IDebug debug)
        {
            items.Remove(item);
            debugs.Remove(debug);
        }
    }
}