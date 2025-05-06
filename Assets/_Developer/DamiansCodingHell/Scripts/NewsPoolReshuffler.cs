using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace ProjectCeros
{
    public class NewsPoolReshuffler : MonoBehaviour
    {
        // Für UsesPair == true
        [SerializeField]
        [Range(0f, 1f)]
        private float pairedChanceOffset1 = 0.4f;

        [SerializeField]
        [Range(0f, 1f)]
        private float pairedChanceOffset2 = 0.7f;

        // Für UsesPair == false
        [SerializeField]
        [Range(0f, 1f)]
        private float unpairedChanceOffset1 = 0.3f;

        [SerializeField]
        [Range(0f, 1f)]
        private float unpairedChanceOffset2 = 0.6f;
        [Header("Genre Pool Pairs")]
        [Tooltip("List of genre pool pairs linking all articles with their eligible counterparts.")]
        [SerializeField] private List<GenrePoolPair> genrePoolPairs;

        [Header("Debugging")]
        [SerializeField] private bool debugLogging = false;

        private void Start()
        {
            RefreshAllPools();
            if (debugLogging)
                Debug.Log("[NewsPoolReshuffler] Initialized all pools.");
        }

        /// <summary>
        /// Context-Menü im Inspector: Alle Pools komplett neu aufbauen.
        /// </summary>
        [ContextMenu("Reshuffle All Pools")]
        public void RefreshAllPools()
        {
            foreach (var pair in genrePoolPairs)
            {
                BuildEligibleQueue(pair);
                if (debugLogging)
                    Debug.Log($"[NewsPoolReshuffler] 🔄 Full reshuffle for genre: {GetGenreName(pair)}");
            }
        }

        /// <summary>
        /// Context-Menü im Inspector: Prüft alle Pools und reshufflet nur die, die 'empty enough' sind.
        /// </summary>
        [ContextMenu("Reshuffle Pools If Needed")]
        public void ReshufflePoolsIfNeeded()
        {
            foreach (var pair in genrePoolPairs)
            {
                if (NeedsReshuffle(pair))
                {
                    BuildEligibleQueue(pair);
                    if (debugLogging)
                        Debug.Log($"[NewsPoolReshuffler] 🔄 Conditional reshuffle for genre: {GetGenreName(pair)}");
                }
            }
        }

        /// <summary>
        /// Kontext-Menü im Inspector: Gibt in der Console die fertigen Queues aus.
        /// </summary>
        [ContextMenu("Print Eligible Queues")]
        private void PrintEligibleQueues()
        {
            foreach (var pair in genrePoolPairs)
            {
                Debug.Log($"--- Eligible Queue for genre: {GetGenreName(pair)} ---");
                var items = pair.EligibleArticles.Items;
                for (int i = 0; i < items.Count; i++)
                {
                    var a = items[i];
                    Debug.Log(
                        $"{i}: {a.Headline} (StoryID={a.StoryID}, Part={a.StoryPart}, PairID={a.PairID})"
                    );
                }
            }
        }

        /// <summary>
        /// Entscheidet, ob ein Genre-Pool neu aufgebaut werden muss:
        /// true, wenn nur noch ≤1 Block übrig ist (einzelner Artikel oder Paar).
        /// </summary>
        private bool NeedsReshuffle(GenrePoolPair pair)
        {
            return pair.EligibleArticles.Items.Count == 0;
        }

        /// <summary>
        /// Baut für ein Genre die Eligible-Liste als flache Queue aus Blöcken auf,
        /// in der Story-Fortsetzungen bereits im 1- oder 2-Slot-Versatz eingeplant sind.
        /// </summary>
        private void BuildEligibleQueue(GenrePoolPair pair)
        {
            // 1) Baue Block-Liste (Paare & Singles)
            var blocks = new List<ArticleBlock>();

            // -- alle Paare (PairID > 0)
            var pairGroups = pair.AllArticles.Items
                .Where(a => a.PairID != 0)
                .GroupBy(a => a.PairID);
            foreach (var grp in pairGroups)
                blocks.Add(new ArticleBlock(grp.ToList()));

            // -- alle Einzel-Artikel (PairID == 0)
            var singleArticles = pair.AllArticles.Items
                .Where(a => a.PairID == 0);
            foreach (var art in singleArticles)
                blocks.Add(new ArticleBlock(new List<Article> { art }));

            // 2) Basis-Blocks: alle Nicht-Story + nur Part1 von Stories
            var baseBlocks = blocks
                .Where(b => b.StoryID == 0 || b.StoryPart == 1)
                .OrderBy(_ => Random.value)
                .ToList();

            // 3) Story-Lookup: StoryID → (StoryPart → Block)
            var storyLookup = blocks
                .Where(b => b.StoryID != 0)
                .GroupBy(b => b.StoryID)
                .Where(g => g.Select(b => b.StoryPart).Distinct().Count() > 1)
                .ToDictionary(
                    g => g.Key,
                    g => g.ToDictionary(b => b.StoryPart, b => b)
                );

            // 4) Für jede Story mit >1 Part: Part2..n an Versatz‐Positionen einfügen
            foreach (var kv in storyLookup)
            {
                var parts = kv.Value;
                // muss mindestens Part1 enthalten
                if (!parts.ContainsKey(1)) continue;

                // finde Part1-Block in baseBlocks
                var part1Block = parts[1];
                int idx = baseBlocks.IndexOf(part1Block);
                if (idx < 0) continue;

                // dann für Parts ab 2
                for (int partNum = 2; parts.ContainsKey(partNum); partNum++)
                {
                    var block = parts[partNum];
                    // 40% direkt drunter, sonst einen Slot weiter
                    float chance1 = pair.UsesPairs ? pairedChanceOffset1 : unpairedChanceOffset1;
                    float chance2 = pair.UsesPairs ? pairedChanceOffset2 : unpairedChanceOffset2;

                    // Offset-Logik
                    float rnd = Random.value;
                    int offset;
                    if (rnd < chance1)
                        offset = 1;
                    else if (rnd < chance2)
                        offset = 2;
                    else
                        offset = 3;

                    int insertPos = Mathf.Clamp(idx + offset, 0, baseBlocks.Count);
                    baseBlocks.Insert(insertPos, block);
                    idx = insertPos;
                }
            }

            // 5) „Flatten“: schreibe die Artikel-Blöcke in die Eligible-Liste
            var eligible = pair.EligibleArticles.Items;
            eligible.Clear();
            foreach (var block in baseBlocks)
                foreach (var art in block.Articles)
                    eligible.Add(art);
        }

        private string GetGenreName(GenrePoolPair pair)
            => pair.AllArticles != null ? pair.AllArticles.name : "(Unnamed Genre)";

        // Hilfsklasse: Ein Block ist entweder ein Paar (2 Varianten) oder ein einzelner Artikel.
        private class ArticleBlock
        {
            public List<Article> Articles { get; }
            public int StoryID => Articles[0].StoryID;
            public int StoryPart => Articles[0].StoryPart;
            public ArticleBlock(List<Article> articles) => Articles = articles;
        }
    }
}