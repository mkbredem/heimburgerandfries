import { getCollection } from 'astro:content';

export async function bookRecipes() {
  const all = await getCollection('recipes');
  return all.sort((a, b) => a.data.book_order - b.data.book_order);
}

export const recipeUrl = (id: string) => `/recipes/${id}/`;
export const sectionUrl = (slug: string) => `/sections/${slug}/`;
