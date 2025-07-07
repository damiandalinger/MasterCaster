using UnityEngine;

namespace ProjectCeros
{


    public class InvitationReseter : MonoBehaviour
    {
        public IntReference InvitationsSend;


        public void ResetInvitation()
        {
            InvitationsSend.Variable.SetValue(0);

        }
    }
}