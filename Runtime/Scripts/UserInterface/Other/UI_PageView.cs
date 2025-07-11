using System;
using UnityEngine;
using UnityEngine.Events;

namespace IbrahKit
{
    public class UI_PageView : MonoBehaviour
    {
        private int currentPageIndex;
        private int amountPerPage;
        private int maxPageIndex;
        private UnityEvent<int> OnPageChanged;

        [SerializeField] private UI_Selectable left;
        [SerializeField] private UI_Selectable right;
        [SerializeField] private UI_Localization pageText;
        [SerializeField] private Transform pageContent;

        private void Awake()
        {
            if (left == null)
            {
                Debug.LogWarning($"{nameof(left)} is null");
            }
            else
            {
                left.OnClickEvent.AddListener(GoLeft);
            }

            if (right == null)
            {
                Debug.LogWarning($"{nameof(right)} is null");
            }
            else
            {
                right.OnClickEvent.AddListener(GoRight);
            }

            UpdateUI();
        }

        public void Initialize(int amount, int amountPerPage)
        {
            int i = 0;

            int maxPageIndex = 0;

            while (i < amount)
            {
                i++;

                if (i == (amountPerPage * (maxPageIndex + 1))) maxPageIndex++;
            }

            currentPageIndex = 0;

            this.amountPerPage = amountPerPage;

            this.maxPageIndex = maxPageIndex;

            UpdateUI();
        }

        public void GoLeft() => ChangePage(-1);
        public void GoRight() => ChangePage(1);

        public void ChangePage(int dir)
        {
            foreach (Transform child in pageContent)
            {
                Destroy(child.gameObject);
            }

            currentPageIndex += dir;

            currentPageIndex = Mathf.Clamp(currentPageIndex, 0, maxPageIndex);

            OnPageChanged?.Invoke(currentPageIndex);

            UpdateUI();
        }

        public void UpdateUI()
        {
            if(pageText == null)
            {
                Debug.LogWarning($"{nameof(pageText)} is null");
            }
            else
            {
                pageText.SetParam(new() { (currentPageIndex + 1 + "/" + (maxPageIndex + 1)).ToString() });
            }

            if (left == null)
            {
                Debug.LogWarning($"{nameof(left)} is null");
            }
            else
            {
                left.gameObject.SetActive(currentPageIndex != 0);
            }

            if(right == null)
            {
                Debug.LogWarning($"{nameof(right)} is null");
            }
            else
            {
                right.gameObject.SetActive(currentPageIndex != maxPageIndex);
            }
        }

        public UnityEvent<int> GetOnPageChanged()
        {
            return OnPageChanged;
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