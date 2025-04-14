/// <summary>
/// Place for the summary.
/// </summary>

/// <remarks>
/// 09/04/2025 by Your Name: Script Creation.
/// </remarks>

using UnityEngine;


namespace ProjectCeros
{
    public class FloatReferenceTester : MonoBehaviour
    {
        // Inspector-Zuweisung (für Variable-Objekte)
        public FloatReference Health;
        public GameEvent PlayerDied;

        void Start()
        {



            PlayerDied.Raise();



            Health.Variable.ApplyChange(-55f);
        }
        public void SayHi()
        {
            Debug.Log("Hi");
        }
}


}

    