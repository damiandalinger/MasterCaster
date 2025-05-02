using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace ProjectCeros
{
    public class UnlockManager : MonoBehaviour
    {
        [Header("Initial Unlocked Tags")]
        [Tooltip("Tags that are unlocked at game start.")]
        [SerializeField] private List<string> _initiallyUnlockedTags;

        [Header("Available Article Databases")]
        [Tooltip("All important articles (imported from JSON).")]
        [SerializeField] private ArticleDatabase _allImportantArticles;

        [Tooltip("All random articles (imported from JSON).")]
        [SerializeField] private ArticleDatabase _allRandomArticles;

        [Header("Eligible Article Pools")]
        [Tooltip("Important articles eligible for selection.")]
        [SerializeField] private ArticleDatabase _eligibleImportantArticles;

        [Tooltip("Random articles eligible for selection.")]
        [SerializeField] private ArticleDatabase _eligibleRandomArticles;

        [Header("Debug Tools (Playmode Only)")]
        [Tooltip("Type a tag here and press Unlock Tag to test during playmode.")]
        [SerializeField] private string _debugTagToUnlock;

        private readonly List<string> _unlockedTags = new();

        #region Unity Lifecycle

        private void Awake()
        {
            _unlockedTags.Clear();
            _unlockedTags.AddRange(_initiallyUnlockedTags.Distinct());
            UpdateEligiblePools();
        }

        private void OnValidate()
        {
            // Ensures duplicates are cleaned at design time.
            _initiallyUnlockedTags = _initiallyUnlockedTags.Distinct().ToList();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Unlocks a new tag during runtime (e.g., from UI or debug).
        /// </summary>
        /// <param name="tag">The tag to unlock.</param>
        public void UnlockTag(string tag)
        {
            if (!_unlockedTags.Contains(tag))
            {
                _unlockedTags.Add(tag);
                UpdateEligiblePools();
                Debug.Log($"[UnlockManager] ✅ Unlocked tag: {tag}");
            }
            else
            {
                Debug.Log($"[UnlockManager] 🔁 Tag already unlocked: {tag}");
            }
        }

        #endregion

        #region Debug GUI (Editor Only)

#if UNITY_EDITOR
        [ContextMenu("🔓 Unlock Debug Tag")]
        private void DebugUnlockTag()
        {
            if (Application.isPlaying && !string.IsNullOrWhiteSpace(_debugTagToUnlock))
            {
                UnlockTag(_debugTagToUnlock.Trim().ToLower());
                _debugTagToUnlock = ""; // Clear after use
            }
        }
#endif

        #endregion

        #region Private Methods

        private void UpdateEligiblePools()
        {
            _eligibleImportantArticles.Items = _allImportantArticles.Items
                .Where(a => _unlockedTags.Contains(a.Subgenre)).ToList();

            _eligibleRandomArticles.Items = _allRandomArticles.Items
                .Where(a => _unlockedTags.Contains(a.Subgenre)).ToList();

            Debug.Log($"[UnlockManager] 🗂 Eligible pools updated. Total Tags: {_unlockedTags.Count}");
        }

        #endregion
    }
}