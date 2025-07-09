using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace ProjectCeros
{
    public class Fader : MonoBehaviour
    {
        public Image fadeImage; // The black UI Image for fading
        public GameObject[] backgrounds; // Your different background objects

        public float fadeDuration = 1f;



        public void Start()
        {
            FadeIn();
            
        }



        public void FadeToBlack()
        {
            StartCoroutine(Fade(0f, 1f));      
        }

        public void FadeIn()
        {
            StartCoroutine(Fade(0f, 1f));      
        }



        private IEnumerator FadeRoutine()
        {
            // Fade to black
            yield return StartCoroutine(Fade(0f, 1f));

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


    }
}
