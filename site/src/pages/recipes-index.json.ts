// Published at /recipes-index.json. The Google Apps Script that publishes
// approved family submissions uses it to match "Which recipe?" to a recipe file.
import { listedRecipes } from '../lib/recipes';

export async function GET() {
  const recipes = await listedRecipes();
  const body = recipes.map((r) => ({ id: r.id, title: r.data.title }));
  return new Response(JSON.stringify(body), { headers: { 'Content-Type': 'application/json' } });
}
