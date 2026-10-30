#!/usr/bin/env python3
"""Small dependency-free repository quality checks for the project foundation."""

from __future__ import annotations

import re
import sys
from pathlib import Path

ROOT = Path(__file__).resolve().parents[1]

REQUIRED_FILES = (
    "README.md",
    "Assets/_Project/Scenes/Charso.unity",
    "Assets/_Project/Scenes/Charso.unity.meta",
    "Assets/_Project/Scripts/CharsoGameBootstrap.cs.meta",
    "ProjectSettings/EditorBuildSettings.asset",
    "AI_WORKFLOW.md",
    "CONTRIBUTING.md",
    "SECURITY.md",
    "docs/GAME_DESIGN.md",
    "docs/ARCHITECTURE.md",
    "docs/ROADMAP.md",
    "docs/HERITAGE_ASSET_PIPELINE.md",
    "docs/MODULE_OWNERSHIP.md",
    "docs/DECISIONS.md",
    "docs/RELEASE_QUALITY_BAR.md",
    "docs/CONTENT_BIBLE.md",
    "docs/QA_DEVICE_MATRIX.md",
    "docs/PRODUCTION_BACKLOG.md",
    "scripts/validate_puzzles.py",
    "docs/SECURITY_THREAT_MODEL.md",
    "docs/ART_DIRECTION.md",
    "docs/RTL_TEXT_PIPELINE.md",
    "docs/HERITAGE_RESEARCH/HAFEZIEH.md",
    ".github/CODEOWNERS",
    ".github/PULL_REQUEST_TEMPLATE.md",
    ".github/workflows/quality-gates.yml",
)

LINK_RE = re.compile(r"!?\[[^\]]*\]\(([^)]+)\)")
SECRET_PATTERNS = (
    re.compile(r"-----BEGIN (?:RSA |EC |OPENSSH )?PRIVATE KEY-----"),
    re.compile(r"\bgh[pousr]_[A-Za-z0-9]{30,}\b"),
    re.compile(r"\bAKIA[0-9A-Z]{16}\b"),
    re.compile(r"(?i)service[_-]?role[_-]?key\s*[:=]\s*['\"][^'\"]{12,}"),
)
CONFLICT_MARKERS = ("<" * 7 + " ", ">" * 7 + " ", "=" * 7)


def check_required_files(errors: list[str]) -> None:
    for relative in REQUIRED_FILES:
        if not (ROOT / relative).is_file():
            errors.append(f"Missing required file: {relative}")



def check_playable_scene(errors: list[str]) -> None:
    scene_path = ROOT / "Assets/_Project/Scenes/Charso.unity"
    scene_meta_path = ROOT / "Assets/_Project/Scenes/Charso.unity.meta"
    bootstrap_meta_path = ROOT / "Assets/_Project/Scripts/CharsoGameBootstrap.cs.meta"
    build_settings_path = ROOT / "ProjectSettings/EditorBuildSettings.asset"
    if not all(p.is_file() for p in (scene_path, scene_meta_path, bootstrap_meta_path, build_settings_path)):
        return
    scene = scene_path.read_text(encoding="utf-8")
    scene_meta = scene_meta_path.read_text(encoding="utf-8")
    bootstrap_meta = bootstrap_meta_path.read_text(encoding="utf-8")
    build_settings = build_settings_path.read_text(encoding="utf-8")
    scene_guid_match = re.search(r"(?m)^guid: ([0-9a-f]{32})$", scene_meta)
    bootstrap_guid_match = re.search(r"(?m)^guid: ([0-9a-f]{32})$", bootstrap_meta)
    if not scene_guid_match or scene_guid_match.group(1) not in build_settings:
        errors.append("Playable scene GUID is missing from EditorBuildSettings.asset.")
    if not bootstrap_guid_match or f"guid: {bootstrap_guid_match.group(1)}" not in scene:
        errors.append("Playable scene does not reference CharsoGameBootstrap.cs.")
    if "m_Name: CharsoGame" not in scene:
        errors.append("Playable scene is missing the CharsoGame bootstrap object.")


def check_markdown_links(errors: list[str]) -> None:
    for markdown in ROOT.rglob("*.md"):
        if any(part in {".git", "Library", "Temp", "Obj"} for part in markdown.parts):
            continue
        try:
            content = markdown.read_text(encoding="utf-8")
        except UnicodeDecodeError:
            errors.append(f"Markdown is not UTF-8: {markdown.relative_to(ROOT)}")
            continue
        for target in LINK_RE.findall(content):
            target = target.strip().split("#", 1)[0]
            if not target or re.match(r"^(?:https?://|mailto:|tel:)", target, re.I):
                continue
            target = target.split(" ", 1)[0]
            candidate = (markdown.parent / target).resolve()
            if not candidate.exists():
                errors.append(
                    f"Broken local Markdown link in {markdown.relative_to(ROOT)}: {target}"
                )


def check_text_safety(errors: list[str]) -> None:
    ignored_parts = {".git", "Library", "Temp", "Obj", "Build", "Builds"}
    for path in ROOT.rglob("*"):
        if not path.is_file() or any(part in ignored_parts for part in path.parts):
            continue
        if path.suffix.lower() not in {".md", ".py", ".yml", ".yaml", ".json", ".cs", ".txt"}:
            continue
        try:
            content = path.read_text(encoding="utf-8")
        except (UnicodeDecodeError, OSError):
            continue
        relative = path.relative_to(ROOT)
        for marker in CONFLICT_MARKERS:
            if marker in content:
                errors.append(f"Possible merge-conflict marker in {relative}: {marker}")
        for pattern in SECRET_PATTERNS:
            if pattern.search(content):
                errors.append(f"Possible secret-like value in {relative} (pattern: {pattern.pattern})")


def main() -> int:
    errors: list[str] = []
    check_required_files(errors)
    check_playable_scene(errors)
    check_markdown_links(errors)
    check_text_safety(errors)

    if errors:
        print("Repository validation failed:")
        for error in errors:
            print(f" - {error}")
        return 1

    print("Repository foundation checks passed.")
    print(f"Checked {len(REQUIRED_FILES)} required files and local Markdown links.")
    print("Note: this is a lightweight check, not a full secret scanner or security audit.")
    return 0


if __name__ == "__main__":
    sys.exit(main())
