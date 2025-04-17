/// <summary>
/// A ScriptableObject-based container for a string value that allows runtime value modification.
/// Includes utility methods for setting, appending and clearing the value.
/// </summary>

/// <remarks>
/// 09/04/2025 by Damian Dalinger: Script Creation.
/// </remarks>

using UnityEngine;

namespace ProjectCeros
{
    [CreateAssetMenu(menuName = "Variables/String Variable")]
    public class StringVariable : ScriptableObject
    {
#if UNITY_EDITOR
        [Multiline]
        public string DeveloperDescription = "";
#endif
        [SerializeField] private string value = "";

        public string Value
        {
            get => value;
            set => this.value = value;
        }

        #region Public Methods
        #region Append
        public void Append(string extra)
        {
            Value += extra;
        }

        public void Append(StringVariable extra)
        {
            Value += extra;
        }
        #endregion

        #region Clear
        public void Clear()
        {
            Value = "";
        }
        #endregion

        #region SetValue
        public void SetValue(string value)
        {
            Value = value;
        }

        public void SetValue(StringVariable value)
        {
            Value = value.Value;
        }
        #endregion
        #endregion
    }
}