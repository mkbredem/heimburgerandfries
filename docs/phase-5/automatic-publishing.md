# Automatic publishing of approved submissions

A Google Apps Script, `publish-approved.gs`, checks the **Share a dish — responses** sheet every 15 minutes. For each row with **Status = Approved** and an empty **Imported** cell, it adds the submission and a resized copy of any photo to the GitHub repository in one commit, then writes the date in **Imported**. Cloudflare Pages rebuilds heimburgerandfries.com a few minutes later.

If something goes wrong with a row, the script writes `Error: …` in that row's **Imported** cell instead. Fix the cause (usually a recipe name in "Which recipe?" that doesn't match a recipe on the site), then clear the **Imported** cell; the next run tries again.

## One-time setup

### 1. Create a GitHub token for this one repository

1. On github.com, click your profile picture → **Settings** → **Developer settings** (bottom of the left menu) → **Personal access tokens** → **Fine-grained tokens** → **Generate new token**.
2. **Token name:** `heimburgerandfries publishing`.
3. **Expiration:** choose the longest option offered. GitHub emails you before it expires; when it does, generate a new token and replace it in step 2.3 below.
4. **Repository access:** **Only select repositories** → `mkbredem/heimburgerandfries`.
5. **Permissions → Repository permissions → Contents:** **Read and write**. Leave everything else at "No access."
6. Click **Generate token** and copy it (it starts with `github_pat_`). GitHub shows it only once.

### 2. Add the script and the token to the Apps Script project

1. Open script.google.com and the project you used to create the form.
2. Click **+** next to **Files** → **Script**, name it `publish`, delete the sample code, paste all of `publish-approved.gs`, and click **Save**.
3. Click the gear icon (**Project Settings**) on the left, scroll to **Script properties**, click **Add script property**: Property `GITHUB_TOKEN`, Value = the token from step 1. Click **Save script properties**.
4. Back in the editor (`<>` icon), choose **setupTrigger** in the function menu and click **Run**. Approve the new permissions (Advanced → Go to project → Allow) — it now also needs to connect to GitHub and the website.
5. To publish what's already approved without waiting 15 minutes, choose **publishApproved** and click **Run**.

## What the script will and won't do

- Publishes only rows marked **Approved**, and each row only once.
- Shows only the submitter's first name. Email addresses never leave the sheet.
- Photos are published as Google's resized copies (1600 pixels wide), which carry no camera or location data.
- Text from the form is published as plain text; any HTML in it is shown as characters, not run.
- The GitHub token can change only this one repository's files.

## Taking a submission down

Ask Claude to remove it, or delete the file under `site/src/content/submissions/<recipe>/` or `site/src/content/family-additions/` and push. Set the row's Status to **Rejected** so it isn't published again if someone clears the Imported cell.
