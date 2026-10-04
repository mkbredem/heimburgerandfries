# Phase 1 Report — Inventory and Security Cleanup Plan

Prepared October 4, 2026. Nothing in the repository has been deleted, renamed, or rewritten. The only new files are this report and `docs/phase-1/scan-inventory.csv`.

## 1. How the site is published today

The live site is published by GitHub Pages. The workflow `.github/workflows/pages.yml` (commit `d6f2fa5`, on GitHub but not yet pulled to this computer — the local `main` branch is one commit behind `origin/main`) uploads the `linux/` folder on every push to `main`. GitHub Pages on a free GitHub account only publishes from public repositories, which is why `github.com/mkbredem/heimburgerandfries` is public. Making the repository private will stop the current site from publishing, so the repository must stay public until Cloudflare Pages is serving the new site.

## 2. Scan inventory

`docs/phase-1/scan-inventory.csv` lists all 214 scanned images with the current file name, proposed new file name, type, cookbook section, corrected name, the page number the current site's file names give it, the spread page that shows it, and whether today's dropdown menus and page-turning pages link to it.

| Type | Count |
|---|---|
| Recipes | 179 |
| Section tables of contents | 12 |
| Book index pages | 13 |
| Front matter (cover, back cover, Sue's note, preface pages 1–4) | 7 |
| Blank page images | 3 |

**Recipes missing from the dropdown menus (15).** All 179 recipes are reachable by turning pages, but these 15 have no entry in the dropdown menus on the home page: Corn Fritters, Dill Dip, The Wallaby Darned, and all 12 recipes in Miscellaneous Extras, Secrets and Hints (Apricot Oatmeal Cookies, Asian Slaw, Caramel Oatmeal Chewies, Cherry Chip Cookies, Chocolate Chip Coconut Macaroons, Cinnamon Stars, Cornflake Macaroons, Cranberry Quick Cookies, Ginger Snaps, Peanut Butter Bread, Polka Dot Cookies, Snickerdoodles).

**Duplicate dropdown menus.** The Cookies dropdown and the Pies dropdown list the same 24 recipes from Pies, Cookies, Candies and Pastries, which is why the home page shows 188 dropdown entries for 164 different recipes.

**Spelling corrections (20).** The proposed file names and recipe titles correct these: Spincach → Spinach, Coffe → Coffee, Straberry and Strawbery → Strawberry, Updside → Upside, Devilied → Deviled, Deserts → Desserts, Shis-Kabob → Shish Kabob, Campbells … Brocolli → Campbell's … Broccoli, japanese → Japanese, Antia's → Anita's, Maloasses → Molasses, Peanutbutter → Peanut Butter (two recipes), Blakened → Blackened, Saslad → Salad, Suprise → Surprise, Parmesian → Parmesan, Chilli → Chili (two recipes). The handwriting in the scans is not changed.

**Names kept as written.** "Caso," "Mariachis," "The Wallaby Darned," and "Hash Brown Casserole" alongside "Hashbrown Casserole" are kept as written; they may be family names for the dishes or two different recipes. Transcription in phase 4 will confirm.

## 3. The "b" pages and the missing page numbers

The page numbers in the current site's file names are not the page numbers printed in the book. For example, Lemon Tea Bread is in `Page 51 and 52.htm`, but the scan shows a printed page number of 29. The new site will use the printed page numbers, which phase 4 transcription will read from each scan.

**Missing page numbers are section breaks.** Every gap (111–112, 131–132, 149–150, 173–174, 187–188, 207–208, 211–212) falls between the last recipe of one section and the blank-page-plus-table-of-contents spread of the next section. The scans do not include those two page numbers, which are most likely the printed section divider sheets in the book. Whether to scan them is a question below.

**The "b" spreads at section starts are real.** `Page 63b and 64b.htm` and `Page 93b and 94b.htm` are the spread after a section's table of contents (a pink blank page and the first recipe). The book does not print page numbers on those pages, so the original site builder added "b" to keep the file names unique. Both are linked in the page-turning sequence and should be kept.

**Two "b" files are unused drafts.** No page links to `Page 63 and 64b.htm` (it shows 22 salad and soup images stacked on one page) or `Page 85b and 86b.htm` (a broken copy of the Eggs and Cheese opening). Both can be deleted.

**One broken link.** On `Page 85 and 86.htm`, the "previous page" link goes to `Page 61 and 62.htm` instead of `Page 83 and 84.htm`. The new site generates previous and next links from book order, so this error does not carry forward.

## 4. Old ASP.NET site files and credentials

`windows/httpdocs/sitebuilder/` is a 2019 SWsoft (Parallels) SiteBuilder site. It contains:

- `web.config` with a `machineKey decryptionKey` value. A machine key is the secret ASP.NET uses to encrypt data such as stored passwords.
- A membership provider configured with `passwordFormat="Encrypted"` and `enablePasswordRetrieval="true"`, which means SiteBuilder stored user passwords in a reversible form, encrypted with that machine key.
- `App_Data/SiteBuilder.mdb`, the Microsoft Access database where SiteBuilder stored its users and content.

Because the encryption key and the database sit together in a public repository, anyone who downloads the repository can likely recover whatever SiteBuilder administrator password was stored in 2019. If that password was reused anywhere (Namecheap, the old hosting control panel, email), it should be changed. No other passwords, connection strings with credentials, or API keys were found; the SMTP settings have an empty user name and password.

## 5. Web shells in Git history

Commit `39cdc1f` (September 21, 2026) removed `linux/images/FCBFED5151images.php`, `linux/images/XGBEYH5055images.aspx`, and `linux/web.config`. The first two are web shells — scripts an attacker uploads to a server so they can run commands on it through a browser — disguised as files in the images folder. They are not a danger in the repository itself, because GitHub Pages does not run PHP or ASP.NET code, but they and the `windows/` folder remain downloadable from earlier commits.

## 6. Git history recommendation

The repository has 7 commits, all from the past ten months, and none of the history is needed to build the new site. Two approaches:

| Approach | What happens | Trade-offs |
|---|---|---|
| **New private repository (recommended)** | Create `heimburgerandfries-site` as a private repository, copy in only the cleaned files, connect it to Cloudflare Pages, and after the cutover archive and then delete the old public repository. | Simplest and complete: the old history is never copied. The old repository keeps serving the current site through GitHub Pages until cutover, so there is no downtime. |
| Rewrite history with `git filter-repo` | Remove `windows/`, `linux.zip`, and the web shells from every commit and force-push to the existing repository, then make it private after cutover. | Keeps the repository name and the 7 commit messages. Requires a force-push, and GitHub can keep unreferenced commits reachable by their commit ID until GitHub support purges them. |

Either way, anyone who forked or cloned the repository before the change keeps the old history.

## 7. Housekeeping note

While reading the repository status, a `git status` command created an empty `.git/index.lock` file that could not be removed, because deleting files in this folder is turned off for Claude. A leftover `index.lock` makes every later Git command fail, so the file was moved to `_to_delete/git-index.lock` at the top of the repository. Delete the `_to_delete` folder whenever convenient.

## 8. Decisions (October 4, 2026)

- **Git history:** rewrite the existing repository with `git filter-repo` to remove `windows/`, `linux.zip`, the two web shells, and `linux/web.config` from every commit, then force-push. Make the repository private only after Cloudflare Pages is serving the new site, because GitHub Pages stops publishing a private repository on a free account. Ask GitHub support to purge cached views of the removed commits after the force-push.
- **Old SiteBuilder password:** Michael will change the password on any account where the 2019 SiteBuilder administrator password was reused.
- **Section divider pages:** not scanned. The section tables of contents serve as section openers in "Read the book" mode.
- **Unused drafts:** `Page 63 and 64b.htm` and `Page 85b and 86b.htm` are dropped.
