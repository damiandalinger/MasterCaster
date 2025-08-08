/// <summary>
/// Displays the most recent string entry from a StringRuntimeSet in a text field.
/// </summary>

/// <remarks>
/// 30/06/2025 by Damian Dalinger: Script Creation.
/// </remarks>

using TMPro;
using UnityEngine;

namespace ProjectCeros
{
    public class LatestStringDisplay : MonoBehaviour
    {
        [Tooltip("The runtime set containing strings (e.g. titles) to monitor.")]
        [SerializeField] private StringRuntimeSet _stringSet;

        [Tooltip("The TextMeshPro element used to display the latest string.")]
        [SerializeField] private TMP_Text _targetText;

        private void OnEnable()
        {
            if (_stringSet == null || _stringSet.Items.Count == 0)
            {
                _targetText.text = "No Title";
                return;
            }

            string latest = _stringSet.Items[_stringSet.Items.Count - 1];
            _targetText.text = latest;
        }
    }
}
