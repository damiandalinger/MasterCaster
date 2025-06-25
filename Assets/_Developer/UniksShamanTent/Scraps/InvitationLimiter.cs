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
            if (InvitationLimit.Value != InvitationsSend.Value)
            {
                if (!GuestToInvite.isRequested)
                {
                    _event.Raise(GuestToInvite.GuestID);

                    InvitationsSend.Variable.ApplyChange(1);
                }

                else
                    Debug.Log("You already send this person an invtiation.");
            }

            else
            {
                Debug.Log("No more Invitations for you, you lonely scaliwag!");

            }

        }

    }
}