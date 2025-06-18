using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace ProjectCeros
{
    public class StarRatingVisualizer : MonoBehaviour
    {
        [Tooltip("Images for the filled part of the stars.")]
        [SerializeField] private List<Image> _filledStars; // 5 Items, each with type = Filled
        public float srating;

        void Update()
        {
            SetRating(srating);
        }
        public void SetRating(float rating)
        {
            for (int i = 0; i < _filledStars.Count; i++)
            {
                if (rating >= i + 1)
                {
                    _filledStars[i].fillAmount = 1f;
                }
                else if (rating > i)
                {
                    _filledStars[i].fillAmount = rating - i;
                }
                else
                {
                    _filledStars[i].fillAmount = 0f;
                }
            }
        }
    }
}
