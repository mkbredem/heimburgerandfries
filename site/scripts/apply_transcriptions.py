#!/usr/bin/env python3
"""Merge drafted transcriptions into the recipe Markdown files.

Reads ../docs/phase-4/transcriptions.json and updates every recipe file in
src/content/recipes/ whose transcription_status is still "pending".
Files already marked draft or family-reviewed are left alone, so this can be
re-run safely. Also writes ../docs/phase-4/proofreading-checklist.md.

Run from the site/ folder:  python3 scripts/apply_transcriptions.py
"""
import json
import re
from pathlib import Path

SITE = Path(__file__).resolve().parent.parent
DOCS = SITE.parent / "docs" / "phase-4"
RECIPES = SITE / "src" / "content" / "recipes"

# Pages that only continue the previous recipe: continuation id -> main id.
CONTINUATIONS = {
    "breads-strawberry-jam-freezer-2": "breads-strawberry-jam-freezer",
    "cakes-apple-upside-down-cake-2": "cakes-apple-upside-down-cake",
    "vegetables-chunky-squash-and-potato-puree-2": "vegetables-chunky-squash-and-potato-puree",
}

q = lambda s: json.dumps(s, ensure_ascii=False)  # JSON strings are valid YAML


def read_front(path):
    text = path.read_text()
    m = re.match(r"---\n(.*?)\n---\n?(.*)", text, re.S)
    lines = m.group(1).splitlines()
    keep = {}
    for line in lines:
        k, _, v = line.partition(":")
        keep[k.strip()] = v.strip()
    return keep


def write_recipe(rid, front, t, extra_scans=None, continuation_of=None):
    out = ["---", f"title: {front['title']}", f"section: {front['section']}",
           f"book_order: {front['book_order']}"]
    if t.get("printed_page"):
        out.append(f"printed_page: {int(t['printed_page'])}")
    out += [f"scan: {front['scan']}", "source: book"]
    if extra_scans:
        out.append("more_scans:")
        out += [f"  - {s}" for s in extra_scans]
    if continuation_of:
        out.append(f"continuation_of: {continuation_of}")
    if t.get("serves"):
        out.append(f"serves: {q(t['serves'])}")
    out.append("transcription_status: draft")
    out.append("tags: [" + ", ".join(q(x) for x in t.get("tags", [])) + "]")
    groups = [g for g in t.get("ingredients", []) if g.get("items")]
    if groups:
        out.append("ingredients:")
        for g in groups:
            out.append(f"  - group: {q(g['group'])}" if g.get("group") else "  - items:")
            if g.get("group"):
                out.append("    items:")
            out += [f"      - {q(i)}" if g.get("group") else f"      - {q(i)}" for i in g["items"]]
    else:
        out.append("ingredients: []")
    out.append("---")
    body = [f"{n}. {s}" for n, s in enumerate(t.get("directions", []), 1)]
    for note in t.get("notes", []):
        body += ["", f"**Note:** {note}"]
    (RECIPES / f"{rid}.md").write_text("\n".join(out) + "\n" + "\n".join(body) + "\n")


def main():
    data = json.loads((DOCS / "transcriptions.json").read_text())
    updated, skipped = 0, 0
    for rid, t in data.items():
        path = RECIPES / f"{rid}.md"
        front = read_front(path)
        if front.get("transcription_status") != "pending":
            skipped += 1
            continue
        if rid in CONTINUATIONS:
            write_recipe(rid, front, t, continuation_of=CONTINUATIONS[rid])
        elif rid in CONTINUATIONS.values():
            cont = next(c for c, m in CONTINUATIONS.items() if m == rid)
            merged = dict(t)
            merged["directions"] = [s for s in t.get("directions", []) if "continued on next page" not in s.lower()] + data[cont].get("directions", [])
            merged["notes"] = [n for n in t.get("notes", []) if "ontinu" not in n] + \
                              [n for n in data[cont].get("notes", []) if "ontinu" not in n]
            merged["tags"] = t.get("tags", [])
            cont_scan = read_front(RECIPES / f"{cont}.md")["scan"]
            write_recipe(rid, front, merged, extra_scans=[cont_scan])
        else:
            write_recipe(rid, front, t)
        updated += 1

    # Proofreading checklist, one line per recipe, uncertain words listed.
    lines = ["# Proofreading checklist", "",
             "Each recipe below was typed from its scan by Claude and is marked as a draft.",
             "To proofread one: open the recipe page on heimburgerandfries.com, compare the typed",
             "recipe with the scan, fix the Markdown file in `site/src/content/recipes/`, and change",
             "`transcription_status: draft` to `transcription_status: family-reviewed`.", "",
             "Recipes with words Claude could not read are listed first.", ""]
    unsure = {k: v for k, v in data.items() if v.get("uncertain")}
    lines.append(f"## Words to check ({len(unsure)} recipes)\n")
    for rid, t in sorted(unsure.items()):
        lines.append(f"- [ ] **{t['title']}** (`{rid}.md`)")
        lines += [f"  - {u}" for u in t["uncertain"]]
    lines.append(f"\n## All other recipes ({len(data) - len(unsure)})\n")
    for rid, t in sorted(data.items()):
        if rid not in unsure:
            lines.append(f"- [ ] {t['title']} (`{rid}.md`)")
    (DOCS / "proofreading-checklist.md").write_text("\n".join(lines) + "\n")
    print(f"updated {updated}, skipped {skipped}, {len(unsure)} recipes have words to check")


if __name__ == "__main__":
    main()
