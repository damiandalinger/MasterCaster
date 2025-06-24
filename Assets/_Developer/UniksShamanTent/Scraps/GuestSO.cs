using UnityEngine;

namespace ProjectCeros


{
    [CreateAssetMenu(menuName = "Guests/Guest")]
    public class GuestSO : ScriptableObject
    {
        [Tooltip("The name of the guest.")]
        public string Name;

        [Tooltip("The description of the guest.")]
        [TextArea(3, 10)]
        public string Description;

        [Tooltip("The preferred topic of the Guest")]
        public string Topic;


        [Tooltip("The Bonus this guest provides to the podcast.")]
        public float Modifier;


        [Tooltip("The unique ID of the guest.")]
        public int GuestID;

        [Tooltip("The days that cooldown remains active.")]
        public int CooldownCounter;

        [Tooltip("The star rating of the guest used to determine likelihood of acccepting an invite.")]
        public int Rating;

        [Tooltip("The (base) amount of money the player needs to pay when the guest wants to get paid.")]
        public int Fee;

        [Tooltip("The preferred topic of the Guest determined by id.")]
        public int TopicID;



        [Tooltip("The picture of the guest.")]
        public Sprite GuestSprite;


        [Tooltip("If true, the player sent an invite to the guest.")]
        public bool isRequested;

        [Tooltip("If true, the guest is available for the podcast.")]
        public bool hasAccepted;

        [Tooltip("If true, the guest is currently unavailable.")]
        public bool isOnCooldown;






    }



}