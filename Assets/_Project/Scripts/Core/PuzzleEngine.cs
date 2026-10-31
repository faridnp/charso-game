using System;
using System.Collections.Generic;
using System.Text;

namespace Charso.Core
{
    public static class PuzzleEngine
    {
        public static bool IsDefinitionValid(PuzzleDefinition puzzle, out string error)
        {
            if (puzzle == null) { error = "Puzzle is null."; return false; }
            if (string.IsNullOrWhiteSpace(puzzle.Id)) { error = "Stable puzzle ID is required."; return false; }
            if (puzzle.Revision < 1) { error = "Puzzle revision must be positive."; return false; }
            if (string.IsNullOrWhiteSpace(puzzle.Clue)) { error = "Clue is required."; return false; }
            if (puzzle.LetterTiles == null || puzzle.LetterTiles.Length == 0) { error = "At least one letter tile is required."; return false; }
            if (puzzle.AcceptedAnswers == null || puzzle.AcceptedAnswers.Length == 0) { error = "At least one accepted answer is required."; return false; }
            for (int i = 0; i < puzzle.LetterTiles.Length; i++)
                if (string.IsNullOrWhiteSpace(puzzle.LetterTiles[i])) { error = "Letter tiles cannot be empty."; return false; }
            for (int i = 0; i < puzzle.AcceptedAnswers.Length; i++)
            {
                string answer = PersianText.NormalizeForMatch(puzzle.AcceptedAnswers[i]);
                if (answer.Length == 0) { error = "Accepted answers cannot be empty."; return false; }
                if (!CanForm(puzzle.LetterTiles, answer)) { error = "Accepted answer cannot be formed from the supplied tiles: " + puzzle.AcceptedAnswers[i]; return false; }
            }
            error = null;
            return true;
        }

        public static string Compose(string[] tiles, IList<int> selectedIndices)
        {
            if (tiles == null || selectedIndices == null) return string.Empty;
            var text = new StringBuilder();
            var used = new HashSet<int>();
            for (int i = 0; i < selectedIndices.Count; i++)
            {
                int index = selectedIndices[i];
                if (index < 0 || index >= tiles.Length || !used.Add(index)) return string.Empty;
                text.Append(tiles[index]);
            }
            return text.ToString();
        }

        public static bool IsAccepted(PuzzleDefinition puzzle, string answer)
        {
            if (puzzle == null || puzzle.AcceptedAnswers == null) return false;
            string normalized = PersianText.NormalizeForMatch(answer);
            if (normalized.Length == 0) return false;
            for (int i = 0; i < puzzle.AcceptedAnswers.Length; i++)
                if (PersianText.NormalizeForMatch(puzzle.AcceptedAnswers[i]) == normalized) return true;
            return false;
        }

        static bool CanForm(string[] tiles, string answer)
        {
            var available = new Dictionary<char, int>();
            for (int i = 0; i < tiles.Length; i++)
            {
                string tile = PersianText.NormalizeForMatch(tiles[i]);
                if (tile.Length != 1) return false;
                char c = tile[0];
                available[c] = available.TryGetValue(c, out int count) ? count + 1 : 1;
            }
            string normalized = PersianText.NormalizeForMatch(answer);
            for (int i = 0; i < normalized.Length; i++)
            {
                char c = normalized[i];
                if (c == ' ') continue;
                if (!available.TryGetValue(c, out int count) || count == 0) return false;
                available[c] = count - 1;
            }
            return true;
        }
    }
}