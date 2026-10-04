/**
 * Publishes approved "Share a dish" submissions to heimburgerandfries.com.
 *
 * Every 15 minutes this script reads the "Share a dish — responses" sheet. For
 * each row whose Status is "Approved" and whose Imported cell is empty, it:
 *   - resizes any uploaded photos (Google's resized copies carry no camera or
 *     location data),
 *   - writes a Markdown file (and the photos) into the GitHub repository
 *     mkbredem/heimburgerandfries in a single commit,
 *   - writes the date, or an error message, into the row's Imported cell.
 * Cloudflare Pages rebuilds the site a few minutes after each commit.
 *
 * One-time setup (see docs/phase-5/automatic-publishing.md):
 *   1. Project Settings → Script properties → add GITHUB_TOKEN with a
 *      fine-grained GitHub token (this repository only, Contents: read and write).
 *   2. Run setupTrigger once.
 * To publish right away instead of waiting, run publishApproved.
 */

var CONFIG = {
  SHEET_ID: '1Sn7QH1kkAFqk11y9v-DeYDm08nkGFihMuMKFhRCA494',
  REPO: 'mkbredem/heimburgerandfries',
  BRANCH: 'main',
  RECIPE_INDEX_URL: 'https://heimburgerandfries.com/recipes-index.json',
  PHOTO_WIDTH: 1600,
  TIME_ZONE: 'America/New_York'
};

var SECTION_SLUGS = {
  'Appetizers, Beverages & Party Foods': 'appetizers',
  'Breads, Preserves, Jellies & Pickles': 'breads',
  'Salads, Dressings, Soups & Sandwiches': 'salads-soups',
  'Eggs & Cheese': 'eggs-cheese',
  'Vegetables & Vegetable Snacks': 'vegetables',
  'Pasta & Grains': 'pasta-grains',
  'Poultry, Fish & Seafoods': 'poultry-seafood',
  'Meats, Gravies & Meat Sauces': 'meats',
  'Desserts, Fruits & Ices': 'desserts',
  'Cakes, Frostings & Fillings': 'cakes',
  'Pies, Cookies, Candies & Pastries': 'pies-cookies',
  'Miscellaneous Extras, Secrets & Hints': 'miscellaneous'
};

/** Run once: checks the sheet every 15 minutes. */
function setupTrigger() {
  ScriptApp.getProjectTriggers().forEach(function (t) {
    if (t.getHandlerFunction() === 'publishApproved') ScriptApp.deleteTrigger(t);
  });
  ScriptApp.newTrigger('publishApproved').timeBased().everyMinutes(15).create();
  Logger.log('Trigger created: publishApproved runs every 15 minutes.');
}

function publishApproved() {
  var lock = LockService.getScriptLock();
  if (!lock.tryLock(1000)) return;
  try {
    var token = PropertiesService.getScriptProperties().getProperty('GITHUB_TOKEN');
    if (!token) throw new Error('Add GITHUB_TOKEN under Project Settings → Script properties.');

    var sheet = findResponseSheet_();
    var values = sheet.getDataRange().getValues();
    var headers = values[0].map(String);
    var col = function (name) { return headers.indexOf(name); };
    var statusCol = col('Status'), importedCol = col('Imported');
    if (statusCol < 0 || importedCol < 0) throw new Error('The sheet needs Status and Imported columns.');

    var recipes = null;
    for (var r = 1; r < values.length; r++) {
      var row = values[r];
      if (String(row[statusCol]).trim() !== 'Approved' || String(row[importedCol]).trim() !== '') continue;
      var cell = sheet.getRange(r + 1, importedCol + 1);
      try {
        if (!recipes) recipes = fetchRecipeIndex_();
        var files = buildFiles_(row, headers, recipes);
        commitFiles_(token, files, 'Add family submission from ' + firstName_(row[col('Your first name')]));
        cell.setValue(Utilities.formatDate(new Date(), CONFIG.TIME_ZONE, 'yyyy-MM-dd HH:mm'));
      } catch (e) {
        cell.setValue('Error: ' + e.message);
      }
      SpreadsheetApp.flush();
    }
  } finally {
    lock.releaseLock();
  }
}

