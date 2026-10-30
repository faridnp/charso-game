using System;

namespace Charso.Core
{
    [Serializable]
    public sealed class PuzzleDefinition
    {
        public string Id;
        public int Revision = 1;
        public string Clue;
        public string[] LetterTiles = Array.Empty<string>();
        public string[] AcceptedAnswers = Array.Empty<string>();
        public string[] HintStages = Array.Empty<string>();
        public int Difficulty = 1;
        public string EditorialStatus = "draft";
    }
}