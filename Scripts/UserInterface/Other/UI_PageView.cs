using System;
using UnityEngine;
using UnityEngine.Events;

namespace TemplateTools
{
    public class UI_PageView : MonoBehaviour
    {
        private int currentPageIndex;
        private int amountPerPage;
        private int maxPageIndex;

        [SerializeField] private UI_Selectable left;
        [SerializeField] private UI_Selectable right;
        [SerializeField] private UI_Localization pageText;
        [SerializeField] private Transform pageContent;

        public Action<int> OnPageChanged;
        public UnityEvent<int> OnPageChangedEvent;

        private void Awake()
        {
            left.OnClickEvent.AddListener(Left);
            right.OnClickEvent.AddListener(Right);
            UpdateUI();
        }

        public void Initialize(int amount, int amountPerPage)
        {
            int am = 0;
            int pages = 0;

            while (am < amount)
            {
                am += amountPerPage;
                pages++;
            }

            if (am > amount)
            {
                pages++;
            }

            currentPageIndex = 0;
            this.amountPerPage = amountPerPage;
            this.maxPageIndex = pages;

            Debug.Log(this.maxPageIndex.ToString());

            UpdateUI();
        }

        public void Left() => ChangePage(-1);
        public void Right() => ChangePage(1);

        public void ChangePage(int dir)
        {
            foreach (Transform child in pageContent)
            {
                Destroy(child.gameObject);
            }
            currentPageIndex += dir;
            currentPageIndex = Mathf.Clamp(currentPageIndex, 0, maxPageIndex);
            OnPageChanged?.Invoke(currentPageIndex);
            OnPageChangedEvent?.Invoke(currentPageIndex);
            UpdateUI();
        }

        public void UpdateUI()
        {
            if (pageText != null) pageText.SetParam(new() { (currentPageIndex + 1 + "/" + (maxPageIndex + 1)).ToString() });
            left.gameObject.SetActive((currentPageIndex != 0));
            right.gameObject.SetActive(currentPageIndex != maxPageIndex);
        }

        public Transform GetPageContent()
        {
            return pageContent;
        }

        public (int, int) GetIndexRange(int maxIndex)
        {
            int startIndex = amountPerPage * currentPageIndex;
            int endIndex = Mathf.Clamp(startIndex + amountPerPage, startIndex, maxIndex);

            return (startIndex, endIndex);
        }

        public int GetCurrentPage()
        {
            return currentPageIndex;
        }
    }
}