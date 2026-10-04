export const SITE = {
  name: 'Heimburger & Fries',
  bookTitle: 'With Love From My Kitchen',
  author: 'Sue Heimburger Frisby',
};

// Google Form for modifications, dish photos and new recipes (phase 5).
// Leave formUrl empty until the form exists; the buttons then explain that
// sharing is coming soon instead of linking nowhere.
export const SHARE_FORM = {
  formUrl: '',
  // Pre-filled field IDs from the form's "Get pre-filled link" option, e.g. 'entry.123456'.
  recipeField: '',
  typeField: '',
};

export function shareLink(recipeTitle?: string, type?: string): string | null {
  if (!SHARE_FORM.formUrl) return null;
  const url = new URL(SHARE_FORM.formUrl);
  url.searchParams.set('usp', 'pp_url');
  if (recipeTitle && SHARE_FORM.recipeField) url.searchParams.set(SHARE_FORM.recipeField, recipeTitle);
  if (type && SHARE_FORM.typeField) url.searchParams.set(SHARE_FORM.typeField, type);
  return url.toString();
}
