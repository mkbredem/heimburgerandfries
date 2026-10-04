#!/usr/bin/env bash
# Removes the old ASP.NET site, the zip copy, the web shells and the old
# web.config from every commit in this repository, then force-pushes.
#
# Run this on your Mac from the repository folder:  bash docs/phase-1/rewrite-history.sh
# Requires git-filter-repo:  brew install git-filter-repo
#
# The script stops before the force-push and asks you to type "yes".
set -euo pipefail

cd "$(git rev-parse --show-toplevel)"

echo "== 1. Bring local main up to date with GitHub (local main is 1 commit behind)"
git checkout main
git pull --ff-only origin main

echo "== 2. Make a full backup copy of the repository before rewriting"
backup="../heimburgerandfries-backup-$(date +%Y%m%d-%H%M%S).git"
git clone --mirror . "$backup"
echo "   Backup written to $backup"

echo "== 3. Remove the paths from every commit"
git filter-repo --force \
  --invert-paths \
  --path windows/ \
  --path linux.zip \
  --path _to_delete/ \
  --path linux/web.config \
  --path linux/images/FCBFED5151images.php \
  --path linux/images/XGBEYH5055images.aspx

echo "== 4. Confirm the paths are gone from history (no output means clean)"
git log --all --name-only --format='' | grep -E '^(windows/|linux\.zip|linux/web\.config|linux/images/.*\.(php|aspx))' || echo "   History is clean."

# git filter-repo removes the 'origin' remote as a safety measure; add it back.
git remote add origin https://github.com/mkbredem/heimburgerandfries.git 2>/dev/null || true

read -r -p "== 5. Force-push the rewritten history to GitHub? Type yes to continue: " answer
if [ "$answer" = "yes" ]; then
  git push --force origin main
  echo "   Pushed. GitHub Pages will republish linux/ from the rewritten main branch."
  echo "   Next: ask GitHub Support to purge cached views of the removed commits"
  echo "   (https://support.github.com, 'Remove sensitive data' request) and list"
  echo "   commit 3fd4f23 and the repository name."
else
  echo "   Not pushed. The rewritten history is only on this computer."
fi
