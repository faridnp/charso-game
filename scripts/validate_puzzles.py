#!/usr/bin/env python3
"""Validate the authored Persian puzzle bank and its local coin economy."""

from __future__ import annotations

import collections
import re
import sys
import unicodedata
from pathlib import Path

ROOT = Path(__file__).resolve().parents[1]
OPENING = ROOT / "Assets/_Project/Scripts/WordPuzzle.cs"
EXTENDED = ROOT / "Assets/_Project/Scripts/WordPuzzleExtendedContent.cs"
ECONOMY = ROOT / "Assets/_Project/Scripts/Core/RewardEconomy.cs"


def normalize(value: str) -> str:
    value = value.replace("ي", "ی").replace("ى", "ی").replace("ك", "ک")
    value = value.replace("\u0640", "").replace("\u200c", "").replace("\u200f", "")
    return "".join(ch for ch in value if not unicodedata.combining(ch) and not ch.isspace())


def fail(message: str) -> None:
    raise ValueError(message)


def main() -> int:
    opening_source = OPENING.read_text(encoding="utf-8")
    extended_source = EXTENDED.read_text(encoding="utf-8")
    economy_source = ECONOMY.read_text(encoding="utf-8")

    opening_pattern = re.compile(
        r'new PuzzleLevel\{Id=(\d+),Clue="([^"]+)",Answer="([^"]+)",'
        r'Letters=new\[\]\{([^}]*)\},Topic="([^"]+)",Difficulty=(\d+)\}'
    )
    opening_rows = []
    for match in opening_pattern.finditer(opening_source):
        puzzle_id, clue, answer, letters, topic, difficulty = match.groups()
        tiles = re.findall(r'"([^"]+)"', letters)
        opening_rows.append((int(puzzle_id), clue, answer, tiles, topic, int(difficulty)))

    data_match = re.search(r'const string Data = @"\s*(.*?)\s*";', extended_source, re.S)
    if not data_match:
        fail("Could not locate extended puzzle data string.")
    extended_rows = []
    for line_number, line in enumerate(data_match.group(1).splitlines(), start=1):
        if not line.strip():
            continue
        fields = line.split("|")
        if len(fields) != 4:
            fail(f"Extended row {line_number} must contain answer|clue|topic|difficulty.")
        answer, clue, topic, difficulty_text = fields
        try:
            difficulty = int(difficulty_text)
        except ValueError:
            fail(f"Extended row {line_number} has a non-integer difficulty.")
        extended_rows.append((20 + len(extended_rows) + 1, clue, answer, list(answer), topic, difficulty))

    if len(opening_rows) != 20:
        fail(f"Expected 20 opening puzzles; found {len(opening_rows)}.")
    if len(extended_rows) != 200:
        fail(f"Expected 200 extended puzzles; found {len(extended_rows)}.")

    all_rows = opening_rows + extended_rows
    ids = [row[0] for row in all_rows]
    if ids != list(range(1, 221)):
        fail("Puzzle IDs must be contiguous and ordered from 1 through 220.")

    seen_answers: dict[str, int] = {}
    difficulty_counts = collections.Counter()
    total_coins = 0
    for puzzle_id, clue, answer, tiles, topic, difficulty in all_rows:
        if len(tiles) != len(answer) or collections.Counter(tiles) != collections.Counter(list(answer)):
            fail(f"Puzzle {puzzle_id} tile inventory does not exactly match answer {answer!r}.")
        if len(normalize(answer)) < 2:
            fail(f"Puzzle {puzzle_id} answer is too short.")
        if len(clue.strip()) < 12 or not topic.strip():
            fail(f"Puzzle {puzzle_id} is missing a meaningful clue or topic.")
        if difficulty not in (1, 2, 3):
            fail(f"Puzzle {puzzle_id} has unsupported difficulty {difficulty}.")
        key = normalize(answer)
        if key in seen_answers:
            fail(f"Duplicate normalized answer in puzzles {seen_answers[key]} and {puzzle_id}: {answer}.")
        seen_answers[key] = puzzle_id
        difficulty_counts[difficulty] += 1
        total_coins += 8 if difficulty == 1 else 10 if difficulty == 2 else 12

    project_pattern = re.compile(r'Id\s*=\s*"([^"]+)"\s*,\s*DisplayName\s*=\s*"[^"]+"\s*,\s*CoinCost\s*=\s*(\d+)')
    projects = project_pattern.findall(economy_source)
    if len(projects) != 8 or len({project_id for project_id, _ in projects}) != 8:
        fail(f"Expected eight city projects with unique IDs; found {len(projects)}.")
    project_cost = sum(int(cost) for _, cost in projects)
    tutorial_restore_cost = 3 * 10
    net_after_tutorial = total_coins - tutorial_restore_cost
    reserve = net_after_tutorial - project_cost
    if reserve < 0:
        fail(f"Economy is insolvent: {net_after_tutorial} earned after tutorial restorations vs {project_cost} project cost.")
    if (len(all_rows), total_coins, project_cost, reserve) != (220, 1898, 1770, 98):
        fail(
            "Unexpected balance snapshot: "
            f"puzzles={len(all_rows)}, earnings={total_coins}, "
            f"project_cost={project_cost}, reserve={reserve}."
        )

    print("Puzzle and economy validation passed.")
    print(f"Puzzle bank: {len(all_rows)} total ({len(opening_rows)} opening + {len(extended_rows)} extended).")
    print(f"Difficulty distribution: easy={difficulty_counts[1]}, medium={difficulty_counts[2]}, hard={difficulty_counts[3]}.")
    print(f"Extended bank earnings: {sum(8 if row[5] == 1 else 10 if row[5] == 2 else 12 for row in extended_rows)}.")
    print(f"Full-clear earnings: {total_coins}; after tutorial restoration costs: {net_after_tutorial}.")
    print(f"City projects: {len(projects)}, total cost: {project_cost}, remaining reserve: {reserve}.")
    return 0


if __name__ == "__main__":
    try:
        sys.exit(main())
    except (OSError, ValueError) as error:
        print(f"Puzzle validation failed: {error}", file=sys.stderr)
        sys.exit(1)
