using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace ProjectCeros
{
    public class BackgroundFader : MonoBehaviour
    {
        public Image fadeImage; // The black UI Image for fading
        public GameObject[] backgrounds; // Your different background objects
        private int currentIndex = 0;

        public float fadeDuration = 1f;

        private bool _once;

        public void Awake()
        {
            _once = false;
        }


        public void ChangeBackground()
        {

            if (!_once)
            {
                backgrounds[currentIndex].SetActive(true);
                _once = true;
            }

            StartCoroutine(FadeRoutine());
        }

        private IEnumerator FadeRoutine()
        {
            // Fade to black
            yield return StartCoroutine(Fade(0f, 1f));

            // Switch background while black screen is shown
            SetNextBackground();

            // Fade back in
            yield return StartCoroutine(Fade(1f, 0f));
        }

        private IEnumerator Fade(float from, float to)
        {
            float time = 0f;

            while (time < fadeDuration)
            {
                float alpha = Mathf.Lerp(from, to, time / fadeDuration);
                fadeImage.color = new Color(0, 0, 0, alpha);
                time += Time.deltaTime;
                yield return null;
            }

            fadeImage.color = new Color(0, 0, 0, to);
        }

        private void SetNextBackground()
        {
            backgrounds[currentIndex].SetActive(false);
            currentIndex = (currentIndex + 1) % backgrounds.Length;
            backgrounds[currentIndex].SetActive(true);
        }
    }
}
