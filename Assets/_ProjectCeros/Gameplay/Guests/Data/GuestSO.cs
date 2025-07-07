/// <summary>
/// This script stores all information for the Guests.
/// </summary>

/// <remarks>
/// 25/06/2025 by Unik Kelmendi: Initial creation.
/// </remarks>

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

        [Tooltip("The probability for the guest to come")]
        public float Chance;


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

        [Tooltip("If true, the guest's preferred topic is shown in the UI.")]
        public bool wasInterviewed;

        [Tooltip("The Color the text has when talking to the guest")]
        public Color Color;


    }



}