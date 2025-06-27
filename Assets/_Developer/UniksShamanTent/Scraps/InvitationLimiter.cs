using UnityEngine;

namespace ProjectCeros

{
    public class InvitationLimiter : MonoBehaviour

    {
        public IntReference InvitationLimit;

        public IntReference InvitationsSend;

        public GuestSO GuestToInvite;

        [SerializeField] private IntGameEvent _event;




        // Accesed by other scripts to tell InvitationLimiter which GuestSO is in question for inviting.
        public void SetGuestToInvite(GuestSO item)
        {
            GuestToInvite = item;
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

    }
}