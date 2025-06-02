using System.Collections;
using UnityEngine;

namespace IbrahKit
{
    public class UI_Dots : MonoBehaviour
    {
        private string dots = "";
        private const float waitInterval = 0.5f;

        [SerializeField] private UI_Localization dotsText;
        [SerializeField] private float delay;

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
            yield return new WaitForSeconds(waitInterval);

            while (true)
            {
                dots = dots == "..." ? "" : dots + ".";
                dotsText.SetParam(new() { dots });
                yield return new WaitForSeconds(delay);
            }
        }
    }
}