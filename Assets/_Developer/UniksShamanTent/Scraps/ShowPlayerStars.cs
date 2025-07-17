using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectCeros
{
       public class ShowPlayerStats : MonoBehaviour
    {
        public Transform starParent;

        public IntReference Stars;

        public float fillSpeed = 2f; // How fast each star fills (units per second)

        public void ShowStarRating()
        {
            StopAllCoroutines();
            StartCoroutine(FillStarsSmoothly());
        }

        private IEnumerator FillStarsSmoothly()
        {
            int targetStars = Mathf.Clamp(Stars, 0, starParent.childCount);

            // Reset all stars to empty
            for (int i = 0; i < starParent.childCount; i++)
            {
                var image = starParent.GetChild(i).GetComponent<Image>();
                image.type = Image.Type.Filled;
                image.fillAmount = 0f;
            }

            // Smoothly fill each star
            for (int i = 0; i < targetStars; i++)
            {
                var image = starParent.GetChild(i).GetComponent<Image>();
                image.type = Image.Type.Filled;

                while (image.fillAmount < 1f)
                {
                    image.fillAmount += Time.deltaTime * fillSpeed;
                    yield return null;
                }

                image.fillAmount = 1f;
            }
        }
    }



}

