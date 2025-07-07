/// <summary>
/// This script limits the amount of invites the player can send.
/// </summary>

/// <remarks>
/// 25/06/2025 by Unik Kelmendi: Initial creation.
/// </remarks>


using UnityEngine;
using TMPro;

namespace ProjectCeros

{
    public class InvitationLimiter : MonoBehaviour

    {
        public IntReference InvitationLimit;

        public IntReference InvitationsSend;

        public GuestSO GuestToInvite;

        [SerializeField] private IntGameEvent _event;

        [SerializeField] private TextMeshProUGUI _requestedText;


        // Accesed by other scripts to tell InvitationLimiter which GuestSO is in question for inviting.
        public void SetGuestToInvite(GuestSO item)
        {
            GuestToInvite = item;
        }


        public void Start()
        {
            ShowRequestedText();
        }

        public void InviteGuest()
        {

            if (!GuestToInvite.isOnCooldown)

            {

                if (InvitationLimit.Value != InvitationsSend.Value)
                {
                    if (!GuestToInvite.isRequested)
                    {
                        _event.Raise(GuestToInvite.GuestID);

                        InvitationsSend.Variable.ApplyChange(1);

                        GuestToInvite.isRequested = true;

                        ShowRequestedText();

                    }

                    else
                        Debug.Log("You already send this person an invitation.");
                }

                else
                {
                    Debug.Log("No more Invitations for you, you lonely scaliwag!");

                }

            }

            else

            {

                Debug.Log("Guest is currently unavailable.");
            }

        }


        public void ShowRequestedText()
        {
            _requestedText.text = $"{InvitationsSend.Value}/{InvitationLimit.Value} Requests sent";
        }

    }
}