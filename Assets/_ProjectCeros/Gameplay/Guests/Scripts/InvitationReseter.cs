/// <summary>
/// This script resets the invitation send count at the end of the day.
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