using System;
using UnityEngine;
using UnityEngine.Events;

namespace TemplateTools
{
    public class UI_PageView : MonoBehaviour
    {
        private int currentPage;
        private int perPage;

        [SerializeField] private int maxPage;
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

        public void Left() => ChangePage(-1);
        public void Right() => ChangePage(1);

        public void ChangePage(int dir)
        {
            foreach (Transform child in pageContent)
            {
                Destroy(child.gameObject);
            }
            currentPage += dir;
            currentPage = Mathf.Clamp(currentPage, 0, maxPage);
            OnPageChanged?.Invoke(currentPage);
            OnPageChangedEvent?.Invoke(currentPage);
            UpdateUI();
        }

        public int GetCurrentPage()
        {
            return currentPage;
        }

        public void SetContentAmount(int amount, int contentPerPage)
        {
            perPage = contentPerPage;

            int pages = amount / perPage;

            Debug.Log("Am: " + amount + " Per: " + contentPerPage + " Pages: " + pages);

            if (amount % contentPerPage != 0) pages++;
            pages--;

            currentPage = 0;
            maxPage = pages;
            UpdateUI();
        }

        public void UpdateUI()
        {
            if (pageText != null) pageText.SetParam(new() { (currentPage + 1 + "/" + (maxPage + 1)).ToString() });
            left.gameObject.SetActive((currentPage != 0));
            right.gameObject.SetActive(currentPage != maxPage);
        }

        public Transform GetPageContent()
        {
            return pageContent;
        }

        public (int, int) GetIndexRange(int maxIndex)
        {
            int startIndex = perPage * currentPage;
            int endIndex = Mathf.Clamp(startIndex + perPage, startIndex, maxIndex);

            return (startIndex, endIndex);
        }
    }
}