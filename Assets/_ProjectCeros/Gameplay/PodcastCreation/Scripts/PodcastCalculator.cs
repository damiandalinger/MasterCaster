/// <summary>
/// Handles podcast creation, listener gain calculation, and user feedback display based on genre and subgenre selection.
/// </summary>

/// <remarks>
/// 20/05/2025 by Damian Dalinger: Initial creation.
/// </remarks>

using System.Linq;
using NUnit.Framework.Interfaces;
using UnityEngine;

namespace ProjectCeros
{
    public class PodcastCalculator : MonoBehaviour
    {

        [SerializeField] private FloatReference _equipmentMod;
        [SerializeField] private FloatReference _sponsorMod;
        [SerializeField] private FloatReference _guestMod;
        [SerializeField] private FloatReference _darkWebMod;
        [SerializeField] private FloatReference _subgenreMod;
        [SerializeField] private FloatReference _sizeMod;
        [SerializeField] private FloatReference _previousListenerMod;
        [SerializeField] private FloatReference _wrongGenrePenalty;
        [SerializeField] private IntReference _currentListeners;
        [SerializeField] private ArticleDatabase _selectedImportantArticles;
        [SerializeField] private PodcastResult _result;

        // Calculates listener growth based on current selections and article match data.
        public void Calculate(PodcastInputData input)
        {
            float baseListeners = _currentListeners.Value;
            float baseValue = 2 + (baseListeners * _previousListenerMod.Value);

            float guest = _guestMod.Value;
            float equip = _equipmentMod.Value;
            float sponsor = _sponsorMod.Value;
            float dark = _darkWebMod.Value;
            float subgenre = 0f;
            float other = 1 + _sizeMod.Value;

            float topicMod = _wrongGenrePenalty.Value;
            bool genreMatched = false;
            bool subgenreMatched = false;

            var article = _selectedImportantArticles.Items.FirstOrDefault(a => a.PairID / 1000 == input.Genre);
            if (article != null)
            {
                topicMod = input.Spin == 1 ? article.ValuePositive : article.ValueNegative;
                genreMatched = true;

                if (!string.IsNullOrEmpty(article.Subgenre) && article.Subgenre == input.Subgenre)
                {
                    subgenre = _subgenreMod.Value;
                    subgenreMatched = true;
                }
            }

            float bonusMult = guest + equip + sponsor + dark + other + subgenre;
            float afterBonus = baseValue * bonusMult;
            float finalValue = afterBonus * topicMod;

            int totalListeners = Mathf.CeilToInt(finalValue);
            int gain = totalListeners - (int)baseListeners;
            int gainAfterBonus = Mathf.CeilToInt(afterBonus - baseListeners);

            // Create minimal result
            _result.OverwriteWith(
       totalListeners,
       gain,
       gainAfterBonus,
       guest,
       equip,
       sponsor,
       dark,
       subgenre,
       other,
       bonusMult,
       topicMod
   );

            // DEBUG LOG (vollständig)
            Debug.Log(
                "--- Podcast Evaluation Debug ---\n" +
                $"Selected Genre: {GetGenreName(input.Genre)} (Matched: {genreMatched})\n" +
                $"Selected Spin: {(input.Spin == 1 ? "Positive" : "Negative")}\n" +
                $"Selected Subgenre: {input.Subgenre} (Matched: {subgenreMatched})\n\n" +

                $"Base Listeners: {baseListeners}\n" +
                $"PreviousListenersMod: {_previousListenerMod.Value}\n" +
                $"BaseValue = 2 + (BaseListeners * PreviousListenersMod) = {baseValue:F2}\n\n" +

                $"--- Additive Modifiers ---\n" +
                $"Guest: +{guest}\n" +
                $"Equipment: +{equip}\n" +
                $"Sponsor: +{sponsor}\n" +
                $"Dark Web: +{dark}\n" +
                $"Other: +{other}\n" +
                $"Subgenre Bonus: +{subgenre}\n" +
                $"Final Multiplier = 1 + sum = {bonusMult:F2}\n" +
                $"After Additives = BaseValue * FinalMult = {afterBonus:F2}\n" +
                $"GainAfterBonus = AfterAdditives - BaseListeners = {afterBonus - baseListeners:F2}\n\n" +

                $"--- Topic Modifier ---\n" +
                $"Topic Mod: x{topicMod:F2}\n" +
                $"Final = AfterAdditives * TopicMod = {finalValue:F2}\n\n" +

                $"Final Listeners: {totalListeners}\n" +
                $"Total Gain: {(gain >= 0 ? "+" : "")}{gain}\n" +
                "------------------------------"
            );
        }

        public void ApplyFinalListenerGain()
        {
            _currentListeners.Variable.SetValue(_result.TotalListeners);
        }

        private string GetGenreName(int genreId)
        {
            return genreId switch
            {
                1 => "Action",
                2 => "Indie",
                3 => "RPG",
                4 => "Shooter",
                5 => "Simulation",
                6 => "Strategy",
                _ => "Unknown"
            };
        }
    }
}