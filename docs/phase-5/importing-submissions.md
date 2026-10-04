# Importing approved submissions (instructions for Claude)

Michael asks: "import approved submissions." Follow these steps.

1. Download the response sheet as CSV with the Google Drive connector:
   file ID `1Sn7QH1kkAFqk11y9v-DeYDm08nkGFihMuMKFhRCA494` (Share a dish — responses), export type `text/csv`.
2. Read `docs/phase-5/imported.json` in the repository. It lists the Timestamp of every row already imported. Skip those rows.
3. Take only rows whose **Status** is `Approved`. Match columns by header name, not position:
   `Timestamp`, `Your first name`, `What are you sharing?`, `Which recipe?`, `Your change`, `Recipe name`, `Which part of the book would it fit in?`, `Serves or makes`, `Ingredients`, `Directions`, `Photo of your dish`, `Photo of the dish`.
4. For each approved row:
   - **"A change to a recipe in the book"** or **"A photo of a dish from the book"**: find the recipe file in `site/src/content/recipes/` whose title matches "Which recipe?" (ask Michael if it's ambiguous). Write `site/src/content/submissions/<recipe-id>/<YYYY-MM-DD>-<first-name>.md` with front matter `recipe`, `kind` (`modification` if "Your change" has text, else `photo`), `submitted_by` (first name only), `submitted_on`, and `photo` / `photo_alt` when a photo was uploaded. The body is the "Your change" text.
   - **"A new recipe for Family Additions"**: write `site/src/content/family-additions/<slug>.md` with `title`, `submitted_by`, `submitted_on`, `section` (mapped to the section slug), `serves`, `ingredients` (one item per line of "Ingredients"), optional `photo`, and the "Directions" lines as a numbered list in the body.
   - **Photos**: the photo cells contain Google Drive links (`https://drive.google.com/open?id=<ID>`). Download each file with the Drive connector, remove location and camera data (EXIF), resize to at most 1600 pixels wide, save it as a JPEG beside the Markdown file, and reference it with a relative path.
   - Never publish the email address. Use the first name only.
5. Add each imported row's Timestamp to `docs/phase-5/imported.json`.
6. Build the site (`npm run build` in `site/`) to confirm it succeeds, then tell Michael which entries were added and give him the commit and push commands.
