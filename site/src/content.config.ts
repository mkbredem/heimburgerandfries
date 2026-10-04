import { defineCollection } from 'astro:content';
import { glob } from 'astro/loaders';
import { z } from 'astro/zod';
import { SECTION_SLUGS } from './data/sections';

const ingredientGroups = z
  .array(z.object({ group: z.string().optional(), items: z.array(z.string()) }))
  .default([]);

// One file per recipe from the original cookbook.
const recipes = defineCollection({
  loader: glob({ pattern: '*.md', base: './src/content/recipes' }),
  schema: ({ image }) =>
    z.object({
      title: z.string(),
      section: z.enum(SECTION_SLUGS),
      book_order: z.number().int(),
      printed_page: z.number().int().optional(),
      scan: image(),
      source: z.literal('book'),
      serves: z.string().optional(),
      // pending = not typed yet; draft = typed by Claude, not yet proofread
      transcription_status: z.enum(['pending', 'draft', 'family-reviewed']),
      tags: z.array(z.string()).default([]),
      ingredients: ingredientGroups,
    }),
});

// Recipes the family adds after the book.
const familyAdditions = defineCollection({
  loader: glob({ pattern: '*.md', base: './src/content/family-additions' }),
  schema: ({ image }) =>
    z.object({
      title: z.string(),
      submitted_by: z.string(), // first name only
      submitted_on: z.coerce.date(),
      section: z.enum(SECTION_SLUGS).optional(),
      serves: z.string().optional(),
      photo: image().optional(),
      tags: z.array(z.string()).default([]),
      ingredients: ingredientGroups,
    }),
});

// Approved modifications and dish photos for book recipes.
const submissions = defineCollection({
  loader: glob({ pattern: '**/*.md', base: './src/content/submissions' }),
  schema: ({ image }) =>
    z.object({
      recipe: z.string(), // recipe file name without .md
      kind: z.enum(['modification', 'photo']),
      submitted_by: z.string(), // first name only
      submitted_on: z.coerce.date(),
      photo: image().optional(),
      photo_alt: z.string().optional(),
    }),
});

export const collections = { recipes, 'family-additions': familyAdditions, submissions };
