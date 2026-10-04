export const SITE = {
  name: 'Heimburger & Fries',
  bookTitle: 'With Love From My Kitchen',
  author: 'Sue Heimburger Frisby',
};

// Google Form for modifications, dish photos and new recipes (phase 5).
// Answers for "What are you sharing?" must match the form's options exactly.
export const SHARE_TYPES = {
  change: 'A change to a recipe in the book',
  photo: 'A photo of a dish from the book',
  newRecipe: 'A new recipe for Family Additions',
} as const;

export const SHARE_FORM = {
  formUrl: 'https://docs.google.com/forms/d/e/1FAIpQLSeGKWhHIwLQ16H2O1SFpH_sZ9LKc5IOPKTZ_E0P7yDswK_qsg/viewform',
  // Pre-filled field IDs from the form's "Get pre-filled link" option, e.g. 'entry.123456'.
  recipeField: 'entry.568378890', // "Which recipe?"
  typeField: 'entry.14002873', // "What are you sharing?"
};

export function shareLink(recipeTitle?: string, type?: string): string | null {
  if (!SHARE_FORM.formUrl) return null;
  const url = new URL(SHARE_FORM.formUrl);
  url.searchParams.set('usp', 'pp_url');
  if (recipeTitle && SHARE_FORM.recipeField) url.searchParams.set(SHARE_FORM.recipeField, recipeTitle);
  if (type && SHARE_FORM.typeField) url.searchParams.set(SHARE_FORM.typeField, type);
  return url.toString();
}
