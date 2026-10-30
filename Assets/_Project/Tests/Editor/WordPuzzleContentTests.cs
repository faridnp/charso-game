using NUnit.Framework;
using Charso.Core;

namespace Charso.Tests
{
    public sealed class WordPuzzleContentTests
    {
        [Test]
        public void FirstChapterContainsTwentyUniquePuzzleIds()
        {
            Assert.AreEqual(20, WordPuzzle.Levels.Length);
            var ids = new System.Collections.Generic.HashSet<int>();
            foreach (PuzzleLevel level in WordPuzzle.Levels)
                Assert.IsTrue(ids.Add(level.Id), "Duplicate puzzle ID: " + level.Id);
        }

        [Test]
        public void EveryAuthoredAnswerCanBeFormedFromItsTiles()
        {
            foreach (PuzzleLevel level in WordPuzzle.Levels)
            {
                var definition = new PuzzleDefinition
                {
                    Id = "charso.chapter1." + level.Id,
                    Revision = 1,
                    Clue = level.Clue,
                    LetterTiles = level.Letters,
                    AcceptedAnswers = new[] { level.Answer },
                    EditorialStatus = "review"
                };
                Assert.IsTrue(PuzzleEngine.IsDefinitionValid(definition, out string error),
                    "Puzzle " + level.Id + " (" + level.Answer + "): " + error);
            }
        }

        [Test]
        public void RestorationIsRequiredOnlyAtFivePuzzleMilestones()
        {
            for (int i = 0; i < WordPuzzle.Levels.Length; i++)
                Assert.AreEqual((i + 1) % 5 == 0, WordPuzzle.RequiresRestoration(i), "Level index " + i);
        }
    }
}