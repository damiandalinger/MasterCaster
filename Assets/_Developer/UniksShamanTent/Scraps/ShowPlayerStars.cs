using UnityEngine;
using UnityEngine.UI;

namespace ProjectCeros
{
    public class ShowPlayerStats : MonoBehaviour
    {

        public Transform starParent;

        public Sprite filledStar;
        public Sprite emptyStar;

        public IntReference Stars;


        public void Update()
        {
            ShowStarRating(Stars.Value);
        }

        public void ShowStarRating(int rating)
        {
            for (int i = 0; i < starParent.childCount; i++)
            {
                var image = starParent.GetChild(i).GetComponent<Image>();
                image.sprite = (i < rating) ? filledStar : emptyStar;
            }
        }

      }



}

