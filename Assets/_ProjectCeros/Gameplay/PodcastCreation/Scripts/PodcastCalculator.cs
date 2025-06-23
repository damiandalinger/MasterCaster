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

            float baseBonus = 1f;
            float guest = _guestMod.Value;
            float equip = _equipmentMod.Value;
            float sponsor = _sponsorMod.Value;
            float dark = _darkWebMod.Value;
            float subgenre = 0f;
            float sizeMult = CalculateSizeModifier(_currentListeners.Value);
            _sizeMod.Variable.SetValue(sizeMult);

            float topicMult = _wrongGenrePenalty.Value;
            bool genreMatched = false;
            bool subgenreMatched = false;

            var article = _selectedImportantArticles.Items.FirstOrDefault(a => a.PairID / 1000 == input.Genre);
            if (article != null)
            {
                topicMult = input.Spin == 1 ? article.ValuePositive : article.ValueNegative;
                genreMatched = true;

                if (!string.IsNullOrEmpty(article.Subgenre) && article.Subgenre == input.Subgenre)
                {
                    subgenre = _subgenreMod.Value;
                    subgenreMatched = true;
                }
            }

            float bonusMult = baseBonus + guest + equip + sponsor + dark + subgenre;
            float finalValue = baseValue * topicMult * bonusMult * sizeMult;

            int totalListeners = Mathf.CeilToInt(finalValue);
            int gain = totalListeners - (int)baseListeners;

            // Create minimal result
            _result.OverwriteWith(
       totalListeners,
       gain,
       baseBonus,
       guest,
       equip,
       sponsor,
       dark,
       subgenre,
       bonusMult,
       topicMult
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
                $"Base: +{baseBonus}\n" +
                $"Guest: +{guest}\n" +
                $"Equipment: +{equip}\n" +
                $"Sponsor: +{sponsor}\n" +
                $"Dark Web: +{dark}\n" +
                $"Subgenre Bonus: +{subgenre}\n" +
                $"Final Multiplier = 1 + sum = {bonusMult:F2}\n" +
                $"Size Multiplier: x{sizeMult}\n" +
                $"--- Topic Modifier ---\n" +
                $"Topic Multiplier: x{topicMult:F2}\n" +
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

        private static float CalculateSizeModifier(int currentListeners)
        {
            if (currentListeners <= 1000)
            {
                Debug.Log("1");
                return -0.00015f * currentListeners + 1.15f;
            }
            else if (currentListeners <= 10000)
            {
                Debug.Log("2");
                return -0.000010001f * currentListeners + 0.9899889989f;
            }
            else if (currentListeners <= 100000)
            {
                Debug.Log("3");
                return -0.00000300003f * currentListeners + 0.86999669996f;
            }
            else if (currentListeners <= 3000000)
            {
                Debug.Log("4");
                return -1.33333378e-7f * currentListeners + 0.58666652888f;
            }
            else
            {
                Debug.Log("5");
                return 0.2f;
            }
        }
    }
}