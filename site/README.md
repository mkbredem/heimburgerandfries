# heimburgerandfries.com — new site

A static site built with [Astro](https://astro.build) and searched with [Pagefind](https://pagefind.app). There is no server code, no database and no login on the public host.

## Everyday tasks

| Task | Command (run in this `site/` folder) |
|---|---|
| Install dependencies (first time, or after `package.json` changes) | `npm install` |
| Preview while editing, at http://localhost:4321 | `npm run dev` |
| Build the finished site into `dist/` and build the search index | `npm run build` |
| Preview the finished build | `npm run preview` |

Search only works after `npm run build`, because Pagefind builds its index from the finished pages.

## Where things live

- `src/content/recipes/` — one Markdown file per recipe in the book. `transcription_status` is `pending` (not typed yet), `draft` (typed by Claude, not proofread) or `family-reviewed`. `breads-lemon-tea-bread.md` is the complete example.
- `src/content/family-additions/` — new recipes from the family (phase 5).
- `src/content/submissions/` — approved modifications and dish photos for book recipes (phase 5).
- `src/assets/scans/` — the cookbook scans with corrected file names. The build makes WebP copies at several sizes.
- `src/data/book.json` — every page in book order, used by "Read the book".
- `src/data/site.ts` — the Google Form link for sharing (empty until phase 5).
- `public/_headers` — security headers for Cloudflare Pages.
- `public/_redirects` — old `Page N and N+1.htm` and old image URLs sent to the new pages.

## Importing the old scans again

`scripts/import_legacy.py` reads `../docs/phase-1/scan-inventory.csv` and `../linux/`, copies the scans, writes recipe files and rebuilds `book.json` and `_redirects`. It never overwrites a recipe file that already exists, so typed recipes are safe.

## Cloudflare Pages settings (phase 6)

- Root directory: `site`
- Build command: `npm run build`
- Build output directory: `dist`
- Environment variable: `NODE_VERSION` = `22`

## Expected build warnings

Until the first Family Addition and the first approved submission exist, the build prints "The collection … does not exist or is empty" for `family-additions` and `submissions`. The warnings are harmless.
