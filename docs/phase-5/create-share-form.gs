/**
 * Creates the "Share a dish — Heimburger & Fries" Google Form and its response sheet.
 *
 * How to run:
 *   1. Go to https://script.google.com while signed in as michael.bredemeier@gmail.com.
 *   2. Click "New project", delete the sample code, paste this whole file, and click Save.
 *   3. Choose "createShareForm" in the function menu at the top and click Run.
 *   4. Approve the permissions Google asks for (Forms, Sheets, Drive in your own account).
 *   5. Open "Execution log" and copy the three links it prints.
 *
 * Google does not let scripts add "File upload" questions, so add the two photo
 * questions by hand afterward (see the steps printed at the end of the log).
 */
function createShareForm() {
  var SECTIONS = [
    'Appetizers, Beverages & Party Foods',
    'Breads, Preserves, Jellies & Pickles',
    'Salads, Dressings, Soups & Sandwiches',
    'Eggs & Cheese',
    'Vegetables & Vegetable Snacks',
    'Pasta & Grains',
    'Poultry, Fish & Seafoods',
    'Meats, Gravies & Meat Sauces',
    'Desserts, Fruits & Ices',
    'Cakes, Frostings & Fillings',
    'Pies, Cookies, Candies & Pastries',
    'Miscellaneous Extras, Secrets & Hints'
  ];
  var CHANGE = 'A change to a recipe in the book';
  var PHOTO = 'A photo of a dish from the book';
  var NEW_RECIPE = 'A new recipe for Family Additions';

  // --- Form and settings ---------------------------------------------------
  var form = FormApp.create('Share a dish — Heimburger & Fries');
  form.setDescription(
    'Share a photo of something you made from the cookbook, a change you make to a recipe, ' +
    'or a new recipe for the Family Additions section. A family member reviews everything ' +
    'before it appears on the site. Only your first name is shown.');
  form.setConfirmationMessage(
    'Thank you! A family member will review your submission before it appears on the site.');
  form.setLimitOneResponsePerUser(false);
  form.setAllowResponseEdits(false);
  form.setProgressBar(false);
  try {
    form.setEmailCollectionType(FormApp.EmailCollectionType.VERIFIED);
  } catch (e) {
    form.setCollectEmail(true);
  }

  // --- Section 1: About you ------------------------------------------------
  form.addTextItem().setTitle('Your first name').setRequired(true);
  var kind = form.addMultipleChoiceItem().setTitle('What are you sharing?').setRequired(true);

  // --- Section 2: A recipe from the book ---------------------------------
  var bookPage = form.addPageBreakItem().setTitle('A recipe from the book');
  var recipe = form.addTextItem()
    .setTitle('Which recipe?')
    .setHelpText('Filled in for you when you start from a recipe page.')
    .setRequired(true);
  form.addParagraphTextItem()
    .setTitle('Your change')
    .setHelpText('For example: "I use butter instead of margarine" or "Bake 50 minutes in a ' +
                 'convection oven." Leave blank if you are only sending a photo.');

  // --- Section 3: A new recipe ---------------------------------------------
  var newPage = form.addPageBreakItem().setTitle('A new recipe for Family Additions');
  // After finishing Section 2, submit instead of continuing into Section 3.
  newPage.setGoToPage(FormApp.PageNavigationType.SUBMIT);
  form.addTextItem().setTitle('Recipe name').setRequired(true);
  form.addListItem().setTitle('Which part of the book would it fit in?').setChoiceValues(SECTIONS);
  form.addTextItem().setTitle('Serves or makes').setHelpText('For example "6" or "2 loaves."');
  form.addParagraphTextItem().setTitle('Ingredients')
    .setHelpText('One ingredient per line.').setRequired(true);
  form.addParagraphTextItem().setTitle('Directions')
    .setHelpText('One step per line.').setRequired(true);

  // Branching on the first question.
  kind.setChoices([
    kind.createChoice(CHANGE, bookPage),
    kind.createChoice(PHOTO, bookPage),
    kind.createChoice(NEW_RECIPE, newPage)
  ]);

  // --- Response sheet --------------------------------------------------------
  var ss = SpreadsheetApp.create('Share a dish — responses');
  form.setDestination(FormApp.DestinationType.SPREADSHEET, ss.getId());
  SpreadsheetApp.flush();
  Utilities.sleep(3000);
  ss = SpreadsheetApp.openById(ss.getId());
  var sheet = ss.getSheets().filter(function (s) { return s.getFormUrl(); })[0] || ss.getSheets()[0];
  var lastCol = Math.max(sheet.getLastColumn(), 1);
  var statusCol = lastCol + 1;
  sheet.getRange(1, statusCol, 1, 2).setValues([['Status', 'Imported']]).setFontWeight('bold');
  var rule = SpreadsheetApp.newDataValidation()
    .requireValueInList(['Approved', 'Rejected'], true)
    .setAllowInvalid(false)
    .build();
  sheet.getRange(2, statusCol, 999, 1).setDataValidation(rule);
  sheet.setFrozenRows(1);
  // Remove the empty default tab if Google created one.
  ss.getSheets().forEach(function (s) {
    if (s.getSheetId() !== sheet.getSheetId() && s.getLastRow() === 0 && ss.getSheets().length > 1) {
      ss.deleteSheet(s);
    }
  });

  // --- Keep both files in one Drive folder ------------------------------------
  var folder = DriveApp.createFolder('Heimburger & Fries submissions');
  DriveApp.getFileById(form.getId()).moveTo(folder);
  DriveApp.getFileById(ss.getId()).moveTo(folder);

  // --- Pre-filled test link (gives Claude the field numbers) -----------------
  var prefilled = form.createResponse()
    .withItemResponse(kind.createResponse(CHANGE))
    .withItemResponse(recipe.createResponse('TEST RECIPE'))
    .toPrefilledUrl();

  Logger.log('FORM LINK (send to Claude): ' + form.getPublishedUrl());
  Logger.log('PRE-FILLED LINK (send to Claude): ' + prefilled);
  Logger.log('RESPONSE SHEET (send to Claude): ' + ss.getUrl());
  Logger.log('Form editor (for you): ' + form.getEditUrl());
  Logger.log('NEXT, by hand in the form editor: add a "File upload" question named ' +
             '"Photo of your dish" at the end of section 2, and one named "Photo of the dish" ' +
             'at the end of section 3. For each: allow only Image, 3 files, 10 MB.');
}