// ---------------------------------------------------------------------------

function findResponseSheet_() {
  var ss = SpreadsheetApp.openById(CONFIG.SHEET_ID);
  var sheets = ss.getSheets();
  for (var i = 0; i < sheets.length; i++) if (sheets[i].getFormUrl()) return sheets[i];
  return sheets[0];
}

function fetchRecipeIndex_() {
  var res = UrlFetchApp.fetch(CONFIG.RECIPE_INDEX_URL, { muteHttpExceptions: true });
  if (res.getResponseCode() !== 200) throw new Error('Could not load the recipe list from the website.');
  return JSON.parse(res.getContentText());
}

function buildFiles_(row, headers, recipes) {
  var get = function (name) {
    var i = headers.indexOf(name);
    return i < 0 ? '' : String(row[i] || '').trim();
  };
  var name = firstName_(get('Your first name'));
  if (!name) throw new Error('No first name.');
  var ts = row[headers.indexOf('Timestamp')];
  var date = Utilities.formatDate(ts instanceof Date ? ts : new Date(), CONFIG.TIME_ZONE, 'yyyy-MM-dd');
  var stamp = Utilities.formatDate(ts instanceof Date ? ts : new Date(), CONFIG.TIME_ZONE, 'HHmmss');
  var photos = photoIds_(row);
  var kind = get('What are you sharing?');
  var files = [];

  if (kind.indexOf('new recipe') >= 0) {
    var title = get('Recipe name');
    if (!title) throw new Error('No recipe name.');
    var slug = slugify_(title) + '-' + slugify_(name);
    var dir = 'site/src/content/family-additions/';
    var front = ['---', 'title: ' + q_(title), 'submitted_by: ' + q_(name), 'submitted_on: ' + date];
    var section = SECTION_SLUGS[get('Which part of the book would it fit in?')];
    if (section) front.push('section: ' + section);
    if (get('Serves or makes')) front.push('serves: ' + q_(get('Serves or makes')));
    if (photos.length) {
      files.push({ path: dir + slug + '.jpg', base64: resizedPhoto_(photos[0]) });
      front.push('photo: ./' + slug + '.jpg');
    }
    var items = lines_(get('Ingredients'));
    front.push('ingredients:');
    front.push('  - items:');
    items.forEach(function (i) { front.push('      - ' + q_(i)); });
    front.push('---');
    var steps = lines_(get('Directions')).map(function (s, n) { return (n + 1) + '. ' + safe_(s); });
    files.push({ path: dir + slug + '.md', text: front.join('\n') + '\n' + steps.join('\n') + '\n' });
    return files;
  }

  // A change or a photo for a recipe in the book.
  var recipe = matchRecipe_(get('Which recipe?'), recipes);
  if (!recipe) throw new Error('No recipe on the site is named "' + get('Which recipe?') + '". Fix the name in the "Which recipe?" cell, then clear this cell.');
  var change = get('Your change');
  if (!change && !photos.length) throw new Error('Nothing to publish: no change text and no photo.');
  var base = 'site/src/content/submissions/' + recipe.id + '/' + date + '-' + slugify_(name) + '-' + stamp;
  var count = Math.max(photos.length, 1);
  for (var p = 0; p < count; p++) {
    var suffix = count > 1 ? '-' + (p + 1) : '';
    var fm = ['---', 'recipe: ' + recipe.id,
              'kind: ' + (change && p === 0 ? 'modification' : 'photo'),
              'submitted_by: ' + q_(name), 'submitted_on: ' + date];
    if (photos[p]) {
      var file = base.split('/').pop() + suffix + '.jpg';
      files.push({ path: base + suffix + '.jpg', base64: resizedPhoto_(photos[p]) });
      fm.push('photo: ./' + file);
      fm.push('photo_alt: ' + q_(recipe.title + ' made by ' + name));
    }
    fm.push('---');
    var body = change && p === 0 ? safe_(change) + '\n' : '';
    files.push({ path: base + suffix + '.md', text: fm.join('\n') + '\n' + body });
  }
  return files;
}

/** Every Google Drive file link in the row (photo upload columns hold these). */
function photoIds_(row) {
  var ids = [];
  row.forEach(function (v) {
    String(v || '').replace(/[?&]id=([\w-]+)/g, function (_, id) { ids.push(id); return _; });
  });
  return ids;
}

