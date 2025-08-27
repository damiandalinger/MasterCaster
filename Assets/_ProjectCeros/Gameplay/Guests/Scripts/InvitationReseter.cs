/// <summary>
/// This script looks at the requested guests and starts the process of determing if they accept or not.
/// Also handles guest cooldown.
/// </summary>

/// <remarks>
/// 25/06/2025 by Unik Kelmendi: Initial creation.
/// </remarks>

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