/// <summary>
/// Handles podcast listener calculation and evaluation based on genre and article matchups.
/// </summary>

/// <remarks>
/// 20/06/2025 by Damian Dalinger: Initial creation.
/// 27/06/2025 by Damian Dalinger: Tech Bible refactor.
/// </remarks>

using System.Linq;
using UnityEngine;

namespace ProjectCeros
{
    public class PodcastCalculator : MonoBehaviour
    {
        #region Fields

        [Header("Modifier References")]
        [Tooltip("Guest segment bonus.")]
        [SerializeField] private FloatReference _guestMod;

        [Tooltip("Equipment quality bonus.")]
        [SerializeField] private FloatReference _equipmentMod;

        [Tooltip("Sponsor segment bonus.")]
        [SerializeField] private FloatReference _sponsorMod;

        [Tooltip("Dark web segment bonus.")]
        [SerializeField] private FloatReference _darkWebMod;

        [Tooltip("Subgenre match bonus.")]
        [SerializeField] private FloatReference _subgenreMod;

        [Tooltip("Previous audience growth modifier.")]
        [SerializeField] private FloatReference _previousListenerMod;

        [Tooltip("Default penalty multiplier if genre is wrong.")]
        [SerializeField] private FloatReference _wrongGenrePenalty;

        [Tooltip("Listener size scaling modifier.")]
        [SerializeField] private FloatReference _sizeMultiplier;

        [Header("Runtime Values")]
        [SerializeField] private IntReference _currentListeners;

        [Tooltip("Selected genre ID.")]
        [SerializeField] private IntVariable _selectedGenre;

        [Tooltip("Selected spin (1 = positive, 2 = negative).")]
        [SerializeField] private IntVariable _selectedSpin;

        [Tooltip("Selected subgenre ID.")]
        [SerializeField] private IntVariable _selectedSubgenre;

        [Header("Data Sources")]
        [Tooltip("The RUntimeSet storing the selected articles from the current newspaper.")]
        [SerializeField] private ArticleDatabase _selectedImportantArticles;

        [Tooltip("The ScriptableObject which stores the results of this script.")]
        [SerializeField] private PodcastResult _result;

        #endregion

        #region Public Methods

        // Performs the full listener calculation based on selected genre, modifiers and article matches.
        public void Calculate()
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
            _sizeMultiplier.Variable.SetValue(sizeMult);

            float topicMult = _wrongGenrePenalty.Value;
            bool genreMatched = false;
            bool subgenreMatched = false;

            var article = _selectedImportantArticles.Items.FirstOrDefault(a => a.PairID / 1000 == _selectedGenre.RuntimeValue);
            if (article != null)
            {
                topicMult = _selectedSpin.RuntimeValue == 1 ? article.ValuePositive : article.ValueNegative;
                genreMatched = true;

                if (article.Subgenre > 0 && article.Subgenre == _selectedSubgenre.RuntimeValue)
                {
                    subgenre = _subgenreMod.Value;
                    subgenreMatched = true;
                }
            }

            float bonusMult = baseBonus + guest + equip + sponsor + dark + subgenre;
            float finalValue = baseValue * topicMult * bonusMult * sizeMult;

            int totalListeners = Mathf.CeilToInt(finalValue);
            int gain = totalListeners - (int)baseListeners;

            _result.OverwriteWith(totalListeners, gain, baseBonus, guest, equip, sponsor, dark, subgenre, bonusMult, topicMult);
            PrintDebugOutput(baseListeners, baseValue, guest, equip, sponsor, dark,
                                       subgenre, baseBonus, topicMult, sizeMult,
                                       finalValue, totalListeners, gain, genreMatched, subgenreMatched);
        }

        #endregion

        #region Private Methods

        // Calculates a dynamic modifier based on current audience size.
        private static float CalculateSizeModifier(int currentListeners)
        {
            if (currentListeners <= 1000)
            {
                return -0.00015f * currentListeners + 1.15f;
            }
            else if (currentListeners <= 10000)
            {
                return -0.000010001f * currentListeners + 0.9899889989f;
            }
            else if (currentListeners <= 100000)
            {
                return -0.00000300003f * currentListeners + 0.86999669996f;
            }
            else if (currentListeners <= 3000000)
            {
                return -1.33333378e-7f * currentListeners + 0.58666652888f;
            }
            else
            {
                return 0.2f;
            }
        }

        // Outputs detailed calculation breakdown to console for debugging.
        private void PrintDebugOutput(
           float baseListeners, float baseValue,
           float guest, float equip, float sponsor, float darkWeb, float subgenre,
           float bonusMult, float topicMult, float sizeMult,
           float final, int total, int gain, bool genreMatched, bool subgenreMatched)
        {
            Debug.Log(
                "--- Podcast Evaluation Debug ---\n" +
                $"Selected Genre: {GetGenreName(_selectedGenre.RuntimeValue)} (Matched: {genreMatched})\n" +
                $"Selected Spin: {(_selectedSpin.RuntimeValue == 1 ? "Positive" : "Negative")}\n" +
                $"Selected Subgenre: {GetSubgenreDisplayName(_selectedSubgenre.RuntimeValue)} (Matched: {subgenreMatched})\n\n" +

                $"Base Listeners: {baseListeners}\n" +
                $"PreviousListenersMod: {_previousListenerMod.Value}\n" +
                $"BaseValue = 2 + (BaseListeners * PreviousListenersMod) = {baseValue:F2}\n\n" +

                $"--- Additive Modifiers ---\n" +
                $"Guest: +{guest}\n" +
                $"Equipment: +{equip}\n" +
                $"Sponsor: +{sponsor}\n" +
                $"Dark Web: +{darkWeb}\n" +
                $"Subgenre: +{subgenre}\n" +
                $"Total Bonus Multiplier: {bonusMult:F2}\n\n" +

                $"Size Multiplier: x{sizeMult:F2}\n" +
                $"Topic Multiplier: x{topicMult:F2}\n" +
                $"Final Value: {final:F2}\n" +

                $"Total Listeners: {total}\n" +
                $"Gain: {(gain >= 0 ? "+" : "")}{gain}\n" +
                "------------------------------"
            );
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

        private string GetSubgenreDisplayName(int subgenreId)
        {
            return subgenreId switch
            {
                1 => "FPS",
                2 => "Hero Shooter",
                3 => "Loot Shooter",
                4 => "Fighting Game",
                5 => "Stealth Game",
                6 => "Hack & Slash",
                7 => "Souls Like",
                8 => "Open World",
                9 => "MMORPG",
                10 => "RTS",
                11 => "Grand Strategy",
                12 => "TBS",
                13 => "Sport",
                14 => "Living Simulation",
                15 => "Job Simulation",
                16 => "Farming Game",
                17 => "Side Scroller",
                18 => "Roguelike",
                _ => "Unknown"
            };
        }

        #endregion
    }
}