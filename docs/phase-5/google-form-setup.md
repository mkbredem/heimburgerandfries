# Setting up the "Share a dish" Google Form

This form collects three kinds of family submissions: a change to a recipe in the book, a photo of a dish made from the book, and a new recipe for Family Additions. Nothing a family member sends appears on heimburgerandfries.com until an approver marks it "Approved" and the submission is imported.

Create the form while signed in to michael.bredemeier@gmail.com, so the responses and uploaded photos are stored in that Google Drive.

## 1. Create the form

1. Go to forms.google.com and click **Blank form**.
2. Title: **Share a dish — Heimburger & Fries**
3. Description: *Share a photo of something you made from the cookbook, a change you make to a recipe, or a new recipe for the Family Additions section. A family member reviews everything before it appears on the site. Only your first name is shown.*

## 2. Section 1 — About you

| # | Question | Type | Required | Settings |
|---|---|---|---|---|
| 1 | Your first name | Short answer | Yes | |
| 2 | What are you sharing? | Multiple choice | Yes | Options, in this order: **A change to a recipe in the book** · **A photo of a dish from the book** · **A new recipe for Family Additions**. Click the three-dot menu on this question → **Go to section based on answer**. Send the first two options to Section 2 and the third option to Section 3. |

## 3. Section 2 — A recipe from the book

Click **Add section** (the icon of two stacked rectangles) and title it **A recipe from the book**.

| # | Question | Type | Required | Settings |
|---|---|---|---|---|
| 3 | Which recipe? | Short answer | Yes | Help text: *Filled in for you when you start from a recipe page.* |
| 4 | Your change | Paragraph | No | Help text: *For example: "I use butter instead of margarine" or "Bake 50 minutes in a convection oven." Leave blank if you're only sending a photo.* |
| 5 | Photo of your dish | File upload | No | Allow only specific file types → **Image**. Maximum number of files: **3**. Maximum file size: **10 MB**. |

At the bottom of Section 2, set **After section 2** to **Submit form**.

## 4. Section 3 — A new recipe

Add another section titled **A new recipe for Family Additions**.

| # | Question | Type | Required | Settings |
|---|---|---|---|---|
| 6 | Recipe name | Short answer | Yes | |
| 7 | Which part of the book would it fit in? | Dropdown | No | Appetizers, Beverages & Party Foods · Breads, Preserves, Jellies & Pickles · Salads, Dressings, Soups & Sandwiches · Eggs & Cheese · Vegetables & Vegetable Snacks · Pasta & Grains · Poultry, Fish & Seafoods · Meats, Gravies & Meat Sauces · Desserts, Fruits & Ices · Cakes, Frostings & Fillings · Pies, Cookies, Candies & Pastries · Miscellaneous Extras, Secrets & Hints |
| 8 | Serves or makes | Short answer | No | Help text: *For example "6" or "2 loaves."* |
| 9 | Ingredients | Paragraph | Yes | Help text: *One ingredient per line.* |
| 10 | Directions | Paragraph | Yes | Help text: *One step per line.* |
| 11 | Photo of the dish | File upload | No | Image only, 3 files, 10 MB. |

## 5. Settings

Open the **Settings** tab:

- **Responses → Collect email addresses: Verified.** Google requires sign-in for file uploads anyway. Email addresses stay in your Google Sheet and are never shown on the site.
- **Responses → Limit to 1 response: Off.**
- **Presentation → Confirmation message:** *Thank you! A family member will review your submission before it appears on the site.*

## 6. Link the responses to a Google Sheet

1. Open the **Responses** tab and click **Link to Sheets** → **Create a new spreadsheet**. Name it **Share a dish — responses**.
2. In the new sheet, add two columns to the right of the last response column:
   - **Status** — select the column, then **Data → Data validation → Dropdown** with the options **Approved** and **Rejected**.
   - **Imported** — leave empty. The import fills in the date when a row has been added to the site.
3. Share the sheet (**Share** button) as **Editor** with each family member who should approve submissions.

## 7. Send Claude three links

1. **The form's link:** click **Send** → the link icon → **Copy**. It looks like `https://docs.google.com/forms/d/e/…/viewform`.
2. **A pre-filled link:** click the three-dot menu at the top right of the form editor → **Get pre-filled link**. Type `TEST RECIPE` in "Which recipe?" and choose **A change to a recipe in the book** for "What are you sharing?", then click **Get link** → **Copy link**. Claude reads the field numbers (`entry.123…`) from this link so each recipe page can fill in its own recipe name.
3. **The response sheet's link.**

Claude then connects the "Share your version" buttons on every recipe page, the "Share your dish" and "Submit a new recipe" buttons, and the Share a dish page to the form.
