using System.Collections;
using UnityEngine;

namespace IbrahKit
{
    public class UI_Dots : MonoBehaviour
    {
        private string dots = "";
        private const float initialDelay = 0.5f;

        [SerializeField] private float delay;
        [SerializeField] private UI_Localization dotsText;

        private void Awake()
        {
            dotsText.SetParam(new() { dots });
        }

        private void OnEnable()
        {
            StartCoroutine(DotsAnimator());
        }

        private IEnumerator DotsAnimator()
        {
            if (dotsText != null)
            {
                yield return new WaitForSeconds(initialDelay);

                while (true)
                {
                    dots = dots == "..." ? "" : dots + ".";
                    dotsText.SetParam(new() { dots });
                    yield return new WaitForSeconds(delay);
                }
            }
            else
            {
                Debug.LogWarning($"{nameof(dotsText)} is null");
            }
        }
    }
}