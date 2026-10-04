import { getCollection } from 'astro:content';

export async function bookRecipes() {
  const all = await getCollection('recipes');
  return all.sort((a, b) => a.data.book_order - b.data.book_order);
}

/** Recipes shown in lists, search and previous/next links (continuation pages left out). */
export async function listedRecipes() {
  return (await bookRecipes()).filter((r) => !r.data.continuation_of);
}

export const recipeUrl = (id: string) => `/recipes/${id}/`;
export const sectionUrl = (slug: string) => `/sections/${slug}/`;
