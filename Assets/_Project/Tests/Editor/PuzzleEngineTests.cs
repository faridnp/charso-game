using System.Collections.Generic;
using NUnit.Framework;
using Charso.Core;

namespace Charso.Tests
{
    public sealed class PuzzleEngineTests
    {
        PuzzleDefinition CreateBookPuzzle() => new PuzzleDefinition
        {
            Id = "fa.word.book.001", Revision = 1, Clue = "چیزی برای خواندن",
            LetterTiles = new[] { "ک", "ت", "ا", "ب" },
            AcceptedAnswers = new[] { "کتاب", "كِتاب" }, Difficulty = 1, EditorialStatus = "approved"
        };

        [Test]
        public void ValidPuzzle_PassesDefinitionValidation()
        {
            Assert.IsTrue(PuzzleEngine.IsDefinitionValid(CreateBookPuzzle(), out string error), error);
        }

        [Test]
        public void AnswerAcceptsArabicCharacterAndDiacriticVariants()
        {
            Assert.IsTrue(PuzzleEngine.IsAccepted(CreateBookPuzzle(), "كِتاب"));
        }

        [Test]
        public void InvalidAnswerIsRejected()
        {
            Assert.IsFalse(PuzzleEngine.IsAccepted(CreateBookPuzzle(), "دفتر"));
        }

        [Test]
        public void ComposeRejectsReusingSameTileIndex()
        {
            string result = PuzzleEngine.Compose(new[] { "ک", "ت" }, new List<int> { 0, 0 });
            Assert.AreEqual(string.Empty, result);
        }

        [Test]
        public void DefinitionRejectsAnswerThatCannotBeFormed()
        {
            PuzzleDefinition puzzle = CreateBookPuzzle();
            puzzle.AcceptedAnswers = new[] { "دفتر" };
            Assert.IsFalse(PuzzleEngine.IsDefinitionValid(puzzle, out _));
        }
    }
}