/** Downloads a resized copy of a Drive photo; Google's resized copies have no camera or location data. */
function resizedPhoto_(fileId) {
  DriveApp.getFileById(fileId); // confirms access and keeps the Drive permission on this script
  var auth = { Authorization: 'Bearer ' + ScriptApp.getOAuthToken() };
  var meta = UrlFetchApp.fetch('https://www.googleapis.com/drive/v3/files/' + fileId + '?fields=thumbnailLink',
                               { headers: auth, muteHttpExceptions: true });
  var link = meta.getResponseCode() === 200 ? JSON.parse(meta.getContentText()).thumbnailLink : null;
  if (!link) throw new Error('Google has not finished processing the photo yet. Clear this cell to try again later.');
  var img = UrlFetchApp.fetch(link.replace(/=s\d+$/, '=s' + CONFIG.PHOTO_WIDTH), { headers: auth, muteHttpExceptions: true });
  var type = String(img.getHeaders()['Content-Type'] || '');
  if (img.getResponseCode() !== 200 || type.indexOf('image/') !== 0) throw new Error('Could not download a resized copy of the photo.');
  var blob = img.getBlob();
  if (type.indexOf('image/jpeg') !== 0) blob = blob.getAs('image/jpeg');
  return Utilities.base64Encode(blob.getBytes());
}

/** Creates all files in one commit on the main branch. */
function commitFiles_(token, files, message) {
  var api = function (method, path, body) {
    var res = UrlFetchApp.fetch('https://api.github.com/repos/' + CONFIG.REPO + path, {
      method: method,
      contentType: 'application/json',
      payload: body ? JSON.stringify(body) : undefined,
      headers: { Authorization: 'Bearer ' + token, Accept: 'application/vnd.github+json' },
      muteHttpExceptions: true
    });
    if (res.getResponseCode() >= 300) throw new Error('GitHub ' + res.getResponseCode() + ': ' + res.getContentText().slice(0, 200));
    return JSON.parse(res.getContentText());
  };
  var ref = api('get', '/git/ref/heads/' + CONFIG.BRANCH);
  var parent = api('get', '/git/commits/' + ref.object.sha);
  var tree = files.map(function (f) {
    var blob = f.base64
      ? api('post', '/git/blobs', { content: f.base64, encoding: 'base64' })
      : api('post', '/git/blobs', { content: f.text, encoding: 'utf-8' });
    return { path: f.path, mode: '100644', type: 'blob', sha: blob.sha };
  });
  var newTree = api('post', '/git/trees', { base_tree: parent.tree.sha, tree: tree });
  var commit = api('post', '/git/commits', { message: message, tree: newTree.sha, parents: [ref.object.sha] });
  api('patch', '/git/refs/heads/' + CONFIG.BRANCH, { sha: commit.sha });
}

// --- small helpers -----------------------------------------------------------

function matchRecipe_(title, recipes) {
  var norm = function (s) { return String(s).toLowerCase().replace(/&/g, 'and').replace(/[^a-z0-9]/g, ''); };
  var want = norm(title);
  if (!want) return null;
  var exact = recipes.filter(function (r) { return norm(r.title) === want; });
  return exact.length ? exact[0] : null;
}

function firstName_(s) {
  return String(s || '').trim().split(/\s+/)[0].replace(/[^\p{L}'-]/gu, '').slice(0, 30);
}

function slugify_(s) {
  return String(s).toLowerCase().replace(/'/g, '').replace(/[^a-z0-9]+/g, '-').replace(/^-+|-+$/g, '').slice(0, 60) || 'item';
}

function lines_(s) {
  return String(s || '').split(/\r?\n/).map(function (l) { return l.replace(/^\s*(\d+[.)]|[-*•])\s*/, '').trim(); })
    .filter(function (l) { return l; });
}

/** Text from the form goes into Markdown; turn < > & into plain characters so no HTML can be injected. */
function safe_(s) {
  return String(s).replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;');
}

/** A YAML-safe quoted string. */
function q_(s) {
  return JSON.stringify(String(s));
}
