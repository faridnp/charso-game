using NUnit.Framework;
using Charso.Core;
using System.Collections.Generic;

namespace Charso.Tests
{
    public sealed class WordPuzzleContentTests
    {
        [Test]
        public void OpeningArcAndExtendedBankContainTwoHundredTwentyUniqueIds()
        {
            Assert.AreEqual(220, WordPuzzle.Levels.Length);
            var ids = new HashSet<int>();
            foreach (PuzzleLevel level in WordPuzzle.Levels)
                Assert.IsTrue(ids.Add(level.Id), "Duplicate puzzle ID: " + level.Id);
            for (int i = 0; i < WordPuzzle.Levels.Length; i++)
                Assert.AreEqual(i + 1, WordPuzzle.Levels[i].Id, "Puzzle IDs must be contiguous.");
        }

        [Test]
        public void EveryAnswerCanBeFormedFromItsExactLetterInventory()
        {
            foreach (PuzzleLevel level in WordPuzzle.Levels)
            {
                var definition = new PuzzleDefinition
                {
                    Id = "charso.word." + level.Id,
                    Revision = 1,
                    Clue = level.Clue,
                    LetterTiles = level.Letters,
                    AcceptedAnswers = new[] { level.Answer },
                    EditorialStatus = "draft"
                };
                Assert.IsTrue(PuzzleEngine.IsDefinitionValid(definition, out string error),
                    "Puzzle " + level.Id + " (" + level.Answer + "): " + error);
            }
        }

        [Test]
        public void ExtendedBankContainsTwoHundredUniqueAnswersWithMetadata()
        {
            var answers = new HashSet<string>();
            for (int i = 20; i < WordPuzzle.Levels.Length; i++)
            {
                PuzzleLevel level = WordPuzzle.Levels[i];
                Assert.IsTrue(answers.Add(PersianText.NormalizeForMatch(level.Answer)),
                    "Duplicate extended answer: " + level.Answer);
                Assert.IsFalse(string.IsNullOrWhiteSpace(level.Clue), "Missing clue at ID " + level.Id);
                Assert.IsTrue(level.Difficulty >= 1 && level.Difficulty <= 3, "Invalid difficulty at ID " + level.Id);
                Assert.IsFalse(string.IsNullOrWhiteSpace(level.Topic), "Missing topic at ID " + level.Id);
            }
            Assert.AreEqual(200, answers.Count);
        }

        [Test]
        public void FullClearCoinEarningsMatchTheDraftCityProjectBudget()
        {
            int extendedCoins = 0;
            for (int i = 20; i < WordPuzzle.Levels.Length; i++)
                extendedCoins += RewardEconomy.PuzzleReward(WordPuzzle.Levels[i].Difficulty);
            Assert.AreEqual(1730, extendedCoins);
            int projectCosts = 0;
            foreach (CityProjectDefinition project in RewardEconomy.CityProjects)
                projectCosts += project.CoinCost;
            Assert.AreEqual(1770, projectCosts);
            Assert.AreEqual(1890, extendedCoins + (20 * 8));
        }

        [Test]
        public void RestorationMilestonesAreLimitedToThreeVisibleTransitions()
        {
            for (int i = 0; i < WordPuzzle.Levels.Length; i++)
                Assert.AreEqual(i < 15 && (i + 1) % 5 == 0, WordPuzzle.RequiresRestoration(i), "Level index " + i);
        }
    }
}