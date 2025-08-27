/// <summary>
/// A Fader effect for the Dialogues that also changes the background images.
/// </summary>

/// <remarks>
/// 04/07/2025 by Unik Kelmendi: Initial creation.
/// </remarks>

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace ProjectCeros
{
    public class BackgroundFader : MonoBehaviour
    {
         
        public GameObject[] backgrounds;
        public Image fadeImage;
        public float fadeDuration = 1f;
        private int currentIndex = 0;
        private bool _once;

        public void Awake()
        {
            _once = false;
        }
        IEnumerator Start()
        {
            yield return new WaitForSeconds(0.5f); // Wait until rendering is done
            StartCoroutine(Fade(1f, 0f));
        }

        // This method gets called by other scripts to initiate the fade.
        public void ChangeBackground()
        {
            if (!_once)
            {
                backgrounds[currentIndex].SetActive(true);
                _once = true;
            }

            else
            {
                StartCoroutine(FadeRoutine());
            }
        }

        // This method handles the individual fadings playing in sequence.
        private IEnumerator FadeRoutine()
        {
            // Fade to black
            yield return StartCoroutine(Fade(0f, 1f));

            // Switch background while black screen is shown
            SetNextBackground();

            // Fade back in
            yield return StartCoroutine(Fade(1f, 0f));
        }

        // This method handles the actual fading.
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

        // This method changes the background while the screen is dark.
        private void SetNextBackground()
        {
            backgrounds[currentIndex].SetActive(false);
            currentIndex = (currentIndex + 1) % backgrounds.Length;
            backgrounds[currentIndex].SetActive(true);
        }
    }
}
