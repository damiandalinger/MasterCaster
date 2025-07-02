using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace ProjectCeros
{

    public class UIFader : MonoBehaviour
    {
        public CanvasGroup canvasGroup;     // Reference to the UI's CanvasGroup
        public float fadeDuration = 1f;     // Duration of the fade-out
        public float visibleTime = 1f;      // Time it stays fully visible before fading

        public void TriggerFade()
        {
            StopAllCoroutines(); // In case it's already fading
            StartCoroutine(FadeOutRoutine());
        }

        private IEnumerator FadeOutRoutine()
        {
            canvasGroup.alpha = 1;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;

            // Ensure it's visible
            canvasGroup.gameObject.SetActive(true);

            // Wait before starting to fade
            yield return new WaitForSeconds(visibleTime);

            float elapsed = 0f;

            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
                yield return null;
            }

            canvasGroup.alpha = 0;
            canvasGroup.gameObject.SetActive(false); // Optional: hide it completely after fade
        }
    }
}
